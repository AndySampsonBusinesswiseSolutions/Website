USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @DailyTimePeriodId BIGINT = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '00:00:00.0000000' AND EndTime = '00:00:00.0000000')
DECLARE @GasGenericProfileId BIGINT = (SELECT ProfileId FROM [DemandForecast].[ProfileDetail] WHERE ProfileDetailDescription = 'Gas Generic Profile')
DECLARE @ProfileValue DECIMAL(19, 19) = (SELECT 1/CAST(366 AS DECIMAL))
DECLARE @ProfileValueId BIGINT = (SELECT ProfileValueId FROM [DemandForecast].[ProfileValue] WHERE Value = @ProfileValue)

INSERT INTO [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue]
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupToTimePeriodToProfileId,
        ProfileValueId
    )
SELECT
	@CreatedByUserId,
	@SourceId,
	[ForecastGroupToTimePeriodToProfile].ForecastGroupToTimePeriodToProfileId,
	@ProfileValueId
FROM
	[Mapping].[ForecastGroupToTimePeriodToProfile]
INNER JOIN
	[Mapping].[ForecastGroupToTimePeriod]
	ON ForecastGroupToTimePeriod.ForecastGroupToTimePeriodId = ForecastGroupToTimePeriodtoProfile.ForecastGroupToTimePeriodId
	AND ForecastGroupToTimePeriod.TimePeriodId = @DailyTimePeriodId
WHERE
	ForecastGroupToTimePeriodToProfile.ProfileId = @GasGenericProfileId

SET @ProfileValueId = (SELECT ProfileValueId FROM [DemandForecast].[ProfileValue] WHERE Value = (SELECT 1/CAST(17568 AS DECIMAL)))

INSERT INTO [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue]
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupToTimePeriodToProfileId,
        ProfileValueId
    )
SELECT
	@CreatedByUserId,
	@SourceId,
	[ForecastGroupToTimePeriodToProfile].ForecastGroupToTimePeriodToProfileId,
	@ProfileValueId
FROM
	[Mapping].[ForecastGroupToTimePeriodToProfile]
INNER JOIN
	[Mapping].[ForecastGroupToTimePeriod]
	ON ForecastGroupToTimePeriod.ForecastGroupToTimePeriodId = ForecastGroupToTimePeriodtoProfile.ForecastGroupToTimePeriodId
	AND ForecastGroupToTimePeriod.TimePeriodId != @DailyTimePeriodId
WHERE
	ForecastGroupToTimePeriodToProfile.ProfileId != @GasGenericProfileId