USE [EMaas]
GO

DELETE FROM [Mapping].APIToPageToProcess

INSERT INTO [Mapping].APIToPageToProcess
(
    CreatedByUserId,
    APIId,
    PageToProcessId
)
VALUES
(
    1,
    2,
    1
)