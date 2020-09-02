USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE 
	@DateId BIGINT,
	@GranularityToTimePeriodId BIGINT

SELECT
	[Date].DateId,
	[DateDetail].DateDetailDescription
INTO
	#DateTemp
FROM
	[Information].[Date]
LEFT OUTER JOIN
	[Information].[DateDetail]
	ON [DateDetail].DateId = [Date].DateId
	AND [DateDetail].EffectiveToDateTime = '9999-12-31'
LEFT OUTER JOIN
	[Information].[DateAttribute]
	ON [DateAttribute].DateAttributeId = [DateDetail].DateAttributeId
	AND [DateAttribute].DateAttributeDescription = 'UK October Clock Change'
	AND [DateAttribute].EffectiveToDateTime = '9999-12-31'
WHERE
	[Date].EffectiveToDateTime = '9999-12-31'

SELECT DISTINCT
	[GranularityToTimePeriod].GranularityToTimePeriodId,
	[GranularityToTimePeriod].GranularityId,
	[TimePeriodDetail].TimePeriodDetailDescription
INTO
	#GranularityToTimePeriodTemp
FROM
	[Mapping].[GranularityToTimePeriod]
INNER JOIN
	[Information].[Granularity]
	ON [Granularity].GranularityId = [GranularityToTimePeriod].GranularityId
	AND [Granularity].GranularityDescription IN('Half Hour')
LEFT OUTER JOIN
	[Information].[TimePeriodDetail]
	ON [TimePeriodDetail].TimePeriodId = [GranularityToTimePeriod].TimePeriodId
	AND [TimePeriodDetail].EffectiveToDateTime = '9999-12-31'
LEFT OUTER JOIN
	[Information].[TimePeriodAttribute]
	ON [TimePeriodAttribute].TimePeriodAttributeId = [TimePeriodDetail].TimePeriodAttributeId
	AND [TimePeriodAttribute].TimePeriodAttributeDescription = 'Is Additional Time Period'
	AND [TimePeriodAttribute].EffectiveToDateTime = '9999-12-31'
WHERE
	[GranularityToTimePeriod].EffectiveToDateTime = '9999-12-31'

DECLARE GranularityToTimePeriodCursor CURSOR FOR
SELECT GranularityToTimePeriodId
FROM #GranularityToTimePeriodTemp
WHERE TimePeriodDetailDescription IS NULL

OPEN GranularityToTimePeriodCursor

FETCH NEXT FROM GranularityToTimePeriodCursor
INTO @GranularityToTimePeriodId

WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE DateCursor CURSOR FOR
		SELECT DateId
		FROM #DateTemp
		WHERE DateDetailDescription IS NULL

		OPEN DateCursor

		FETCH NEXT FROM DateCursor
		INTO @DateId

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC [Mapping].[DateToGranularityToTimePeriod_Insert] @CreatedByUserId, @SourceId, @DateId, @GranularityToTimePeriodId

			FETCH NEXT FROM DateCursor
			INTO @DateId
		END

		CLOSE DateCursor;
		DEALLOCATE DateCursor;

		FETCH NEXT FROM GranularityToTimePeriodCursor
		INTO @GranularityToTimePeriodId
	END
CLOSE GranularityToTimePeriodCursor;
DEALLOCATE GranularityToTimePeriodCursor;

DECLARE GranularityToTimePeriodCursor CURSOR FOR
SELECT GranularityToTimePeriodId
FROM #GranularityToTimePeriodTemp

OPEN GranularityToTimePeriodCursor

FETCH NEXT FROM GranularityToTimePeriodCursor
INTO @GranularityToTimePeriodId

WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE DateCursor CURSOR FOR
		SELECT DateId
		FROM #DateTemp
		WHERE DateDetailDescription IS NOT NULL

		OPEN DateCursor

		FETCH NEXT FROM DateCursor
		INTO @DateId

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC [Mapping].[DateToGranularityToTimePeriod_Insert] @CreatedByUserId, @SourceId, @DateId, @GranularityToTimePeriodId

			FETCH NEXT FROM DateCursor
			INTO @DateId
		END

		CLOSE DateCursor;
		DEALLOCATE DateCursor;

		FETCH NEXT FROM GranularityToTimePeriodCursor
		INTO @GranularityToTimePeriodId
	END
CLOSE GranularityToTimePeriodCursor;
DEALLOCATE GranularityToTimePeriodCursor;

DROP TABLE #GranularityToTimePeriodTemp

DROP TABLE #DateTemp