USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterToMeterTimeswitchCodeDataApp')
    BEGIN
        DROP LOGIN [CommitMeterToMeterTimeswitchCodeDataApp]
    END
GO
