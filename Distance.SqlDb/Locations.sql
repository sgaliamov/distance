CREATE TABLE [dbo].[Locations]
(
    [Id] [BIGINT] IDENTITY(1,1) NOT NULL,
    [Address] [NVARCHAR](max) NOT NULL,
    [Coordinate] [GEOGRAPHY] NOT NULL,

    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE SPATIAL INDEX [SI_Locations_Coordinate]
    ON [dbo].[Locations] ([Coordinate])
GO
