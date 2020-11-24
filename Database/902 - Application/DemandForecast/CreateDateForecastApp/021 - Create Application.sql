USE [EMaaS]
GO

DECLARE @ApplicationGUID UNIQUEIDENTIFIER = 'DAD4DACC-FAC2-4B3D-8EFB-5823352ADF5D'
DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[Application_Insert] @CreatedByUserId, @SourceId, @ApplicationGUID
