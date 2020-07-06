USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageId BIGINT = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'D39E768A-D06D-4EB3-80E3-895EDC556A6B')
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = '1ACFB189-9C95-4DCD-A21A-93CDB2928620')

EXEC [Mapping].[PageToProcess_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @PageId,
    @ProcessId