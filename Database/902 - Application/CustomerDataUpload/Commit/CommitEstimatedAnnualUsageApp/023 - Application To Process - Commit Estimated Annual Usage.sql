USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ApplicationId BIGINT = (SELECT ApplicationId FROM [System].[Application] WHERE ApplicationGUID = '0ECACB10-AAD6-4238-AAD7-8CE8A589B5C6')
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = '9A15F6EB-42B8-4283-A232-D0312575654D')

EXEC [Mapping].[ApplicationToProcess_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ProcessId
