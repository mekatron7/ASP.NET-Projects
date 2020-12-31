use EmekaTestDb
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddBrand')
		drop procedure AddBrand
go

create procedure AddBrand(
	@BrandName varchar(35),
	@AddedBy varchar(35)
)
as
	insert into Brand values(@BrandName, GETDATE(), @AddedBy)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteBrand')
		drop procedure DeleteBrand
go

create procedure DeleteBrand(
	@BrandId int
)
as
	delete from Brand
	where BrandId = @BrandId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetBrands')
		drop procedure GetBrands
go

create procedure GetBrands
as
	select * from Brand
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddModel')
		drop procedure AddModel
go

create procedure AddModel(
	@ModelNumber varchar(15),
	@BrandId int,
	@AddedBy varchar(35),
	@Cost money
)
as
	insert into Model values(@ModelNumber, @BrandId, GETDATE(), @AddedBy)
	insert into ModelCost values(SCOPE_IDENTITY(), @Cost, GETDATE(), null, @AddedBy)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditModelCost')
		drop procedure EditModelCost
go

create procedure EditModelCost(
	@ModelId varchar(15),
	@Cost money,
	@ModifiedBy varchar(35)
)
as
	update ModelCost
	set EndDate = GETDATE()
	where ModelId = @ModelId
	and EndDate is null

	insert into ModelCost values(@ModelId, @Cost, GETDATE(), null, @ModifiedBy)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteModel')
		drop procedure DeleteModel
go

create procedure DeleteModel(
	@ModelId int
)
as
	delete from Model
	where ModelId = @ModelId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetModels')
		drop procedure GetModels
go

create procedure GetModels
as
	select BrandName, Model.*, mc.Cost from Model
	inner join Brand on Model.BrandId = Brand.BrandId
	left join ModelCost mc on mc.ModelId = Model.ModelId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddPart')
		drop procedure AddPart
go

create procedure AddPart(
	@PartName varchar(35),
	@AddedBy varchar(35)
)
as
	insert into Part values(@PartName, GETDATE(), @AddedBy)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeletePart')
		drop procedure DeletePart
go

create procedure DeletePart(
	@PartId int
)
as
	delete from Part
	where PartId = @PartId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetParts')
		drop procedure GetParts
go

create procedure GetParts
as
	select * from Part
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddModelPart')
		drop procedure AddModelPart
go

create procedure AddModelPart(
	@ModelId int,
	@PartId int,
	@Cost money,
	@AddedBy varchar(35)
)
as
	insert into ModelPart values(@ModelId, @PartId, GETDATE(), @AddedBy)
	insert into MPCost values(SCOPE_IDENTITY(), @Cost, GETDATE(), null)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditModelPart')
		drop procedure EditModelPart
go

create procedure EditModelPart(
	@ModelPartId int,
	@Cost money,
	@ModifiedBy varchar(35)
)
as
	update ModelPart
	set AddedBy = @ModifiedBy,
	DateAdded = GETDATE()
	where ModelPartId = @ModelPartId

	update MPCost
	set EndDate = GETDATE()
	where ModelPartId = @ModelPartId
	and EndDate is null

	insert into MPCost values(@ModelPartId, @Cost, GETDATE(), null)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteModelPart')
		drop procedure DeleteModelPart
go

create procedure DeleteModelPart(
	@ModelPartId int
)
as
	delete from ModelPart
	where ModelPartId = @ModelPartId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetModelParts')
		drop procedure GetModelParts
go

create procedure GetModelParts
as
	select mp.*, mc.Cost, mc.MPCostId, PartName, ModelNumber, BrandName from ModelPart mp
	inner join MPCost mc on mc.ModelPartId = mp.ModelPartId
	and EndDate is null
	inner join Part p on mp.PartId = p.PartId
	inner join Model m on mp.ModelId = m.ModelId
	inner join Brand b on b.BrandId = m.BrandId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddClient')
		drop procedure AddClient
go

create procedure AddClient(
	@ClientId int output,
	@Username varchar(35),
	@Grade int,
	@AddedBy varchar(35),
	@DoesNotExist bit output
)
as
	if not exists (select personID from robbinsdale.dbo.UserAccount where username = @Username)
	begin
	set @DoesNotExist = 1
	end
	else
	begin
	create table ##StudentInfo(
	FName varchar(35),
	LName varchar(35),
	StudentNumber int
	)

	declare @Fname as varchar(35)
	declare @Lname as varchar(35)
	declare @StudentNumber as int

	insert into ##StudentInfo
	select firstName, lastName, studentNumber
	from robbinsdale.dbo.[Identity] i
	inner join robbinsdale.dbo.UserAccount u on i.personID = u.personID
	inner join robbinsdale.dbo.Person p on i.personID = p.personID
	where username = @Username

	set @Fname = (select FName from ##StudentInfo)
	set @Lname = (select LName from ##StudentInfo)
	set @StudentNumber = (select StudentNumber from ##StudentInfo)
	set @Username = (select username from robbinsdale.dbo.UserAccount where username = @Username)
	set @DoesNotExist = 0

	insert into Client values(@StudentNumber, @Fname, @Lname, @Username, @Grade, GETDATE(), @AddedBy)
	set @ClientId = SCOPE_IDENTITY()
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddClientAutomatic')
		drop procedure AddClientAutomatic
go

create procedure AddClientAutomatic(
	@ClientId int output,
	@Username varchar(35),
	@AddedBy varchar(35),
	@DoesNotExist bit output,
	@NotEnrolled bit output,
	@SchoolId int output
)
as
	set @DoesNotExist = 0
	set @NotEnrolled = 0
	if not exists (select personID from robbinsdale.dbo.UserAccount where username = @Username)
	begin
	set @DoesNotExist = 1
	end
	else
	begin
	create table ##StudentInfo(
	FName varchar(35),
	LName varchar(35),
	StudentNumber int,
	Grade int,
	SchoolId int
	)

	declare @Fname as varchar(35)
	declare @Lname as varchar(35)
	declare @StudentNumber as int
	declare @Grade as int

	if ISNUMERIC(right(@username, 2)) = 1
	begin
	insert into ##StudentInfo
	select firstName, lastName, studentNumber, e.grade, s.schoolID
	from robbinsdale.dbo.[Identity] i
	inner join robbinsdale.dbo.UserAccount u on i.personID = u.personID
	inner join robbinsdale.dbo.Person p on i.personID = p.personID
	inner join robbinsdale.dbo.Enrollment e on e.personID = p.personID
	and e.serviceType = 'P'
	and e.endDate is null
	and e.active = 1
	inner join robbinsdale.dbo.Calendar c on c.calendarID = e.calendarID
	and GETDATE() between c.startDate and c.endDate
	inner join robbinsdale.dbo.School s on s.schoolID = c.schoolID
	where username = @Username

	if not exists (select * from ##StudentInfo)
	begin
	set @NotEnrolled = 1
	end
	else
	begin
	set @Fname = (select FName from ##StudentInfo)
	set @Lname = (select LName from ##StudentInfo)
	set @StudentNumber = (select StudentNumber from ##StudentInfo)
	set @Grade = (select Grade from ##StudentInfo)
	set @Username = (select username from robbinsdale.dbo.UserAccount where username = @Username)
	set @SchoolId = (select SchoolId from ##StudentInfo)

	insert into Client values(@StudentNumber, @Fname, @Lname, @Username, @Grade, GETDATE(), @AddedBy)
	end
	end
	else
	begin
	insert into ##StudentInfo (FName, LName, SchoolId)
	select firstName, lastName, ea.schoolID
	from robbinsdale.dbo.[Identity] i
	inner join robbinsdale.dbo.UserAccount u on i.personID = u.personID
	inner join robbinsdale.dbo.Person p on i.personID = p.personID
	inner join robbinsdale.dbo.EmploymentAssignment ea on ea.personID = p.personID
	and ea.endDate is null
	where username = @Username

	set @Fname = (select FName from ##StudentInfo)
	set @Lname = (select LName from ##StudentInfo)
	set @SchoolId = (select SchoolId from ##StudentInfo)

	insert into Client (FirstName, LastName, Username, DateAdded, AddedBy)
	values(@Fname, @Lname, @Username, GETDATE(), @AddedBy)
	end
	set @ClientId = SCOPE_IDENTITY()
	end
go

--as
--	if (select personID from robbinsdale.dbo.UserAccount where username = @Username) is null
--	set @DoesNotExist = 1
--	else

--	create table ##StudentInfo(
--	FName varchar(35),
--	LName varchar(35),
--	StudentNumber int
--	)

--	declare @Fname as varchar(35)
--	declare @Lname as varchar(35)
--	declare @StudentNumber as int

--	insert into ##StudentInfo
--	select firstName, lastName, studentNumber
--	from robbinsdale.dbo.[Identity] i
--	inner join robbinsdale.dbo.UserAccount u on i.personID = u.personID
--	inner join robbinsdale.dbo.Person p on i.personID = p.personID
--	where username = @Username

--	set @Fname = (select FName from ##StudentInfo)
--	set @Lname = (select LName from ##StudentInfo)
--	set @StudentNumber = (select StudentNumber from ##StudentInfo)

--	insert into Client values(@StudentNumber, @Fname, @Lname, @Username, @Grade, GETDATE(), @AddedBy)
--	insert into ClientSchool values(SCOPE_IDENTITY(), @SchoolId, GETDATE(), @AddedBy)
--	set @ClientSchoolId = SCOPE_IDENTITY()
--go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddUnassignedClient')
		drop procedure AddUnassignedClient
go

create procedure AddUnassignedClient(
	@AddedBy varchar(35),
	@ClientId int output
)
as
	insert into Client (FirstName, LastName, DateAdded, AddedBy)
	values ('Unassigned', 'Unassigned', GETDATE(), @AddedBy)
	set @ClientId = SCOPE_IDENTITY()
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditClient')
		drop procedure EditClient
go

create procedure EditClient(
	@ClientId int,
	@Grade int
)
as
	update Client
	set Grade = @Grade
	where ClientId = @ClientId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'NextGrade')
		drop procedure NextGrade
go

create procedure NextGrade
as
	update Client
	set Grade += 1
	where Grade != 12
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteClient')
		drop procedure DeleteClient
go

create procedure DeleteClient(
	@ClientId int
)
as
	delete from Client
	where ClientId = @ClientId

	declare @maxID as int = (select max(ClientId) from Client)
	if @maxID is null
	begin
	set @maxID = 0
	end

	dbcc checkident(Client, RESEED, @maxID)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllClients')
		drop procedure GetAllClients
go

create procedure GetAllClients
as
	select distinct c.*, s.name as CurrentSchool, cd.StartDate as CSDateAdded, cd.Barcode, cd.ClientDeviceId from Client c
	left join ClientDevice cd on cd.ClientId = c.ClientId
	and cd.ClientDeviceId = (select ClientDeviceId from RepairLog where RepairTimestamp = 
	(select max(RepairTimestamp) from RepairLog r
	inner join ClientDevice cd2 on cd2.ClientDeviceId = r.ClientDeviceId
	where cd2.ClientId = c.ClientId))
	left join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetClientsBySchool')
		drop procedure GetClientsBySchool
go

create procedure GetClientsBySchool(
	@SchoolId int
)
as
	select c.*, s.name as CurrentSchool, cd2.StartDate as CSDateAdded from Client c
	inner join ClientDevice cd on cd.ClientId = c.ClientId
	and cd.SchoolId = @SchoolId
	inner join ClientDevice cd2 on cd2.ClientId = c.ClientId
	and cd2.StartDate = (select max(StartDate) from ClientDevice where ClientId = c.ClientId)
	inner join robbinsdale.dbo.School s on s.schoolID = cd2.SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetClient')
		drop procedure GetClient
go

create procedure GetClient(
	@ClientId int
)
as
	select c.*, cd.StartDate, s.schoolID as SchoolId, s.name as SchoolName, s.number as SchoolNumber
	from Client c
	inner join ClientDevice cd on cd.ClientId = c.ClientId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where c.ClientId = @ClientId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetClientByUsername')
		drop procedure GetClientByUsername
go

create procedure GetClientByUsername(
	@Username varchar(35)
)
as
	select c.*, cd.StartDate, s.schoolID as SchoolId, s.name as SchoolName, s.number as SchoolNumber
	from Client c
	inner join ClientDevice cd on cd.ClientId = c.ClientId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where c.Username = @Username
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetUnassignedClient')
		drop procedure GetUnassignedClient
go

create procedure GetUnassignedClient
as
	select c.*, cd.StartDate, s.schoolID as SchoolId, s.name as SchoolName, s.number as SchoolNumber
	from Client c
	inner join ClientDevice cd on cd.ClientId = c.ClientId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where c.Username is null
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddDevice')
		drop procedure AddDevice
go

create procedure AddDevice(
	@Barcode varchar(10),
	@SerialNumber varchar(10),
	@StorageCapacity varchar(6),
	@ModelId int,
	@AddedBy varchar(35),
	@ClientId int,
	@SchoolId int,
	@ClientDeviceId int output
)
as
	insert into Device values(@Barcode, @SerialNumber, @StorageCapacity, @ModelId, GETDATE(), @AddedBy)
	if @SchoolId is not null
	begin
	insert into ClientDevice values(@ClientId, @Barcode, @SchoolId, 0, GETDATE(), null, @AddedBy)
	set @ClientDeviceId = SCOPE_IDENTITY()
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddClientDevice')
		drop procedure AddClientDevice
go

create procedure AddClientDevice(
	@ClientId int,
	@SchoolId int,
	@Barcode varchar(10),
	@AddedBy varchar(35),
	@ClientDeviceId int output
)
as
	insert into ClientDevice values(@ClientId, @Barcode, @SchoolId, 0, GETDATE(), null, @AddedBy)
	set @ClientDeviceId = SCOPE_IDENTITY()
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditDevice')
		drop procedure EditDevice
go

create procedure EditDevice(
	@Barcode varchar(10),
	@SerialNumber varchar(10),
	@StorageCapacity varchar(6)
)
as
	update Device
	set SerialNumber = @SerialNumber,
	StorageCapacity = @StorageCapacity
	where Barcode = @Barcode
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteDevice')
		drop procedure DeleteDevice
go

create procedure DeleteDevice(
	@Barcode varchar(10)
)
as
	delete from Device
	where Barcode = @Barcode
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetDevice')
		drop procedure GetDevice
go

create procedure GetDevice(
	@Barcode varchar(10)
)
as
	select d.*, m.ModelNumber, b.BrandName from Device d
	inner join Model m on m.ModelId = d.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	where Barcode = @Barcode
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllDevices')
		drop procedure GetAllDevices
go

create procedure GetAllDevices
as
	select distinct d.*, m.ModelNumber, b.BrandName, c.Username as CurrentOwner from Device d
	inner join Model m on m.ModelId = d.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join ClientDevice cd on cd.Barcode = d.Barcode
	inner join Client c on c.ClientId = cd.ClientId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetDevicesBySchool')
		drop procedure GetDevicesBySchool
go

create procedure GetDevicesBySchool(
	@SchoolId int
)
as
	select d.*, m.ModelNumber, b.BrandName, c.Username as CurrentOwner from Device d
	inner join Model m on m.ModelId = d.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join ClientDevice cd on cd.Barcode = d.Barcode
	inner join Client c on c.ClientId = c.ClientId
	where cd.SchoolId = @SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetDevicesByClient')
		drop procedure GetDevicesByClient
go

create procedure GetDevicesByClient(
	@ClientId int
)
as
	select d.*, m.ModelNumber, b.BrandName, cd.ClientDeviceId, cd.Loaner, cd.StartDate, cd.EndDate, s.name as SchoolName from Device d
	inner join Model m on m.ModelId = d.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join ClientDevice cd on cd.Barcode = d.Barcode
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where cd.ClientId = @ClientId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetSchools')
		drop procedure GetSchools
go

create procedure GetSchools
as
	select schoolID, name as SchoolName, number as SchoolNumber
	from robbinsdale.dbo.School
	where schoolID in (select schoolID from School)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddIssueType')
		drop procedure AddIssueType
go

create procedure AddIssueType(
	@IssueName varchar(50),
	@IssueDescription varchar(300),
	@AddedBy varchar(35)
)
as
	insert into IssueType values (@IssueName, @IssueDescription, GETDATE(), @AddedBy)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditIssueType')
		drop procedure EditIssueType
go

create procedure EditIssueType(
	@IssueId int,
	@IssueName varchar(50),
	@IssueDescription varchar(300)
)
as
	update IssueType
	set IssueName = @IssueName,
	IssueDescription = @IssueDescription
	where IssueId = @IssueId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteIssueType')
		drop procedure DeleteIssueType
go

create procedure DeleteIssueType(
	@IssueId int
)
as
	delete from IssueType
	where IssueId = @IssueId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetIssueTypes')
		drop procedure GetIssueTypes
go

create procedure GetIssueTypes
as
	select * from IssueType
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddRepairLog')
		drop procedure AddRepairLog
go

create procedure AddRepairLog(
	@ClientDeviceId int,
	@IssueId int,
	@IssueDescription varchar(300),
	@EmailAddress varchar(50),
	@AddedBy varchar(35),
	@RepairId int output
)
as
	insert into RepairLog (RepairTimestamp, ClientDeviceId, IssueId, IssueDescription, EmailAddress, AddedBy)
	values (GETDATE(), @ClientDeviceId, @IssueId, @IssueDescription, @EmailAddress, @AddedBy)
	set @RepairId = SCOPE_IDENTITY()
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddPartUsed')
		drop procedure AddPartUsed
go

create procedure AddPartUsed(
	@RepairId int,
	@InventoryId int,
	@Recycled bit
)
as
	declare @currentQty as int
	if @Recycled = 0
	begin
	set @currentQty = (select Qty from Inventory where InventoryId = @InventoryId)
	end
	else
	begin
	set @currentQty = (select RecycledQty from Inventory where InventoryId = @InventoryId)
	end
	if @currentQty > 0
	begin
	declare @mpID as int = (select ModelPartId from Inventory where InventoryId = @InventoryId)
	declare @schoolId as int = (select SchoolId from Inventory where InventoryId = @InventoryId)
	declare @mpCostId as int = (select MPCostId from MPCost where ModelPartId = @mpID and EndDate is null)

	declare @PartName as varchar(50) = (select b.BrandName + ' ' + m.ModelNumber + ' ' + p.PartName
	from ModelPart mp inner join Model m on mp.ModelId = m.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	where mp.ModelPartId = @mpID)

	insert into PartsUsed values(@RepairId, @mpID, @schoolId, @PartName, @mpCostId, @Recycled)
	
	if @Recycled = 0
	begin
	update Inventory
	set Qty -= 1
	where InventoryId = @InventoryId
	end
	else
	begin
	update Inventory
	set RecycledQty -= 1
	where InventoryId = @InventoryId
	end
	if exists (select Qty, RecycledQty from Inventory where Qty = 0 and RecycledQty = 0 and InventoryId = @InventoryId)
	begin
	delete from Inventory where InventoryId = @InventoryId
	end
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'RemovePartUsed')
		drop procedure RemovePartUsed
go

create procedure RemovePartUsed(
	@PartUsedId int
)
as
	declare @ModelPartId as int = (select ModelPartId from PartsUsed where PartUsedId = @PartUsedId)
	declare @SchoolId as int = (select SchoolId from PartsUsed where PartUsedId = @PartUsedId)
	declare @Recycled as bit = (select Recycled from PartsUsed where PartUsedId = @PartUsedId)

	delete from PartsUsed
	where PartUsedId = @PartUsedId
	
	declare @InventoryId as int = (select InventoryId from Inventory where ModelPartId = @ModelPartId and SchoolId = @SchoolId)
	if @InventoryId is not null
	begin
	if (@Recycled = 0)
	begin
	update Inventory
	set Qty += 1
	where InventoryId = @InventoryId
	end
	else
	begin
	update Inventory
	set RecycledQty += 1
	where InventoryId = @InventoryId
	end
	end
	else
	begin
	if (@Recycled = 0)
	begin
	insert into Inventory values (@ModelPartId, @SchoolId, 1, GETDATE(), 'System', 0)
	end
	else
	begin
	insert into Inventory values (@ModelPartId, @SchoolId, 0, GETDATE(), 'System', 1)
	end
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPartsUsed')
		drop procedure GetPartsUsed
go

create procedure GetPartsUsed(
	@RepairId int
)
as
	select pu.*, mc.Cost, s.name as SchoolName from PartsUsed pu
	inner join MPCost mc on mc.MPCostId = pu.MPCostId
	inner join robbinsdale.dbo.School s on s.schoolID = pu.SchoolId
	where RepairId = @RepairId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPartsUsedByClient')
		drop procedure GetPartsUsedByClient
go

create procedure GetPartsUsedByClient(
	@ClientId int
)
as
	select pu.*, mc.Cost, s.name as SchoolName from PartsUsed pu
	inner join MPCost mc on mc.MPCostId = pu.MPCostId
	inner join RepairLog r on r.RepairId = pu.RepairId
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join robbinsdale.dbo.School s on s.schoolID = pu.SchoolId
	where ClientId = @ClientId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditRepairLog')
		drop procedure EditRepairLog
go

create procedure EditRepairLog(
	@RepairId int,
	@RepairNotes varchar(200),
	@RepairReturnedDate date,
	@Notes varchar(200),
	@WarrantyRepairSentDate date
)
as
	update RepairLog
	set RepairNotes = @RepairNotes,
	RepairReturnedDate = @RepairReturnedDate,
	Notes = @Notes,
	WarrantyRepairSentDate = @WarrantyRepairSentDate
	where RepairId = @RepairId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteRepairLog')
		drop procedure DeleteRepairLog
go

create procedure DeleteRepairLog(
	@RepairId int
)
as
	declare @ClientId as int,
	@Barcode as varchar(10),
	@ClientDeviceId as int,
	@ModelPartId as int,
	@SchoolId as int,
	@Recycled as bit
	set @ClientId = (select cd.ClientId from RepairLog r
	inner join ClientDevice cd on r.ClientDeviceId = cd.ClientDeviceId
	where r.RepairId = @RepairId)
	set @Barcode = (select cd.Barcode from RepairLog r
	inner join ClientDevice cd on r.ClientDeviceId = cd.ClientDeviceId
	where r.RepairId = @RepairId)
	set @ClientDeviceId = (select ClientDeviceId from RepairLog where RepairId = @RepairId)

	if exists (select * from PartsUsed where RepairId = @RepairId)
	begin

	declare cursor_parts cursor
	for select ModelPartId, SchoolId, Recycled
	from PartsUsed
	where RepairId = @RepairId

	open cursor_parts

	fetch next from cursor_parts into
	@ModelPartId,
	@SchoolId,
	@Recycled

	while @@FETCH_STATUS = 0
	begin
	if exists (select InventoryId from Inventory where ModelPartId = @ModelPartId and SchoolId = @SchoolId)
	begin
	if @Recycled = 0
	begin
	update Inventory
	set Qty += 1
	where ModelPartId = @ModelPartId
	and SchoolId = @SchoolId
	end
	else
	begin
	update Inventory
	set RecycledQty += 1
	where ModelPartId = @ModelPartId
	and SchoolId = @SchoolId
	end
	end
	else
	begin
	if @Recycled = 0
	begin
	insert into Inventory values (@ModelPartId, @SchoolId, 1, GETDATE(), 'System', 0)
	end
	else
	begin
	insert into Inventory values (@ModelPartId, @SchoolId, 0, GETDATE(), 'System', 1)
	end
	end

	fetch next from cursor_parts into
	@ModelPartId,
	@SchoolId
	end

	close cursor_parts
	deallocate cursor_parts

	delete from PartsUsed
	where RepairId = @RepairId
	end

	delete from RepairLog
	where RepairId = @RepairId

	if not exists (select ClientDeviceId from RepairLog where ClientDeviceId = @ClientDeviceId)
	begin
	delete from ClientDevice
	where ClientDeviceId = @ClientDeviceId

	if not exists (select Barcode from ClientDevice where Barcode = @Barcode)
	begin
	delete from Device
	where Barcode = @Barcode
	end

	if not exists (select ClientId from ClientDevice where ClientId = @ClientId)
	begin
	delete from Client
	where ClientId = @ClientId
	end
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetRepairLogs')
		drop procedure GetRepairLogs
go

create procedure GetRepairLogs
as
	select r.*, c.Username, c.Grade, cd.Barcode, d.SerialNumber, i.IssueName, s.schoolID, s.name as SchoolName from RepairLog r
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join Client c on c.ClientId = cd.ClientId
	inner join Device d on d.Barcode = cd.Barcode
	inner join IssueType i on i.IssueId = r.IssueId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetRepairLogBySchool')
		drop procedure GetRepairLogBySchool
go

create procedure GetRepairLogBySchool(
	@SchoolId int
)
as
	select r.*, c.Username, c.Grade, cd.Barcode, d.SerialNumber, i.IssueName from RepairLog r
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join Client c on c.ClientId = cd.ClientId
	inner join Device d on d.Barcode = cd.Barcode
	inner join IssueType i on i.IssueId = r.IssueId
	where SchoolId = @SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetRepairLogByClient')
		drop procedure GetRepairLogByClient
go

create procedure GetRepairLogByClient(
	@ClientId int
)
as
	select r.*, c.Username, c.Grade, cd.Barcode, d.SerialNumber, i.IssueName, s.schoolID, s.name as SchoolName from RepairLog r
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join Client c on c.ClientId = cd.ClientId
	inner join Device d on d.Barcode = cd.Barcode
	inner join IssueType i on i.IssueId = r.IssueId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where c.ClientId = @ClientId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetRepairLogByDevice')
		drop procedure GetRepairLogByDevice
go

create procedure GetRepairLogByDevice(
	@Barcode int
)
as
	select r.*, c.Username, c.Grade, cd.Barcode, d.SerialNumber, i.IssueName, s.schoolID, s.name as SchoolName from RepairLog r
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join Client c on c.ClientId = cd.ClientId
	inner join Device d on d.Barcode = cd.Barcode
	inner join IssueType i on i.IssueId = r.IssueId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where d.Barcode = @Barcode
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetOpenRepairLogs')
		drop procedure GetOpenRepairLogs
go

create procedure GetOpenRepairLogs
as
	select r.*, c.Username, c.Grade, cd.Barcode, d.SerialNumber, i.IssueName, s.schoolID, s.name as SchoolName from RepairLog r
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join Client c on c.ClientId = cd.ClientId
	inner join Device d on d.Barcode = cd.Barcode
	inner join IssueType i on i.IssueId = r.IssueId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where RepairReturnedDate is null
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetFulfilledRepairLogs')
		drop procedure GetFulfilledRepairLogs
go

create procedure GetFulfilledRepairLogs
as
	select r.*, c.Username, c.Grade, cd.Barcode, d.SerialNumber, i.IssueName, s.schoolID, s.name as SchoolName from RepairLog r
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join Client c on c.ClientId = cd.ClientId
	inner join Device d on d.Barcode = cd.Barcode
	inner join IssueType i on i.IssueId = r.IssueId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where RepairReturnedDate is not null
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetRepairLog')
		drop procedure GetRepairLog
go

create procedure GetRepairLog(
	@RepairId int
)
as
	select r.*, c.Username, c.Grade, cd.Barcode, d.SerialNumber, i.IssueName, s.schoolID, s.name as SchoolName from RepairLog r
	inner join ClientDevice cd on cd.ClientDeviceId = r.ClientDeviceId
	inner join Client c on c.ClientId = cd.ClientId
	inner join Device d on d.Barcode = cd.Barcode
	inner join IssueType i on i.IssueId = r.IssueId
	inner join robbinsdale.dbo.School s on s.schoolID = cd.SchoolId
	where RepairId = @RepairId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddInventory')
		drop procedure AddInventory
go

create procedure AddInventory(
	@ModelPartId int,
	@SchoolId int,
	@Qty int,
	@LastModifiedBy varchar(35),
	@Recycled bit
)
as
	declare @InvId as int = (select InventoryId from Inventory where ModelPartId = @ModelPartId and SchoolId = @SchoolId)
	declare @schName as varchar(50) = (select name from robbinsdale.dbo.School where schoolID = @SchoolId)
	declare @partName as varchar(50) = (select b.BrandName + ' ' + m.ModelNumber + ' ' + p.PartName
	from ModelPart mp inner join Model m on mp.ModelId = m.ModelId
	inner join Part p on p.PartId = mp.PartId
	inner join Brand b on b.BrandId = m.BrandId
	where mp.ModelPartId = @ModelPartId)
	if @InvId is not null 
	begin
	declare @OldQty as int
	declare @Message as varchar(100)
	if @Recycled = 0
	begin
	set @OldQty = (select Qty from Inventory where InventoryId = @InvId)
	if @OldQty = 0
	begin
	set @Message = 'New inventory added to '
	end
	else
	begin
	set @Message = 'New inventory added to previously existing inventory at '
	end
	update Inventory
	set Qty += @Qty,
	DateLastModified = GETDATE(),
	LastModifiedBy = @LastModifiedBy
	where ModelPartId = @ModelPartId
	and SchoolId = @SchoolId
	end
	else
	begin
	set @OldQty = (select RecycledQty from Inventory where InventoryId = @InvId)
	if @OldQty = 0
	begin
	set @Message = 'New recycled inventory added to '
	end
	else
	begin
	set @Message = 'New recycled inventory added to previously existing inventory at '
	end
	update Inventory
	set RecycledQty += @Qty,
	DateLastModified = GETDATE(),
	LastModifiedBy = @LastModifiedBy
	where ModelPartId = @ModelPartId
	and SchoolId = @SchoolId
	end
	insert into InventoryEditHistory values(@ModelPartId, @SchoolId, @partName, @OldQty, @OldQty + @Qty, @Message + @schName + '.', GETDATE(), @LastModifiedBy, @Recycled)
	end
	else
	begin
	if @Recycled = 0
	begin
	insert into Inventory values (@ModelPartId, @SchoolId, @Qty, GETDATE(), @LastModifiedBy, 0)
	set @Message = 'New inventory added to '
	end
	else
	begin
	insert into Inventory values (@ModelPartId, @SchoolId, 0, GETDATE(), @LastModifiedBy, @Qty)
	set @Message = 'New recycled inventory added to '
	end
	insert into InventoryEditHistory values(@ModelPartId, @SchoolId, @partName, 0, @Qty, @Message + @schName + '.', GETDATE(), @LastModifiedBy, @Recycled)
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditInventory')
		drop procedure EditInventory
go

create procedure EditInventory(
	@InventoryId int,
	@Qty int,
	@RecycledQty int,
	@Notes varchar(150),
	@LastModifiedBy varchar(35)
)
as
	declare @OldQty as int = (select Qty from Inventory where InventoryId = @InventoryId)
	declare @OldRecycledQty as int = (select RecycledQty from Inventory where InventoryId = @InventoryId)
	declare @mpId as int = (select ModelPartId from Inventory where InventoryId = @InventoryId)
	declare @schId as int = (select SchoolId from Inventory where InventoryId = @InventoryId)
	declare @partName as varchar(50) = (select b.BrandName + ' ' + m.ModelNumber + ' ' + p.PartName
	from ModelPart mp inner join Model m on mp.ModelId = m.ModelId
	inner join Part p on p.PartId = mp.PartId
	inner join Brand b on b.BrandId = m.BrandId
	where mp.ModelPartId = @mpId)

	if(@Qty > 0 or @RecycledQty > 0)
	begin
	update Inventory
	set Qty = @Qty,
	RecycledQty = @RecycledQty,
	DateLastModified = GETDATE(),
	LastModifiedBy = @LastModifiedBy
	where InventoryId = @InventoryId

	if(@Qty != @OldQty)
	begin
	insert into InventoryEditHistory values(@mpId, @schId, @partName, @OldQty, @Qty, @Notes, GETDATE(), @LastModifiedBy, 0)
	end
	if(@RecycledQty != @OldRecycledQty)
	begin
	insert into InventoryEditHistory values(@mpId, @schId, @partName, @OldRecycledQty, @RecycledQty, @Notes, GETDATE(), @LastModifiedBy, 1)
	end
	end
	else
	begin
	insert into InventoryEditHistory values(@mpId, @schId, @partName, @OldQty, @Qty, 'Regular and recycled inventory set to 0 and deleted.', GETDATE(), @LastModifiedBy, 0)
	delete from Inventory where InventoryId = @InventoryId
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteInventory')
		drop procedure DeleteInventory
go

create procedure DeleteInventory(
	@InventoryId int
)
as
	delete from Inventory
	where InventoryId = @InventoryId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetAllInventory')
		drop procedure GetAllInventory
go

create procedure GetAllInventory
as
	select InventoryId, mp.ModelId, mp.PartId, mp.ModelPartId, b.BrandId, b.BrandName, m.ModelNumber, p.PartName, mc.Cost as UnitCost, mc.Cost * Qty as TotalCost, s.name as SchoolName, i.SchoolId, Qty, DateLastModified, LastModifiedBy, RecycledQty from Inventory i
	inner join ModelPart mp on mp.ModelPartId = i.ModelPartId
	inner join MPCost mc on mc.ModelPartId = mp.ModelPartId
	and mc.EndDate is null
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	inner join robbinsdale.dbo.School s on s.schoolID = i.SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetInventoryBySchool')
		drop procedure GetInventoryBySchool
go

create procedure GetInventoryBySchool(
	@SchoolId int
)
as
	select InventoryId, mp.ModelId, mp.PartId, mp.ModelPartId, b.BrandId, b.BrandName, m.ModelNumber, p.PartName, mc.Cost as UnitCost, mc.Cost * Qty as TotalCost, s.name as SchoolName, i.SchoolId, Qty, DateLastModified, LastModifiedBy, RecycledQty from Inventory i
	inner join ModelPart mp on mp.ModelPartId = i.ModelPartId
	inner join MPCost mc on mc.ModelPartId = mp.ModelPartId
	and mc.EndDate is null
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	inner join robbinsdale.dbo.School s on s.schoolID = i.SchoolId
	where i.SchoolId = @SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetInventory')
		drop procedure GetInventory
go

create procedure GetInventory(
	@InventoryId int
)
as
	select InventoryId, mp.ModelId, mp.PartId, mp.ModelPartId, b.BrandId, b.BrandName, m.ModelNumber, p.PartName, mc.Cost as UnitCost, mc.Cost * Qty as TotalCost, s.name as SchoolName, i.SchoolId, Qty, DateLastModified, LastModifiedBy, RecycledQty from Inventory i
	inner join ModelPart mp on mp.ModelPartId = i.ModelPartId
	inner join MPCost mc on mc.ModelPartId = mp.ModelPartId
	and mc.EndDate is null
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	inner join robbinsdale.dbo.School s on s.schoolID = i.SchoolId
	where InventoryId = @InventoryId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetInventory2')
		drop procedure GetInventory2
go

create procedure GetInventory2(
	@ModelPartId int,
	@SchoolId int
)
as
	select InventoryId, mp.ModelId, mp.PartId, mp.ModelPartId, b.BrandId, b.BrandName, m.ModelNumber, p.PartName, mc.Cost as UnitCost, mc.Cost * Qty as TotalCost, s.name as SchoolName, i.SchoolId, Qty, DateLastModified, LastModifiedBy, RecycledQty from Inventory i
	inner join ModelPart mp on mp.ModelPartId = i.ModelPartId
	inner join MPCost mc on mc.ModelPartId = mp.ModelPartId
	and mc.EndDate is null
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	inner join robbinsdale.dbo.School s on s.schoolID = i.SchoolId
	where i.ModelPartId = @ModelPartId
	and i.SchoolId = @SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddPurchaseOrder')
		drop procedure AddPurchaseOrder
go

create procedure AddPurchaseOrder(
	@Username varchar(35),
	@TotalQty int,
	@ModelPartId int,
	@MPCostId int,
	@PONumber bigint,
	@Notes varchar(300)
)
as
	declare @TotalPrice as money = (select Cost from MPCost where MPCostId = @MPCostId) * @TotalQty
	insert into PurchaseOrder values(@PONumber, @Username, GETDATE(), @Notes)
	insert into PurchaseOrderLI values(@PONumber, @ModelPartId, @TotalQty, @MPCostId, @TotalPrice, null)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditPONotes')
		drop procedure EditPONotes
go

create procedure EditPONotes(
	@PONumber bigint,
	@Notes varchar(300)
)
as
	update PurchaseOrder
	set Notes = @Notes
	where PONumber = @PONumber
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeletePurchaseOrder')
		drop procedure DeletePurchaseOrder
go

create procedure DeletePurchaseOrder(
	@PONumber bigint
)
as
	delete from PurchaseOrderLI where PONumber = @PONumber
	delete from PurchaseOrder where PONumber = @PONumber
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPurchaseOrders')
		drop procedure GetPurchaseOrders
go

create procedure GetPurchaseOrders
as
	select po.*,
	sum(li.Qty) as TotalQty,
	sum(li.TotalPrice) as TotalCost,
	(select count(PONumber) from PurchaseOrderLI where PONumber = po.PONumber and DateReceived is null) as NumOfPendingLIs,
	(select count(PONumber) from PurchaseOrderLI where PONumber = po.PONumber) as NumOfLIs
	from PurchaseOrder po
	inner join PurchaseOrderLI li on li.PONumber = po.PONumber
	group by po.PONumber, po.Username, po.TransactionDate, po.Notes
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPurchaseOrdersByUsername')
		drop procedure GetPurchaseOrdersByUsername
go

create procedure GetPurchaseOrdersByUsername(
	@Username varchar(35)
)
as
	select po.*,
	sum(li.Qty) as TotalQty,
	sum(li.TotalPrice) as TotalCost,
	(select count(PONumber) from PurchaseOrderLI where PONumber = po.PONumber and DateReceived is null) as NumOfPendingLIs,
	(select count(PONumber) from PurchaseOrderLI where PONumber = po.PONumber) as NumOfLIs
	from PurchaseOrder po
	inner join PurchaseOrderLI li on li.PONumber = po.PONumber
	where Username = @Username
	group by po.PONumber, po.Username, po.TransactionDate, po.Notes
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddPurchaseOrderLI')
		drop procedure AddPurchaseOrderLI
go

create procedure AddPurchaseOrderLI(
	@PONumber bigint,
	@ModelPartId int,
	@Qty int,
	@MPCostId int,
	@TotalPrice money
)
as
	declare @Cost as money = (select Cost from MPCost where MPCostId = @MPCostId)
	if(@TotalPrice is null)
	begin
	set @TotalPrice = @Cost * @Qty
	end
	insert into PurchaseOrderLI values(@PONumber, @ModelPartId, @Qty, @MPCostId, @TotalPrice, null)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddToPOLIQty')
		drop procedure AddToPOLIQty
go

create procedure AddToPOLIQty(
	@LineItemId int,
	@Qty int
)
as
	declare @MPCostId as int = (select mc.MPCostId from PurchaseOrderLI li
	inner join MPCost mc on li.MPCostId = mc.MPCostId
	where POLineItemId = @LineItemId
	and mc.ModelPartId = li.ModelPartId
	and EndDate is null)
	declare @Cost as money = (select Cost from MPCost where MPCostId = @MPCostId)

	update PurchaseOrderLI
	set Qty += @Qty,
	TotalPrice += @Cost * @Qty,
	MPCostId = @MPCostId
	where POLineItemId = @LineItemId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditPurchaseOrderLI')
		drop procedure EditPurchaseOrderLI
go

create procedure EditPurchaseOrderLI(
	@POLineItemId int,
	@Qty int,
	@TotalPrice money,
	@DateReceived datetime
)
as
	declare @MPCostId as money = (select MPCostId from PurchaseOrderLI where POLineItemId = @POLineItemId)
	declare @Cost as money = (select Cost from MPCost where MPCostId = @MPCostId)

	update PurchaseOrderLI
	set Qty = @Qty,
	DateReceived = @DateReceived
	where POLineItemId = @POLineItemId

	if(@TotalPrice is null)
	begin
	update PurchaseOrderLI
	set TotalPrice = @Qty * @Cost
	where POLineItemId = @POLineItemId
	end
	else
	begin
	update PurchaseOrderLI
	set TotalPrice = @TotalPrice
	where POLineItemId = @POLineItemId
	end
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeletePurchaseOrderLI')
		drop procedure DeletePurchaseOrderLI
go

create procedure DeletePurchaseOrderLI(
	@LineItemId int
)
as
	delete from PurchaseOrderLI
	where POLineItemId = @LineItemId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPurchaseOrderLIs')
		drop procedure GetPurchaseOrderLIs
go

create procedure GetPurchaseOrderLIs(
	@PONumber bigint
)
as
	select po.*, b.BrandName, m.ModelNumber, p.PartName, mc.Cost as UnitPrice from PurchaseOrderLI po
	inner join ModelPart mp on mp.ModelPartId = po.ModelPartId
	inner join MPCost mc on mc.MPCostId = po.MPCostId
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	where PONumber = @PONumber
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPurchaseOrderLI')
		drop procedure GetPurchaseOrderLI
go

create procedure GetPurchaseOrderLI(
	@POLineItemId int
)
as
	select po.*, b.BrandName, m.ModelNumber, p.PartName, mc.Cost as UnitPrice from PurchaseOrderLI po
	inner join ModelPart mp on mp.ModelPartId = po.ModelPartId
	inner join MPCost mc on mc.MPCostId = po.MPCostId
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	where POLineItemId = @POLineItemId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'TransferInventory')
		drop procedure TransferInventory
go

create procedure TransferInventory(
	@FromSource int,
	@ToSchool int,
	@Qty int,
	@Username varchar(35),
	@Recycled bit
)
as
	declare @fromMPID as int = (select ModelPartId from Inventory where InventoryId = @FromSource)
	declare @FromSchool as int = (select SchoolId from Inventory where InventoryId = @FromSource)
	declare @ToSource as int = (select InventoryId from Inventory where SchoolId = @ToSchool and ModelPartId = @fromMPID)
	declare @PartName as varchar(50) = (select b.BrandName + ' ' + m.ModelNumber + ' ' + p.PartName
	from ModelPart mp inner join Model m on mp.ModelId = m.ModelId
	inner join Part p on p.PartId = mp.PartId
	inner join Brand b on b.BrandId = m.BrandId
	where mp.ModelPartId = @fromMPID)

	declare @emptyInv as bit
	if @Recycled = 0
	begin
	set @emptyInv = case when exists (select * from Inventory where InventoryId = @FromSource and RecycledQty = 0 and Qty - @Qty = 0) then 1 else 0
	end
	end
	else
	begin
	set @emptyInv = case when exists (select * from Inventory where InventoryId = @FromSource and Qty = 0 and RecycledQty - @Qty = 0) then 1 else 0
	end
	end
	if @emptyInv = 1
	begin
	delete from Inventory
	where InventoryId = @FromSource
	end
	else
	begin
	if @Recycled = 0
	begin
	update Inventory
	set Qty -= @Qty
	where InventoryId = @FromSource
	end
	else
	begin
	update Inventory
	set RecycledQty -= @Qty
	where InventoryId = @FromSource
	end
	end

	if @ToSource is not null
	begin
	if @Recycled = 0
	begin
	update Inventory
	set Qty += @Qty
	where SchoolId = @ToSchool
	and ModelPartId = @fromMPID
	end
	else
	begin
	update Inventory
	set RecycledQty += @Qty
	where SchoolId = @ToSchool
	and ModelPartId = @fromMPID
	end
	end
	else
	begin
	if @Recycled = 0
	begin
	insert into Inventory values (@fromMPID, @ToSchool, @Qty, GETDATE(), @Username, 0)
	end
	else
	begin
	insert into Inventory values (@fromMPID, @ToSchool, 0, GETDATE(), @Username, @Qty)
	end
	set @ToSource = SCOPE_IDENTITY()
	end

	insert into InventoryTransfer values(@FromSchool, @ToSchool, @fromMPID, @PartName, GETDATE(), @Qty, @Username, @Recycled)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetInventoryTransfers')
		drop procedure GetInventoryTransfers
go

create procedure GetInventoryTransfers
as
	select t.*, fsch.name as FromSchoolName, tsch.name as ToSchoolName from InventoryTransfer t
	inner join ModelPart mp on mp.ModelPartId = t.ModelPartId
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	inner join Part p on p.PartId = mp.PartId
	inner join robbinsdale.dbo.School fsch on fsch.schoolID = t.FromSchool
	inner join robbinsdale.dbo.School tsch on tsch.schoolID = t.ToSchool
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddNotification')
		drop procedure AddNotification
go

create procedure AddNotification(
	@Username varchar(35),
	@NotifMessage varchar(100),
	@NotifType varchar(25),
	@FromUsername varchar(35),
	@SchoolId int,
	@ModelPartId int,
	@Qty int
)
as
	insert into [Notification] values (@Username, GETDATE(), @NotifType, @NotifMessage, 0, @FromUsername, @SchoolId, @ModelPartId, @Qty)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditNotification')
		drop procedure EditNotification
go

create procedure EditNotification(
	@NotificationId int,
	@NotifMessage varchar(100),
	@NotifType varchar(25)
)
as
	update [Notification]
	set NotifMessage = @NotifMessage,
	NotifType = @NotifType
	where NotificationId = @NotificationId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteNotification')
		drop procedure DeleteNotification
go

create procedure DeleteNotification(
	@NotificationId int
)
as
	delete from [Notification]
	where NotificationId = @NotificationId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'DeleteNotifications')
		drop procedure DeleteNotifications
go

create procedure DeleteNotifications(
	@Username varchar(35)
)
as
	delete from [Notification]
	where Username = @Username
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetNotifications')
		drop procedure GetNotifications
go

create procedure GetNotifications(
	@Username varchar(35)
)
as
	select * from [Notification]
	where Username = @Username
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'NotificationsSeen')
		drop procedure NotificationsSeen
go

create procedure NotificationsSeen(
	@Username varchar(35)
)
as
	update [Notification]
	set Seen = 1
	where Username = @Username
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'AddUser')
		drop procedure AddUser
go

create procedure AddUser(
	@Username varchar(35),
	@FirstName varchar(35),
	@LastName varchar(35),
	@Email varchar(50),
	@Role varchar(10),
	@AddedBy varchar(35)
)
as
	insert into [User] values (@Username, @FirstName, @LastName, @Email, @Role, GETDATE(), @AddedBy)
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditUser')
		drop procedure EditUser
go

create procedure EditUser(
	@Username varchar(35),
	@Email varchar(50),
	@Role varchar(10)
)
as
	update [User]
	set [Role] = @Role,
	Email = @Email
	where Username = @Username
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetUserDetails')
		drop procedure GetUserDetails
go

create procedure GetUserDetails(
	@Username varchar(35)
)
as
	select username, firstName, lastName, c.email
	from robbinsdale.dbo.[Identity] i
	inner join robbinsdale.dbo.UserAccount u on i.personID = u.personID
	inner join robbinsdale.dbo.Person p on i.personID = p.personID
	inner join robbinsdale.dbo.Contact c on c.personID = p.personID
	where username = @Username
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetInvEditHistory')
		drop procedure GetInvEditHistory
go

create procedure GetInvEditHistory
as
	select i.*, s.name as School from InventoryEditHistory i
	inner join robbinsdale.dbo.School s
	on s.schoolID = i.SchoolId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'EditInvEditNotes')
		drop procedure EditInvEditNotes
go

create procedure EditInvEditNotes(
	@InvEditId int,
	@Notes varchar(150)
)
as
	update InventoryEditHistory
	set Notes = @Notes
	where InvEditId = @InvEditId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetPartCostHistory')
		drop procedure GetPartCostHistory
go

create procedure GetPartCostHistory(
	@ModelPartId as int
)
as
	select mc.*, b.BrandName + ' ' + m.ModelNumber + ' ' + p.PartName as ModelPartName from MPCost mc
	inner join ModelPart mp on mc.ModelPartId = mp.ModelPartId
	inner join Part p on p.PartId = mp.PartId
	inner join Model m on m.ModelId = mp.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	where mc.ModelPartId = @ModelPartId
go

if exists (select * from INFORMATION_SCHEMA.ROUTINES
	where ROUTINE_NAME = 'GetModelCostHistory')
		drop procedure GetModelCostHistory
go

create procedure GetModelCostHistory(
	@ModelId as int
)
as
	select mc.*, b.BrandName + ' ' + m.ModelNumber as ModelName from ModelCost mc
	inner join Model m on m.ModelId = mc.ModelId
	inner join Brand b on b.BrandId = m.BrandId
	where mc.ModelId = @ModelId
go

