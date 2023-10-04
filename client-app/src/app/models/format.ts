export interface Format {
    id: string;
    name: string;
}

export class Format implements Format {
    constructor(init?: FormatFormValues) {
        Object.assign(this, init);
    }
}

export class FormatFormValues {
    id?: string = undefined;
    name: string = "";

    constructor(publisher?: FormatFormValues) {
        if(publisher) {
            this.id = publisher.id;
            this.name = publisher.name;
        }
    }
}
