USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = 'B2638F86-A973-4938-808C-DCD89892ED78')
DECLARE @ProcessAttributeId BIGINT = (SELECT ProcessAttributeId FROM [System].[ProcessAttribute] WHERE ProcessAttributeDescription = 'Process Name')

EXEC [System].[ProcessDetail_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @ProcessId,
    @ProcessAttributeId,
    'Create Forecast Usage'