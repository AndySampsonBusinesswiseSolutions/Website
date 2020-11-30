USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreMeterExemptionData.api')
    BEGIN
       CREATE LOGIN [StoreMeterExemptionData.api] WITH PASSWORD=N'CNs2z2TrsqzZMu2J', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
