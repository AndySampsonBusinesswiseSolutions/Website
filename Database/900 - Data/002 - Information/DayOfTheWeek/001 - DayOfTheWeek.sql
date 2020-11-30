USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[DayOfTheWeek_Insert] @CreatedByUserId, @SourceId, 'Monday'
EXEC [Information].[DayOfTheWeek_Insert] @CreatedByUserId, @SourceId, 'Tuesday'
EXEC [Information].[DayOfTheWeek_Insert] @CreatedByUserId, @SourceId, 'Wednesday'
EXEC [Information].[DayOfTheWeek_Insert] @CreatedByUserId, @SourceId, 'Thursday'
EXEC [Information].[DayOfTheWeek_Insert] @CreatedByUserId, @SourceId, 'Friday'
EXEC [Information].[DayOfTheWeek_Insert] @CreatedByUserId, @SourceId, 'Saturday'
EXEC [Information].[DayOfTheWeek_Insert] @CreatedByUserId, @SourceId, 'Sunday'