USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSubMeterDataApp')
    BEGIN
        DROP LOGIN [StoreSubMeterDataApp]
    END
GO
