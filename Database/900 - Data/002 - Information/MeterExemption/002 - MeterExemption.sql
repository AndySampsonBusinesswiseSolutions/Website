USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[MeterExemption_Insert] @CreatedByUserId, @SourceId, '5C820C90-B69E-496F-BE65-3DDD5B7E56D7' --CCA
EXEC [Information].[MeterExemption_Insert] @CreatedByUserId, @SourceId, '1F290E00-C62C-49C2-A047-061BA692A7C9' --EII
EXEC [Information].[MeterExemption_Insert] @CreatedByUserId, @SourceId, '4F4EE12E-83BA-4A3A-BC4F-4C921B046E2A' --MINMET