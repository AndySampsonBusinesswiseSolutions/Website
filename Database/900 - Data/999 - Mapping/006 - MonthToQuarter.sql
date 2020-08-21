USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @QuarterId BIGINT = (SELECT QuarterId FROM [Information].[Quarter] WHERE QuarterDescription = 'Quarter 1')
DECLARE @MonthId BIGINT = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'January')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'February')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'March')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @QuarterId = (SELECT QuarterId FROM [Information].[Quarter] WHERE QuarterDescription = 'Quarter 2')
SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'April')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'May')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'June')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @QuarterId = (SELECT QuarterId FROM [Information].[Quarter] WHERE QuarterDescription = 'Quarter 3')
SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'July')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'August')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'September')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @QuarterId = (SELECT QuarterId FROM [Information].[Quarter] WHERE QuarterDescription = 'Quarter 4')
SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'October')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'November')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId

SET @MonthId = (SELECT MonthId FROM [Information].[Month] WHERE MonthDescription = 'December')
EXEC [Mapping].[MonthToQuarter_Insert] @CreatedByUserId, @SourceId, @MonthId, @QuarterId