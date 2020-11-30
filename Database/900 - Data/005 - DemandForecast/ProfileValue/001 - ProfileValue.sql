USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ProfileValue DECIMAL(19, 19) = (SELECT 1/CAST(17568 AS DECIMAL))
EXEC [DemandForecast].[ProfileValue_Insert] @CreatedByUserId, @SourceId, @ProfileValue

SET @ProfileValue = (SELECT 1/CAST(366 AS DECIMAL))
EXEC [DemandForecast].[ProfileValue_Insert] @CreatedByUserId, @SourceId, @ProfileValue