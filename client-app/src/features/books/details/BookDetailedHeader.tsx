import { format } from 'date-fns';
import { observer } from 'mobx-react-lite';
import { Header, Item, Segment, Image} from 'semantic-ui-react'
import {Book} from "../../../app/models/book";

const bookImageStyle = {
    filter: 'brightness(30%)'
};

const bookImageTextStyle = {
    position: 'absolute',
    bottom: '5%',
    left: '5%',
    width: '100%',
    height: 'auto',
    color: 'white'
};

interface Props {
    book: Book
}

export default observer (function BooketailedHeader(book: Props) {
    return (
        <Segment.Group>
            <Segment basic attached='top' style={{padding: '0'}}>
                <Image src={`/assets/categoryImages/food.jpg`} fluid style={bookImageStyle}/>
                <Segment style={bookImageTextStyle} basic>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header
                                    size='huge'
                                    content={book.book.title}
                                    style={{color: 'white'}}
                                />
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
        </Segment.Group>
    )
})