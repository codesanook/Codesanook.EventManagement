import { DatePicker as AntdDatePicker } from 'antd';

import moment, { utc } from 'moment';
import { useState } from 'react';

interface IProps {
  value: string;
  name: string;
}

export default function DateTimePicker({ name, value = null }: IProps) {

  //var str = '2011-04-11T10:20:30.000Z';
  const [inputValue, setInputValue] = useState<string>(value)
  var dateValue = value != null ? moment(value) : null;

  const onChangedHandler = (date: moment.Moment, dateString: string) => {
    // set to ISO date string
    setInputValue(date.toISOString());
  };

  return (
    <>
      {/* https://medium.com/@duhseekoh/react-performance-expression-vs-string-props-351f3b6737f9 */}
      <input type={'hidden'} name={name} value={inputValue} />
      <AntdDatePicker
        format='YYYY-MM-DD HH:mm'
        defaultValue={dateValue}
        showTime={{ minuteStep: 5, defaultValue: moment('08:00', 'HH:mm') }}
        onChange={onChangedHandler} />
    </>
  )
}