CREATE TABLE [dbo].[Game] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [userId]     INT          NOT NULL,
    [gameStatus] VARCHAR (50) NULL,
    [score]      INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
);
