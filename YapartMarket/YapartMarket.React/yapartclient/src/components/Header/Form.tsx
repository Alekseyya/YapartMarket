import * as React from "react";

interface IFormProps {
    /* The http path that the form will be posted to */
    action: string;
}

export interface IValues {
    /* Key value pairs for all the field values with key being the field name */
    [key: string]: any;
}

export interface IErrors {
    /* The validation error messages for each field (key is the field name */
    [key: string]: string;
}

export interface IFormState {
    /* The field values */
    values: IValues;

    /* The field validation error messages */
    errors: IErrors;

    /* Whether the form has been successfully submitted */
    submitSuccess?: boolean;
}


export class Form extends React.Component<IFormProps, IFormState> {
    constructor(props: IFormProps) {
        super(props);
        const errors: IErrors = {};
        const values: IValues = {};
        this.state = {
            errors,
            values
        };
    }
}