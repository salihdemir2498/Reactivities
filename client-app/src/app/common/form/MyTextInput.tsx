import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";


interface Props {
    placeholder: string;
    name: string;
    label?: string;
    type?: string;
}

export default function MyTextInput(props: Props) {
    const [field, meta] = useField(props.name);
     // console.log(meta); //Form alanlarına hiç dokunmadan formu submit ettiğinizde meta.touched özelliğinin true olması, Formik kütüphanesinin varsayılan davranışıdır. Formik, formun gönderilmesiyle birlikte tüm form alanlarının dokunulduğunu (touched) kabul eder.
    return (
        // !! operatörü, bir değeri boolean (mantıksal) tipe dönüştürmek için kullanılan bir JavaScript işlemidir. İki adet ünlem işareti (!!) kullanarak bir değeri boolean tipe dönüştürmek, değerin varlık durumunu veya "truthiness"ini değil, sadece true veya false olarak değeri döndürür.
        //meta.error ifadesi ise form alanının hata durumunda ilgili hata mesajını içerir. Bu ifadenin değeri, form alanı dokunulduğunda ve hata mesajı mevcutsa true olur, aksi halde false olur.
        <Form.Field error={meta.touched && !!meta.error} >  
            <label>{props.label}</label>
            <input {...field} {...props} /> {/* field ve props objelerinin tüm özelliklerini <input> bileşenine aktarır. */}
            {meta.touched && meta.error ? (
                <Label basic color="red">{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}