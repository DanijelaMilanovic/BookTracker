export interface Publisher {
    id: string;
    name: string;
    address: string;
}

export class Publisher implements Publisher {
    constructor(init?: PublisherFormValues) {
        Object.assign(this, init);
    }
}

export class PublisherFormValues {
    id?: string = undefined;
    name: string = "";
    address: string = "";

    constructor(publisher?: PublisherFormValues) {
        if(publisher) {
            this.id = publisher.id;
            this.name = publisher.name;
            this.address = publisher.address;
        }
    }
}
