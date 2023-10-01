import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { Book, BookFormValues } from "../models/book"
import { router } from "../router/Routes";
import { store } from "../stores/store";
import { User, UserFormValues } from "../models/user";

const sleep = (deley: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, deley)
    })
}

axios.defaults.baseURL = 'http://localhost:5161/api';

axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if(token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {

    return response;

}, (error: AxiosError) => {
    const { data, status, config } = error.response as AxiosResponse;
    console.log(data, status, config);
    switch (status) {
        case 400:
            if(config.method === 'get' && data.errors.hasOwnProperty('id')) {
                router.navigate('/not-found');
            }
            if(data.errors) {
                const modalStateErrors = [];
                for(const key in data.errors) {
                    if(data.errors[key]) {
                        modalStateErrors.push(data.errors[key]);
                    }
                }
                throw modalStateErrors.flat();
            } else {
                toast.error(data);
            }
            break;
        case 401:
            toast.error('unathorised');
            break;
        case 403:
            toast.error('forbiden');
            break;
        case 404:
            router.navigate('/not-found');
            break;
        case 500:
            store.commonStore.setServiceError(data);
            router.navigate('/service-error');
            break;
        default:
            break;
    }

    return Promise.reject(error);
});

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody)
}

const Books = {
    list: () => requests.get<Book[]>('/books'),
    details: (id: string) => requests.get<Book>(`/books/${id}`),
    create: (book: BookFormValues) => requests.post<void>('/books', book),
    update: (book: BookFormValues) => requests.put<void>(`/books/${book.bookId}`, book),
    delete: (id: string) => requests.del<void>(`/book/${id}`),
}

const Account = {
    current: () => requests.get<User>('/Account'),
    login: (user: UserFormValues) => requests.post<User>('/Account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/Account/register', user),
}

const agent = {
    Books,
    Account
}

export default agent;