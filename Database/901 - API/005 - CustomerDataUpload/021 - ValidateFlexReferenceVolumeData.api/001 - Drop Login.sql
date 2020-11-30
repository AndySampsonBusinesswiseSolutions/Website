USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexReferenceVolumeData.api')
    BEGIN
        DROP LOGIN [ValidateFlexReferenceVolumeData.api]
    END
GO
