USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSubAreaToSubMeterData.api')
    BEGIN
       CREATE LOGIN [CommitSubAreaToSubMeterData.api] WITH PASSWORD=N'x96nW5RAYrnXu9tc', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
