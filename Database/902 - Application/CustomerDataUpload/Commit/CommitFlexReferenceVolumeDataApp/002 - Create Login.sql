USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexReferenceVolumeDataApp')
    BEGIN
       CREATE LOGIN [CommitFlexReferenceVolumeDataApp] WITH PASSWORD=N'6TVnwAK6jeDk2kVb', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
