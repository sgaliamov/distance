DECLARE @Count INT = 1000000
DECLARE @Index INT = 0

WHILE @Index < @Count BEGIN

    DECLARE @Lan NUMERIC (7, 4) = 90 - RAND() * 180
    DECLARE @Lon NUMERIC (7, 4) = 180 - RAND() * 360

    INSERT [dbo].[Locations] ([Address], [Coordinate]) VALUES
    (
        CONCAT('TEST_', @Index),
        GEOGRAPHY::STPointFromText(CONCAT('POINT(', @Lon, ' ', @Lan, ')'), 4326)
    )

    SET @Index = @Index + 1

END
GO