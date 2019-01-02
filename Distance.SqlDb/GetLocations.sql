CREATE PROCEDURE [dbo].[GetLocations]
    @Longitude NUMERIC(18, 8),
    @Latitude NUMERIC(18, 8),
    @Count INT NULL,
    @Distance INT NULL
AS BEGIN
    DECLARE @Point GEOGRAPHY = CONCAT('POINT(', @Longitude,' ', @Latitude,')')
    DECLARE @CountStatement VARCHAR(4000) = ''
    IF @Count IS NOT NULL BEGIN
        SET @CountStatement = CONCAT('TOP(', @Count, ')')
    END
    
    DECLARE @Statement VARCHAR(4000) = CONCAT(
        'SELECT ', @CountStatement, ' [Address], 
            [Coordinate].Long AS [Longitude], [Coordinate].Lat AS [Latitude], 
            [Coordinate].STDistance(@Point) AS [Distance]
            FROM [dbo].[Locations]
            WHERE @Distance IS NULL OR [Coordinate].STDistance(@Point) <= @Distance
            ORDER BY [Coordinate].STDistance(@Point)')

    EXECUTE sp_executesql @Statement, N'@Distance INT, @Point GEOGRAPHY', @Distance, @Point

END
GO
