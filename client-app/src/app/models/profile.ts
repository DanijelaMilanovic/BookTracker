import { User } from "./user";

export interface Profile {
    username: string;
    forename: string;
    surename: string;
    image?: string;
}

export class Profile implements Profile {
    constructor(user: User) {
        this.username = user.username;
        this.forename = user.forename;
        this.surename = user.surename;
        this.image = user.image;
    }
}