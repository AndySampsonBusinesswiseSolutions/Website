USE [EMaaS]
GO

DELETE FROM [Administration.User].[UserAttribute]

INSERT INTO [Administration.User].[UserAttribute]
(
    CreatedByUserId,
    UserAttributeDescription
)
VALUES
(
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    'Email Address'
),
(
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    'User Name'
)