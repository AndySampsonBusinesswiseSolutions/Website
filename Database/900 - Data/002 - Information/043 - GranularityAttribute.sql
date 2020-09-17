USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Granularity Code'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Granularity Description'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Granularity Display Description'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Is Time Period'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Is Electricity Default'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Is Gas Default'

EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Five Minute History Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Half Hour History Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Date History Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Week History Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Month History Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Quarter History Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Year History Table SQL'

EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Five Minute Latest Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Half Hour Latest Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Date Latest Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Week Latest Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Month Latest Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Quarter Latest Table SQL'
EXEC [Information].[GranularityAttribute_Insert] @CreatedByUserId, @SourceId, 'Forecast Usage Year Latest Table SQL'