USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = 'AE819292-ED1D-4A35-BBB2-8AAE86DA2078'
DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[API_Insert] @CreatedByUserId, @SourceId, @APIGUID
