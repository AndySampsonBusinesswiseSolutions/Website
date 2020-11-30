USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexReferenceVolumeDataApp')
    BEGIN
        DROP LOGIN [ValidateFlexReferenceVolumeDataApp]
    END
GO
