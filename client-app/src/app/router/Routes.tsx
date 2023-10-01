import { RouteObject, createBrowserRouter, Navigate } from "react-router-dom";
import BookDashboard from "../../features/books/dashboard/BookDashboard";
import BookDetails from "../../features/books/details/BookDetails";
import BookForm from "../../features/books/form/BookForm";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import App from "../layout/App";
import LoginForm from "../../features/users/LoginForm";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            {path:'books', element: <BookDashboard />},
            {path:'books/:id', element: <BookDetails />},
            {path:'createBook', element: <BookForm key='create'/>},           
            {path:'manage/:id', element: <BookForm key='manage'/>},
            {path:'login', element: <LoginForm />},
            {path:'not-found', element: <NotFound />},
            {path:'service-error', element: <ServerError/>},
            {path:'*', element: <Navigate replace to='/not-found'/>}
        ]
    }
]
export const router = createBrowserRouter(routes); 