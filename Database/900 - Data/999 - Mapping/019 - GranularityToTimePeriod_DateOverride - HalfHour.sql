USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityDescription = 'Half Hour')

SELECT
	[Date].DateId
INTO
	#DateTemp
FROM
	[Information].[Date]
INNER JOIN
	[Information].[DateDetail]
	ON [DateDetail].DateId = [Date].DateId
	AND [DateDetail].EffectiveToDateTime = '9999-12-31'
INNER JOIN
	[Information].[DateAttribute]
	ON [DateAttribute].DateAttributeId = [DateDetail].DateAttributeId
	AND [DateAttribute].DateAttributeDescription = 'UK October Clock Change'
	AND [DateAttribute].EffectiveToDateTime = '9999-12-31'
WHERE
	[Date].EffectiveToDateTime = '9999-12-31'

INSERT INTO [Mapping].GranularityToTimePeriod_DateOverride
    (
        CreatedByUserId,
        SourceId,
        GranularityId,
        TimePeriodId,
		DateId
    )
SELECT
	@CreatedByUserId CreatedByUserId,
	@SourceId SourceId,
	@GranularityId GranularityId,
	TimePeriod.TimePeriodId,
	DateTemp.DateId
FROM 
	[Information].[TimePeriod]
LEFT OUTER JOIN
	[Information].[TimePeriodDetail]
	ON TimePeriodDetail.TimePeriodId = TimePeriod.TimePeriodId
	AND TimePeriodDetail.EffectiveToDateTime = '9999-12-31'
LEFT OUTER JOIN
	[Information].[TimePeriodAttribute]
	ON TimePeriodAttribute.TimePeriodAttributeId = TimePeriodDetail.TimePeriodAttributeId
	AND TimePeriodAttribute.TimePeriodAttributeDescription = 'Is Additional Time Period'
	AND TimePeriodAttribute.EffectiveToDateTime = '9999-12-31'
CROSS APPLY
	#DateTemp DateTemp
WHERE 
	(DATEDIFF(minute, TimePeriod.StartTime, TimePeriod.EndTime) = 30
		OR DATEDIFF(minute, TimePeriod.StartTime, TimePeriod.EndTime) = -1410)
	AND TimePeriod.EffectiveToDateTime = '9999-12-31'
	AND TimePeriodDetail.TimePeriodDetailId IS NULL

DROP TABLE #DateTemp