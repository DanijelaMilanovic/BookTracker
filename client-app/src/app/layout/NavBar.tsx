import { Link, NavLink } from 'react-router-dom';
import { Button, Container, Menu, MenuItem, Image, Dropdown } from 'semantic-ui-react';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';

export default observer ( function NavBar() {
    const {userStore: {user, logout}} = useStore();
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item as={NavLink} to='/' header>
                    <img src='/assets/book.png' alt='logo' style={{marginRight: '10px'}}/>
                    BookTracker
                </Menu.Item>
                <Menu.Item as={NavLink} to='/books' name='Books'/>
                <Menu.Item>
                    <Button as={NavLink} to='/createBook' className='custom-button-color' content = 'Add Book'/>
                </Menu.Item>
                <MenuItem position='right'>
                    <Image src={user?.image || '/assets/user.png'} avatar  spaced='right' />
                    <Dropdown pointing = 'top left' text={user?.username}>
                        <Dropdown.Menu>
                            <Dropdown.Item as={Link} to={`/profile/${user?.username}`} text="My profile" icon = 'user'></Dropdown.Item>
                            <Dropdown.Item onClick={logout}  text="Logout" icon = 'power'></Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </MenuItem>
            </Container>
        </Menu>
    )
    
})