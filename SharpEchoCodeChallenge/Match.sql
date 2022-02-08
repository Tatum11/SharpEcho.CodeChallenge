CREATE TABLE [dbo].[Match]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [HomeTeamId] BIGINT NOT NULL, 
    [VisitorTeamId] BIGINT NOT NULL, 
    [WinnerId] BIGINT NOT NULL, 
    [MatchDate] DATE NOT NULL, 
    CONSTRAINT [FK_Match_Team] FOREIGN KEY ([HomeTeamId]) REFERENCES [Team]([Id]), 
    CONSTRAINT [FK_Match_Team2] FOREIGN KEY ([VisitorTeamId]) REFERENCES [Team]([Id]), 
    CONSTRAINT [FK_Match_Team3] FOREIGN KEY ([WinnerId]) REFERENCES [Team]([Id]), 

   
)

GO

