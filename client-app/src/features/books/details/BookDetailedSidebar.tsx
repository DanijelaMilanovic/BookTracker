import { Segment } from 'semantic-ui-react'
import { observer } from 'mobx-react-lite'
import { Book } from '../../../app/models/book';

interface Props {
    book: Book;
}

export default observer(function BooketailedSidebar (book: Props) {
    return (
        <>
            <Segment
                textAlign='center'
                style={{ border: 'none' }}
                attached='top'
                secondary
                inverted
                color='teal'
            >
            </Segment>
        </>

    )
})