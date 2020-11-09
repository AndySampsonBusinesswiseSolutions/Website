USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = 'D0FC5C37-400C-41BE-A612-10E69EE44CBA')
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = '7A8E05B5-34EF-4A14-9076-34F53BD7C5F6')

EXEC [Mapping].[APIToProcess_Insert] @CreatedByUserId, @SourceId, @APIId, @ProcessId
