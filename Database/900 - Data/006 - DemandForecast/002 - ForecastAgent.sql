USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, 'E04283B4-2EAB-4F6A-A4BC-5255115F1120'
EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, '2A0FB263-7521-4480-9F5F-63E1BBD72EF8'
EXEC [DemandForecast].[ForecastAgent_Insert] @CreatedByUserId, @SourceId, '9692BE1F-3FE3-40E9-833D-ED4A76DC0D4E'