USE [EMaas]
GO

DELETE FROM [Information].[SourceType]

INSERT INTO [Information].[SourceType]
(
    CreatedByUserId,
    SourceTypeDescription
)
VALUES
(
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    'User Generated'
)