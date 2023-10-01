import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Container, Header, Segment,Image, Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

export default observer( function HomePage() {
    const {userStore, modalStore} = useStore();
    return (
        <Segment inverted textAlign="center" vertical className="masthead">
            <Container text className="fancy-border">
                <Header as='h1' inverted>
                    <Image size="massive" src='/assets/book.png' alt='logo' style={{marginBottom: 12}}/>
                    BookTracker
                </Header>
                {userStore.isLogedIn ? (
                    <>
                    <Header as='h2' inverted content='Welcome to BookTracker'/>
                    <Button as={Link} to='/books' size="huge" inverted>
                        Go to My Books
                    </Button>
                    </>
                ) : (
                    <>
                        <Button onClick={()=> modalStore.openModal(<LoginForm />)} size="huge" inverted>
                            Login
                        </Button>
                        <Button onClick={()=> modalStore.openModal(<RegisterForm />)} size="huge" inverted>
                            Register
                        </Button>
                    </>
                    
                )}
                
            </Container>
        </Segment>
    )
})