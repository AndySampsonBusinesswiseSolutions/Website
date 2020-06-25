USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND SourceTypeEntityId = 0)

EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'January'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'February'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'March'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'April'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'May'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'June'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'July'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'August'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'September'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'October'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'November'
EXEC [Information].[Month_Insert] @CreatedByUserId, @SourceId, 'December'