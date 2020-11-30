USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSubMeterData.api')
    BEGIN
        DROP LOGIN [ValidateSubMeterData.api]
    END
GO
