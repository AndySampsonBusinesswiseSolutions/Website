USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitLineLossFactorClassToMeterData.api')
    BEGIN
        DROP LOGIN [CommitLineLossFactorClassToMeterData.api]
    END
GO
