USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitExemptionToMeterData.api')
    BEGIN
        DROP LOGIN [CommitExemptionToMeterData.api]
    END
GO
