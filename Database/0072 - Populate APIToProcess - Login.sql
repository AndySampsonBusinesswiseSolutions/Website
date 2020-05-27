USE [EMaaS]
GO

DECLARE @APIId INT = (SELECT APIId FROM System.API WHERE GUID = 'DCD106B2-36BB-4553-985C-19EB8F2F3191')
DECLARE @ProcessId INT = (SELECT ProcessId FROM System.Process WHERE GUID = 'AF10359F-FD78-4345-9F26-EF5A921E72FD')
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)

IF NOT EXISTS(SELECT TOP 1 1 FROM Mapping.APIToProcess WHERE APIId = @APIId AND ProcessId = @ProcessId)
    BEGIN
        INSERT INTO [Mapping].APIToProcess
        (
            CreatedByUserId,
            SourceId,
            APIId,
            ProcessId
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @APIId,
            @ProcessId
        )
    END