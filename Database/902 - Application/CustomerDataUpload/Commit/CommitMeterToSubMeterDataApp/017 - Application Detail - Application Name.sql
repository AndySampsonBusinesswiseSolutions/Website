USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ApplicationId BIGINT = (SELECT ApplicationId FROM [System].[Application] WHERE ApplicationGUID = 'BBA77F1B-F9A3-4CAA-B213-D11285994B0A')
DECLARE @ApplicationAttributeId BIGINT = (SELECT ApplicationAttributeId FROM [System].[ApplicationAttribute] WHERE ApplicationAttributeDescription = 'Application Name')

EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'CommitMeterToSubMeterDataApp'
