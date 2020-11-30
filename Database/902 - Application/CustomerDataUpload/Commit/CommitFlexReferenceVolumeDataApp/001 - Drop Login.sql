USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexReferenceVolumeDataApp')
    BEGIN
        DROP LOGIN [CommitFlexReferenceVolumeDataApp]
    END
GO
