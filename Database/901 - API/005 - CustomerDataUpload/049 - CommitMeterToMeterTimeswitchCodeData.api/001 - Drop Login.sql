USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToMeterTimeswitchCodeData.api')
    BEGIN
        DROP LOGIN [CommitMeterToMeterTimeswitchCodeData.api]
    END
GO
