USE master
GO

DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('HotelReservations')

EXEC(@kill);

if exists (select * from sysdatabases where name='HotelReservations')
		drop database HotelReservations
go

create database HotelReservations
go

use HotelReservations

create table RoomType(
	RoomTypeId int primary key identity(1,1),
	RoomTypeName varchar(25) not null,
)

create table RoomRate(
	RoomRateId int primary key identity(1,1),
	RateName varchar(50) not null,
	Rate money not null,
	RateStartDate datetime not null,
	RateEndDate datetime not null,
	RoomTypeId int foreign key references RoomType(RoomTypeId) 
)


create table Room(
	RoomNumber int primary key,
	FloorNumber int not null,
	NumberOfBeds int not null,
	PullOutBed char(1) not null,
	RoomRateId int foreign key references RoomRate(RoomRateId) ,
	constraint CK_PullOutBed check (PullOutBed in ('Y', 'N')) 
)

create table Amenity(
	AmenityId int primary key identity(1,1),
	AmenityName varchar(25) not null,
	AmenityPrice money not null
)

create table RoomAmenities(
	RoomNumber int foreign key references Room(RoomNumber)not null,
	AmenityId int foreign key references Amenity(AmenityId) not null
)

create table HomeAddress(
	AddressId int primary key identity(1,1),
	Street varchar(50) not null,
	City varchar(25) not null,
	StateName char(2) not null,
	zip varchar(10) not null
)

create table Customer(
	CustomerId int primary key identity(1,1),
	FirstName varchar(25) not null,
	LastName varchar(25) not null,
	DOB datetime not null,
	Gender char(1),
	Email varchar(50) not null,
	AddressId int foreign key references HomeAddress(AddressId) not null,
	constraint CK_Gender check (gender in ('M', 'F', 'O')) --Male, Female, Other (cuz we're so progressive)
)

create table Phone(
	PhoneId int primary key identity(1,1),
	PhoneNumber varchar(12) not null,
	PhoneType char(1) not null,
	CustomerId int foreign key references Customer(CustomerId) not null,
	constraint CK_PhoneType check (PhoneType in ('H', 'M', 'W')) --Home, Mobile, Work
)

create table PromoCode(
	PromoCode varchar(8) primary key,
	PromoDescripton varchar(100),
	DollarValueOff money, 
	PercentOff decimal(5,2),
	StartDate datetime not null,
	EndDate datetime not null
)

create table Reservation(
	ReservationId int primary key identity(1,1),
	BookDate datetime not null,
	CheckinDate datetime not null,
	CheckoutDate datetime not null,
	CheckinTime time not null,
	CheckOutTime time not null,
	PromoCode varchar(8) foreign key references PromoCode(PromoCode)
)

create table CustomerReservation(
	CustomerID int foreign key references Customer(CustomerId) not null,
	ReservationID int foreign key references Reservation(ReservationId) not null
)

create table RoomReservation(
	RoomNumber int foreign key references Room(RoomNumber) not null,
	ReservationId int foreign key references Reservation(ReservationId) not null,
	ReservationRate money not null
)

create table AddOn(
	AddOnId int primary key identity(1,1),
	AddOnName varchar(25) not null,
)

create table Bill(
	BillId int primary key identity(1,1),
	BillDate datetime not null,
	BillTax money not null,
	BillTotal money not null,
	Paid char(1) not null,
	ReservationId int foreign key references Reservation(ReservationId),
	constraint CK_Paid check (Paid in ('Y', 'N')) 
)

create table AddOnRates(
	AddOnRateId int primary key identity(1,1),
	AddOnRateName varchar(35) not null,
	AddOnRate money not null,
	AddOnRateStartDate datetime not null,
	AddOnRateEndDate datetime not null,
	AddOnId int foreign key references AddOn(AddOnId)
)

create table BillDetails(
	BillDetailsId int primary key identity(1,1),
	AddOnRatesId int foreign key references AddOnRates(AddOnRateId),
	DetailDescription varchar(100) not null,
	Charge money not null,
	BillId int foreign key references Bill(BillId)
)

insert into HomeAddress values('1 Kame House', 'Kame Island', 'FL', '19436')
insert into HomeAddress values('5585 Hollywood Blvd', 'Hollywood', 'CA', '90210')
insert into HomeAddress values('548 Trap House Ln', 'Atlanta', 'GA', '22316')
insert into HomeAddress values('420 N Hale Ln', 'Los Angeles', 'CA', '90420')

insert into Customer values('Goku', 'Jackson', '6-16-1994', 'M', 'kamehameha69@zmail.com', 1)
insert into Customer values('Chris', 'Chrisserton', '8-25-1974', 'M', 'fchrischrisserton@gmail.com', 2)
insert into Customer values('Cardi', 'Bish', '2-14-1985', 'F', 'playgurlcardi@gmail.com', 3)
insert into Customer values('Chichi', 'Jackson', '3-4-1996', 'F', 'chichis@yahoo.com', 1)
insert into Customer values('Wiz', 'Khalifa', '8-8-1991', 'M', 'taylorgang4life@gmail.com', 4)

insert into Amenity values('Fridge', 5.00)
insert into Amenity values('Jacuzzi', 20.00)
insert into Amenity values('4K TV', 20.00)
insert into Amenity values('Full Kitchen', 25.00)

insert into AddOn values('Movie')
insert into AddOn values('Netflix')
insert into AddOn values('Hulu')
insert into AddOn values('Room Service')
insert into AddOn values('Food Delivery')
insert into AddOn values('Alcohol Delivery')
insert into AddOn values('Green Delivery')

insert into PromoCode values('GANJA420', '20% off Green Delivery and 10% off hotel room', null, .20, '04-20-2018', '04-30-2018')
insert into PromoCode values('RF4GD86S', '$20 off hotel room per night', 20, null, '4-01-2018', '4-30-2018')
insert into PromoCode values('DBZZZZZZ', '15% off hotel room for saving the world multiple times', null, .15, '1-01-2018', '12-31-2018')
insert into PromoCode values('REGULAR1', '$10 off hotel room per night', 10, null, '1-01-2018', '12-31-2018')

insert into Reservation values('3-24-2018', '4-20-18', '4-27-18', '9:00AM', '4:20PM', 'GANJA420')
insert into Reservation values('4-12-2018', '5-15-18', '5-19-18', '3:00PM', '12:00PM', null)
insert into Reservation values('3-29-2018', '4-21-18', '4-28-18', '10:00AM', '12:00PM', 'RF4GD86S')
insert into Reservation values('3-14-2018', '4-19-18', '4-25-18', '9:00AM', '12:00PM', 'DBZZZZZZ')

insert into CustomerReservation values(1, 4)
insert into CustomerReservation values(2, 2)
insert into CustomerReservation values(3, 3)
insert into CustomerReservation values(4, 4)
insert into CustomerReservation values(5, 1)

insert into Phone values('219-774-1297', 'M', 1)
insert into Phone values('884-555-8870', 'M', 2)
insert into Phone values('385-619-4433', 'M', 3)
insert into Phone values('219-774-1296', 'H', 4)
insert into Phone values('420-225-6116', 'M', 5)

insert into Bill values('4-27-2018', 42.96, 2784.43, 'N', 1)
insert into Bill values('4-12-2018', 12.22, 495.36, 'Y', 2)
insert into Bill values('3-29-2018', 32.60, 1069.69, 'Y', 3)
insert into Bill values('4-25-2018', 22.01, 786.68, 'Y', 4)

insert into AddOnRates values('Regular Movie', 4.99, '2-14-2018','2-28-2018', 1)
insert into AddOnRates values('Adult XXX Movie', 14.99, '2-14-2018','2-28-2018', 1)
insert into AddOnRates values('Netflix Standard', 4.99, '2-14-2018','2-28-2018', 2)
insert into AddOnRates values('Netflix 4K', 6.99, '2-14-2018','2-28-2018', 2)
insert into AddOnRates values('Hulu No Ads', 5.99, '2-14-2018','2-28-2018', 3)
insert into AddOnRates values('Room Service Standard', 4.99, '2-14-2018','2-28-2018', 4)
insert into AddOnRates values('Premium Room Service', 8.99, '2-14-2018','2-28-2018', 4)
insert into AddOnRates values('Food Delivery Standard', 4.99, '2-14-2018','2-28-2018', 5)
insert into AddOnRates values('High Class Food Delivery', 7.99, '2-14-2018','2-28-2018', 5)
insert into AddOnRates values('Top Shelf Alcohol Delivery', 10.99, '2-14-2018','2-28-2018', 6)
insert into AddOnRates values('Green Delivery Skunk', 8.99, '2-14-2018','2-28-2018', 7)
insert into AddOnRates values('Dank Green Delivery', 19.99, '2-14-2018','2-28-2018', 7)

insert into BillDetails values(7, 'Premium Room Service - 7 days', 120.00, 1)
insert into BillDetails values(4, 'Netflix 4K - 7 days', 35.00, 1)
insert into BillDetails values(5, 'Hulu No Ads - 7 Days', 35.00, 1)
insert into BillDetails values(9, 'High Class Food Delivery - 7 days', 175.00, 1)
insert into BillDetails values(10, 'Top Shelf Alcohol Delivery - 7 days', 140.00, 1)
insert into BillDetails values(12, 'Dank Green Delivery - 7 days - Discount code GANJA420 applied', 220.00, 1)
insert into BillDetails values(6, 'Standard Room Service - 4 days', 20.00, 2)
insert into BillDetails values(2, 'Movie - Lonely Friday Night Adult Package', 28.00, 2)
insert into BillDetails values(7, 'Premium Room Service - 7 days', 120.00, 3)
insert into BillDetails values(10, 'Top Shelf Alcohol Delivery - 7 days', 140.00, 3)
insert into BillDetails values(4, 'Netflix 4K - 7 days', 35.00, 3)
insert into BillDetails values(9, 'High Class Food Delivery - 6 days', 90.00, 4)

insert into RoomType
values('Full Bed Standard'),
	('Full Bed Plus'),
	('Queen Bed Standard'),
	('King Bed Standard'),
	('Cali King Bed Premium'),
	('King Bed Plus'),
	('Cali King Plus')

insert into RoomRate values('Full Bed Standard', 90, '2-14-2018','2-28-2018', 1)
insert into RoomRate values('Full Bed Plus', 120, '2-14-2018','2-28-2018', 2)
insert into RoomRate values('Queen Bed Standard', 140, '2-14-2018','2-28-2018', 3)
insert into RoomRate values('Queen Bed Standard - Holiday Season', 160, '2-14-2018','2-28-2018', 3)
insert into RoomRate values('King Bed Standard', 150, '2-14-2018','2-28-2018', 4)
insert into RoomRate values('Cali King Bed Premium', 220, '2-14-2018','2-28-2018', 5)
insert into RoomRate values('Cali King Bed Premium - Group Rate', 190, '2-14-2018','2-28-2018', 5)
insert into RoomRate values('King Bed Plus', 170, '2-14-2018','2-28-2018', 6)
insert into RoomRate values('Cali King Plus', 200, '2-14-2018','2-28-2018', 7)
insert into RoomRate values('Cali King Plus - Spring Break', 250, '2-14-2018','2-28-2018', 7)

insert into Room values(101, 1, 1, 'N', 1)
insert into Room values(124, 1, 3, 'Y', 2)
insert into Room values(225, 2, 2, 'Y', 4)
insert into Room values(218, 2, 2, 'Y', 5)
insert into Room values(420, 4, 3, 'Y', 7)
insert into Room values(541, 5, 3, 'Y', 8)
insert into Room values(725, 7, 2, 'Y', 10)

insert into RoomAmenities values(101, 1)
insert into RoomAmenities values(124, 1)
insert into RoomAmenities values(225, 1)
insert into RoomAmenities values(218, 1)
insert into RoomAmenities values(218, 2)
insert into RoomAmenities values(420, 1)
insert into RoomAmenities values(420, 2)
insert into RoomAmenities values(420, 3)
insert into RoomAmenities values(420, 4)
insert into RoomAmenities values(541, 1)
insert into RoomAmenities values(541, 2)
insert into RoomAmenities values(541, 3)
insert into RoomAmenities values(725, 3)
insert into RoomAmenities values(725, 4)

insert into RoomReservation values(420, 1, 1250)
insert into RoomReservation values(124, 2, 480)
insert into RoomReservation values(725, 3, 1400)
insert into RoomReservation values(218, 4, 750)

select FirstName, LastName, DOB, DetailDescription, Charge, BillTotal from Customer
inner join CustomerReservation on Customer.CustomerId = CustomerReservation.CustomerId
inner join Reservation on CustomerReservation.ReservationId = Reservation.ReservationId
inner join Bill on Reservation.ReservationId = Bill.ReservationId
inner join BillDetails on Bill.BillId = BillDetails.BillId
where DetailDescription like '%Adult%'

select FirstName, LastName, Room.RoomNumber, AmenityName, AmenityPrice from Customer
inner join CustomerReservation on Customer.CustomerId = CustomerReservation.CustomerId
inner join Reservation on CustomerReservation.ReservationId = Reservation.ReservationId
inner join RoomReservation on Reservation.ReservationId = RoomReservation.ReservationId
inner join Room on RoomReservation.RoomNumber = Room.RoomNumber
inner join RoomAmenities on RoomAmenities.RoomNumber = Room.RoomNumber
inner join Amenity on RoomAmenities.AmenityId = Amenity.AmenityId
where Customer.FirstName ='Wiz'

select FirstName, LastName, sum(Charge) "Total Add-On Charges" from Customer
inner join CustomerReservation on Customer.CustomerId = CustomerReservation.CustomerId
inner join Reservation on CustomerReservation.ReservationId = Reservation.ReservationId
inner join Bill on Reservation.ReservationId = Bill.ReservationId
inner join BillDetails on Bill.BillId = BillDetails.BillId
where LastName = 'Khalifa'
group by FirstName, LastName