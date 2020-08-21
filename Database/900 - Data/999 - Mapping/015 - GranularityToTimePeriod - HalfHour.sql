USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityDescription = 'Half Hour')

DECLARE @TimePeriodId BIGINT

DECLARE TimePeriodCursor CURSOR FOR
SELECT TimePeriodId
FROM [Information].[TimePeriod]
WHERE DATEDIFF(minute, StartTime, EndTime) = 30
OR DATEDIFF(minute, StartTime, EndTime) = -1410

OPEN TimePeriodCursor

FETCH NEXT FROM TimePeriodCursor
INTO @TimePeriodId

WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [Mapping].[GranularityToTimePeriod_Insert] 
			@CreatedByUserId, 
			@SourceId, 
			@GranularityId, 
			@TimePeriodId

		FETCH NEXT FROM TimePeriodCursor
		INTO @TimePeriodId
	END
CLOSE TimePeriodCursor;
DEALLOCATE TimePeriodCursor;