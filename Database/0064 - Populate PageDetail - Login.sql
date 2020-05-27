USE [EMaaS]
GO

DECLARE @PageId INT = (SELECT PageId FROM System.Page WHERE GUID = '6641A1BF-84C8-48F8-9D79-70D0AB2BB787')
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)
DECLARE @PageAttributeId BIGINT = (SELECT PageAttributeId FROM System.PageAttribute WHERE PageAttributeDescription = 'PageName')

IF NOT EXISTS(SELECT TOP 1 1 FROM System.PageDetail WHERE PageId = @PageId AND PageAttributeId = @PageAttributeId AND PageDetailDescription = 'Login')
    BEGIN
        INSERT INTO System.PageDetail
        (
            CreatedByUserId,
            SourceId,
            PageId,
            PageAttributeId,
            PageDetailDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @PageId,
            @PageAttributeId,
            'Login'
        )
    END