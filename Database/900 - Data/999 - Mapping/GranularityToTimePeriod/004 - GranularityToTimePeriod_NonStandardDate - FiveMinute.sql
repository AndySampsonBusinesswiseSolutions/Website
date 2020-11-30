USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[GranularityDetail] WHERE GranularityDetailDescription = 'Five Minute')

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

INSERT INTO [Mapping].GranularityToTimePeriod_NonStandardDate
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
CROSS APPLY
	#DateTemp DateTemp
WHERE 
	(DATEDIFF(minute, TimePeriod.StartTime, TimePeriod.EndTime) = 5
		OR DATEDIFF(minute, TimePeriod.StartTime, TimePeriod.EndTime) = -1435)
	AND TimePeriod.EffectiveToDateTime = '9999-12-31'

DROP TABLE #DateTemp