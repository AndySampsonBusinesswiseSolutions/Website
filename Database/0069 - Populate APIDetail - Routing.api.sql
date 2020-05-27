USE [EMaaS]
GO

DECLARE @APIId BIGINT = (SELECT APIId FROM System.API WHERE GUID = 'A4F25D07-86AA-42BD-ACD7-51A8F772A92B')
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)
DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM System.APIAttribute WHERE APIAttributeDescription = 'APIName')

IF NOT EXISTS(SELECT TOP 1 1 FROM System.APIDetail WHERE APIId = @APIId AND APIAttributeId = @APIAttributeId AND APIDetailDescription = 'Routing.api')
    BEGIN
        INSERT INTO System.APIDetail
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
            'Routing.api'
        )
    END

SET @APIAttributeId = (SELECT APIAttributeId FROM System.APIAttribute WHERE APIAttributeDescription = 'ApplicationURL')
IF NOT EXISTS(SELECT TOP 1 1 FROM System.APIDetail WHERE APIId = @APIId AND APIAttributeId = @APIAttributeId AND APIDetailDescription = 'http://localhost:5002/')
    BEGIN
        INSERT INTO System.APIDetail
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
            'http://localhost:5002/'
        )
    END