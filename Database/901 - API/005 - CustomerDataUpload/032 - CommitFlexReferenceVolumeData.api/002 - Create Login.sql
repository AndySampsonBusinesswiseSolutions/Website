USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexReferenceVolumeData.api')
    BEGIN
       CREATE LOGIN [CommitFlexReferenceVolumeData.api] WITH PASSWORD=N'6TVnwAK6jeDk2kVb', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
