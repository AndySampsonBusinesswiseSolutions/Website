USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'StandingCharge', 'Standing Charge'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'CapacityCharge', 'Capacity Charge'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate1', 'Unit Rate 1'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate2', 'Unit Rate 2'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate3', 'Unit Rate 3'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate4', 'Unit Rate 4'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate5', 'Unit Rate 5'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate6', 'Unit Rate 6'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate7', 'Unit Rate 7'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate8', 'Unit Rate 8'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate9', 'Unit Rate 9'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'UnitRate10', 'Unit Rate 10'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'ShapeFee', 'Shape Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'AdminFee', 'Admin Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'ImbalanceFee', 'Imbalance Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'RiskFee', 'Risk Fee'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'GreenPremium', 'Green Premium'
EXEC [Information].[RateType_Insert] @CreatedByUserId, @SourceId, 'OptimisationBenefit', 'Optimisation Benefit'