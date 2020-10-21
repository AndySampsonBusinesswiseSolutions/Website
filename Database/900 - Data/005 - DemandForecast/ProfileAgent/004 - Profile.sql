USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, '860DB634-8C06-46EC-8335-5136F7BC5D07' --Electricity Profile Class 1
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, '0EEBDC0D-C4BA-4D02-B358-BA9519BCEC67' --Electricity Profile Class 2
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, 'C06D87A0-D7CC-40CD-B512-0262007EA179' --Electricity Profile Class 3
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, 'E1BFF30D-6C08-4BD5-BB07-336953BD7511' --Electricity Profile Class 4
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, '3342CC78-582E-499B-B053-093F50365FAD' --Electricity Profile Class 5
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, '3D45D041-22B8-4A53-B5E1-B6C7F932285C' --Electricity Profile Class 6
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, '01BD095A-4F09-4B98-8978-21BFB70C179E' --Electricity Profile Class 7
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, '2E3E5721-2849-47CE-ABC9-27C7D2A42310' --Electricity Profile Class 8
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, 'BF75CC1F-65C7-4571-9B1E-83BDD834F50D' --Electricity Profile Class 0
EXEC [DemandForecast].[Profile_Insert] @CreatedByUserId, @SourceId, '632CED37-64BB-4DA0-8B34-C0B9C1C98E0D' --Gas