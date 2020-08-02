USE [EMaaS]
GO

SET DATEFIRST 1

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @StartDate DATE = '2015-01-01'
DECLARE @EndDate DATE = '2030-12-31'
DECLARE @DateDescription DATE

SET @DateDescription = @StartDate

WHILE @DateDescription <= @EndDate
	BEGIN
		EXEC [Information].[Date_Insert] @CreatedByUserId, @SourceId, @DateDescription

		SET @DateDescription = DATEADD(DAY, 1, @DateDescription)
	END