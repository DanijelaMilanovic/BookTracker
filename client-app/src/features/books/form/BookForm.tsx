import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { Button, Header, Segment } from "semantic-ui-react";
import { BookFormValues} from "../../../app/models/book";
import { useStore } from "../../../app/stores/store";
import { Formik, Form } from "formik";
import * as Yup from 'yup';
import CustomTextInput from "../../../app/common/form/CustomTextInput";
import CustomTextArea from "../../../app/common/form/CustomTextArea";
import CustomSelectorInput from "../../../app/common/form/CustomSelectorInput";
import { publisherOptions } from "../../../app/common/options/publisherOptions";
import CustomDateInput from "../../../app/common/form/CustomDateInput";
import {v4 as uuid} from "uuid";
import CustomNumberInput from "../../../app/common/form/CustomNumberInput";
import { formatOptions } from "../../../app/common/options/formatOptions";


export default observer (function BookForm() {
    const {bookStore} = useStore(); 
    const { loadBook, createBook, updateBook} = bookStore;
    const navigate = useNavigate();

    const {id} = useParams();

    const [book, setBook] = useState<BookFormValues>( new BookFormValues());

    const validationSchema = Yup.object({
        title: Yup.string().required('The book title is required'),
        description: Yup.string().required('The book description is required'),
        format: Yup.string().required('Format is required'),
        noOfPages: Yup.string().required('Number of pages is required'),
        publishingYear: Yup.string().required('Publishing year is required')
    })

    useEffect(()=> {
        if(id) loadBook(id).then(book => setBook(new BookFormValues(book)))
    },[id, loadBook]);

    function handleFormSubmit(book: BookFormValues) {
        if(!book.bookId) {
            let newBook = {
                ...book,
                id: uuid()
            }
            createBook(newBook).then(()=> navigate(`/books/${book.bookId}`));
        } else {
            updateBook(book).then(()=> navigate(`/books/${book.bookId}`));
        }
    }

    return (
        <Segment clearing>
            <Header content='Book Details' sub color="teal"/>
            <Formik 
            validationSchema={validationSchema}
            enableReinitialize 
            initialValues={book} 
            onSubmit={values => handleFormSubmit(values)}>
                {({handleSubmit, isValid, isSubmitting, dirty})=> (
                    <Form className="ui form" onSubmit={handleSubmit} autoComplete='off'>
                    <CustomTextInput name="title" placeholder="Title"/>
                    <CustomTextInput name="isbn" placeholder="ISBN"/>
                    <CustomTextArea placeholder='Description'  name='description' rows={3}/>
                    <CustomNumberInput name="noOfPages" placeholder="Number Of Pages" type="number" />
                    <CustomNumberInput name="publishingYear" placeholder="Publishing Year" type="number" />
                    <CustomNumberInput name="price" placeholder="Price" type="number" />
                    <CustomSelectorInput options={publisherOptions} placeholder='Publisher' name='publlisher' />
                    <CustomSelectorInput options={formatOptions} placeholder='Format' name='format' />
                    <CustomDateInput
                        placeholderText='PurchaseDate' 
                        name='date'
                        dateFormat="MMMM d, yyyy" />
                    <Header content='Author details' sub color="teal"/>
                    <CustomTextInput placeholder='Forename' name='forename' />
                    <CustomTextInput placeholder='Surname' name='surename'/>
                    <CustomTextArea placeholder='Bio'  name='bio' rows={3}/>
                    <Button 
                        disabled={isSubmitting || !dirty || !isValid}
                        loading={isSubmitting} 
                        floated="right" 
                        positive 
                        type="submit" 
                        content='Submit'/>
                    <Button as={Link} to='/books' floated="right" type="button" content='Cancel'/>
                </Form>
                )}
            </Formik>
            
        </Segment>
    )
})