import { observer } from 'mobx-react-lite';
import { Header, Segment, Image, Grid, Label} from 'semantic-ui-react'
import {Book} from "../../../app/models/book";

interface Props {
    book: Book
}

export default observer (function BooketailedHeader({book}: Props) {
    return (
        <Segment.Group>
            <Grid style ={{backoundColor: 'white'}}>
                <Grid.Column width={6}>
                <Segment basic attached='top' style={{padding: '0', borde: 'none'}}>
                    <Image size='medium' src={`/assets/bookImages/${book.image}`}  />
                </Segment>
                </Grid.Column>
                <Grid.Column width={10}>
                <Segment basic attached='top' style={{padding: '0', borde: 'none'}}>
                <Segment textAlign='center'  style={{ border: 'none' }} attached='top' secondary inverted className='custom-header-color'> 
                    <Header size='huge'>
                        {book.title}
                    </Header>
                </Segment>    
                <Segment>                
                    {book.authors.map((author, index) => (
                        <Label basic className='custom-label-color' style={{ border: 'none' }} key={index} size='big'>
                            by {author.forename} {author.surename}
                        </Label>
                    ))}   <br 
                />    
                    <Label basic className='custom-label-color' style={{ border: 'none' }} size='medium'>{book.noOfPages} pages, {book.format.name}</Label> <br />
                    <Label basic className='custom-label-color' style={{ border: 'none' }} size='medium'>Published by {book.publisher.name}, {book.yearOfPublishing}</Label>    
                    </Segment>
                </Segment>
                </Grid.Column>
            </Grid>
        </Segment.Group>
    )
})