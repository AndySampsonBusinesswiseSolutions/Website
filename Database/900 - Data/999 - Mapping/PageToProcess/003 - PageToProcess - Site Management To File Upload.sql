USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageId BIGINT = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '714F10C4-ACF3-4409-97A8-C605E8E2FD0C')
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID = '3AFF25CB-06BD-4BD1-A409-13D10A08044F')

EXEC [Mapping].[PageToProcess_Insert] 
    @CreatedByUserId, 
    @SourceId,
    @PageId,
    @ProcessId