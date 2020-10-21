USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @FileTypeId BIGINT = (SELECT FileTypeId FROM [Information].[FileType] WHERE FileTypeDescription = 'Customer Data Upload')
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[ProcessDetail] WHERE ProcessAttributeId = 1 AND ProcessDetailDescription = 'Customer Data Upload')

EXEC [Mapping].[FileTypeToProcess_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @FileTypeId,
    @ProcessId