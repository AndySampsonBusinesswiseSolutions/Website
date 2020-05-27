USE [EMaaS]
GO

DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')

IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
    BEGIN
        INSERT INTO [Information].[SourceType]
        (
            CreatedByUserId,
            SourceTypeDescription
        )
        VALUES
        (
            @UserId,
            'User Generated'
        )
    END