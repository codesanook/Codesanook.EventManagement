import { DatePicker as AntdDatePicker } from 'antd';
import moment from 'moment';

interface IProps {
  value: string;
  name: string;
  onChange?: Function;
  className?: string;
}

export default function DateTimePicker({ name, value = null, onChange = () => { }, className }: IProps) {

  var dateValue = value != null ? moment(value) : null;
  const onChangedHandler = (date: moment.Moment, dateString: string) => {
    onChange(date.toISOString())
  };

  return (
    <>
      <AntdDatePicker
        format='YYYY-MM-DD'
        name={name}
        className={className}
        value={dateValue}
        defaultValue={dateValue}
        onChange={onChangedHandler} />
    </>
  )
}