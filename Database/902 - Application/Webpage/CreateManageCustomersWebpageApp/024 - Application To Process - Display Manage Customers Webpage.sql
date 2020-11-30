USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ApplicationId BIGINT = (SELECT ApplicationId FROM [System].[Application] WHERE ApplicationGUID = 'DD39D82C-3D6B-472B-883B-FD92192F54D1')
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = 'DC33F3E1-B925-4EB7-B2B2-74ED3DD828A1')

EXEC [Mapping].[ApplicationToProcess_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ProcessId
