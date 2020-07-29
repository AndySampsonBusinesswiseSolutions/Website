USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Five Minute', 'Five Minutely'
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Half Hour', 'Half Hourly'
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Day', 'Daily'
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Week', 'Weekly'
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Month', 'Monthly'
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Quarter', 'Quarterly'
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Year', 'Yearly'