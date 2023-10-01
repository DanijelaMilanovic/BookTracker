import { useEffect } from 'react'
import { observer } from "mobx-react-lite";
import { Grid, } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import BookList from "./BookList";
import LoadingComponent from '../../../app/layout/LoadingComponent';
import BookFilters from './BookFilters';


const BookDashboard = () => {

  const {bookStore} = useStore();
  const {loadBooks, bookRegistry} = bookStore;

  useEffect(() => {
     if(bookRegistry.size < 1) loadBooks();
  },[bookStore, bookRegistry.size]);

  if(bookStore.loadingInitial) return <LoadingComponent content="Loading books... "/>

  return (
    <Grid>
      <Grid.Column width="4">
        <BookFilters />
      </Grid.Column>
      <Grid.Column width="12">
        <BookList />
      </Grid.Column>
    </Grid>
  );
};

export default observer (BookDashboard);
