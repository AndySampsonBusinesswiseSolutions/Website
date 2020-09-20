USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, 'Date', 'Maps directly against date from previous years'
EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, 'ByForecastGroupByYear', 'Tries to map against any ForecastGroup on a historical year before moving to next historical year'
EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, 'ByYearByForecastGroup', 'Tries to map against any historical year on a ForecastGroup before moving to next ForecastGroup'