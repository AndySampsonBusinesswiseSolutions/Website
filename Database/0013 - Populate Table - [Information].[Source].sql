USE [EMaas]
GO

DELETE FROM [Information].[Source]

INSERT INTO [Information].[Source]
(
    CreatedByUserId,
    SourceTypeId,
    SourceTypeEntityId
)
VALUES
(
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated'),
    1
)