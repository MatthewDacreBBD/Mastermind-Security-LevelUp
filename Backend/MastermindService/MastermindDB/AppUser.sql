CREATE TABLE [dbo].[AppUser]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY , 
    [username] VARCHAR(MAX) NOT NULL, 
    [password] VARCHAR(MAX) NOT NULL, 
    [wins] INT NULL, 
    [losses] INT NULL,
    [authenticationToken] VARCHAR(MAX) NULL
)
