USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSubMeterUsageDataApp')
    BEGIN
        DROP LOGIN [ValidateSubMeterUsageDataApp]
    END
GO
