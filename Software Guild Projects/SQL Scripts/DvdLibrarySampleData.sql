use DVDDB
go

delete from DVD

set identity_insert DVD on

insert into DVD (DVDId, Title, Rating, Director, ReleaseYear)
values (1, 'Django Unchained', 'R', 'Quentin Tarentino', 2012),
	(2, 'Jumanji: Welcome to the Jungle', 'PG-13', 'Jake Kasdan', 2017),
	(3, 'Wall-E', 'PG', 'Andrew Stanton', 2008),
	(4, 'The Revenant', 'R', 'Alejandro Iñárritu', 2016),
	(5, 'Star Wars: The Force Awake and Bakens', 'R', 'JJ Dabrams', 2016)

set identity_insert DVD off