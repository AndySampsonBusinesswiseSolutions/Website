USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterExemptionDataApp')
    BEGIN
        DROP LOGIN [CommitMeterExemptionDataApp]
    END
GO
