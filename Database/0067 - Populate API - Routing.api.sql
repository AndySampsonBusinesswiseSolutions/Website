USE [EMaas]
GO

DELETE FROM [System].API

INSERT INTO [System].API
(
    CreatedByUserId
)
VALUES
(
    1
)

DECLARE @APIId INT = SCOPE_IDENTITY()

DELETE FROM [System].APIDetail

INSERT INTO System.APIDetail
(
    CreatedByUserId,
    APIId,
    APIAttributeId,
    APIDetailDescription
)
VALUES
(
    1,
    @APIId,
    (SELECT APIAttributeId FROM System.APIAttribute WHERE APIAttributeDescription = 'APIName'),
    'Routing.api'
),
(
    1,
    @APIId,
    (SELECT APIAttributeId FROM System.APIAttribute WHERE APIAttributeDescription = 'ApplicationURL'),
    'http://localhost:5002/'
)