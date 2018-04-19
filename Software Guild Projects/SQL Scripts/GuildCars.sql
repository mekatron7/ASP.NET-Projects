USE [master];

DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('GuildCars')

EXEC(@kill);

use master
go

if exists(select * from sys.databases where name='GuildCars')
drop database GuildCars
go

create database GuildCars
go

use GuildCars
go

--Tables
if exists(select * from sys.tables where name='Car')
	drop table Car
go

if exists(select * from sys.tables where name='Model')
	drop table Model
go

if exists(select * from sys.tables where name='Make')
	drop table Make
go

if exists(select * from sys.tables where name='InteriorColor')
	drop table InteriorColor
go

if exists(select * from sys.tables where name='ExteriorColor')
	drop table ExteriorColor
go

if exists(select * from sys.tables where name='BodyStyle')
	drop table BodyStyle
go

if exists(select * from sys.tables where name='Transmission')
	drop table Transmission
go

if exists(select * from sys.tables where name='Contact')
	drop table Contact
go

if exists(select * from sys.tables where name='Special')
	drop table Special
go

if exists(select * from sys.tables where name='Purchase')
	drop table Purchase
go

create table Make (
	MakeId int identity(1,1) primary key not null,
	MakeName varchar(25) not null
)

create table Model (
	ModelId int identity(1,1) primary key not null,
	ModelName varchar(25) not null,
	ModelEdition varchar(10) not null,
	MakeId int foreign key references Make(MakeId) not null
)

create table ExteriorColor (
	ExtColorId int identity(1,1) primary key not null,
	ExtColorName varchar(25) not null,
	ExtColorType varchar(10) not null,
)

create table InteriorColor (
	IntColorId int identity(1,1) primary key not null,
	IntColorName varchar(25) not null,
	IntColorType varchar(10) not null
)

create table BodyStyle (
	BSId int identity(1,1) primary key not null,
	BSName varchar(25) not null
)

create table Transmission (
	TransmissionId int identity(1,1) primary key not null,
	TransmissionType varchar(25) not null
)

create table Car (
	VIN# varchar(17) primary key not null,
	ModelId int foreign key references Model(ModelId) not null,
	ExtColorId int foreign key references ExteriorColor(ExtColorId) not null,
	IntColorId int foreign key references InteriorColor(IntColorId) not null,
	BSId int foreign key references BodyStyle(BSId) not null,
	TransmissionId int foreign key references Transmission(TransmissionId) not null,
	CarType varchar(5) not null,
	Purchased char(1) not null, --'Y' or 'N'
	Featured char(1) not null, --'Y' or 'N'
	CarYear int not null,
	Mileage int not null,
	MSRP decimal not null,
	SalePrice decimal not null,
	CarDescription varchar(1000) not null,
	CarPicture varchar(200), --A file name
	constraint CK_CarType check (CarType in ('New', 'Used')),
	constraint CK_Featured check (Featured in ('Y', 'N')),
	constraint CK_Purchased check (Purchased in ('Y', 'N'))
)

create table Contact (
	ContactId int identity(1,1) primary key not null,
	ContactDate datetime not null,
	ContactName varchar(50) not null,
	ContactEmail varchar(50) not null,
	ContactPhone varchar(12) not null,
	ContactMessage varchar(1000) not null,
	VIN# varchar(17) foreign key references Car(VIN#)
)

create table Special (
	SpecialId int identity(1,1) primary key not null,
	SpecialName varchar(50) not null,
	SpecialStartDate datetime not null,
	SpecialEndDate datetime not null,
	SpecialDescription varchar(500) not null
)

create table Purchase (
	PurchaseId int identity(1,1) primary key not null,
	VIN# varchar(17) foreign key references Car(VIN#) not null,
	PurchaseName varchar(50) not null,
	PurchasePhone varchar(12),
	PurchaseEmail varchar(50),
	Street1 varchar(50) not null,
	Street2 varchar(50),
	City varchar(35) not null,
	PState char(2) not null,
	ZipCode int not null,
	PurchasePrice decimal not null,
	PurchaseType varchar(15) not null,
	constraint CK_PurchaseType check (PurchaseType in ('Bank Finance', 'Cash', 'Dealer Finance'))
)

insert into Make
values ('Toyota'), --1
	('Honda'), --2
	('Nissan'), --3
	('Kia'), --4
	('Subaru'), --5
	('Mazda'), --6
	('Mitsubishi'), --7
	('Audi'), --8
	('Volvo'), --9
	('Volkswagen'), --10
	('Jaguar'), --11
	('Ford'), --12
	('Chevrolet'), --13
	('GMC'), --14
	('Dodge'),--15
	('Lincoln'), --16
	('Buick'), --17
	('Cadillac'), --18
	('Chrysler'), --19
	('Fiat'), --20
	('Lexus'), --21
	('Acura'), --22
	('Infiniti'), --23
	('Land Rover'), --24
	('Rolls Royce'), --25
	('Mercedes-Benz'), --26
	('BMW'), --27
	('Tesla'), --28
	('Porsche'), --29
	('Fererri'), --30
	('Lamborghini'), --31
	('Bugatti') --32

insert into Model
values('Camry', 'SE', 1), --1
	('Camry', 'XSE', 1), --2
	('Camry', 'XLE', 1), --3
	('Corolla', 'SE', 1), --4
	('Corolla', 'LE', 1), --5
	('Accord', 'LE', 2), --6
	('Accord', 'SE', 2), --7
	('Civic', 'SE', 2), --8
	('Civic', 'XLE', 2), --9
	('Altima', 'SE', 3), --10
	('Altima', 'LE', 3), --11
	('Maxima', 'SE', 3), --12
	('Titan', 'SE', 3), --13
	('Skyline', 'SE', 3), --14
	('Eclipse', 'XE', 7), --15
	('Focus', 'LE', 12) --16

insert into ExteriorColor
values('Black', 'Gloss'), --1
	('Black', 'Matte'), --2
	('White', 'Gloss'), --3
	('White', 'Matte'), --4
	('Gray', 'Gloss'), --5
	('Gray', 'Matte'), --6
	('Tan', 'Gloss'), --7
	('Tan', 'Matte'), --8
	('Red', 'Gloss'), --9
	('Red', 'Matte'), --10
	('Blue', 'Gloss'), --11
	('Blue', 'Matte'), --12
	('Yellow', 'Gloss'), --13
	('Orange', 'Gloss'), --14
	('Orange', 'Matte'), --15
	('Indigo', 'Gloss'), --16
	('Indigo', 'Matte'), --17
	('Slate', 'Gloss') --18

insert into InteriorColor
values('Black', 'Leather'), --1
	('Black', 'Suede'), --2
	('Black', 'Gucci'), --3
	('Tan', 'Leather'), --4
	('Tan', 'Cloth'), --5
	('Gray', 'Leather'), --6
	('Gray', 'Cloth'), --7
	('Dark Gray', 'Leather'), --8
	('Dark Gray', 'Suede'), --9
	('White', 'Leather') --10

insert into BodyStyle
values('Coupe'),
	('Sedan'),
	('SUV'),
	('Truck'),
	('Mini-Van'),
	('Droptop'),
	('Motorcycle')

insert into Transmission
values('Manual'),
	('Automatic 2WD'),
	('Automatic 4WD')

insert into Car --VIN, Model, Int Color, Ext Color, BS, Trans, Car Type, Feat, Year, Mileage, MSRP, Sale Price, Car Descrip, Car Picture
values('19UUA5668Y1061854', 2, 3, 3, 2, 2, 'New', 'Y', 'Y', 2017, 200, 28095, 25000, 'The car to get if you''re looking to turn heads and achieve instant baller status as a young professional.', 'Photos/New/Toyota/Camry.guccicamry.jpg'),
	('1FAHP35N78W149542', 7, 1, 9, 2, 2, 'Used', 'Y', 'N', 2015, 14500, 18500, 16800, 'Looks as if nobody has ever even driven it before! Complete with power sunroof and Bluetooth sound system.', 'Photos/Used/Honda/Accord.accord2015black.jpg'),
	('2HHFD557090014986', 13, 18, 10, 4, 3, 'New', 'Y', 'Y', 2016, 17600, 20200, 18500, 'Tired of that same ol F-150 or Silverado? Grab yourself a Titan.', 'Photos/Used/Nissan/Titan.titansgo.jpg'),
	('JH4DC54863S002994', 16, 15, 5, 1, 1, 'Used', 'N', 'N', 1997, 130600, 5000, 2800, 'Hey parents, looking to surprise your high school son or daughter with their first car? Check this one out!', 'Photos/Used/Ford/Focus.poscar.jpg')

insert into Contact
values('3-29-2018', 'Chris Chrisserton', 'fchrischrisserton@gmail.com', '257-631-3882', 'Hey I was wondering if I could come check out one of your 2014 Honda Civics during your Civic Duty event. I''m considering buying one with a full cash payment.', null),
	('4-03-2018', 'Jack Awpherman', 'jackawph@gmail.com', '255-121-1256', 'I want to come in to the dealership on Monday and take a look at your 2018 Honda Accord SE and possibly go for a test drive. Are there any appointments available for that day? Also, does this car come in manual? I''m all about that stick shift ya know.', null),
	('4-05-2018', 'Hanzo Jefferies', 'hjefferies@gmail.com', '408-404-9473', 'It would be of the highest honor if I could secure a 2018 Toyota Corolla during yor Get Ready for a Shock sales event.', null)

insert into Special
values('Give a Truck Sales Event', '04-01-2018', '04-20-2018', '$1500 Cash Allowance on any new truck in our inventory. And with any purchase new or used, you have a chance to win your choice of a brand new 2018 Ford F-150, Chevy Silverado, Toyota Tundra, or Honda Ridgeline.'),
	('Civic Duty Sales Weekend', '04-28-2018', '04-30-2018', '$1000 Cash Allowance on any new Honda Civic and 8% off any used Honda Civic.'),
	('A-Ford-able Sales Event', '05-01-2018', '05-07-2018', '$12% off any used Ford with a listed price between $6000 and $12,000.'),
	('Get Redy for a Shock Sales Event', '05-10-2018', '05-20-2018', 'Oi mate, it''s me, Junkrat. Instead of ripping tires, I''m gonna be ripping prices left and right! Get ready for a shock when you see the type of prices we slapped onto these cars. Check out our offerings and drive off with a new whip for cheap today!'),
	('Let''s Break It Down Sales Event', '06-14-2018', '06-22-2018', 'Give yourself to the rhythm of unbeatable prices on all new and used Cadillac and Chrysler vehicles. $4000 Cash Allowance on new models and 12% off any used model is a tune anyone can vibe to.')

insert into Purchase --Car, Name, Phone, Email, St1, St2, City, State, Zip, Price, Type
values('1FAHP35N78W149542', 'Gill Bates', '545-834-1200', 'gillbates@gmail.com', '7742 Windows Ave', 'Apt 10', 'Redmond', 'WA', 23118, 16400, 'Cash'),
	('19UUA5668Y1061854', 'Kimberly Jawngoone', '744-155-8323', 'kjawngoone@gmail.com', '1313 N Kia Blvd', null, 'Sacramento', 'CA', 90211, 24400, 'Bank Finance'),
	('19UUA5668Y1061854', 'Dean Southman', null, 'deepsouthman123@gmail.com', '230 Buckeroo Stallion Rd', null, 'Dallas', 'TX', 68777, 18500, 'Dealer Finance')