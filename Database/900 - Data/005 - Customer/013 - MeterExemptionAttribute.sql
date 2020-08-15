USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Customer].[MeterExemptionAttribute_Insert] @CreatedByUserId, @SourceId, 'Date From'
EXEC [Customer].[MeterExemptionAttribute_Insert] @CreatedByUserId, @SourceId, 'Date To'
EXEC [Customer].[MeterExemptionAttribute_Insert] @CreatedByUserId, @SourceId, 'Exemption Proportion'