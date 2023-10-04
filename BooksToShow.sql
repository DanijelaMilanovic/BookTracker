-- SQLite
INSERT into Format VALUES ("15daccf3-30a9-4943-8d24-ec39aa675493","Paperback");
INSERT into Format VALUES ("437a701e-f1bc-4140-8488-62930ae4c7e1","Hardcover");

INSERT into Publisher VALUES ("ae297139-b0a9-4d67-b35c-67f1658e3d49", "Laguna", "Kralja Petra 45, Beograd");
INSERT into Publisher VALUES ("5055016b-de69-4bc5-95b6-2e5dce36e713", "CET Computer Equipment and Trade", "Skadarska 45, Beograd");

INSERT INTO Author VALUES ("d7daeb12-eaf4-409e-bcda-1f8234726ebf", "Martin", "Fowler","");
INSERT INTO Author VALUES ("1d89a914-4b41-43aa-8644-d91b98bc2c6d", "Oscar", "Wilde", "");

insert into Book (BookId, ISBN, Title,Image, NoOfPages, YearOfPublishing, Price, Rate, IsRead, PublisherId, AppUserId, FormatId, PurshaseDate)
VALUES ("96c4bb7b-6329-4a23-80d5-f2e699183ebf","9788679914316", "Refaktorisanje: Poboljšanje dizajna postojećeg koda","ref.png", 417, 2020, 21.50, 5.0, true,"5055016b-de69-4bc5-95b6-2e5dce36e713","5226ac9e-4be1-4806-bee8-32692be5eff6","15daccf3-30a9-4943-8d24-ec39aa675493", "1/01/2023");

insert into Book (BookId, ISBN, Title,Image, NoOfPages, YearOfPublishing, Price, Rate, IsRead, PublisherId, AppUserId, FormatId, PurshaseDate)
VALUES ("4a7403a1-6ba5-4dc1-a3fe-a8c41abbff29","9788652133598", "Slika Dorijana Greja","sdg.png", 294, 2019, 15.70, 0.0, false,"ae297139-b0a9-4d67-b35c-67f1658e3d49","5226ac9e-4be1-4806-bee8-32692be5eff6","437a701e-f1bc-4140-8488-62930ae4c7e1", "1/01/2023");

insert into BookAuthors values ("96c4bb7b-6329-4a23-80d5-f2e699183ebf","5226ac9e-4be1-4806-bee8-32692be5eff6","d7daeb12-eaf4-409e-bcda-1f8234726ebf");
insert into BookAuthors values ("4a7403a1-6ba5-4dc1-a3fe-a8c41abbff29","5226ac9e-4be1-4806-bee8-32692be5eff6","1d89a914-4b41-43aa-8644-d91b98bc2c6d");

delete from Book;
delete from Format;
delete from Publisher;
delete from AspNetUsers;
delete from Author;
delete from BookAuthors;

update book 
set description = "Već više od dvadeset godina iskusni programeri širom sveta se oslanjaju u svom radu na Refaktorisanje, knjigu koju je napisao Martin Fowler, kako bi unapredili dizajn postojećeg koda i na taj način ujedno unapredili i održavanje softvera, a sve u cilju da njihov postojeći kôd bude lakši za razumevanje.
Novo izdanje knjige, koje se čekalo sa nestrpljenjem, potpuno je ažurirano u cilju prikazivanja ključnih promena na polju programiranja. Knjiga Refaktorisanje, Drugo izdanje sadrži ažuriran katalog refaktorisanja i uključuje primere koda u JavaScript-u, kao i nove primere sa funkcijama koji prikazuju refaktorisanje bez klasa.
Poput prethodnog izdanja, ova knjiga objašnjava šta je refaktorisanje; zašto je potrebno refaktorisati; kako prepoznati kôd kojem je potrebno refaktorisanje; kako da uspešno primenite refaktorisanje bez obzira na to koji programski jezik koristite.
Razumevanje procesa i opštih principa refaktorisanja;
Brzo primenjivanje korisnih refaktorisanja kako bi bilo lakše razumeti i izmeniti program;
Prepoznavanje mesta u kodu koja „zaudaraju” i koja nam zapravo daju signal da je potrebno uraditi refaktorisanje;
Istraživanje refaktorisanja za koja su data objašnjenja, motivacija, mehanizam, kao i jednostavni primeri;
Kreiranje testova za vaša refaktorisanja;
Prepoznavanje kompromisa i prepreka u vezi sa refaktorisanjem;"
WHERE Book.BookId ="96c4bb7b-6329-4a23-80d5-f2e699183ebf";