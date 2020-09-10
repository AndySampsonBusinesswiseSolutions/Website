USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @CommodityId BIGINT = (SELECT CommodityId FROM [Information].[Commodity] WHERE CommodityDescription = 'Electricity')

DECLARE @ProfileId BIGINT = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '860DB634-8C06-46EC-8335-5136F7BC5D07')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '0EEBDC0D-C4BA-4D02-B358-BA9519BCEC67')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = 'C06D87A0-D7CC-40CD-B512-0262007EA179')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = 'E1BFF30D-6C08-4BD5-BB07-336953BD7511')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '3342CC78-582E-499B-B053-093F50365FAD')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '3D45D041-22B8-4A53-B5E1-B6C7F932285C')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '01BD095A-4F09-4B98-8978-21BFB70C179E')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '2E3E5721-2849-47CE-ABC9-27C7D2A42310')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId

SET @CommodityId = (SELECT CommodityId FROM [Information].[Commodity] WHERE CommodityDescription = 'Gas')
SET @ProfileId = (SELECT ProfileId FROM [DemandForecast].[Profile] WHERE ProfileGUID = '632CED37-64BB-4DA0-8B34-C0B9C1C98E0D')
EXEC [Mapping].[CommodityToProfile_Insert] @CreatedByUserId, @SourceId, @CommodityId, @ProfileId