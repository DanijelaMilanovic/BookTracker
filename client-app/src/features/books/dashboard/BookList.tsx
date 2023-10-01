import { observer } from "mobx-react-lite";
import { useStore } from "../../../app/stores/store";
import BookListItem from "./BookListItem";

export default observer(function BookList() {
  const { bookStore } = useStore();
  const {bookRegistry} = bookStore;

  return (
    <>
      {Array.from(bookRegistry.values()).map((book) => {
        return <BookListItem key={book.bookId} book={book} />
      })}
    </>
  );
});
