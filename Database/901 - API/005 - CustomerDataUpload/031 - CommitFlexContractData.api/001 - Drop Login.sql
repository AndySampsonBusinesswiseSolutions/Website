USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexContractData.api')
    BEGIN
        DROP LOGIN [CommitFlexContractData.api]
    END
GO
