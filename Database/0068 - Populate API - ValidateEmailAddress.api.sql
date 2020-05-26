USE [EMaas]
GO

INSERT INTO [System].API
(
    CreatedByUserId
)
VALUES
(
    1
)

DECLARE @APIId INT = SCOPE_IDENTITY()

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
    'ValidateEmailAddress.api'
),
(
    1,
    @APIId,
    (SELECT APIAttributeId FROM System.APIAttribute WHERE APIAttributeDescription = 'ApplicationURL'),
    'http://localhost:5004/'
)