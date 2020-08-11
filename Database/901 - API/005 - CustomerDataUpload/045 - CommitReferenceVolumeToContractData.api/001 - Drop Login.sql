USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitReferenceVolumeToContractData.api')
    BEGIN
        DROP LOGIN [CommitReferenceVolumeToContractData.api]
    END
GO
