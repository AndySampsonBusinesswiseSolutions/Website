USE [EMaaS]
GO

DECLARE @ProcessId INT = (SELECT ProcessId FROM System.Process WHERE GUID = 'AF10359F-FD78-4345-9F26-EF5A921E72FD')
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)
DECLARE @ProcessAttributeId BIGINT = (SELECT ProcessAttributeId FROM System.ProcessAttribute WHERE ProcessAttributeDescription = 'ProcessName')

IF NOT EXISTS(SELECT TOP 1 1 FROM System.ProcessDetail WHERE ProcessId = @ProcessId AND ProcessAttributeId = @ProcessAttributeId AND ProcessDetailDescription = 'Login')
    BEGIN
        INSERT INTO System.ProcessDetail
        (
            CreatedByUserId,
            SourceId,
            ProcessId,
            ProcessAttributeId,
            ProcessDetailDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @ProcessId,
            @ProcessAttributeId,
            'Login'
        )
    END