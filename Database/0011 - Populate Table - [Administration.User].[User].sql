USE [EMaas]
GO

DELETE FROM [Administration.User].[User]

INSERT INTO [Administration.User].[User]
(
    CreatedByUserId,
    SourceId
)
VALUES
(
    1,
    1
)