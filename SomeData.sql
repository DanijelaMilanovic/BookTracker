-- SQLite
INSERT into Format VALUES ("0390d402-1700-4995-86f4-6de7b79b1c6a","Paperback");
INSERT into Format VALUES ("3675d4d5-a6fb-4ec1-92dd-5309845e838c","Hardcover");

INSERT into Publisher VALUES ("ea713364-2c09-4b2b-821f-c8b4e7a35c5c", "Laguna", "Kralja Petra 45, Beograd");
INSERT into Publisher VALUES ("efc3a0cf-79cb-4611-8fbc-98c16f53b4c6", "Urban Reads", "Njegoševa 53, Beograd");
INSERT into Publisher VALUES ("f0b651a1-b419-4b5f-8092-774fdb130427", "Mikro knjiga", "Kneza Višeslava 34, Beograd");

INSERT INTO Genre VALUES ("61b43d36-836a-42fb-ac62-68f78e6a8c56", "Fiction");
INSERT INTO Genre VALUES ("0a13260d-e891-428e-9f01-ea30d525cb6b", "Romance");
INSERT INTO Genre VALUES ("7d776fe2-c874-4f43-a7b5-5063ccb27774", "Academic");

insert into Series values ("a5dd6260-2d0b-4226-9c32-c758e813417c","ACOTAR");

delete FROM Author
delete from AspNetUsers

insert into Book (BookId, ISBN, Title,Image, NoOfPages, YearOfPublishing, Price, Rate, IsRead, PublisherId, AppUserId, FormatId, PurshaseDate)
VALUES ("1ae9c17a-e3d1-4650-8cac-76fa5399027f","9788681856062", "Dvor trnja i ruža","dvor-trnja-i-ruza.png", 410, 2021, 20.00, 5.0, true,"efc3a0cf-79cb-4611-8fbc-98c16f53b4c6","c6878776-9e45-452a-874f-f8f078593c20","0390d402-1700-4995-86f4-6de7b79b1c6a", "1/01/2022");

insert into Book (BookId, ISBN, Title,Image, NoOfPages, YearOfPublishing, Price, Rate, IsRead, PublisherId, AppUserId, FormatId, PurshaseDate)
VALUES ("1ae9c17a-e3d1-1650-8cac-76fa5399027f","9788681856062", "Dvor magle i srdzbe","dvor-magle-i-srdzbe.png", 410, 2021, 20.00, 4.5, true,"efc3a0cf-79cb-4611-8fbc-98c16f53b4c6","c6878776-9e45-452a-874f-f8f078593c20","0390d402-1700-4995-86f4-6de7b79b1c6a", "1/01/2022");

insert into Book (BookId, ISBN, Title,Image, NoOfPages, YearOfPublishing, Price, Rate, IsRead, PublisherId, AppUserId, FormatId, PurshaseDate)
VALUES ("1ae9c17a-e3d1-1650-8cac-76fa5399020f","9788681856062", "Dvor srebrnih plamenova","dvor-srebrnih-plamenova.png", 410, 2021, 20.00, 0.0, false,"efc3a0cf-79cb-4611-8fbc-98c16f53b4c6","c6878776-9e45-452a-874f-f8f078593c20","0390d402-1700-4995-86f4-6de7b79b1c6a", "1/01/2022");

select Book.Title, Author.Forename from Author,Book JOIN BookAuthors on BookAuthors.AuthorId = Author.AuthorId and BookAuthors.BookId = Book.BookId and BookAuthors.AppUserId = Book.AppUserId;

UPDATE Book
SET IsRead = "false"
WHERE Book.BookId ="1ae9c17a-e3d1-1650-8cac-76fa5399020f";

DELETE from book
