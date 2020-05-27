USE [EMaaS]
GO

DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)
DECLARE @UserAttributeId BIGINT = (SELECT UserAttributeId FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'Email Address')

IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[UserDetail] WHERE UserDetailDescription = 'Email Address')
    BEGIN
        INSERT INTO [Administration.User].[UserDetail]
        (
            CreatedByUserId,
            SourceId,
            UserId,
            UserAttributeId,
            UserDetailDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @UserId,
            @UserAttributeId,
            'andy.sampson@businesswisesolutions.co.uk'
        )
    END

SET @UserAttributeId = (SELECT UserAttributeId FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'User Name')
IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[UserDetail] WHERE UserDetailDescription = 'User Name')
    BEGIN
        INSERT INTO [Administration.User].[UserDetail]
        (
            CreatedByUserId,
            SourceId,
            UserId,
            UserAttributeId,
            UserDetailDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @UserId,
            @UserAttributeId,
            'System'
        )
    END