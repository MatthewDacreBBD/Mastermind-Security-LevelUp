USE master

IF EXISTS(select * from sys.databases where name='GameDB')
DROP DATABASE GameDB;

CREATE DATABASE GameDB;
GO


CREATE TABLE [Game] (
  [Id] integer PRIMARY KEY IDENTITY(1, 1),
  [userId] int NOT NULL,
  [gameStatus] varchar(max) NOT NULL,
  [score] int NOT NULL,
)
GO