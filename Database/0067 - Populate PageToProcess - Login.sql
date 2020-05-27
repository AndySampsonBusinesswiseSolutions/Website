USE [EMaaS]
GO

DECLARE @PageId INT = (SELECT PageId FROM System.Page WHERE GUID = '6641A1BF-84C8-48F8-9D79-70D0AB2BB787')
DECLARE @ProcessId INT = (SELECT ProcessId FROM System.Process WHERE GUID = 'AF10359F-FD78-4345-9F26-EF5A921E72FD')
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)

IF NOT EXISTS(SELECT TOP 1 1 FROM Mapping.PageToProcess WHERE PageId = @PageId AND ProcessId = @ProcessId)
    BEGIN
        INSERT INTO [Mapping].PageToProcess
        (
            CreatedByUserId,
            SourceId,
            PageId,
            ProcessId
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @PageId,
            @ProcessId
        )
    END