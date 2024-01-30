DROP TABLE ProductsEntity
DROP TABLE StoresEntity

CREATE TABLE StoresEntity
(
	Id int not null identity primary key,
	StoreName nvarchar(50) not null unique
)

CREATE TABLE ProductsEntity
(
	Id int not null primary key identity,
	ProductName nvarchar(50) not null,
	Price money,
	StoreId int null references StoresEntity(Id)
)

