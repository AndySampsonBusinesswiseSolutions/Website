USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

INSERT INTO
	[Mapping].[DateToDayOfTheWeek]
	(
		CreatedByUserId,
		SourceId,
		DateId,
		DayOfTheWeekId
	)
SELECT
	@CreatedByUserId,
	@SourceId,
	[Date].[DateId],
	[DayOfTheWeek].[DayOfTheWeekId]
FROM 
	[Information].[Date]
INNER JOIN
	[Information].[DayOfTheWeek]
	ON [DayOfTheWeek].DayOfTheWeekDescription = DATENAME(dw, [Date].[DateDescription])
LEFT OUTER JOIN
	[Mapping].[DateToDayOfTheWeek]
	ON [DateToDayOfTheWeek].[DateId] = [Date].[DateId]
	AND [DateToDayOfTheWeek].[DayOfTheWeekId] = [DayOfTheWeek].[DayOfTheWeekId]
WHERE
	[DateToDayOfTheWeek].[DateToDayOfTheWeekId] IS NULL