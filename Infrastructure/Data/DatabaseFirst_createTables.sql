DROP TABLE StoresEntity
DROP TABLE ProductsEntity

CREATE TABLE StoresEntity
(
	Id int not null identity primary key,
	StoreName nvarchar(50) not null unique
)

CREATE TABLE ProductsEntity
(
	Id int not null primary key,
	ProductName nvarchar(50) not null,
	Price money,
	StoreId int null references StoresEntity(Id)
)

