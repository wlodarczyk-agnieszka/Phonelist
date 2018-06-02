USE Phonelist

create table People (
ID int identity,
FirstName varchar(255) not null,
LastName varchar(255) DEFAULT NULL,
Phone varchar(30) not null,
Email varchar(255) DEFAULT NULL,
Created datetime DEFAULT GETDATE(),
Updated datetime DEFAULT NULL
);