USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = 'B14A84F5-0B0D-41D2-9F3F-B9BFFFDC23BF')
DECLARE @ProcessAttributeId BIGINT = (SELECT ProcessAttributeId FROM [System].[ProcessAttribute] WHERE ProcessAttributeDescription = 'Process Name')

EXEC [System].[ProcessDetail_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @ProcessId,
    @ProcessAttributeId,
    'Get Profile Id'