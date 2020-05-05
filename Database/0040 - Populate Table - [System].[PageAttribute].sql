USE [EMaaS]
GO

DELETE FROM [System].[PageAttribute]

INSERT INTO [System].[PageAttribute]
(
    CreatedByUserId,
    PageAttributeDescription
)
VALUES
(
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    'Page Name'
)