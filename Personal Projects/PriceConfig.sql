use master
go

create database PriceConfig
go

use PriceConfig

create table Prices (
	PriceId int primary key,
	Price money not null
)