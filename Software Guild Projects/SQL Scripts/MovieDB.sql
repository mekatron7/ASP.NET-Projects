if exists(select * from sys.databases where name='DVDDB')
BEGIN
USE [master]

ALTER DATABASE DVDDB
SET OFFLINE WITH ROLLBACK IMMEDIATE


USE [master];

DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('DVDDB')

EXEC(@kill);


drop database DVDDB

END

create database DVDDB
go

use DVDDB
go

--DVD Table
if exists(select * from sys.tables where name='DVD')
	drop table DVD
go

create table DVD (
	DVDId int identity(1,1) primary key not null,
	Title varchar(100),
	Rating varchar(5) not null,
	Director varchar(50) not null,
	ReleaseYear int,
	Notes varchar(2000)
)

--Stored Procedures
if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDGetAll')
		drop procedure DVDGetAll
go

create procedure DVDGetAll
as
	select DVDId, Title, Rating, Director, ReleaseYear
	from DVD
	order by Title
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDAdd')
		drop procedure DVDAdd
go

create procedure DVDAdd (
	@DVDId int output,
	@Title varchar(100),
	@Rating varchar(5),
	@Director varchar(50),
	@ReleaseYear int,
	@Notes varchar(2000)
)
as
	insert into DVD (Rating, Director, Title, ReleaseYear, Notes)
	values(@Rating, @Director, @Title, @ReleaseYear, @Notes)

	set @DVDId = SCOPE_IDENTITY()
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDEdit')
		drop procedure DVDEdit
go

create procedure DVDEdit (
	@DVDId int output,
	@Title varchar(100),
	@Rating varchar(5),
	@Director varchar(50),
	@ReleaseYear int,
	@Notes varchar(2000)
)
as
	update DVD
		set Rating = @Rating,
		Director = @Director,
		Title = @Title,
		ReleaseYear = @ReleaseYear,
		Notes = @Notes
	where DVDId = @DVDId
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDRemove')
		drop procedure DVDRemove
go

create procedure DVDRemove (
	@DVDId int output
)
as
	delete from DVD
	where DVDId = @DVDId
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDSelectRating')
		drop procedure DVDSelectRating
go

create procedure DVDSelectRating (
	@Search varchar(5)
)
as
	select DVDId, Title, Rating, Director, ReleaseYear
	from DVD
	where Rating = @Search
	order by Title
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDSelectDirector')
		drop procedure DVDSelectDirector
go

create procedure DVDSelectDirector (
	@Search varchar(35)
)
as
	select DVDId, Title, Rating, Director, ReleaseYear
	from DVD
	where Director like '%' + @Search + '%'
	order by Title
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDSelectTitle')
		drop procedure DVDSelectTitle
go

create procedure DVDSelectTitle (
	@Search varchar(50)
)
as
	select DVDId, Title, Rating, Director, ReleaseYear
	from DVD
	where Title like '%' + @Search + '%'
	order by Title
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
	where routine_name = 'DVDSelectYear')
		drop procedure DVDSelectYear
go

create procedure DVDSelectYear (
	@Search int
)
as
	select DVDId, Title, Rating, Director, ReleaseYear
	from DVD
	where ReleaseYear = @Search
	order by Title
go

set identity_insert DVD on

insert into DVD (DVDId, Title, Rating, Director, ReleaseYear)
values (1, 'Django Unchained', 'R', 'Quentin Tarentino', 2012),
	(2, 'Jumanji: Welcome to the Jungle', 'PG-13', 'Jake Kasdan', 2017),
	(3, 'Wall-E', 'PG', 'Andrew Stanton', 2008),
	(4, 'The Revenant', 'R', 'Alejandro Iñárritu', 2016),
	(5, 'Star Wars: The Force Awake and Bakens', 'R', 'JJ Dabrams', 2016)

set identity_insert DVD off