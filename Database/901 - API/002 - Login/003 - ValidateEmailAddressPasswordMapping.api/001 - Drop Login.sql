USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddressPasswordMapping.api')
    BEGIN
        DROP LOGIN [ValidateEmailAddressPasswordMapping.api]
    END
GO