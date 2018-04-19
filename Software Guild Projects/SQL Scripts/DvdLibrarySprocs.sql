use DVDDB
go

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