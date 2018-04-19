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