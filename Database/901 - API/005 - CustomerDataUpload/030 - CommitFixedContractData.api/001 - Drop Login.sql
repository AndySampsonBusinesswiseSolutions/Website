USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFixedContractData.api')
    BEGIN
        DROP LOGIN [CommitFixedContractData.api]
    END
GO
