USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitFlexReferenceVolumeData.api')
    BEGIN
        DROP LOGIN [CommitFlexReferenceVolumeData.api]
    END
GO
