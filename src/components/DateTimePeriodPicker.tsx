import moment from 'moment';
import { useState } from 'react';
import DatePicker from './DatePicker';
import TimePicker from './TimePicker';
interface EventPeriod {
  startUtc: string;
  finishUtc: string;
}

interface LocalizeName {
  name: string;
  value: string;
}

interface LabelPeriod {
  date: LocalizeName;
  startTime: LocalizeName;
  finishTime: LocalizeName;
}

interface IProps {
  prefix: string;
  periods: EventPeriod[];
  title: LocalizeName;
  periodLabel: LabelPeriod;
}

// nameof in TypeScript https://www.meziantou.net/typescript-nameof-operator-equivalent.htm
function nameof<T>(key: keyof T, instance?: T): keyof T {
  return key;
}

export default function DateTimePeriodPicker(props: IProps) {

  const [periods, setPeriods] = useState<EventPeriod[]>(props.periods ?? []);

  const onChange = (date: string, index: number, type: string) => {
    switch (type) {
      case 'date': {
        let _periods = [...periods]
        const _date = moment(date)
        _periods[index] = {
          startUtc: moment(_periods[index].startUtc).set({
            date: _date.get('date'),
            month: _date.get('month'),
            year: _date.get('year')
          }).toISOString(),
          finishUtc: moment(_periods[index].finishUtc).set({
            date: _date.get('date'),
            month: _date.get('month'),
            year: _date.get('year')
          }).toISOString()
        }
        setPeriods(_periods)
        return;
      }

      case 'startTime': {
        let _periods = [...periods]
        const _time = moment(date)
        const _newPeriod = _periods[index] = {
          ..._periods[index],
          startUtc: moment(_periods[index].startUtc).set({
            hour: _time.get('hour'),
            minute: _time.get('minute'),
            second: _time.get('second')
          }).toISOString(),
        }
        _periods[index] = _newPeriod
        setPeriods(_periods)
        return;
      }

      case 'endTime': {
        let _periods = [...periods]
        const _time = moment(date)
        const _newPeriod = _periods[index] = {
          ..._periods[index],
          finishUtc: moment(_periods[index].finishUtc).set({
            hour: _time.get('hour'),
            minute: _time.get('minute'),
            second: _time.get('second')
          }).toISOString(),
        }
        _periods[index] = _newPeriod
        setPeriods(_periods)
        return;
      }

      default:
        break;
    }
  }

  const removePeriod = (index: number) => () => {
    const _periods = [...periods]
    _periods.splice(index, 1)
    setPeriods(_periods)
  }

  const addPeriod = () => () => {
    const _newPeriod = {
      startUtc: moment().toISOString(),
      finishUtc: moment().toISOString()
    }
    setPeriods([...periods, _newPeriod])
  }

  return (
    <div>
      <div>
        {props.title?.value ?? "Schedule Event"}
      </div>
      {
        periods && periods.map((period, index) => (
          <div key={index}>
            {
              /* 
                expect name patterns: prefix[0].startUtc and prefix[0].finishUtc 
                index must be order 0, 1, 2 if 0, 2 the 2 index will be remove. 
                More detail https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-6.0#collections
              */
            }
            <input type="hidden" name={`${props.prefix}[${index}].${nameof('startUtc', period)}`} value={period.startUtc} />
            <input type="hidden" name={`${props.prefix}[${index}].${nameof('finishUtc', period)}`} value={period.finishUtc} />
            <div className="time-period">
              {
                periods.length != 1 && (
                  <div className="remove-period" onClick={removePeriod(index)}>
                    <span><i className="fa fa-times"></i></span>
                  </div>
                )
              }
              <div className="form-group -fullFlex">
                <label>{props.periodLabel?.date?.value ?? "Date"}</label>
                <div>
                  <DatePicker className="-fullWidth" name={`date-start-${index}`} value={period.startUtc} onChange={(date) => onChange(date, index, 'date')} />
                </div>
              </div>
              <div className="form-group -halfFlex">
               <label>{props.periodLabel?.startTime?.value ?? "Start Time"}</label>
                <div>
                  <TimePicker className="-fullWidth" name={`time-start-${index}`} value={period.startUtc} onChange={(date) => onChange(date, index, 'startTime')} />
                </div>
              </div>
              <div className="form-group -halfFlex">
               <label>{props.periodLabel?.finishTime?.value ?? "Finish Time"}</label>
                <div>
                  <TimePicker className="-fullWidth" name={`time-finish-${index}`} value={period.finishUtc} onChange={(date) => onChange(date, index, 'endTime')} />
                </div>
              </div>
            </div>
          </div>))
      }
      <div className="add-period" onClick={addPeriod()}>
        <span><i className="fa fa-plus"></i> add more</span>
      </div>
    </div>
  )
}