import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Book, BookFormValues } from "../models/book";

export default class BookStore {
    bookRegistry = new Map<string, Book>();
    selectedBook: Book | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;

    constructor() {
        makeAutoObservable(this)
    }

    loadBooks = async () => {
        this.setLoadingInitial(true);
        try {
            const books = await agent.Books.list();

            books.forEach(book => {
                console.log("set");
                this.setBook(book);
            });
            console.log("BOOOKS" + this.bookRegistry.size);
            this.setLoadingInitial(false);

        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);

        }
    }

    loadBook = async (id: string) => {
        let book = this.getBook(id);
        if(book) {
            this.selectedBook = book;
            return book;
        } 
        else {
            this.setLoadingInitial(true);
            try {
                book = await agent.Books.details(id);
                this.setBook(book);
                runInAction(() => this.selectedBook = book);
                this.setLoadingInitial(false);
                return book;
            } catch (error) {
                console.log(error);
                this.setLoadingInitial(false);
            }
        }

    }

    private getBook = (id: string) => {
        return this.bookRegistry.get(id);
    }

    private setBook = (book: Book) => {
        console.log(book.bookId);
        this.bookRegistry.set(book.bookId, book);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    createBook = async (book: BookFormValues) => {
        try {
            await agent.Books.create(book);
            const newBook = new Book(book);

            runInAction(() => {
                this.selectedBook = newBook;
            })
        } catch (error) {
            console.log(error);
        }
    }

    updateBook = async (book: BookFormValues) => {
        this.loading = true;
        try {
            await agent.Books.update(book);
            runInAction(() => {
                if(book.bookId) {
                    let updatedBook = {...this.getBook(book.bookId), ...book}
                    this.bookRegistry.set(book.bookId, updatedBook as Book);
                    this.selectedBook = updatedBook as Book;
                }

            })
        } catch (error) {
            console.log(error);
        }
    }

    deleteBook = async (id: string) => {
        this.loading = true;
        try {
            await agent.Books.delete(id);
            runInAction(() => {
                this.bookRegistry.delete(id);
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }
}

