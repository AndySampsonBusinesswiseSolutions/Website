USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Standing Charge'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Capacity Charge'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 1'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 2'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 3'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 4'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 5'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 6'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 7'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 8'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 9'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Unit Rate 10'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Shape Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Admin Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Imbalance Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Risk Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Green Premium'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'Optimisation Benefit'