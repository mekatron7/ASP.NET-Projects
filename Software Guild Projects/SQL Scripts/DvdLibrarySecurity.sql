use master
go

create login DVDLibraryApp with password='testing123'
go

use DVDDB
go

create user DVDLibraryApp for login DVDLibraryApp
go

use DVDDBEF
go

create user DVDLibraryApp for login DVDLibraryApp
go

use DVDDB
go

grant execute on DVDAdd to DVDLibraryApp
grant execute on DVDEdit to DVDLibraryApp
grant execute on DVDGetAll to DVDLibraryApp
grant execute on DVDRemove to DVDLibraryApp
grant execute on DVDSelectDirector to DVDLibraryApp
grant execute on DVDSelectRating to DVDLibraryApp
grant execute on DVDSelectTitle to DVDLibraryApp
grant execute on DVDSelectYear to DVDLibraryApp

--create role db_executor
--grant execute to db_executor
--alter role db_excutor add member DVDLibraryApp

grant select on DVD to DVDLibraryApp
grant insert on DVD to DVDLibraryApp
grant update on DVD to DVDLibraryApp
grant delete on DVD to DVDLibraryApp
go