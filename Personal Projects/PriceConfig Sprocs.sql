use PriceConfig
go

select * from Prices

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'InsertPrice')
		drop procedure InsertPrice
go

create procedure InsertPrice(
	@PriceId int,
	@Price money
)
as
	insert into Prices values(@PriceId, @Price)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllPrices')
		drop procedure GetAllPrices
go

create procedure GetAllPrices
as
	select * from Prices
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'UpdatePrice')
		drop procedure UpdatePrice
go

create procedure UpdatePrice(
	@PriceId int,
	@NewPrice money
)
as
	update Prices
	set Price = @NewPrice
	where PriceId = @PriceId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteData')
		drop procedure DeleteData
go

create procedure DeleteData
as
	delete from Prices
go