USE [EMaas]
GO

DELETE FROM [System].Page

INSERT INTO [System].Page
(
    CreatedByUserId
)
VALUES
(
    1
)

DECLARE @PageId INT = SCOPE_IDENTITY()

DELETE FROM [System].PageDetail

INSERT INTO System.PageDetail
(
    CreatedByUserId,
    PageId,
    PageAttributeId,
    PageDetailDescription
)
VALUES
(
    1,
    @PageId,
    (SELECT PageAttributeId FROM System.PageAttribute WHERE PageAttributeDescription = 'PageName'),
    'Login'
)