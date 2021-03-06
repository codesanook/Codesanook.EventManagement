﻿using Codesanook.EventManagement.Controllers;
using Orchard.ContentManagement;
using Orchard.Core.Contents;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Codesanook.EventManagement {
    // Document https://docs.orchardproject.net/en/latest/Documentation/Adding-admin-menu-items/
    public class AdminMenu : INavigationProvider {
        private readonly IContentManager contentManager;
        public Localizer T { get; set; }

        public AdminMenu(IContentManager contentManager) {
            this.contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        // For admin menu
        public string MenuName => "admin";

        // Create parent menu
        public void GetNavigation(NavigationBuilder builder) {
            builder
                // Include menu.event-admin.css
                .AddImageSet("event")
                .Add(T("Event"), "1.5", BuildChildMenuItems);

            // TODO add event booking (admin) at 1.6
            builder
               // only 1 level menu
               .Add(
                   item => item
                       .Caption(T("Event booking"))
                       .Position("1.6")
                       .Action(
                           actionName: nameof(EventBookingAdminController.Index),
                           controllerName: "EventBookingAdmin",
                           new { area = "Codesanook.EventManagement" }
                       )
               );

            builder
                // only 1 level menu
                .Add(
                    item => item
                        .Caption(T("Bank account"))
                        .Position("1.7")
                        .Action(
                            actionName: nameof(BankAccountAdminController.Index),
                            controllerName: "BankAccountAdmin",
                            new { area = "Codesanook.EventManagement" }
                        )
                );
        }

        private void BuildChildMenuItems(NavigationItemBuilder menu) {
            menu.LinkToFirstChild(false);

            // child menu
            menu.Add(
                T("All events"),
                "1.0",
                item => item.Action(
                    "Index",
                    "EventAdmin",
                    new { area = "Codesanook.EventManagement" }
                )
             );

            // child menu
            var contentItem = contentManager.New("Event");
            var contentItemMetadata = contentManager.GetItemMetadata(contentItem);

            menu.Add(
                T("New event"),
                "1.1",
                item => item.Action(
                    contentItemMetadata.CreateRouteValues["Action"] as string,
                    contentItemMetadata.CreateRouteValues["Controller"] as string,
                    contentItemMetadata.CreateRouteValues
                )
                // Apply "CreateContent" permission for the content type
                .Permission(
                    DynamicPermissions.CreateDynamicPermission(
                        DynamicPermissions.PermissionTemplates[Permissions.CreateContent.Name],
                        contentItem.TypeDefinition
                    )
                )
            );

        }
    }
}
