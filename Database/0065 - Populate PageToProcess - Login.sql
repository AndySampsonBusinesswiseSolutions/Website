USE [EMaas]
GO

DELETE FROM [Mapping].PageToProcess

INSERT INTO [Mapping].PageToProcess
(
    CreatedByUserId,
    PageId,
    ProcessId
)
VALUES
(
    1,
    1,
    1
)