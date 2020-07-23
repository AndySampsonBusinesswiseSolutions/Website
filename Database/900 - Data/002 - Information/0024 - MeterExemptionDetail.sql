USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @MeterExemptionProductAttributeId BIGINT = (SELECT MeterExemptionAttributeId FROM [Information].[MeterExemptionAttribute] WHERE MeterExemptionAttributeDescription = 'Meter Exemption Product')
DECLARE @MeterExemptionProportionAttributeId BIGINT = (SELECT MeterExemptionAttributeId FROM [Information].[MeterExemptionAttribute] WHERE MeterExemptionAttributeDescription = 'Meter Exemption Proportion')

DECLARE @MeterExemptionId BIGINT = (SELECT MeterExemptionId FROM [Information].[MeterExemption] WHERE MeterExemptionGUID = '5C820C90-B69E-496F-BE65-3DDD5B7E56D7')
EXEC [Information].[MeterExemptionDetail_Insert] @CreatedByUserId, @SourceId, @MeterExemptionId, @MeterExemptionProductAttributeId, 'CCA'

SET @MeterExemptionId = (SELECT MeterExemptionId FROM [Information].[MeterExemption] WHERE MeterExemptionGUID = '1F290E00-C62C-49C2-A047-061BA692A7C9')
EXEC [Information].[MeterExemptionDetail_Insert] @CreatedByUserId, @SourceId, @MeterExemptionId, @MeterExemptionProductAttributeId, 'EII'
EXEC [Information].[MeterExemptionDetail_Insert] @CreatedByUserId, @SourceId, @MeterExemptionId, @MeterExemptionProportionAttributeId, '0.83'

SET @MeterExemptionId = (SELECT MeterExemptionId FROM [Information].[MeterExemption] WHERE MeterExemptionGUID = '4F4EE12E-83BA-4A3A-BC4F-4C921B046E2A')
EXEC [Information].[MeterExemptionDetail_Insert] @CreatedByUserId, @SourceId, @MeterExemptionId, @MeterExemptionProductAttributeId, 'MINMET'
EXEC [Information].[MeterExemptionDetail_Insert] @CreatedByUserId, @SourceId, @MeterExemptionId, @MeterExemptionProportionAttributeId, '1'