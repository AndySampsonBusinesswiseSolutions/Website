USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitEstimatedAnnualUsage.api')
    BEGIN
        DROP LOGIN [CommitEstimatedAnnualUsage.api]
    END
GO
