USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreMeterExemptionDataApp')
    BEGIN
        DROP LOGIN [StoreMeterExemptionDataApp]
    END
GO
