USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterExemptionData.api')
    BEGIN
        DROP LOGIN [CommitMeterExemptionData.api]
    END
GO
