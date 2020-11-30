USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CheckPrerequisiteApplicationApp')
    BEGIN
        DROP LOGIN [CheckPrerequisiteApplicationApp]
    END
GO