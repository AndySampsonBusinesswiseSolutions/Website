USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @NameForecastAgentAttributeId BIGINT = (SELECT ForecastAgentAttributeId FROM [DemandForecast].[ForecastAgentAttribute] WHERE ForecastAgentAttributeDescription = 'Name')
DECLARE @DescriptionForecastAgentAttributeId BIGINT = (SELECT ForecastAgentAttributeId FROM [DemandForecast].[ForecastAgentAttribute] WHERE ForecastAgentAttributeDescription = 'Description')
DECLARE @APIGUIDForecastAgentAttributeId BIGINT = (SELECT ForecastAgentAttributeId FROM [DemandForecast].[ForecastAgentAttribute] WHERE ForecastAgentAttributeDescription = 'Forecast Agent API GUID')

DECLARE @ForecastAgentId BIGINT = (SELECT ForecastAgentId FROM [DemandForecast].[ForecastAgent] WHERE ForecastAgentGUID = 'E04283B4-2EAB-4F6A-A4BC-5255115F1120')
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @NameForecastAgentAttributeId, 'Date'
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @DescriptionForecastAgentAttributeId, 'Maps directly against date from previous years'
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @APIGUIDForecastAgentAttributeId, '58097D6B-1751-4FEC-9630-0308792958AB'

SET @ForecastAgentId = (SELECT ForecastAgentId FROM [DemandForecast].[ForecastAgent] WHERE ForecastAgentGUID = '2A0FB263-7521-4480-9F5F-63E1BBD72EF8')
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @NameForecastAgentAttributeId, 'ByForecastGroupByYear'
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @DescriptionForecastAgentAttributeId, 'Tries to map against any ForecastGroup on a historical year before moving to next historical year'
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @APIGUIDForecastAgentAttributeId, '1668161B-7E7F-41D5-9251-F04E25004FD7'

SET @ForecastAgentId = (SELECT ForecastAgentId FROM [DemandForecast].[ForecastAgent] WHERE ForecastAgentGUID = '9692BE1F-3FE3-40E9-833D-ED4A76DC0D4E')
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @NameForecastAgentAttributeId, 'ByYearByForecastGroup'
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @DescriptionForecastAgentAttributeId, 'Tries to map against any historical year on a ForecastGroup before moving to next ForecastGroup'
EXEC [DemandForecast].[ForecastAgentDetail_Insert] @CreatedByUserId, @SourceId, @ForecastAgentId, @APIGUIDForecastAgentAttributeId, '5DF6C2B8-AE10-4879-9ECD-D83F7EE92604'