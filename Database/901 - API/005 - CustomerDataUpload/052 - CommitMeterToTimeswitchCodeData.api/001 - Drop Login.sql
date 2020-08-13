USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToTimeswitchCodeData.api')
    BEGIN
        DROP LOGIN [CommitMeterToTimeswitchCodeData.api]
    END
GO
