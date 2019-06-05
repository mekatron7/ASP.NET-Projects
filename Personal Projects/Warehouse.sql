use master
go

if exists(select * from sys.databases where name='Warehouse')
drop database Warehouse
go

create database Warehouse
go

use Warehouse
go

if exists(select * from sys.tables where name='Product')
	drop table Product
go

if exists(select * from sys.tables where name='CustOrder')
	drop table CustOrder
go

if exists(select * from sys.tables where name='OrderLine')
	drop table OrderLine
go

if exists(select * from sys.tables where name='Bin')
	drop table Bin
go

if exists(select * from sys.tables where name='Inventory')
	drop table Inventory
go

create table Product (
	ProductId int identity(1,1) primary key not null,
	SKU varchar(7) unique not null,
	ProductDescription varchar(200) not null,
	Size int not null
)

create table CustOrder (
	OrderNumber int identity(1,1) primary key not null,
	DateOrdered datetime not null,
	CustomerName varchar(35) not null,
	CustomerAddress varchar(50) not null
)

create table OrderLine (
	OrderLineId int identity(1,1) primary key not null,
	OrderId int foreign key references CustOrder(OrderNumber) not null,
	ProductId int foreign key references Product(ProductId) not null,
	Qty int not null
)

create table Bin (
	BinId int identity(1,1) primary key not null,
	BinName varchar(25) unique not null,
	Capacity int not null,
	AvailableSpace int not null
)

create table Inventory (
	InventoryId int identity(1,1) primary key not null,
	ProductId int foreign key references Product(ProductId) not null,
	BinId int foreign key references Bin(BinId) not null,
	Qty int not null
)

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddProduct')
		drop procedure AddProduct
go

create procedure AddProduct(
	@SKU varchar(7),
	@ProductDescrip varchar(200),
	@Size int
)
as
	insert into Product values(@SKU, @ProductDescrip, @Size)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditProduct')
		drop procedure EditProduct
go

create procedure EditProduct(
	@SKU varchar(7),
	@ProductDescrip varchar(200),
	@ProdId int
)
as
	update Product
	set SKU = @SKU,
	ProductDescription = @ProductDescrip
	where ProductId = @ProdId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetProduct')
		drop procedure GetProduct
go

create procedure GetProduct(
	@ProdId int
)
as
	select * from Product
	where ProductId = @ProdId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetProducts')
		drop procedure GetProducts
go

create procedure GetProducts
as
	select * from Product
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteProduct')
		drop procedure DeleteProduct
go

create procedure DeleteProduct(
	@ProdId int
)
as
	delete from Product
	where ProductId = @ProdId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddOrder')
		drop procedure AddOrder
go

create procedure AddOrder(
	@DateOrdered datetime,
	@CustomerName varchar(35),
	@CustomerAddress varchar(50)
)
as
	insert into CustOrder values(@DateOrdered, @CustomerName, @CustomerAddress)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditOrder')
		drop procedure EditOrder
go

create procedure EditOrder(
	@OrderNum int,
	@DateOrdered datetime,
	@CustomerName varchar(35),
	@CustomerAddress varchar(50)
)
as
	update CustOrder
	set DateOrdered = @DateOrdered,
	CustomerName = @CustomerName,
	CustomerAddress = @CustomerAddress
	where OrderNumber = @OrderNum
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetOrder')
		drop procedure GetOrder
go

create procedure GetOrder(
	@OrderNum int
)
as
	select * from CustOrder
	where OrderNumber = @OrderNum
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetOrders')
		drop procedure GetOrders
go

create procedure GetOrders
as
	select * from CustOrder
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteOrder')
		drop procedure DeleteOrder
go

create procedure DeleteOrder(
	@OrderNum int
)
as
	delete from CustOrder
	where OrderNumber = @OrderNum

	delete from OrderLine
	where OrderId = @OrderNum
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddOrderLine')
		drop procedure AddOrderLine
go

create procedure AddOrderLine(
	@OrderId int,
	@ProductId int,
	@Qty int
)
as
	insert into OrderLine values(@OrderId, @ProductId, @Qty)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditOrderLine')
		drop procedure EditOrderLine
go

create procedure EditOrderLine(
	@OrderLineId int,
	@Qty int
)
as
	update OrderLine
	set Qty = @Qty
	where OrderLineId = @OrderLineId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetOrderLine')
		drop procedure GetOrderLine
go

create procedure GetOrderLine(
	@OrderLineId int
)
as
	select * from OrderLine
	where OrderLineId = @OrderLineId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetOrderLine2')
		drop procedure GetOrderLine2
go

create procedure GetOrderLine2(
	@OrderId int,
	@ProdId int
)
as
	select * from OrderLine
	where OrderId = @OrderId
	and ProductId = @ProdId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetOrderLines')
		drop procedure GetOrderLines
go

create procedure GetOrderLines(
	@OrderId int
)
as
	select * from OrderLine
	where OrderId = @OrderId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteOrderLine')
		drop procedure DeleteOrderLine
go

create procedure DeleteOrderLine(
	@OrderLineId int
)
as
	delete from OrderLine
	where OrderLineId = @OrderLineId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddBin')
		drop procedure AddBin
go

create procedure AddBin(
	@BinName varchar(25),
	@Capacity int
)
as
	insert into Bin values(@BinName, @Capacity, @Capacity)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditBin')
		drop procedure EditBin
go

create procedure EditBin(
	@BinName varchar(25),
	@BinId int,
	@AvailableSpace int
)
as
	update Bin
	set BinName = @BinName,
	AvailableSpace = @AvailableSpace
	where BinId = @BinId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetBin')
		drop procedure GetBin
go

create procedure GetBin(
	@BinId int,
	@BinName varchar(25)
)
as
	select * from Bin
	where BinId = @BinId
	or BinName = @BinName
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetBins')
		drop procedure GetBins
go

create procedure GetBins
as
	select * from Bin
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteBin')
		drop procedure DeleteBin
go

create procedure DeleteBin(
	@BinId int,
	@BinName varchar(25)
)
as
	delete from Bin
	where BinId = @BinId
	or BinName = @BinName
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddInventory')
		drop procedure AddInventory
go

create procedure AddInventory(
	@ProductId int,
	@BinId int,
	@Qty int
)
as
	insert into Inventory values(@ProductId, @BinId, @Qty)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditInventory')
		drop procedure EditInventory
go

create procedure EditInventory(
	@InventoryId int,
	@Qty int
)
as
	update Inventory
	set Qty = @Qty
	where InventoryId = @InventoryId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetInventory')
		drop procedure GetInventory
go

create procedure GetInventory(
	@InventoryId int,
	@ProductId int,
	@BinId int
)
as
	select * from Inventory
	where InventoryId = @InventoryId
	or ProductId = @ProductId
	or BinId = @BinId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllInventory')
		drop procedure GetAllInventory
go

create procedure GetAllInventory
as
	select * from Inventory
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteInventory')
		drop procedure DeleteInventory
go

create procedure DeleteInventory(
	@InventoryId int,
	@ProductId int,
	@BinId int
)
as
	delete from Inventory
	where InventoryId = @InventoryId
	or ProductId = @ProductId
	or BinId = @BinId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'TransferInventory')
		drop procedure TransferInventory
go

create procedure TransferInventory(
	@ProductId int,
	@TransferAmount int,
	@FromBinId int,
	@ToBinId int,
	@InvExists bit
)
as
	update Inventory
	set Qty -= @TransferAmount
	where ProductId = @ProductId
	and BinId = @FromBinId
if @InvExists = 1
	update Inventory
	set Qty += @TransferAmount
	where ProductId = @ProductId
	and BinId = @ToBinId
else
	insert into Inventory values(@ProductId, @ToBinId, @TransferAmount)
go

select * from OrderLine

update Bin
set AvailableSpace = 100
where BinId = 16