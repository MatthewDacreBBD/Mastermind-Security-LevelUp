USE master

IF EXISTS(select * from sys.databases where name='MastermindDB')
DROP DATABASE MastermindDB;

CREATE DATABASE MastermindDB;
GO

USE MastermindDB;

CREATE TABLE [AppUser] (
  [Id] integer PRIMARY KEY IDENTITY(1, 1),
  [username] varchar(max) NOT NULL,
  [password] varchar(max) NOT NULL,
  [passwordSalt] varchar(max) NOT NULL,
  [wins] int NOT NULL,
  [losses] int NOT NULL,
)
GO

CREATE TABLE [Game] (
  [Id] integer PRIMARY KEY IDENTITY(1, 1),
  [userId] int NOT NULL,
  [gameStatus] varchar(max) NOT NULL,
  [score] int NOT NULL,
)
GO

ALTER TABLE [Game] ADD FOREIGN KEY ([userId]) REFERENCES [AppUser] ([Id])
GO
