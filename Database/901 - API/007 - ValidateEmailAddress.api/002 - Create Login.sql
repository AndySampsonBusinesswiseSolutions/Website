USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddress.api')
    BEGIN
        CREATE LOGIN [ValidateEmailAddress.api] WITH PASSWORD=N'}h8FfD2r[Rd~PPNR', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO