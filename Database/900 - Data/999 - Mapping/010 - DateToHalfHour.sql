USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

--if last sunday of march, ignore 01:00 to 01:30
--if last sunday of october, add 01:31 and 01:32

SELECT
	DATEADD(DAY, -7, CONVERT(DATE, [Date].[DateDescription])) ClockChangeDate
INTO 
    #ClockChangeDate
FROM
	[Information].[Date]
INNER JOIN
	[Mapping].[DateToForecastGroup]
	on DateToForecastGroup.DateId = Date.DateId
INNER JOIN
	[DemandForecast].[ForecastGroup]
	on ForecastGroup.ForecastGroupId = DateToForecastGroup.ForecastGroupId
	and ForecastGroup.ForecastGroupDescription IN ('1st Sunday in April','1st Sunday in November')

SELECT
	[Date].DateId,
    [HalfHour].HalfHourId
INTO
    #DateToHalfHourMapping
FROM
	[Information].[Date]
LEFT OUTER JOIN
	#ClockChangeDate ClockChangeDate
	ON ClockChangeDate.ClockChangeDate = [Date].[DateDescription]
CROSS JOIN
    [Information].[HalfHour]
WHERE
	ClockChangeDate.ClockChangeDate IS NULL
    AND HalfHour.HalfHourDescription NOT IN ('Half Hour 49', 'Half Hour 50')

INSERT INTO
    #DateToHalfHourMapping
    (
        DateId,
        HalfHourId
    )
SELECT
	[Date].DateId,
    [HalfHour].HalfHourId
FROM
	[Information].[Date]
INNER JOIN
	#ClockChangeDate ClockChangeDate
	ON ClockChangeDate.ClockChangeDate = [Date].[DateDescription]
CROSS JOIN
    [Information].[HalfHour]
WHERE
	DATEPART(MONTH, ClockChangeDate.ClockChangeDate) = 3
    AND HalfHour.HalfHourDescription NOT IN ('Half Hour 3', 'Half Hour 4', 'Half Hour 49', 'Half Hour 50')

INSERT INTO
    #DateToHalfHourMapping
    (
        DateId,
        HalfHourId
    )
SELECT
	[Date].DateId,
    [HalfHour].HalfHourId
FROM
	[Information].[Date]
INNER JOIN
	#ClockChangeDate ClockChangeDate
	ON ClockChangeDate.ClockChangeDate = [Date].[DateDescription]
CROSS JOIN
    [Information].[HalfHour]
WHERE
	DATEPART(MONTH, ClockChangeDate.ClockChangeDate) = 10

DECLARE 
@DateId BIGINT,
@HalfHourId BIGINT

DECLARE DateToHalfHourMappingCursor CURSOR FOR
SELECT 
	DateId,
	HalfHourId
FROM #DateToHalfHourMapping

OPEN DateToHalfHourMappingCursor

FETCH NEXT FROM DateToHalfHourMappingCursor
INTO @DateId, @HalfHourId

WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [Mapping].[DateToHalfHour_Insert] @CreatedByUserId, @SourceId, @DateId, @HalfHourId

		FETCH NEXT FROM DateToHalfHourMappingCursor
		INTO @DateId, @HalfHourId
	END
CLOSE DateToHalfHourMappingCursor;
DEALLOCATE DateToHalfHourMappingCursor;

DROP TABLE #ClockChangeDate
DROP TABLE #DateToHalfHourMapping