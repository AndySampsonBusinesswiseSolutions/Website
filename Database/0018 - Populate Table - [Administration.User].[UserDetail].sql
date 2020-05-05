USE [EMaaS]
GO

DELETE FROM [Administration.User].[UserDetail]

INSERT INTO [Administration.User].[UserDetail]
(
    CreatedByUserId,
    UserId,
    UserAttributeId,
    UserDetailDescription
)
VALUES
(
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    (SELECT UserAttributeId FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'Email Address'),
    'andy.sampson@businesswisesolutions.co.uk'
),
(
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    (SELECT MIN(UserId) FROM [Administration.User].[User]),
    (SELECT UserAttributeId FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = 'User Name'),
    'System'
)