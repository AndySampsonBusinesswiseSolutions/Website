USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateFlexReferenceVolumeDataApp')
    BEGIN
       CREATE LOGIN [ValidateFlexReferenceVolumeDataApp] WITH PASSWORD=N'h9CMbkML68NCyMNj', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
