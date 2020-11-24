USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFlexReferenceVolumeDataApp')
    BEGIN
        DROP LOGIN [StoreFlexReferenceVolumeDataApp]
    END
GO
