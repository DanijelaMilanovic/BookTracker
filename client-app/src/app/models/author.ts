export interface Author {
    authorId: string;
    forename: string;
    surename: string;
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
    surename: string = "";
    bio: string = "";

    constructor(author?: AuthorFormValues) {
        if(author) {
            this.authorId = author.authorId;
            this.forename = author.forename;
            this.surename = author.surename;
            this.bio = author.bio;
        }
    }
}