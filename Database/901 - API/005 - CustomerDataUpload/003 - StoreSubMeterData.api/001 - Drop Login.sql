USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSubMeterData.api')
    BEGIN
        DROP LOGIN [StoreSubMeterData.api]
    END
GO
