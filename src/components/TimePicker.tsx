import { TimePicker as AntdTimePicker } from 'antd';
import moment from 'moment';

interface IProps {
  value: string;
  name: string;
  minuteStep?: number;
  onChange?: Function;
  className?: string;
}

export default function TimePicker({ minuteStep = 15, name, value = null, onChange = () => { }, className }: IProps) {

  const dateValue = value != null ? moment(value) : null;
  const onChangedHandler = (date: moment.Moment, dateString: string) => {
    onChange(date.toISOString())
  };

  return (
    <>
      <AntdTimePicker minuteStep={minuteStep} className={className} format="HH:mm" name={name} value={dateValue} onChange={onChangedHandler} defaultValue={moment('08:00', 'HH:mm')} />
    </>
  )
}