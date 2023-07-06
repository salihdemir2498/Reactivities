import { useField } from "formik";
import DatePicker, {ReactDatePickerProps} from "react-datepicker";
import { Form, Label } from "semantic-ui-react";



export default function MyDateInput(props: Partial<ReactDatePickerProps>) {  //React'ta "partial" terimi, TypeScript ile birlikte kullanıldığında, bir nesnenin tüm özelliklerinin zorunlu (required) olmadığını belirtir. Yani, bir nesne tanımı içerisinde bazı özelliklerin opsiyonel olduğunu ifade etmek için kullanılır
    const [field, meta, helpers] = useField(props.name!); //Props arayüzünde name özelliğinin zorunlu (required) olduğunu belirtir. ! işareti, TypeScript'te null veya undefined olmadığını garanti eder
     
    return (
        <Form.Field error={meta.touched && !!meta.error} >  
            <DatePicker  
                {...field}
                {...props}
                selected={(field.value && new Date(field.value)) || null}
                onChange={value => helpers.setValue(value) }
            />
            {meta.touched && meta.error ? (
                <Label basic color="red">{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}