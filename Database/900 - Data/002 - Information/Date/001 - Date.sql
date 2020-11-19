USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @StartDate DATE = '2015-01-01'
DECLARE @EndDate DATE = '2025-12-31'

INSERT INTO
	[Information].[Date]
	(
		CreatedByUserId,
		SourceId,
		DateDescription
	)
SELECT  TOP (DATEDIFF(DAY, @StartDate, @EndDate) + 1)
	@CreatedByUserId,
	@SourceId,
    DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @StartDate)
FROM
	sys.all_objects a
CROSS JOIN 
	sys.all_objects b;