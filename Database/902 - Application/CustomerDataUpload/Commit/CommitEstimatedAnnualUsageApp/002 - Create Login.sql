USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitEstimatedAnnualUsageApp')
    BEGIN
       CREATE LOGIN [CommitEstimatedAnnualUsageApp] WITH PASSWORD=N'ejkzx66NuQxkF2uh', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
