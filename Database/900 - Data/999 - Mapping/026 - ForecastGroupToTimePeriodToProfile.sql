USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @GasGenericProfileId BIGINT = (SELECT ProfileId FROM [DemandForecast].[ProfileDetail] WHERE ProfileDetailDescription = 'Gas Generic Profile')

INSERT INTO [Mapping].[ForecastGroupToTimePeriodToProfile]
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupToTimePeriodId,
        ProfileId
    )
SELECT
	@CreatedByUserId CreatedByUserId,
	@SourceId SourceId,
	ForecastGroupToTimePeriod.ForecastGroupToTimePeriodId,
	Profile.ProfileId
FROM 
	[Mapping].[ForecastGroupToTimePeriod]
INNER JOIN
	[Mapping].[GranularityToTimePeriod]
	ON GranularityToTimePeriod.TimePeriodId = ForecastGroupToTimePeriod.TimePeriodId
INNER JOIN
	[Information].[Granularity]
	ON Granularity.GranularityId = GranularityToTimePeriod.GranularityId
	AND Granularity.GranularityCode = 'Date'
	AND Granularity.EffectiveToDateTime = '9999-12-31'
CROSS APPLY
	[DemandForecast].[Profile]
WHERE 
	ForecastGroupToTimePeriod.EffectiveToDateTime = '9999-12-31'
	AND Profile.ProfileId = @GasGenericProfileId
	AND Profile.EffectiveToDateTime = '9999-12-31'

INSERT INTO [Mapping].[ForecastGroupToTimePeriodToProfile]
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupToTimePeriodId,
        ProfileId
    )
SELECT
	@CreatedByUserId CreatedByUserId,
	@SourceId SourceId,
	ForecastGroupToTimePeriod.ForecastGroupToTimePeriodId,
	Profile.ProfileId
FROM 
	[Mapping].[ForecastGroupToTimePeriod]
INNER JOIN
	[Mapping].[GranularityToTimePeriod]
	ON GranularityToTimePeriod.TimePeriodId = ForecastGroupToTimePeriod.TimePeriodId
INNER JOIN
	[Information].[Granularity]
	ON Granularity.GranularityId = GranularityToTimePeriod.GranularityId
	AND Granularity.GranularityCode = 'HalfHour'
	AND Granularity.EffectiveToDateTime = '9999-12-31'
CROSS APPLY
	[DemandForecast].[Profile]
WHERE 
	ForecastGroupToTimePeriod.EffectiveToDateTime = '9999-12-31'
	AND Profile.ProfileId <> @GasGenericProfileId
	AND Profile.EffectiveToDateTime = '9999-12-31'