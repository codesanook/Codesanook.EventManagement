using System.Linq;
using System.Threading.Tasks;
using Codesanook.EventManagement.Indexes;
using Codesanook.EventManagement.Models;
using Codesanook.EventManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Entities;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using YesSql;

namespace Codesanook.EventManagement.Controllers
{
    public class EventController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISiteService _siteService;
        private readonly ISession _session;
        private readonly dynamic New;
        private readonly IShapeFactory _shapeFactory;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly UserManager<IUser> _userManager;


        public EventController(
            ILogger<EventController> logger,
            ISiteService siteService,
            ISession session,
            IShapeFactory shapeFactory,
            IUpdateModelAccessor updateModelAccessor,
            IContentItemDisplayManager contentItemDisplayManager,
            UserManager<IUser> userManager
        )
        {
            _logger = logger;
            _siteService = siteService;
            _session = session;
            New = shapeFactory;
            _updateModelAccessor = updateModelAccessor;
            _contentItemDisplayManager = contentItemDisplayManager;
            _shapeFactory = shapeFactory;
            _userManager = userManager;
        }

        [Route("/events", Name = "AllEvents")]
        public async Task<ActionResult> Index(PagerParameters pagerParameters, PagerSlimParameters pagerSlimParameters)
        {
            var eventQuery = _session
                .Query<ContentItem>()
                // Query With multiple indexes
                .With<ContentItemIndex>(q => q.ContentType == "Event" && q.Published && q.Latest)
                .With<EventPartIndex>()
                .OrderBy(index => index.StartUtc); // Start by in comming events

            var count = await eventQuery.CountAsync();
            var pageSize = (await _siteService.GetSiteSettingsAsync()).PageSize;
            var pager = new Pager(pagerParameters, pageSize);
            var events = await eventQuery
                .Skip(pager.GetStartIndex())
                .Take(pager.PageSize)
                .ListAsync();

            var shapeTasks = events.Select(
                v => _contentItemDisplayManager.BuildDisplayAsync(v, _updateModelAccessor.ModelUpdater, "Summary")
            );

            var eventShapes = await Task.WhenAll(shapeTasks);
            var list = await New.List(); // Defined in src/OrchardCore/OrchardCore.DisplayManagement/Shapes/CoreShapes.cs
            foreach (var shape in eventShapes)
            {
                await list.AddAsync(shape); // Defined in src/OrchardCore/OrchardCore.DisplayManagement/Shapes/Shape.cs
            }

            var pagerShape = (await New.Pager(pager)).TotalItemCount(count);
            var shapeViewModel = await _shapeFactory.CreateAsync<EventIndexViewModel>(
                "EventList",
                viewModel =>
                {
                    viewModel.List = list;
                    viewModel.Pager = pagerShape;
                }
            );
            return View(shapeViewModel);
        }

        public async Task<ActionResult> Test(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) as User;

            user.Alter<UserProfile>(nameof(UserProfile), p => {
                p.Occupation = Occupation.Staff;
                p.OfficeName = "Hospital A";
                p.MobilePhoneNumber = "0812345678";
            });

            var userProfile = user.As<UserProfile>();
            var phoneNumber = userProfile.MobilePhoneNumber;
            var occupation = userProfile.Occupation;
            var OfficeName = userProfile.OfficeName;

            return Json(user);
        }
    }
}