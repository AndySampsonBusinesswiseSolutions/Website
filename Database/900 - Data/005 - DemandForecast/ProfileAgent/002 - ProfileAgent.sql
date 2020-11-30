USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [DemandForecast].[ProfileAgent_Insert] @CreatedByUserId, @SourceId, '884AA73C-41E6-4718-858E-870D0CC7BF72' --Meter specific profile agent
EXEC [DemandForecast].[ProfileAgent_Insert] @CreatedByUserId, @SourceId, '624DF74E-2D34-43AF-A9A7-EA24F0A525AC' --Flex profile agent
EXEC [DemandForecast].[ProfileAgent_Insert] @CreatedByUserId, @SourceId, 'CB3590BB-3B06-4020-B70B-AEB72A84424F' --Generic profile agent