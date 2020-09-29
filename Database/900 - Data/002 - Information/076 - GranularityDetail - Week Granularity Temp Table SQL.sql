USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryTempTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Temp Table SQL')
DECLARE @ForecastUsageLatestTempTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Temp Table SQL')

--Week
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '8FD4C63A-84D5-4A03-B488-1A99C2331726')

DECLARE @SQL NVARCHAR(MAX) = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageWeekHistory_Temp](
	[ProcessQueueGUID] VARCHAR(255) NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
    [YearId] [bigint] NOT NULL,
	[WeekId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryTempTableSQLGranularityAttributeId, @SQL

SET @SQL = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageWeekLatest_Temp](
	[ProcessQueueGUID] VARCHAR(255) NOT NULL,
    [YearId] [bigint] NOT NULL,
	[WeekId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestTempTableSQLGranularityAttributeId, @SQL