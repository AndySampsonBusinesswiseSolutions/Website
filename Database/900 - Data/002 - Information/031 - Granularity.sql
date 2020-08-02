USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'FiveMinute', 'Five Minute', 'Five Minutely', 1
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'HalfHour', 'Half Hour', 'Half Hourly', 1
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Date', 'Day', 'Daily', 0
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Week', 'Week', 'Weekly', 0
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Month', 'Month', 'Monthly', 0
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Quarter', 'Quarter', 'Quarterly', 0
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'Year', 'Year', 'Yearly', 0