import { Header, Item, Label, Rating, Segment } from 'semantic-ui-react'
import { observer } from 'mobx-react-lite'
import { Book } from '../../../app/models/book';

interface Props {
    book: Book;
}

export default observer(function BooketailedSidebar ({book}: Props) {
    return (
        <>
            <Segment textAlign='center' style={{ border: 'none' }} attached='top' secondary inverted className='custom-header-color'> 
                <Header>
                    Book Rating
                </Header>
            </Segment>
            <Segment>
                <Item style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <div>
                        <Rating icon="star" size="massive" maxRating={5} defaultRating={book.rate}></Rating>
                    </div>
                <Label basic color='teal' size='massive' style={{ border: 'none' }}>{book.rate}</Label>
            </Item>
            </Segment>
            <Segment textAlign='center'  style={{ border: 'none' }} attached='top' secondary inverted className='custom-header-color'> 
                <Header >
                    Status
                </Header>
            </Segment>
            <Segment>
                <Item>
                    <Label basic color="teal" style={{ border: 'none' }} size='huge'> {book.isRead ? "Read" : "Unread"} </Label>            
                </Item>
            </Segment>
            <Segment textAlign='center'  style={{ border: 'none' }} attached='top' secondary inverted className='custom-header-color'> 
                <Header>
                    Price
                </Header>
            </Segment>
            <Segment>
                <Item>
                    <Label basic color="teal" style={{ border: 'none' }} size='huge'> {book.price} KM </Label>            
                </Item>
            </Segment>
        </>
    )
})