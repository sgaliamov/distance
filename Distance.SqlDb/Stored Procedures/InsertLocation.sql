CREATE PROCEDURE [dbo].[InsertLocation]
    @Longitude NUMERIC(12, 8),
    @Latitude NUMERIC(12, 8),
    @Address NVARCHAR(MAX)
AS BEGIN

    INSERT [dbo].[Locations] ([Address], [Coordinate]) 
        OUTPUT INSERTED.[Id]
        VALUES (@Address, GEOGRAPHY::STPointFromText(CONCAT('POINT(', @Longitude, ' ', @Latitude, ')'), 4326))

END
GO
