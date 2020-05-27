USE [EMaaS]
GO

DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)

IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'Email Address')
    BEGIN
        INSERT INTO [Administration.User].[UserAttribute]
        (
            CreatedByUserId,
            SourceId,
            UserAttributeDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            'Email Address'
        )
    END

IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'User Name')
    BEGIN
        INSERT INTO [Administration.User].[UserAttribute]
        (
            CreatedByUserId,
            SourceId,
            UserAttributeDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            'User Name'
        )
    END