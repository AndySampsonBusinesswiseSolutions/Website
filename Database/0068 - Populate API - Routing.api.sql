USE [EMaaS]
GO

DECLARE @GUID UNIQUEIDENTIFIER = 'A4F25D07-86AA-42BD-ACD7-51A8F772A92B'
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND CreatedByUserId = @UserId)

INSERT INTO [System].API
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