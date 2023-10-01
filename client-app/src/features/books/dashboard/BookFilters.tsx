import { Header, Menu } from "semantic-ui-react";

export default function BookFilters() {
  return (
    <>
      <Menu vertical size="large" style={{ width: "100%" }}>
        <Header icon="bookmark outline" color="teal" content="My BookShelfs" attached/>
        <Menu.Item content="My Books"/>
        <Menu.Item content="To Be Read" />
        <Menu.Item content="Loaned" />
        <Menu.Item content="Favourites" />
      </Menu>
      <Header />
    </>
  );
}
