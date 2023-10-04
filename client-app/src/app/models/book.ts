import { Author } from "./author";
import { Format } from "./format";
import { Profile } from "./profile";
import { Publisher } from "./publisher";

export interface Book {
    bookId: string;
    title: string;
    isbn: string;
    image: string;
    purchaseDate: Date | null;
    noOfPages: number;
    yearOfPublishing: number;
    price: number;
    rate: number | 0;
    description: string;
    isRead: boolean;
    profile: Profile;
    publisher: Publisher;
    authors: Author[];
    format: Format;
}

export class Book implements Book {
    constructor(init?: BookFormValues) {
        Object.assign(this, init);
    }
}

export class BookFormValues {
    bookId?: string = undefined;
    title: string = "";
    isbn: string = "";
    image: string = "";
    purchaseDate: Date | null = null;
    noOfPages: number = 0;
    yearOfPublishing: number = 0;
    price: number = 0;
    rate: number | 0 = 0;
    description: string = "";
    isRead: boolean = false;

    constructor(book?: BookFormValues) {
        if(book) {
            this.bookId = book.bookId;
            this.title = book.title;
            this.isbn = book.isbn;
            this.image = book.image;
            this.purchaseDate = book.purchaseDate;
            this.noOfPages = book.noOfPages;
            this.yearOfPublishing = book.yearOfPublishing;;
            this.price = book.price;
            this.rate = book.rate;
            this.description = book.description;
            this.isRead = book.isRead;
        }
    }
}
