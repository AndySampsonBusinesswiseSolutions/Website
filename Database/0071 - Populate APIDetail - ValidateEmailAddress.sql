USE [EMaaS]
GO

DECLARE @APIId BIGINT = (SELECT APIId FROM System.API WHERE GUID = 'DCD106B2-36BB-4553-985C-19EB8F2F3191')
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)
DECLARE @APIAttributeId BIGINT = (SELECT PageAttributeId FROM System.APIAttribute WHERE APIAttributeDescription = 'APIName')

IF NOT EXISTS(SELECT TOP 1 1 FROM System.APIDetail WHERE APIId = @APIId AND APIAttributeId = @APIAttributeId AND APIDetailDescription = 'ValidateEmailAddress.api')
    BEGIN
        INSERT INTO System.PageDetail
        (
            CreatedByUserId,
            SourceId,
            APIId,
            APIAttributeId,
            APIDetailDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @APIId,
            @APIAttributeId,
            'ValidateEmailAddress.api'
        )
    END

SET @APIAttributeId = (SELECT PageAttributeId FROM System.APIAttribute WHERE APIAttributeDescription = 'ApplicationURL')
IF NOT EXISTS(SELECT TOP 1 1 FROM System.APIDetail WHERE APIId = @APIId AND APIAttributeId = @APIAttributeId AND APIDetailDescription = 'http://localhost:5004/')
    BEGIN
        INSERT INTO System.PageDetail
        (
            CreatedByUserId,
            SourceId,
            APIId,
            APIAttributeId,
            APIDetailDescription
        )
        VALUES
        (
            @UserId,
            @SourceId,
            @APIId,
            @APIAttributeId,
            'http://localhost:5004/'
        )
    END