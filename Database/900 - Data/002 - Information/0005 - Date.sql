USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @DateTemp TABLE
(
	DateDescription DATE,
	DayOfTheWeek VARCHAR(10),
	Month VARCHAR(10),
	Year INT,
	Week INT
)

DECLARE @StartDate DATE = '2015-01-01'
DECLARE @EndDate DATE = '2030-12-31'
DECLARE @TempDate DATE

SET @TempDate = @StartDate

WHILE @TempDate <= @EndDate
	BEGIN
		INSERT INTO @DateTemp
					(DateDescription
					,DayOfTheWeek
					,Month
					,Year
					,Week)
		VALUES
					(@TempDate,DATENAME(dw, @TempDate),DATENAME(mm, @TempDate),DATEPART(year, @TempDate),DATEPART(wk, @TempDate))

		SET @TempDate = DATEADD(DAY, 1, @TempDate)
	END

SELECT
	DateTemp.DateDescription,
	DayOfTheWeek.DayOfTheWeekId,
	Month.MonthId,
	Year.YearId,
	Week.WeekId
INTO 
	#DateTempTable
FROM
	@DateTemp DateTemp
LEFT OUTER JOIN
	[Information].DayOfTheWeek
	ON [DayOfTheWeek].DayOfTheWeekDescription = DateTemp.DayOfTheWeek
LEFT OUTER JOIN
	[Information].[Month]
	ON [Month].MonthDescription = DateTemp.Month
LEFT OUTER JOIN
	[Information].[Year]
	ON [Year].YearDescription = DateTemp.Year
LEFT OUTER JOIN
	[Information].[Date]
	ON [Date].DateDescription = DateTemp.DateDescription
LEFT OUTER JOIN
	[Information].[Week]
	ON [Week].WeekDescription = 'Week ' + DateTemp.Week
WHERE
	[Date].DateId IS NULL
ORDER BY
	[DateTemp].DateDescription

DECLARE @DateDescription VARCHAR(255),
@DayOfTheWeekId BIGINT,
@MonthId BIGINT,
@YearId BIGINT,
@WeekId BIGINT

DECLARE DateDescriptionCursor CURSOR FOR
SELECT DateDescription,
	DayOfTheWeekId,
	MonthId,
	YearId,
	WeekId
FROM #DateTempTable

OPEN DateDescriptionCursor

FETCH NEXT FROM DateDescriptionCursor
INTO @DateDescription, @DayOfTheWeekId, @MonthId, @YearId, @WeekId

WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [Information].[Date_Insert] @CreatedByUserId, @SourceId, @DateDescription, @DayOfTheWeekId, @MonthId, @YearId, @WeekId

		FETCH NEXT FROM DateDescriptionCursor
		INTO @DateDescription, @DayOfTheWeekId, @MonthId, @YearId, @WeekId
	END
CLOSE DateDescriptionCursor;
DEALLOCATE DateDescriptionCursor;

DROP TABLE #DateTempTable
GO