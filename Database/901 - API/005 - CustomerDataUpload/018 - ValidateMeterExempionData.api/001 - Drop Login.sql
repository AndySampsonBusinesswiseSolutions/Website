USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateMeterExempionData.api')
    BEGIN
        DROP LOGIN [ValidateMeterExempionData.api]
    END
GO
