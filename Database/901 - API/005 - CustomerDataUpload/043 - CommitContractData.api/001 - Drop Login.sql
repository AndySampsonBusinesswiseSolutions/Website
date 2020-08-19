USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractData.api')
    BEGIN
        DROP LOGIN [CommitContractData.api]
    END
GO
