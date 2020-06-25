USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND SourceTypeEntityId = 0)

DECLARE @YearTemp TABLE
(
	YearDescription INT,
	IsLeapYear BIT
)

INSERT INTO @YearTemp
			(YearDescription)
SELECT ones.n + 10*tens.n + 100*hundreds.n + 1000*thousands.n
FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) ones(n),
     (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) tens(n),
     (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) hundreds(n),
     (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9)) thousands(n)
WHERE ones.n + 10*tens.n + 100*hundreds.n + 1000*thousands.n BETWEEN 2000 AND 2030

UPDATE
	@YearTemp
SET
	IsLeapYear = CASE WHEN datediff(d, dateadd(yyyy, YearDescription-1900, 0), dateadd(yyyy, 1, dateadd(yyyy, YearDescription-1900, 0))) = 366 THEN 1 ELSE 0 END

DECLARE @YearDescription VARCHAR(255)
DECLARE @IsLeapYear BIT

DECLARE YearDescriptionCursor CURSOR FOR
SELECT YearDescription, IsLeapYear
FROM @YearTemp

OPEN YearDescriptionCursor

FETCH NEXT FROM YearDescriptionCursor
INTO @YearDescription, @IsLeapYear

WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [Information].[Year_Insert] @CreatedByUserId, @SourceId, @YearDescription, @IsLeapYear

		FETCH NEXT FROM YearDescriptionCursor
		INTO @YearDescription, @IsLeapYear
	END
CLOSE YearDescriptionCursor;
DEALLOCATE YearDescriptionCursor;