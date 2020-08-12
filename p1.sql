use master
go

create database PizzaBoxDB
go

use PizzaBoxDB
go

select * from __EFMigrationsHistory
go

select * from dbo.Pizza;
select * from dbo.Crust;
select * from dbo.Size;
select * from dbo.Topping;
select * from dbo.PizzaToppings;
go

go;

select * from Specialty

drop table dbo.__EFMigrationsHistory;
drop table dbo.Crust;
drop table dbo.Login;
drop table dbo.[Order];
drop table dbo.OrderPizzas;
drop table dbo.Pizza;
drop table dbo.PizzaToppings;
drop table dbo.Size;
drop table dbo.SpecialtyPizzas;
drop table dbo.Store;
drop table dbo.Topping;
drop table dbo.[User];
go

insert into dbo.Login ([Username], [Password])
values ('jpchionglo','password');
go

select * from dbo.LOGIN
go

insert into dbo.[User] ([Name], LoginId)
values ('Jeremy', 1);
go

select * from dbo.[User]
go

insert into dbo.Login ([Username], [Password])
values ('oakwood', 'password');
go

insert into dbo.Store ([Name], LoginId)
values ('Oakwood', 1);
go

select * from dbo.Store
go

select * from dbo.[ORDER]
go

select * from dbo.OrderPizzas
go