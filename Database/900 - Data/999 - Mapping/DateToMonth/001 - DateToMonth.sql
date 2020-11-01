USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

INSERT INTO
	[Mapping].[DateToMonth]
	(
		CreatedByUserId,
		SourceId,
		DateId,
		MonthId
	)
SELECT
	@CreatedByUserId,
	@SourceId,
	[Date].[DateId],
	[Month].[MonthId]
FROM 
	[Information].[Date]
INNER JOIN
	[Information].[Month]
	ON [Month].MonthDescription = DATENAME(mm, [Date].[DateDescription])
LEFT OUTER JOIN
	[Mapping].[DateToMonth]
	ON [DateToMonth].[DateId] = [Date].[DateId]
	AND [DateToMonth].[MonthId] = [Month].[MonthId]
WHERE
	[DateToMonth].[DateToMonthId] IS NULL