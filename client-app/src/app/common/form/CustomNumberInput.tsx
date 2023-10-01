import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";

interface Props {
    placeholder: string;
    name: string;
    label?: string;
    type?: string;
}

export default function CustomNumberInput(props: Props) {
    const [field, meta, helpers] = useField(props.name);

    const formattedValue = field.value === 0 ? "" : field.value;

    return (
        <Form.Field error={meta.touched && !!meta.error}>
            <label>{props.label}</label>
            <input
                {...field}
                {...props}
                value={formattedValue}
                placeholder={props.type === "number" ? props.placeholder : ""}
                onChange={(e) => {
                    const newValue = e.target.value === "" ? 0 : e.target.value;
                    helpers.setValue(newValue);
                }}
            />
            {meta.touched && meta.error ? (
                <Label basic color="red">{meta.error}</Label>
            ) : null}
        </Form.Field>
    );
}
