USE master

IF EXISTS(select * from sys.databases where name='IdentityDB')
DROP DATABASE IdentityDB;

CREATE DATABASE IdentityDB;
GO

USE IdentityDB;

CREATE TABLE [AppUser] (
  [Id] integer PRIMARY KEY IDENTITY(1, 1),
  [username] varchar(max) NOT NULL,
  [password] varchar(max) NOT NULL,
  [wins] int NOT NULL,
  [losses] int NOT NULL,
  [authenticationToken] varchar(max) NOT NULL
)
GO


