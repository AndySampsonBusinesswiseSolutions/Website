USE [EMaaS]
GO

DECLARE @GUID UNIQUEIDENTIFIER = '6641A1BF-84C8-48F8-9D79-70D0AB2BB787'
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)

IF NOT EXISTS(SELECT TOP 1 1 FROM System.Page WHERE GUID = @GUID)
    BEGIN
        INSERT INTO [System].Page
        (
            CreatedByUserId,
            SourceId,
            GUID
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @GUID
        )
    END