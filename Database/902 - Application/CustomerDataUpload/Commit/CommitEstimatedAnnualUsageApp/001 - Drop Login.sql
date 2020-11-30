USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitEstimatedAnnualUsageApp')
    BEGIN
        DROP LOGIN [CommitEstimatedAnnualUsageApp]
    END
GO
