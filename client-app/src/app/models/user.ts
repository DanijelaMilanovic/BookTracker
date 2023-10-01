export interface User {
    username: string;
    forename: string;
    surename: string;
    token: string;
    image?: string;
}

export interface UserFormValues {
    username?: string;
    forename?: string;
    surename?: string;
    email: string;
    password: string;
}