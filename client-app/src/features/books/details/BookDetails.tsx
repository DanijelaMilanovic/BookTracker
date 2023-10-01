import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import BookDetailedHeader from "./BookDetailedHeader";
import BookDetailedInfo from "./BookDetailedInfo";
import BookDetailedSidebar from "./BookDetailedSidebar";


export default observer(function BookDetails() {
  const {bookStore} = useStore();
  const {selectedBook: book, loadBook, loadingInitial} = bookStore;
  const {id} = useParams();

  useEffect(()=> {
    if(id) loadBook(id)
  }, [id, loadBook]);

  if(loadingInitial || !book) return <LoadingComponent/>;

  return (
    <Grid>
      <Grid.Column width={10}>
        <BookDetailedHeader book={book} />
        <BookDetailedInfo book={book}/>
      </Grid.Column>
      <Grid.Column width={6}>
        <BookDetailedSidebar book={book}/>
      </Grid.Column>
    </Grid>
  );
}) 
