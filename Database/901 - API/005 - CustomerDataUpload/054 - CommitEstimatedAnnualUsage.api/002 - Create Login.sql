USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitEstimatedAnnualUsage.api')
    BEGIN
       CREATE LOGIN [CommitEstimatedAnnualUsage.api] WITH PASSWORD=N'ejkzx66NuQxkF2uh', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
