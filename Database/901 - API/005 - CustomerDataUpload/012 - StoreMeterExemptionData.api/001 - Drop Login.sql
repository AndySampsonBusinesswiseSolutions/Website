USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreMeterExemptionData.api')
    BEGIN
        DROP LOGIN [StoreMeterExemptionData.api]
    END
GO
