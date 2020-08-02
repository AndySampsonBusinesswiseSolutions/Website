USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreMeterData.api')
    BEGIN
        DROP LOGIN [StoreMeterData.api]
    END
GO
