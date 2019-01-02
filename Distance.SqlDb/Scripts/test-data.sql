DECLARE @Count INT = 1000

WHILE @Count > 0 BEGIN

    DECLARE @Lan NUMERIC (7, 4) = 90 - RAND() * 180
    DECLARE @Lon NUMERIC (7, 4) = 180 - RAND() * 360

    INSERT [dbo].[Locations] ([Address], [Coordinate]) VALUES
    (
        CONCAT('TEST_', @Count),
        GEOGRAPHY::STPointFromText(CONCAT('POINT(', @Lon, ' ', @Lan, ')'), 4326)
    )

    SET @Count = @Count - 1

END
GO