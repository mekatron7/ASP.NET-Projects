use master
go

if exists(select * from sys.databases where name='Buddy')
drop database Gematrianator
go

create database Gematrianator
go

use Gematrianator
go

if exists(select * from sys.tables where name='Word')
	drop table Word
go

if exists(select * from sys.tables where name='Cipher')
	drop table Cipher
go

if exists(select * from sys.tables where name='WordCipher')
	drop table WordCipher
go

create table Word (
	WordText varchar(70) primary key not null,
	DateRecorded datetime not null
)

create table Cipher (
	CipherID varchar(6) primary key not null,
	CipherName varchar(40) not null
)

create table WordCipher (
	WordText varchar(70) foreign key references Word(WordText) not null,
	CipherID varchar(6) foreign key references Cipher(CipherID) not null,
	CipherValue int
)

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddWord')
		drop procedure AddWord
go

create procedure AddWord(
	@Word varchar(70),
	@DateAdded datetime
)
as
	insert into Word values(@Word, @DateAdded)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddWordCipher')
		drop procedure AddWordCipher
go

create procedure AddWordCipher(
	@Word varchar(70),
	@CipherID varchar(6),
	@Value int
)
as
	insert into WordCipher values(@Word, @CipherID, @Value)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetWords')
		drop procedure GetWords
go

create procedure GetWords(
	@Value int
)
as
	select * from WordCipher
	where CipherValue = @Value
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetWordCiphers')
		drop procedure GetWordCiphers
go

create procedure GetWordCiphers(
	@Word varchar(70)
)
as
	select * from WordCipher
	where WordText = @Word
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllCiphers')
		drop procedure GetAllCiphers
go

create procedure GetAllCiphers
as
	select * from Cipher
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteWord')
		drop procedure DeleteWord
go

create procedure DeleteWord(
	@Word varchar(70)
)
as
	delete from WordCipher
	where WordText = @Word

	delete from Word
	where WordText = @Word
go

insert into Cipher values('EO', 'English Ordinal')
insert into Cipher values('REO', 'Reverse English Ordinal')
insert into Cipher values('EFR', 'English Full Reduction')
insert into Cipher values('REFR', 'Reverse English Full Reduction')