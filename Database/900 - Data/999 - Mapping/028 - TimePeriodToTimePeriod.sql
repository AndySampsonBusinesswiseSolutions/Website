USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @DayTimePeriodId BIGINT = (SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE StartTime = '00:00:00.0000000' AND EndTime = '00:00:00.0000000')

--Five Minute and Half Hour to Day
INSERT INTO [Mapping].[TimePeriodToTimePeriod]
    (
        CreatedByUserId,
        SourceId,
        TimePeriodId,
        MappedTimePeriodId
    )
SELECT	
	@CreatedByUserId,
	@SourceId,
	TimePeriod.TimePeriodId,
	@DayTimePeriodId
FROM
	[Information].[TimePeriod]
WHERE
	TimePeriod.TimePeriodId != @DayTimePeriodId

--Five Minute to Half Hour except for 
	--last half hour because end time is midnight and before start time
	--clock change periods because they result in too many mappings with this code
INSERT INTO [Mapping].[TimePeriodToTimePeriod]
    (
        CreatedByUserId,
        SourceId,
        TimePeriodId,
        MappedTimePeriodId
    )
SELECT	
	@CreatedByUserId,
	@SourceId,
	TimePeriod.TimePeriodId,
	MappedTimePeriod.TimePeriodId
FROM
	[Information].[TimePeriod]
INNER JOIN
	[Information].[TimePeriod] MappedTimePeriod
	ON MappedTimePeriod.TimePeriodId != TimePeriod.TimePeriodId
	AND MappedTimePeriod.StartTime < TimePeriod.EndTime
	AND MappedTimePeriod.EndTime > TimePeriod.StartTime
	AND MappedTimePeriod.TimePeriodId != @DayTimePeriodId
	AND DATEPART(MINUTE, MappedTimePeriod.StartTime) % 5 = 0
WHERE
	TimePeriod.TimePeriodId != @DayTimePeriodId
	AND DATEDIFF(MINUTE, TimePeriod.StartTime, TimePeriod.EndTime) = 5
	AND DATEPART(MINUTE, TimePeriod.StartTime) % 5 = 0

--Five Minute to Half Hour for last half hour
INSERT INTO [Mapping].[TimePeriodToTimePeriod]
    (
        CreatedByUserId,
        SourceId,
        TimePeriodId,
        MappedTimePeriodId
    )
SELECT	
	@CreatedByUserId,
	@SourceId,
	TimePeriod.TimePeriodId,
	MappedTimePeriod.TimePeriodId
FROM
	[Information].[TimePeriod]
INNER JOIN
	[Information].[TimePeriod] MappedTimePeriod
	ON MappedTimePeriod.TimePeriodId != TimePeriod.TimePeriodId
	AND MappedTimePeriod.TimePeriodId != @DayTimePeriodId
	AND DATEDIFF(MINUTE, MappedTimePeriod.StartTime, MappedTimePeriod.EndTime) = -1410
	AND DATEPART(MINUTE, MappedTimePeriod.StartTime) % 5 = 0
	AND MappedTimePeriod.StartTime <= TimePeriod.StartTime
WHERE
	TimePeriod.TimePeriodId != @DayTimePeriodId
	AND DATEPART(MINUTE, TimePeriod.StartTime) % 5 = 0

--Five Minute to Half Hour for clock change periods
INSERT INTO [Mapping].[TimePeriodToTimePeriod]
    (
        CreatedByUserId,
        SourceId,
        TimePeriodId,
        MappedTimePeriodId
    )
SELECT
	@CreatedByUserId,
	@SourceId,
	TimePeriod.TimePeriodId,
	(SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE EndTime = '01:31:00.0000000')
FROM
	[Information].[TimePeriod]
WHERE
	TimePeriod.EndTime IN 
	(
		'01:36:00.0000000',
		'01:41:00.0000000',
		'01:46:00.0000000',
		'01:51:00.0000000',
		'01:56:00.0000000',
		'02:01:00.0000000'
	)

INSERT INTO [Mapping].[TimePeriodToTimePeriod]
    (
        CreatedByUserId,
        SourceId,
        TimePeriodId,
        MappedTimePeriodId
    )
SELECT
	@CreatedByUserId,
	@SourceId,
	TimePeriod.TimePeriodId,
	(SELECT TimePeriodId FROM [Information].[TimePeriod] WHERE EndTime = '01:32:00.0000000')
FROM
	[Information].[TimePeriod]
WHERE
	TimePeriod.EndTime IN 
	(
		'02:06:00.0000000',
		'02:11:00.0000000',
		'02:16:00.0000000',
		'02:21:00.0000000',
		'02:26:00.0000000',
		'02:31:00.0000000'
	)