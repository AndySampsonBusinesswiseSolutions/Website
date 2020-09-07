USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityDescription = 'Day')

INSERT INTO [Mapping].GranularityToTimePeriod
    (
        CreatedByUserId,
        SourceId,
        GranularityId,
        TimePeriodId
    )
SELECT
	@CreatedByUserId CreatedByUserId,
	@SourceId SourceId,
	@GranularityId GranularityId,
	TimePeriod.TimePeriodId
FROM 
	[Information].[TimePeriod]
WHERE 
	DATEDIFF(minute, TimePeriod.StartTime, TimePeriod.EndTime) = 0
	AND TimePeriod.EffectiveToDateTime = '9999-12-31'