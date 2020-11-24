USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSubMeterUsageDataApp')
    BEGIN
        DROP LOGIN [StoreSubMeterUsageDataApp]
    END
GO
