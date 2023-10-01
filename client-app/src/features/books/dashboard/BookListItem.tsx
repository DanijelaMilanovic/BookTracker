import { Link } from "react-router-dom";
import { Item, Label, Rating, Segment } from "semantic-ui-react";
import { Book } from "../../../app/models/book";
import { useStore } from "../../../app/stores/store";

interface Props {
    book: Book
}

export default function BookListItem({book}: Props) {
    const {bookStore} = useStore();
    const {deleteBook} = bookStore;
    console.log(book.rate);
    /*
    const [target, setTarget] = useState('');

    function handleBookDelete(e: SyntheticEvent<HTMLButtonElement>, id:string) {
        setTarget(e.currentTarget.name);
        deleteBook(id);
    }
    */
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image style={{marginBottom: 3}} size="small" src={`/assets/bookImages/${book.image}`}/>
                        <Item.Content style={{width: "450px"}}>
                            <Item.Header as={Link} to={`/books/${book.bookId}`}>
                                {book.title}
                            </Item.Header>
                            <Item.Description>
                                <Label color="green">
                                    Sara J. Mass
                                </Label>
                            </Item.Description>
                        </Item.Content>
                        <Item.Extra style={{ textAlign: 'right' }}>
                            <Rating icon="star" size="huge" maxRating={5} style={{ marginLeft: 'auto' }} disabled defaultRating={book.rate}></Rating>
                            <Item.Description style={{ textAlign: 'right' }}>
                                <Label basic color={book.isRead ? "green" : "orange"} style={{ marginLeft: 'auto' }}>
                                    {book.isRead ? "Read" : "Unread"}
                                </Label>
                            </Item.Description>
                        </Item.Extra> <br />
                    </Item>
                </Item.Group>
            </Segment>
        </Segment.Group>
    )
}