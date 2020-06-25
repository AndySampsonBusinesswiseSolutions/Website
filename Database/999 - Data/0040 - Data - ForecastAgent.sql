USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND SourceTypeEntityId = 0)

EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, 'Date', 'Maps directly against date from previous years'
EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, 'ByForecastGroupByYear', 'Tries to map against any ForecastGroup on a historical year before moving to next historical year'
EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, 'ByYearByForecastGroup', 'Tries to map against any historical year a ForecastGroup on before moving to next ForecastGroup'