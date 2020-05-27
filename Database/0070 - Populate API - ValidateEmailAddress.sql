USE [EMaaS]
GO

DECLARE @GUID UNIQUEIDENTIFIER = 'DCD106B2-36BB-4553-985C-19EB8F2F3191'
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