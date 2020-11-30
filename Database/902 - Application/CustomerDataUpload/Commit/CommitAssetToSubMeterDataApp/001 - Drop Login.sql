USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitAssetToSubMeterDataApp')
    BEGIN
        DROP LOGIN [CommitAssetToSubMeterDataApp]
    END
GO
