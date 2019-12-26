
-- Switch to the system (aka master) database
USE master;
GO

-- Delete the DemoDB Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='ComicCollector')
DROP DATABASE ComicCollector;
GO

-- Create a new DemoDB Database
CREATE DATABASE ComicCollector;
GO

-- Switch to the DemoDB Database
USE ComicCollector
GO

BEGIN TRANSACTION;

CREATE TABLE users
(
	id			int			identity,
	username	varchar(50)	not null,
	password	varchar(50)	not null,
	salt		varchar(50)	not null,
	role		varchar(50)	default('UserS'),

	constraint pk_users primary key (id)
);

CREATE TABLE collections
(
	collection_id int identity not null,
	collection_name	varchar(50)	not null,
	id int not null,
	public_status bit default(1) not null,

	constraint pk_collections primary key (collection_id),
	constraint fk_collections foreign key (id) references users (id),
);


CREATE TABLE author
(
	author_id	int	identity not null,
	author_name varchar(50) not null,
	
	constraint pk_author primary key (author_id),
);

CREATE TABLE publisher
(
	publisher_id	int	identity not null,
	publisher_name varchar(50) not null,

	constraint pk_publisher primary key (publisher_id),
);

CREATE TABLE characters
(
	character_id	int	identity not null,
	character_name varchar(50) not null,
	publisher_id int not null,

	constraint pk_character primary key (character_id),
	constraint fk_publisher foreign key (publisher_id) references publisher (publisher_id),
);

CREATE TABLE comic
(
	comic_id	int	identity not null,
	title	varchar(50)	not null,
	author_id	int	not null,
	publisher_id	int not null,
	publish_date date,
	description varchar(250),

	constraint pk_comic primary key (comic_id),
	constraint fk_comic_author foreign key (author_id) references author (author_id),
	constraint fk_comic_publisher foreign key (publisher_id) references publisher (publisher_id),
);

CREATE TABLE comicsInCollection
(
	collection_id	int not null,
	comic_id int not null,

	constraint pk_comicsInCollection primary key (collection_id, comic_id),
	constraint fk_comicsInCollection_collections foreign key (collection_id) references collections (collection_id),
	constraint fk_comicsInCollection_comic foreign key (comic_id) references comic (comic_id),
);

CREATE TABLE charactersInComic
(
	character_id int not null,
	comic_id int not null,

	constraint pk_charactersInComic primary key (character_id, comic_id),
	constraint fk_charactersInComic_character foreign key (character_id) references characters (character_id),
	constraint fk_charactersInComic_comic foreign key (comic_id) references comic (comic_id),
);

INSERT INTO publisher (publisher_name) VALUES ('Image');
INSERT INTO publisher (publisher_name) VALUES ('Marvel');
INSERT INTO publisher (publisher_name) VALUES ('DC');
INSERT INTO characters (character_name, publisher_id) VALUES ('Spawn', 1);
INSERT INTO characters (character_name, publisher_id) VALUES ('Other person', 1);
INSERT INTO characters (character_name, publisher_id) VALUES ('Bystander', 1);
INSERT into characters (character_name, publisher_id) VALUES ('Spider-Man', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Hulk', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Wolverine', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Iron Man', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Captain America', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Black Widow', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Hawkeye', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Black Panther', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Doctor Strange', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Ant Man', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Thor', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Vision', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Scarlet Witch', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Falcon', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Winter Soldier', 2);
INSERT into characters (character_name, publisher_id) VALUES ('The Wasp', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Rocket Racoon', 2);
INSERT into characters (character_name, publisher_id) VALUES ('Batman', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Robin', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Nightwing', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Aquaman', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Aqualad', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Flash', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Kid Flash', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Wonder Woman', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Wonder Girl', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Superman', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Super Boy', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Crypto The Superdog', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Blue Beetle', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Cyborg', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Green Lantern', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Starfire', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Beast Boy', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Raven', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Green Arrow', 3);
INSERT into characters (character_name, publisher_id) VALUES ('Batgirl', 3);


INSERT INTO author (author_name) VALUES ('Todd McFarlane');
INSERT INTO author (author_name) VALUES ('Stan Lee');
INSERT INTO author (author_name) VALUES ('Chris Claremont');
INSERT INTO author (author_name) VALUES ('Nathan Edmondson');
INSERT INTO author (author_name) VALUES ('Matt Fraction');
INSERT INTO author (author_name) VALUES ('Christoper Priest');
INSERT INTO author (author_name) VALUES ('Peter B. Gills');
INSERT INTO author (author_name) VALUES ('Nick Spencer');


INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (1, 'Reality Bites', '11-1-2005', 1);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (1, 'Reality Bites, 2', '11-1-2006', 1);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (1, 'Reality Bites, 3', '11-1-2007', 1);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (1, 'Reality Bites, 4', '11-1-2008', 1);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (1, 'Reality Bites Less', '11-1-2002', 1);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (1, 'Reality Bites Less, 2', '11-1-2003', 1);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (1, 'Reality Bites Less, 3', '11-1-2004', 1);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #1', '03-1-1963', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #2', '05-1-1963', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #3', '07-1-1963', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #4', '09-1-1963', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #5', '10-1-1963', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #6', '11-1-1963', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #7', '12-1-1963', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #8', '01-1-1964', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #9', '02-1-1964', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Amazing Spider-Man #10', '03-1-1964', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #102', '04-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #103', '05-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #104', '06-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #105', '07-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #106', '08-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #107', '09-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #108', '10-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #109', '11-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #110', '12-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Incredible Hulk #111', '01-1-1969', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine # 0', '12-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine # 1', '12-15-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #2', '01-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #3', '02-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #4', '03-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #5', '04-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #6', '05-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #7', '06-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #8', '07-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (3, 'Wolverine #9', '08-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #1', '05-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #2', '06-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #3', '07-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #4', '08-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #5', '09-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #6', '10-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #7', '11-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #8', '12-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #9', '01-1-1969', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'The Invincible Iron Man #10', '02-1-1969', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #100', '04-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #101', '05-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #102', '06-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #103', '07-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #104', '08-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #105', '09-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #106', '10-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #107', '11-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #108', '12-1-1968', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (2, 'Captain America #109', '01-1-1969', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #1', '03-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #2', '03-15-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #3', '04-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #4', '05-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #5', '06-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #6', '07-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #7', '08-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #8', '09-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #9', '10-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (4, 'Black Widow #10', '11-1-2014', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #1', '10-1-2012', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #2', '11-1-2012', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #3', '12-1-2012', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #4', '01-1-2013', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #5', '02-1-2013', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #6', '02-15-2013', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #7', '03-1-2013', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #8', '04-1-2013', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #9', '06-1-2013', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (5, 'Hawkeye #10', '07-1-2013', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #1', '11-1-1998', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #2', '12-1-1998', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #3', '01-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #4', '02-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #5', '03-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #6', '03-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #7', '05-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #8', '06-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #9', '07-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (6, 'Black Panther #10', '08-1-1999', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #1', '11-1-1988', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #2', '12-1-1988', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #3', '03-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #4', '05-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #5', '07-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #6', '08-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #7', '09-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #8', '10-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #9', '11-1-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (7, 'Doctor Strange, Sorcerer Supreme #10', '11-15-1989', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #1', '12-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #2', '01-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #3', '02-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #4', '03-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #5', '04-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #6', '05-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #7', '06-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #8', '07-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #9', '08-1-2015', 2);
INSERT INTO comic (author_id, title, publish_date, publisher_id) VALUES (8, 'The Astonishing Ant-Man #10', '09-1-2015', 2);









INSERT INTO users (username, password, salt, role) VALUES ('testUser', '123abc', 'abc123', 'userS');
INSERT INTO collections (collection_name, id) VALUES ('Comics from High School', 1);
INSERT INTO collections (collection_name, id) VALUES ('Comics from Middle School', 1);
INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (1, 1);
INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (1, 2);
INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (1, 3);
INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (1, 4);
INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (2, 5);
INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (2, 6);
INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (2, 7);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (1, 1);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (1, 2);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (1, 3);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (1, 4);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (1, 5);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (1, 6);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (1, 7);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (2, 1);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (2, 2);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (2, 3);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (2, 4);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (2, 5);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (2, 6);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (2, 7);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (3, 1);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (3, 2);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (3, 3);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (3, 4);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (3, 5);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (3, 6);
INSERT INTO charactersInComic (character_id, comic_id) VALUES (3, 7);



COMMIT TRANSACTION;
