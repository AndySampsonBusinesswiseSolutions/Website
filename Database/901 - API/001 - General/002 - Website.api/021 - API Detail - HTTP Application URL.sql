USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = 'CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE')
DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = 'HTTP Application URL')

EXEC [System].[APIDetail_Insert] @CreatedByUserId, @SourceId, @APIId, @APIAttributeId, 'http://localhost:5000/'