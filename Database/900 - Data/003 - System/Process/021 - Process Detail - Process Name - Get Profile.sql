USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = 'CCBD6134-09BF-469A-A9AB-046BAC3A204A')
DECLARE @ProcessAttributeId BIGINT = (SELECT ProcessAttributeId FROM [System].[ProcessAttribute] WHERE ProcessAttributeDescription = 'Process Name')

EXEC [System].[ProcessDetail_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @ProcessId,
    @ProcessAttributeId,
    'Get Profile'