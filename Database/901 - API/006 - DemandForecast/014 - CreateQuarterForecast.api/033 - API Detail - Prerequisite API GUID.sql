USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = 'F9E29252-F769-4FE8-BB10-5F3C406C98F7')
DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'Prerequisite API GUID')

EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'F0664F98-0EE6-413B-9FD1-0D05226EC3DB'

