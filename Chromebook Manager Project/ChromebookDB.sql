use EmekaTestDb
go

create table [User] (
	Username varchar(35) primary key not null,
	FirstName varchar(35) not null,
	LastName varchar(35),
	Email varchar(50) not null,
	[Role] varchar(10) not null,
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table Brand (
	BrandId int identity(1,1) primary key not null,
	BrandName varchar(35) not null,
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table Model (
	ModelId int identity(1,1) primary key not null,
	ModelNumber varchar(15) not null,
	BrandId int foreign key references Brand(BrandId) not null,
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table ModelCost (
	ModelCostId int identity(1,1) primary key not null,
	ModelId int foreign key references Model(ModelId) not null,
	Cost money not null,
	StartDate datetime not null,
	EndDate datetime,
	ModifiedBy varchar(35) not null
)

create table Part (
	PartId int identity(1,1) primary key not null,
	PartName varchar(15) not null,
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table ModelPart (
	ModelPartId int identity(1,1) primary key not null,
	ModelId int foreign key references Model(ModelId) not null,
	PartId int foreign key references Part(PartId) not null,
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table MPCost (
	MPCostId int identity(1,1) primary key not null,
	ModelPartId int foreign key references ModelPart(ModelPartId) not null,
	Cost money not null,
	StartDate datetime not null,
	EndDate datetime
)

create table School (
	SchoolId int primary key not null
)

create table Client (
	ClientId int identity(1,1) primary key not null,
	StudentNumber int,
	FirstName varchar(35) not null,
	LastName varchar(35) not null,
	Username varchar(35),
	Grade int,
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table Device (
	Barcode varchar(10) primary key not null,
	SerialNumber varchar(10) not null,
	StorageCapacity varchar(6),
	ModelId int foreign key references Model(ModelId) not null,
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table ClientDevice (
	ClientDeviceId int identity(1,1) primary key not null,
	ClientId int foreign key references Client(ClientId) not null,
	Barcode varchar(10) foreign key references Device(Barcode) not null,
	SchoolId int not null,
	Loaner bit not null,
	StartDate date not null,
	EndDate date,
	AddedBy varchar(35) not null
)

create table IssueType (
	IssueId int identity(1,1) primary key not null,
	IssueName varchar(50) not null,
	IssueDescription varchar(300),
	DateAdded datetime not null,
	AddedBy varchar(35) not null
)

create table RepairLog (
	RepairId int identity(1,1) primary key not null,
	RepairTimestamp datetime not null,
	ClientDeviceId int foreign key references ClientDevice(ClientDeviceId) not null,
	IssueId int foreign key references IssueType(IssueId) not null,
	IssueDescription varchar(300) not null,
	RepairNotes varchar(200),
	RepairReturnedDate date,
	EmailAddress varchar(50) not null,
	Notes varchar(200),
	WarrantyRepairSentDate date,
	AddedBy varchar(35) not null
)

create table Inventory (
	InventoryId int identity(1,1) primary key not null,
	ModelPartId int foreign key references ModelPart(ModelPartId) not null,
	SchoolId int foreign key references School(SchoolId),
	Qty int not null,
	RecycledQty int not null,
	DateLastModified datetime not null,
	LastModifiedBy varchar(35) not null
)

create table PartsUsed (
	PartUsedId int identity(1,1) primary key not null,
	RepairId int foreign key references RepairLog(RepairId) not null,
	ModelPartId int foreign key references ModelPart(ModelPartId) not null,
	SchoolId int foreign key references School(SchoolId),
	PartName varchar(50) not null,
	MPCostId int foreign key references MPCost(MPCostId) not null,
	Recycled bit not null
)

create table PurchaseOrder (
	PONumber bigint primary key not null,
	Username varchar(35) not null,
	TransactionDate datetime not null,
	Notes varchar(300)
)

create table PurchaseOrderLI (
	POLineItemId int identity(1,1) primary key not null,
	PONumber bigint foreign key references PurchaseOrder(PONumber) not null,
	ModelPartId int foreign key references ModelPart(ModelPartId) not null,
	Qty int not null,
	MPCostId int foreign key references MPCost(MPCostID) not null,
	TotalPrice money not null,
	DateReceived datetime
)

create table InventoryTransfer (
	InvTransferId int identity(1,1) primary key not null,
	FromSchool int not null,
	ToSchool int not null,
	ModelPartId int not null,
	PartName varchar(50) not null,
	TransferDate datetime not null,
	Qty int not null,
	Username varchar(35) not null,
	Recycled bit not null
)

create table [Notification] (
	NotificationId int identity(1,1) primary key not null,
	Username varchar(35) foreign key references [User](Username) not null,
	NotifDate datetime not null,
	NotifType varchar(25) not null,
	NotifMessage varchar(100) not null,
	Seen bit not null,
	FromUsername varchar(35),
	SchoolId int,
	ModelPartId int,
	Qty int
)

create table InventoryEditHistory (
	InvEditId int identity(1,1) primary key not null,
	ModelPartId int not null,
	SchoolId int not null,
	PartName varchar(50) not null,
	OldQty int not null,
	NewQty int not null,
	Notes varchar(150),
	DateModified datetime not null,
	ModifiedBy varchar(35) not null,
	Recycled bit not null
)