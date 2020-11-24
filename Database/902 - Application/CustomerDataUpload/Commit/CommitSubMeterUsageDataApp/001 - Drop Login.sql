USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSubMeterUsageDataApp')
    BEGIN
        DROP LOGIN [CommitSubMeterUsageDataApp]
    END
GO
