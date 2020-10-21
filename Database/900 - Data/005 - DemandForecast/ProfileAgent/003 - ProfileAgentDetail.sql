USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @NameProfileAgentAttributeId BIGINT = (SELECT ProfileAgentAttributeId FROM [DemandForecast].[ProfileAgentAttribute] WHERE ProfileAgentAttributeDescription = 'Name')
DECLARE @PriorityProfileAgentAttributeId BIGINT = (SELECT ProfileAgentAttributeId FROM [DemandForecast].[ProfileAgentAttribute] WHERE ProfileAgentAttributeDescription = 'Priority')
DECLARE @ProfileAgentAPIGUIDProfileAgentAttributeId BIGINT = (SELECT ProfileAgentAttributeId FROM [DemandForecast].[ProfileAgentAttribute] WHERE ProfileAgentAttributeDescription = 'Profile Agent API GUID')

DECLARE @MeterSpecificProfileAgentId BIGINT = (SELECT ProfileAgentId FROM [DemandForecast].[ProfileAgent] WHERE ProfileAgentGUID = '884AA73C-41E6-4718-858E-870D0CC7BF72')
DECLARE @FlexSpecificProfileAgentId BIGINT = (SELECT ProfileAgentId FROM [DemandForecast].[ProfileAgent] WHERE ProfileAgentGUID = '624DF74E-2D34-43AF-A9A7-EA24F0A525AC')
DECLARE @GenericProfileAgentId BIGINT = (SELECT ProfileAgentId FROM [DemandForecast].[ProfileAgent] WHERE ProfileAgentGUID = 'CB3590BB-3B06-4020-B70B-AEB72A84424F')

EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @MeterSpecificProfileAgentId, @NameProfileAgentAttributeId, 'Meter Specific Profile Agent'
EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @FlexSpecificProfileAgentId, @NameProfileAgentAttributeId, 'Flex Specific Profile Agent'
EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @GenericProfileAgentId, @NameProfileAgentAttributeId, 'Generic Profile Agent'

EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @MeterSpecificProfileAgentId, @PriorityProfileAgentAttributeId, '1'
EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @FlexSpecificProfileAgentId, @PriorityProfileAgentAttributeId, '2'
EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @GenericProfileAgentId, @PriorityProfileAgentAttributeId, '3'

EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @MeterSpecificProfileAgentId, @ProfileAgentAPIGUIDProfileAgentAttributeId, '9B59A99A-F7F1-49E3-B109-A3499CA3EDA0'
EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @FlexSpecificProfileAgentId, @ProfileAgentAPIGUIDProfileAgentAttributeId, '75B4535D-1019-4AE0-BEAF-FF6A19DF6FA3'
EXEC [DemandForecast].[ProfileAgentDetail_Insert] @CreatedByUserId, @SourceId, @GenericProfileAgentId, @ProfileAgentAPIGUIDProfileAgentAttributeId, '4C7E4CAF-5C6A-4B18-B939-63F7C3D2FD35'