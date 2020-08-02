USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateMeterData.api')
    BEGIN
        DROP LOGIN [ValidateMeterData.api]
    END
GO
