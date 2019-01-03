CREATE PROCEDURE [dbo].[GetLocations]
    @Longitude NUMERIC(12, 8),
    @Latitude NUMERIC(12, 8),
    @Count INT NULL,
    @Distance INT NULL
AS BEGIN
    DECLARE @Point GEOGRAPHY = CONCAT('POINT(', @Longitude,' ', @Latitude,')')

    IF @Count IS NOT NULL AND @Distance IS NOT NULL BEGIN
        SELECT TOP(@Count)
                [Address],
                [Coordinate].Long AS [Longitude],
                [Coordinate].Lat AS [Latitude],
                [Coordinate].STDistance(@Point) AS [Distance]
            FROM [dbo].[Locations]
            WHERE [Coordinate].STDistance(@Point) <= @Distance
            ORDER BY [Coordinate].STDistance(@Point)
    END 
    ELSE IF @Count IS NULL AND @Distance IS NOT NULL BEGIN
        SELECT    [Address], 
                [Coordinate].Long AS [Longitude],
                [Coordinate].Lat AS [Latitude],
                [Coordinate].STDistance(@Point) AS [Distance]
            FROM [dbo].[Locations]
            WHERE [Coordinate].STDistance(@Point) <= @Distance
            ORDER BY [Coordinate].STDistance(@Point)
    END 
    ELSE IF @Count IS NOT NULL AND @Distance IS NULL BEGIN
        SELECT TOP(@Count)
                [Address],
                [Coordinate].Long AS [Longitude], [Coordinate].Lat AS [Latitude],
                [Coordinate].STDistance(@Point) AS [Distance]
            FROM [dbo].[Locations]
            ORDER BY [Coordinate].STDistance(@Point)
    END 
    ELSE IF @Count IS NULL AND @Distance IS NULL BEGIN
        SELECT    [Address], 
                [Coordinate].Long AS [Longitude],
                [Coordinate].Lat AS [Latitude], 
                [Coordinate].STDistance(@Point) AS [Distance]
            FROM [dbo].[Locations]
            ORDER BY [Coordinate].STDistance(@Point)
    END

END
GO
