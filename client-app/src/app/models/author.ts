export interface Author {
    authorId: string;
    forename: string;
    surname: string;
    bio: string;
}

export class Author implements Author {
    constructor(init?: AuthorFormValues) {
        Object.assign(this, init);
    }
}

export class AuthorFormValues {
    authorId?: string = undefined;
    forename: string = "";
    surname: string = "";
    bio: string = "";

    constructor(author?: AuthorFormValues) {
        if(author) {
            this.authorId = author.authorId;
            this.forename = author.forename;
            this.surname = author.surname;
            this.bio = author.bio;
        }
    }
}