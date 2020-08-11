USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitContractToMeterData.api')
    BEGIN
        DROP LOGIN [CommitContractToMeterData.api]
    END
GO
