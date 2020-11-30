USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[HostEnvironment_Insert] @CreatedByUserId, @SourceId, '0C751518-B6B5-4716-9E84-1AA11C1C6DBC' --Production
EXEC [System].[HostEnvironment_Insert] @CreatedByUserId, @SourceId, '11A5AF04-0A50-4D21-9773-2A445D531997' --UAT
EXEC [System].[HostEnvironment_Insert] @CreatedByUserId, @SourceId, 'B97B795A-8E6A-4A00-88FB-9524037788E8' --Development