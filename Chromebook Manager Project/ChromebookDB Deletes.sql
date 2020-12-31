use EmekaTestDb
go

if exists(select * from sys.tables where name='InventoryEditHistory')
	drop table InventoryEditHistory
go

if exists(select * from sys.tables where name='Notification')
	drop table [Notification]
go

if exists(select * from sys.tables where name='InventoryTransfer')
	drop table InventoryTransfer
go

if exists(select * from sys.tables where name='PurchaseOrderLI')
	drop table PurchaseOrderLI
go

if exists(select * from sys.tables where name='PurchaseOrder')
	drop table PurchaseOrder
go

if exists(select * from sys.tables where name='PartsUsed')
	drop table PartsUsed
go

if exists(select * from sys.tables where name='Inventory')
	drop table Inventory
go

if exists(select * from sys.tables where name='RepairLog')
	drop table RepairLog
go

if exists(select * from sys.tables where name='IssueType')
	drop table IssueType
go

if exists(select * from sys.tables where name='ClientDevice')
	drop table ClientDevice
go

if exists(select * from sys.tables where name='Device')
	drop table Device
go

if exists(select * from sys.tables where name='Client')
	drop table Client
go

if exists(select * from sys.tables where name='School')
	drop table School
go

if exists(select * from sys.tables where name='MPCost')
	drop table MPCost
go

if exists(select * from sys.tables where name='ModelPart')
	drop table ModelPart
go

if exists(select * from sys.tables where name='Part')
	drop table Part
go

if exists(select * from sys.tables where name='Model')
	drop table Model
go

if exists(select * from sys.tables where name='Brand')
	drop table Brand
go

if exists(select * from sys.tables where name='User')
	drop table [User]
go