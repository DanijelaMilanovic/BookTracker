import { observer } from 'mobx-react-lite';
import {Segment, Grid, Icon, Label, Header, Item} from 'semantic-ui-react'
import {Book} from "../../../app/models/book";

interface Props {
    book: Book
}

export default observer(function BookDetailedInfo({book}: Props) {
    return (
        <Segment.Group>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={16}>
                        <Header size='medium' style={{ border: 'none' }}>Description:</Header>
                        <Item.Description>
                            {book.description}
                        </Item.Description>
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
    )
})
