USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexReferenceVolumeData.api')
    BEGIN
       CREATE LOGIN [ValidateFlexReferenceVolumeData.api] WITH PASSWORD=N'h9CMbkML68NCyMNj', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
