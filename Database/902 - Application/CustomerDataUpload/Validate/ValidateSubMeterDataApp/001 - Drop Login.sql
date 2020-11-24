USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSubMeterDataApp')
    BEGIN
        DROP LOGIN [ValidateSubMeterDataApp]
    END
GO
