CREATE TABLE [dbo].[Game]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [gameStatus] VARCHAR(50) NULL, 
    [score] INT NULL,
    FOREIGN KEY ([userId]) REFERENCES [dbo].[User] ([Id]),
)
