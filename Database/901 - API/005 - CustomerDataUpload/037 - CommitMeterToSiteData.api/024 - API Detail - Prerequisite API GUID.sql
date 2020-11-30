USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = '6A49C79C-37F3-4D86-BCAE-27602E5DF416')
DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'Prerequisite API GUID')

EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, '94475084-969D-431F-A8D8-EB591CCFCE26'
EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'EB061C46-A893-4C44-9EB7-E9D5CF6B0895'

