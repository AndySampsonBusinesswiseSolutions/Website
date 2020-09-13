USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetMeterSpecificProfile.api')
    BEGIN
        DROP LOGIN [GetMeterSpecificProfile.api]
    END
GO
