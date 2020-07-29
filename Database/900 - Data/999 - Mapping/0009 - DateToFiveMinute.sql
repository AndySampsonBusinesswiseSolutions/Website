USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

--if last sunday of march, ignore 01:00 to 01:55
--if last sunday of october, add ???

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
    [FiveMinute].FiveMinuteId
INTO
    #DateToFiveMinuteMapping
FROM
	[Information].[Date]
LEFT OUTER JOIN
	#ClockChangeDate ClockChangeDate
	ON ClockChangeDate.ClockChangeDate = [Date].[DateDescription]
CROSS JOIN
    [Information].[FiveMinute]
WHERE
	ClockChangeDate.ClockChangeDate IS NULL
    AND FiveMinute.FiveMinuteDescription NOT IN ('Extra Day Five Minutes')--TODO

INSERT INTO
    #DateToFiveMinuteMapping
    (
        DateId,
        FiveMinuteId
    )
SELECT
	[Date].DateId,
    [FiveMinute].FiveMinuteId
FROM
	[Information].[Date]
INNER JOIN
	#ClockChangeDate ClockChangeDate
	ON ClockChangeDate.ClockChangeDate = [Date].[DateDescription]
CROSS JOIN
    [Information].[FiveMinute]
WHERE
	DATEPART(MONTH, ClockChangeDate.ClockChangeDate) = 3
    AND FiveMinute.FiveMinuteDescription NOT IN ('Five Minute 13', 'Five Minute 14', 'Five Minute 15', 'Five Minute 16', 'Five Minute 17', 'Five Minute 18', 'Five Minute 19', 'Five Minute 20', 'Five Minute 21', 'Five Minute 22', 'Five Minute 23', 'Five Minute 24')
    AND FiveMinute.FiveMinuteDescription NOT IN ('Extra Day Five Minutes') --TODO

INSERT INTO
    #DateToFiveMinuteMapping
    (
        DateId,
        FiveMinuteId
    )
SELECT
	[Date].DateId,
    [FiveMinute].FiveMinuteId
FROM
	[Information].[Date]
INNER JOIN
	#ClockChangeDate ClockChangeDate
	ON ClockChangeDate.ClockChangeDate = [Date].[DateDescription]
CROSS JOIN
    [Information].[FiveMinute]
WHERE
	DATEPART(MONTH, ClockChangeDate.ClockChangeDate) = 10

DECLARE 
@DateId BIGINT,
@FiveMinuteId BIGINT

DECLARE DateToFiveMinuteMappingCursor CURSOR FOR
SELECT 
	DateId,
	FiveMinuteId
FROM #DateToFiveMinuteMapping

OPEN DateToFiveMinuteMappingCursor

FETCH NEXT FROM DateToFiveMinuteMappingCursor
INTO @DateId, @FiveMinuteId

WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [Mapping].[DateToFiveMinute_Insert] @CreatedByUserId, @SourceId, @DateId, @FiveMinuteId

		FETCH NEXT FROM DateToFiveMinuteMappingCursor
		INTO @DateId, @FiveMinuteId
	END
CLOSE DateToFiveMinuteMappingCursor;
DEALLOCATE DateToFiveMinuteMappingCursor;

DROP TABLE #ClockChangeDate
DROP TABLE #DateToFiveMinuteMapping