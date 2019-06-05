use master
go

if exists(select * from sys.databases where name='Buddy')
drop database Buddy
go

create database Buddy
go

use Buddy
go

if exists(select * from sys.tables where name='UndoneCustTransPmt')
	drop table UndoneCustTransPmt
go

if exists(select * from sys.tables where name='UndoneCustTrans')
	drop table UndoneCustTrans
go

if exists(select * from sys.tables where name='Payment')
	drop table Payment
go

if exists(select * from sys.tables where name='CustOrder')
	drop table CustOrder
go

if exists(select * from sys.tables where name='Dealer')
	drop table Dealer
go

if exists(select * from sys.tables where name='CustTransaction')
	drop table CustTransaction
go

if exists(select * from sys.tables where name='Favorite')
	drop table Favorite
go

if exists(select * from sys.tables where name='Customer')
	drop table Customer
go

if exists(select * from sys.tables where name='Addr')
	drop table Addr
go

if exists(select * from sys.tables where name='Price')
	drop table Price
go

if exists(select * from sys.tables where name='DealerTransaction')
	drop table DealerTransaction
go

if exists(select * from sys.tables where name='Inventory')
	drop table Inventory
go

if exists(select * from sys.tables where name='Strain')
	drop table Strain
go

if exists(select * from sys.tables where name='DealerCustomer')
	drop table DealerCustomer
go

create table Dealer (
	DealerId int identity(1,1) primary key not null,
	DealerFName varchar(35) not null,
	DealerLName varchar(35) not null,
	DealerDOB date not null,
	HighOffOwnSupplyThreshold decimal not null,
	DateCreated datetime not null
)

create table Strain (
	StrainId int identity(1,1) primary key not null,
	StrainName varchar(35) not null,
	StrainDescription varchar(200),
	ImageFile varchar(200),
	DealerId int foreign key references Dealer(DealerId) not null
)

create table Price (
	PriceId int identity(1,1) primary key not null,
	Amount money not null,
	PerQty decimal(5,1) not null,
	UoM char(2) not null,
	StrainId int foreign key references Strain(StrainId) not null,
	constraint CK_UoM check (UoM in ('g', 'oz', 'lb'))
)

create table Addr (
	AddressId int identity(1,1) primary key not null,
	Street varchar(50),
	City varchar(35),
	AddrState char(2),
	Zip int
)

create table Customer (
	CustId int identity(1,1) primary key not null,
	CustomerFName varchar(35) not null,
	CustomerLName varchar(35),
	CustomerAge int,
	AddressId int foreign key references Addr(AddressId),
	DateAdded datetime not null
)

create table DealerCustomer (
	DealerId int foreign key references Dealer(DealerId) not null,
	CustId int foreign key references Customer(CustId) not null
)

create table Inventory (
	InventoryId int identity(1,1) primary key not null,
	StrainId int foreign key references Strain(StrainId) not null,
	DealerId int foreign key references Dealer(DealerId) not null,
	InitialQty decimal(10,2) not null,
	Qty decimal(14,6) not null,
	InitialUoM char(2) not null,
	UoM char(2) not null,
	CostToDealer money not null,
	AllGone bit not null,
	DateAdded datetime not null,
	constraint CK_InvUoM check (UoM in ('g', 'oz', 'lb')),
	constraint CK_InvInitUoM check (UoM in ('g', 'oz', 'lb'))
)

create table CustTransaction (
	CustTransId int identity(1,1) primary key not null,
	CustId int not null,
	CustName varchar(80) not null,
	InventoryId int not null,
	StrainName varchar(35) not null,
	StrainPrice money not null,
	PerQty decimal(5,1) not null,
	PriceUoM char(2) not null,
	UoM char(2) not null,
	DealerId int not null,
	Qty decimal(10,2) not null,
	Total money not null,
	PaymentAmount money not null,
	Change money not null,
	PaymentType varchar(20) not null,
	PaidInFull bit not null,
	TransactionDate datetime not null,
	CustOrderId int,
	Memo varchar(200),
	HasPayments bit not null,
	QtyTakenFromInv decimal(14,6) not null,
	QtyTakenUoM char(2) not null,
	constraint CK_PayType check (PaymentType in ('Cash', 'Venmo', 'CashApp')),
	constraint CK_StrainUoM check (UoM in ('g', 'oz', 'lb')),
	constraint CK_PriceUoM check (UoM in ('g', 'oz', 'lb')),
	constraint CK_QtyTakenUoM check (QtyTakenUoM in ('g', 'oz', 'lb'))
)

create table Favorite (
	FavoriteId int identity(1,1) primary key not null,
	CustId int foreign key references Customer(CustId) not null,
	StrainId int foreign key references Strain(StrainId) not null
)

create table DealerTransaction (
	DealerTransId int identity(1,1) primary key not null,
	DealerId int foreign key references Dealer(DealerId) not null,
	InventoryId int not null,
	StrainName varchar(35) not null,
	Qty decimal(14,6) not null,
	UoM char(2) not null,
	TransactionDate datetime not null,
	Memo varchar(200),
	QtyTakenFromInv decimal(14,6) not null,
	QtyTakenUoM char(2) not null,
	constraint CK_DT_PriceUoM check (UoM in ('g', 'oz', 'lb')),
	constraint CK_DT_QtyTakenUoM check (QtyTakenUoM in ('g', 'oz', 'lb'))
)

create table CustOrder (
	CustOrderId int identity(1,1) primary key not null,
	CustId int foreign key references Customer(CustId) not null,
	StrainId int foreign key references Strain(StrainId) not null,
	DealerId int foreign key references Dealer(DealerId) not null,
	Qty decimal(10,2) not null,
	UoM char(2) not null,
	PriceId int,
	Price money not null,
	PerQty decimal(5,1) not null,
	PriceUoM char(2) not null,
	Total money not null,
	OrderDate datetime not null,
	DateLastModified datetime not null,
	Memo varchar(200),
	OrderProcessed bit not null,
	CustTransId int,
	constraint CK_OrderUoM check (UoM in ('g', 'oz', 'lb')),
	constraint CK_Order_Price_UoM check (UoM in ('g', 'oz', 'lb'))
)

create table Payment (
	PaymentId int identity(1,1) primary key not null,
	PaymentAmount money not null,
	PaymentType varchar(10) not null,
	Change money not null,
	PaymentDate datetime not null,
	CustTransId int foreign key references CustTransaction(CustTransId) not null,
	constraint CK_Pmt_PayType check (PaymentType in ('Cash', 'Venmo', 'CashApp'))
)

create table UndoneCustTrans (
	CustTransId int primary key not null,
	CustId int not null,
	CustName varchar(80) not null,
	InventoryId int not null,
	StrainName varchar(35) not null,
	StrainPrice money not null,
	PerQty decimal(5,1) not null,
	PriceUoM char(2) not null,
	UoM char(2) not null,
	DealerId int not null,
	Qty decimal(10,2) not null,
	Total money not null,
	PaymentAmount money not null,
	Change money not null,
	PaymentType varchar(20) not null,
	PaidInFull bit not null,
	TransactionDate datetime not null,
	CustOrderId int,
	Memo varchar(200),
	HasPayments bit not null,
	QtyTakenFromInv decimal(14,6) not null,
	QtyTakenUoM char(2) not null
)

create table UndoneCustTransPmt (
	PaymentId int primary key not null,
	PaymentAmount money not null,
	PaymentType varchar(10) not null,
	Change money not null,
	PaymentDate datetime not null,
	CustTransId int foreign key references UndoneCustTrans(CustTransId) not null
)

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetUndoneCustTransaction')
		drop procedure GetUndoneCustTransaction
go

create procedure GetUndoneCustTransaction(
	@Id int
)
as
	select * from UndoneCustTrans
	where CustTransId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteUndoneTrans')
		drop procedure DeleteUndoneTrans
go

create procedure DeleteUndoneTrans(
	@Id int
)
as
	delete from UndoneCustTransPmt
	where CustTransId = @Id

	delete from UndoneCustTrans
	where CustTransId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllUndoneCustTransPmts')
		drop procedure GetAllUndoneCustTransPmts
go

create procedure GetAllUndoneCustTransPmts(
	@CustTransId int
)
as
	select * from UndoneCustTransPmt
	where CustTransId = @CustTransId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllUndoneCustTransacts')
		drop procedure GetAllUndoneCustTransacts
go

create procedure GetAllUndoneCustTransacts(
	@Id int
)
as
	select * from UndoneCustTrans
	where DealerId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddPayment')
		drop procedure AddPayment
go

create procedure AddPayment(
	@Amount money,
	@PayType varchar(10),
	@Change money,
	@PayDate datetime,
	@Id int
)
as
	insert into Payment values(@Amount, @PayType, @Change, @PayDate, @Id)
	update CustTransaction
	set HasPayments = 1
	where CustTransId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPayments')
		drop procedure GetPayments
go

create procedure GetPayments(
	@Id int
)
as
	select * from Payment
	where CustTransId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeletePayment')
		drop procedure DeletePayment
go

create procedure DeletePayment(
	@Id int
)
as
	declare @a int
	select @a = CustTransId from Payment where PaymentId = @Id
	delete from Payment
	where PaymentId = @Id
	if not exists (select * from Payment where CustTransId = @a)
	update CustTransaction
	set HasPayments = 0,
	PaidInFull = 0
	where CustTransId = @a
	else
	update CustTransaction
	set HasPayments = 1,
	PaidInFull = 0
	where CustTransId = @a
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddCustOrder')
		drop procedure AddCustOrder
go

create procedure AddCustOrder(
	@CustId int,
	@StrainId int,
	@DealerId int,
	@Qty decimal(10,2),
	@UoM char(2),
	@PriceId int,
	@Price money,
	@PerQty decimal(5,1),
	@PriceUoM char(2),
	@OrderDate datetime,
	@Memo varchar(200),
	@Total money
)
as
	insert into CustOrder values(@CustId, @StrainId, @DealerId, @Qty, @UoM, @PriceId, @Price, @PerQty, @PriceUoM, @Total, @OrderDate, @OrderDate, @Memo, 0, null)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditCustOrder')
		drop procedure EditCustOrder
go

create procedure EditCustOrder(
	@CustOrderId int,
	@StrainId int,
	@Qty decimal(10,2),
	@UoM char(2),
	@PriceId int,
	@Price money,
	@PerQty decimal(5,1),
	@PriceUoM char(2),
	@DateModified datetime,
	@Memo varchar(200),
	@Total money
)
as
	update CustOrder
	set StrainId = @StrainId,
	Qty = @Qty,
	UoM = @UoM,
	PriceId = @PriceId,
	Price = @Price,
	PerQty = @PerQty,
	PriceUoM = @PriceUoM,
	DateLastModified = @DateModified,
	Memo = @Memo,
	Total = @Total
	where CustOrderId = @CustOrderId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetCustOrder')
		drop procedure GetCustOrder
go

create procedure GetCustOrder(
	@Id int
)
as
	select * from CustOrder
	where CustOrderId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteCustOrder')
		drop procedure DeleteCustOrder
go

create procedure DeleteCustOrder(
	@CustOrderId int
)
as
	delete from CustOrder
	where CustOrderId = @CustOrderId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetCustOrders')
		drop procedure GetCustOrders
go

create procedure GetCustOrders(
	@CustId int
)
as
	select * from CustOrder
	where CustId = @CustId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllCustOrders')
		drop procedure GetAllCustOrders
go

create procedure GetAllCustOrders(
	@DealerId int
)
as
	select * from CustOrder
	where DealerId = @DealerId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddStrain')
		drop procedure AddStrain
go

create procedure AddStrain(
	@StrainId int output,
	@Name varchar(35),
	@Description varchar(200),
	@Image varchar(200),
	@Id int
)
as
	insert into Strain values(@Name, @Description, @Image, @Id)
	set @StrainId = SCOPE_IDENTITY()
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetStrain')
		drop procedure GetStrain
go

create procedure GetStrain(
	@Id int
)
as
	select * from Strain
	where StrainId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllStrains')
		drop procedure GetAllStrains
go

create procedure GetAllStrains(
	@Id int
)
as
	select * from Strain
	where DealerId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditStrain')
		drop procedure EditStrain
go

create procedure EditStrain(
	@Id int,
	@Name varchar(35),
	@Description varchar(200),
	@Image varchar(200)
)
as
	update Strain
	set StrainName = @Name,
	StrainDescription = @Description,
	ImageFile = @Image
	where StrainId = @id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteStrain')
		drop procedure DeleteStrain
go

create procedure DeleteStrain(
	@Id int
)
as
	delete from Price
	where StrainId = @Id

	delete from Strain
	where StrainId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddPrice')
		drop procedure AddPrice
go

create procedure AddPrice(
	@Amt money,
	@PerQty decimal(5,1),
	@UoM char(2),
	@Id int
)
as
	insert into Price values(@Amt, @PerQty, @UoM, @Id)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditPrice')
		drop procedure EditPrice
go

create procedure EditPrice(
	@Amt money,
	@PerQty decimal(5,1),
	@UoM char(2),
	@Id int
)
as
	update Price
	set Amount = @Amt,
	PerQty = @PerQty,
	UoM = @UoM
	where PriceId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeletePrice')
		drop procedure DeletePrice
go

create procedure DeletePrice(
	@Id int
)
as
	delete from Price
	where PriceId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPrices')
		drop procedure GetPrices
go

create procedure GetPrices(
	@Id int
)
as
	select * from Price
	where StrainId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPrice')
		drop procedure GetPrice
go

create procedure GetPrice(
	@Id int
)
as
	select * from Price
	where PriceId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddCustomer')
		drop procedure AddCustomer
go

create procedure AddCustomer(
	@CustId int output,
	@FName varchar(35),
	@LName varchar(35),
	@Age int,
	@DealerId int,
	@AddrId int,
	@DateAdded datetime
)
as
	insert into Customer values(@FName, @LName, @Age, @AddrId, @DateAdded)
	set @CustId = SCOPE_IDENTITY()
	insert into DealerCustomer values(@DealerId, SCOPE_IDENTITY())
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditCustomer')
		drop procedure EditCustomer
go

create procedure EditCustomer(
	@Id int,
	@FName varchar(35),
	@LName varchar(35),
	@Age int,
	@AddrId int
)
as
	update Customer
	set CustomerFName = @FName,
	CustomerLName = @LName,
	CustomerAge = @Age
	where CustId = @Id
	
	if @AddrId > 0
	update Customer
	set AddressId = @AddrId
	where CustId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteCustomer')
		drop procedure DeleteCustomer
go

create procedure DeleteCustomer(
	@Id int
)
as
	delete from Favorite
	where CustId = @Id

	delete from DealerCustomer
	where CustId = @Id

	delete from CustOrder
	where CustId = @Id

	delete from Customer
	where CustId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetCustomer')
		drop procedure GetCustomer
go

create procedure GetCustomer(
	@Id int
)
as
	select * from Customer
	where CustId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllCustomers')
		drop procedure GetAllCustomers
go

create procedure GetAllCustomers
as
	select * from Customer
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetCustomers')
		drop procedure GetCustomers
go

create procedure GetCustomers(
	@Id int
)
as
	select * from Customer
	where CustId in (select CustId from DealerCustomer where DealerId = @Id)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddAddr')
		drop procedure AddAddr
go

create procedure AddAddr(
	@AddrId int output,
	@Street varchar(50),
	@City varchar(35),
	@AddrState char(2),
	@Zip int
)
as
	declare @existingAddr int
	select @existingAddr = AddressId from Addr where Street = @Street and City = @City and AddrState = @AddrState
	if @existingAddr is not null
	set @AddrId = @existingAddr
	else
	begin
	insert into Addr values(@Street, @City, @AddrState, @Zip)
	set @AddrId = SCOPE_IDENTITY()
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditAddr')
		drop procedure EditAddr
go

create procedure EditAddr(
	@Street varchar(50),
	@City varchar(35),
	@AddrState char(2),
	@Zip int,
	@Id int,
	@CustId int
)
as
	declare @existingAddr int
	select @existingAddr = AddressId from Addr where Street = @Street and City = @City and AddrState = @AddrState
	if @existingAddr is null
	update Addr
	set Street = @Street,
	City = @City,
	AddrState = @AddrState,
	Zip = @Zip
	where AddressId = @Id
	else
	update Customer
	set AddressId = @existingAddr
	where CustId = @CustId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteAddr')
		drop procedure DeleteAddr
go

create procedure DeleteAddr(
	@Id int
)
as
	delete from Addr
	where AddressId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAddr')
		drop procedure GetAddr
go

create procedure GetAddr(
	@Id int
)
as
	select * from Addr
	where AddressId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddDealer')
		drop procedure AddDealer
go

create procedure AddDealer(
	@DealerId int output,
	@FName varchar(35),
	@LName varchar(35),
	@DOB date,
	@Threshold decimal,
	@DateCreated datetime
)
as
	insert into Dealer values(@FName, @LName, @DOB, @Threshold, @DateCreated)
	
	set @DealerId = SCOPE_IDENTITY()
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditDealer')
		drop procedure EditDealer
go

create procedure EditDealer(
	@FName varchar(35),
	@LName varchar(35),
	@DOB date,
	@Threshold decimal,
	@Id int
)
as
	update Dealer
	set DealerFName = @FName,
	DealerLName = @LName,
	DealerDOB = @DOB,
	HighOffOwnSupplyThreshold = @Threshold
	where DealerId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteDealer')
		drop procedure DeleteDealer
go

create procedure DeleteDealer(
	@Id int
)
as
	delete from DealerTransaction
	where DealerId = @Id

	delete from CustOrder
	where DealerId = @Id

	delete from Inventory
	where DealerId = @Id

	delete from DealerCustomer
	where DealerId = @Id

	delete from Price
	where StrainId in (select StrainId from Strain where DealerId = @Id)

	delete from Strain
	where DealerId = @Id

	delete from Dealer
	where DealerId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetDealer')
		drop procedure GetDealer
go

create procedure GetDealer(
	@Id int
)
as
	select * from Dealer
	where DealerId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllDealers')
		drop procedure GetAllDealers
go

create procedure GetAllDealers
as
	select * from Dealer
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddInventory')
		drop procedure AddInventory
go

create procedure AddInventory(
	@StrainId int,
	@DealerId int,
	@Qty decimal(10,2),
	@UoM char(2),
	@Cost money,
	@DateAdded datetime
)
as
	insert into Inventory values(@StrainId, @DealerId, @Qty, @Qty, @UoM, @UoM, @Cost, 0, @DateAdded)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditInventory')
		drop procedure EditInventory
go

create procedure EditInventory(
	@Qty decimal(14,6),
	@UoM char(2),
	@Cost money,
	@InventoryId int,
	@AllGone bit,
	@HasTransactions bit
)
as
	update Inventory
	set Qty = @Qty,
	UoM = @UoM,
	CostToDealer = @Cost,
	AllGone = @AllGone
	where InventoryId = @InventoryId
	if @HasTransactions = 0
	update Inventory
	set InitialQty = @Qty,
	InitialUoM = @UoM
	where InventoryId = @InventoryId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteInventory')
		drop procedure DeleteInventory
go

create procedure DeleteInventory(
	@Id int
)
as
	delete from Inventory
	where InventoryId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllInventory')
		drop procedure GetAllInventory
go

create procedure GetAllInventory(
	@DealerId int
)
as
	select * from Inventory
	where DealerId = @DealerId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetInventory')
		drop procedure GetInventory
go

create procedure GetInventory(
	@Id int
)
as
	select * from Inventory
	where InventoryId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddCustTrans')
		drop procedure AddCustTrans
go

create procedure AddCustTrans(
	@CustTransId int output,
	@CustId int,
	@CustName varchar(80),
	@InventoryId int,
	@StrainName varchar(50),
	@StrainPrice money,
	@PerQty decimal(5,1),
	@PriceUoM char(2),
	@UoM char(2),
	@DealerId int,
	@Qty decimal(12,4),
	@QtyTakenFromInv decimal(14,6),
	@QtyTakenUoM char(2),
	@Total money,
	@PaymentAmount money,
	@Change money,
	@PaymentType varchar(20),
	@PaidInFull bit,
	@TransactionDate datetime,
	@CustOrderId int,
	@Memo varchar(200)
)
as
	insert into CustTransaction values(@CustId, @CustName, @InventoryId, @StrainName, @StrainPrice, @PerQty, @PriceUoM, @UoM, @DealerId, @Qty, @Total, @PaymentAmount, @Change, @PaymentType, @PaidInFull, @TransactionDate, @CustOrderId, @Memo, 0, @QtyTakenFromInv, @QtyTakenUoM)
	set @CustTransId = SCOPE_IDENTITY()
	if @CustOrderId is not null
	update CustOrder
	set OrderProcessed = 1,
	CustTransId = SCOPE_IDENTITY()
	where CustOrderId = @CustOrderId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'UndoTransaction')
		drop procedure UndoTransaction
go

create procedure UndoTransaction(
	@Id int
)
as
	declare @qtyTaken decimal(14,6)
	declare @invId int
	declare @invUoM char(2)
	declare @qtyTakenUoM char(2)
	declare @orderId int
	select @qtyTaken = QtyTakenFromInv from CustTransaction where CustTransId = @Id
	select @invId = InventoryId from CustTransaction where CustTransId = @Id
	select @invUoM = UoM from Inventory where InventoryId = @invId
	select @qtyTakenUoM = QtyTakenUoM from CustTransaction where CustTransId = @Id
	select @orderId = CustOrderId from CustTransaction where CustTransId = @Id

	insert into UndoneCustTrans
	select * from CustTransaction
	where CustTransId = @Id
	insert into UndoneCustTransPmt
	select * from Payment
	where CustTransId = @Id

	if @invUoM != @qtyTakenUoM
	begin
	if @invUoM = 'g'
		begin
		if @qtyTakenUoM = 'oz' select @qtyTaken *= 28.35
		else if @qtyTakenUoM = 'lb' select @qtyTaken *= 453.59
		end
	else if @invUoM = 'oz'
		begin
		if @qtyTakenUoM = 'g' select @qtyTaken /= 28.35
		else if @qtyTakenUoM = 'lb' select @qtyTaken *= 16
		end
	else
		begin
		if @qtyTakenUoM = 'g' select @qtyTaken /= 453.59
		else if @qtyTakenUoM = 'oz' select @qtyTaken /= 16
		end
	end

	if exists (select * from Payment where CustTransId = @Id)
	delete from Payment
	where CustTransId = @Id

	delete from CustTransaction
	where CustTransId = @Id
	update Inventory
	set Qty += @qtyTaken,
	AllGone = 0
	where InventoryId = @invId

	if @orderId is not null
	update CustOrder
	set OrderProcessed = 0
	where CustOrderId = @orderId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'ChangePIFStatus')
		drop procedure ChangePIFStatus
go

create procedure ChangePIFStatus(
	@Id int
)
as
	update CustTransaction
	set PaidInFull = 1
	where CustTransId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetTransaction')
		drop procedure GetTransaction
go

create procedure GetTransaction(
	@Id int
)
as
	select * from CustTransaction
	where CustTransId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetTransactions')
		drop procedure GetTransactions
go

create procedure GetTransactions(
	@Id int
)
as
	select * from CustTransaction
	where DealerId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddFavorite')
		drop procedure AddFavorite
go

create procedure AddFavorite(
	@CustId int,
	@StrainId int
)
as
	insert into Favorite values(@CustId, @StrainId)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteFavorite')
		drop procedure DeleteFavorite
go

create procedure DeleteFavorite(
	@Id int
)
as
	delete from Favorite
	where FavoriteId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteFavorite2')
		drop procedure DeleteFavorite2
go

create procedure DeleteFavorite2(
	@CustId int,
	@StrainId int
)
as
	delete from Favorite
	where CustId = @CustId
	and StrainId = @StrainId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetFavorites')
		drop procedure GetFavorites
go

create procedure GetFavorites(
	@CustId int
)
as
	select * from Favorite
	where CustId = @CustId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddDealerTrans')
		drop procedure AddDealerTrans
go

create procedure AddDealerTrans(
	@DealerId int,
	@InventoryId int,
	@StrainName varchar(35),
	@Qty decimal(10,2),
	@UoM char(2),
	@Date datetime,
	@Memo varchar(200),
	@QtyTaken decimal(14,6),
	@QtyTakenUoM char(2)
)
as
	insert into DealerTransaction values(@DealerId, @InventoryId, @StrainName, @Qty, @UoM, @Date, @Memo, @QtyTaken, @QtyTakenUoM)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteDealerTrans')
		drop procedure DeleteDealerTrans
go

create procedure DeleteDealerTrans(
	@Id int
)
as
	delete from DealerTransaction
	where DealerTransId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetDealerTrans')
		drop procedure GetDealerTrans
go

create procedure GetDealerTrans(
	@Id int
)
as
	select * from DealerTransaction
	where DealerId = @Id
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'SubtractInvQty')
		drop procedure SubtractInvQty
go

create procedure SubtractInvQty(
	@Qty decimal(12,4),
	@InventoryId int,
	@AllGone bit
)
as
	update Inventory
	set Qty = @Qty,
	AllGone = @AllGone
	where InventoryId = @InventoryId
go

select * from Strain full outer join Price on Strain.StrainId = Price.StrainId

select * from CustTransaction

select * from Inventory

select * from Payment

select * from Favorite

select * from UndoneCustTrans

select * from UndoneCustTransPmt

select * from CustOrder

select * from Addr

select * from Customer

alter table Inventory
alter column Qty decimal(14,6)

alter table CustTransaction
alter column QtyTakenFromInv decimal(14,6)