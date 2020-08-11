USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitReferenceVolumeToContractData.api')
    BEGIN
       CREATE LOGIN [CommitReferenceVolumeToContractData.api] WITH PASSWORD=N'wuMYmJm587ehRtED', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
