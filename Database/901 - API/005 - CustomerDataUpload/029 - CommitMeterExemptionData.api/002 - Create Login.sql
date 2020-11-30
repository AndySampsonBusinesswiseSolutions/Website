USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterExemptionData.api')
    BEGIN
       CREATE LOGIN [CommitMeterExemptionData.api] WITH PASSWORD=N'hzRHNnabT4hc6Mzf', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
