USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateMeterExemptionDataApp')
    BEGIN
        DROP LOGIN [ValidateMeterExemptionDataApp]
    END
GO
