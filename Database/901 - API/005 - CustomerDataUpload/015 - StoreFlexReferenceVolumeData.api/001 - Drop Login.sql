USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFlexReferenceVolumeData.api')
    BEGIN
        DROP LOGIN [StoreFlexReferenceVolumeData.api]
    END
GO
