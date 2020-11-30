USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

INSERT INTO
	[Mapping].[DateToWeek]
	(
		CreatedByUserId,
		SourceId,
		DateId,
		WeekId
	)
SELECT
	@CreatedByUserId,
	@SourceId,
	[Date].[DateId],
	[Week].[WeekId]
FROM 
	[Information].[Date]
INNER JOIN
	[Information].[Week]
	ON [Week].WeekDescription = 'Week ' + CONVERT(VARCHAR, DATEPART(wk, [Date].[DateDescription]))
LEFT OUTER JOIN
	[Mapping].[DateToWeek]
	ON [DateToWeek].[DateId] = [Date].[DateId]
	AND [DateToWeek].[WeekId] = [Week].[WeekId]
WHERE
	[DateToWeek].[DateToWeekId] IS NULL