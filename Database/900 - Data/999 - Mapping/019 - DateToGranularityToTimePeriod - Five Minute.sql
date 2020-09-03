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
	[TimePeriodDetail].TimePeriodDetailDescription
INTO
	#GranularityToTimePeriodTemp
FROM
	[Mapping].[GranularityToTimePeriod]
INNER JOIN
	[Information].[Granularity]
	ON [Granularity].GranularityId = [GranularityToTimePeriod].GranularityId
	AND [Granularity].GranularityDescription IN('Five Minute')
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

SELECT
	@CreatedByUserId CreatedByUserId,
	@SourceId SourceId,
	d.DateId,
	g.GranularityToTimePeriodId
INTO 
	#FiveMinuteTemp1
FROM
	#DateTemp d
CROSS APPLY
	#GranularityToTimePeriodTemp g
WHERE
	d.DateDetailDescription IS NULL
	AND g.TimePeriodDetailDescription IS NULL

INSERT INTO
	[Mapping].[DateToGranularityToTimePeriod]
	(
		CreatedByUserId,
		SourceId,
		DateId,
		GranularityToTimePeriodId
	)
SELECT
	CreatedByUserId,
	SourceId,
	DateId,
	GranularityToTimePeriodId
FROM
	#FiveMinuteTemp1

SELECT
	@CreatedByUserId CreatedByUserId,
	@SourceId SourceId,
	d.DateId,
	g.GranularityToTimePeriodId
INTO 
	#FiveMinuteTemp2
FROM
	#DateTemp d
CROSS APPLY
	#GranularityToTimePeriodTemp g
WHERE
	d.DateDetailDescription IS NOT NULL

INSERT INTO
	[Mapping].[DateToGranularityToTimePeriod]
	(
		CreatedByUserId,
		SourceId,
		DateId,
		GranularityToTimePeriodId
	)
SELECT
	CreatedByUserId,
	SourceId,
	DateId,
	GranularityToTimePeriodId
FROM
	#FiveMinuteTemp2

DROP TABLE #FiveMinuteTemp1
DROP TABLE #FiveMinuteTemp2
DROP TABLE #GranularityToTimePeriodTemp
DROP TABLE #DateTemp