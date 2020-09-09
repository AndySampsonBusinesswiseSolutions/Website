USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

INSERT INTO [Mapping].ForecastGroupToTimePeriod
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupId,
        TimePeriodId
    )
SELECT
	@CreatedByUserId CreatedByUserId,
	@SourceId SourceId,
	ForecastGroup.ForecastGroupId,
	TimePeriod.TimePeriodId
FROM 
	[Information].[TimePeriod]
CROSS APPLY
	[DemandForecast].[ForecastGroup]
WHERE 
	TimePeriod.EffectiveToDateTime = '9999-12-31'
	AND ForecastGroup.EffectiveToDateTime = '9999-12-31'