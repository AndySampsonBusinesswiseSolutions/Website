USE [EMaas]
GO

DELETE FROM [System].Process

INSERT INTO [System].Process
(
    CreatedByUserId
)
VALUES
(
    1
)

DECLARE @ProcessId INT = SCOPE_IDENTITY()

DELETE FROM [System].ProcessDetail

INSERT INTO System.ProcessDetail
(
    CreatedByUserId,
    ProcessId,
    ProcessAttributeId,
    ProcessDetailDescription
)
VALUES
(
    1,
    @ProcessId,
    (SELECT ProcessAttributeId FROM System.ProcessAttribute WHERE ProcessAttributeDescription = 'ProcessName'),
    'Login'
)