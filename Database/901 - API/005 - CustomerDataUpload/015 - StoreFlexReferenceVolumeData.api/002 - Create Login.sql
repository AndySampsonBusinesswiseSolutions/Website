USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreFlexReferenceVolumeData.api')
    BEGIN
       CREATE LOGIN [StoreFlexReferenceVolumeData.api] WITH PASSWORD=N'L5msq6pjxEqMAAf4', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
