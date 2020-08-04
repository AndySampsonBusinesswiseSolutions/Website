USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateMeterExemptionData.api')
    BEGIN
        DROP LOGIN [ValidateMeterExemptionData.api]
    END
GO
