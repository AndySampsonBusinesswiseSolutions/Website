USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Table SQL')
DECLARE @ForecastUsageLatestTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Table SQL')

--Month
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '2AB4DC83-C0D0-4C5F-AC95-6A948802E430')

DECLARE @SQL NVARCHAR(MAX) = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageMonthHistory](
	[ForecastUsageMonthHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
    [YearId] [bigint] NOT NULL,
    [MonthId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
 CONSTRAINT [PK_ForecastUsageMonthHistory] PRIMARY KEY CLUSTERED 
(
	[ForecastUsageMonthHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [Supply]
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory] ADD  CONSTRAINT [DF_ForecastUsageMonthHistory_CreatedDateTime]  DEFAULT (GETUTCDATE()) FOR [CreatedDateTime]

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageMonthHistory_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [Administration.User].[User] ([UserId])

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory] CHECK CONSTRAINT [FK_ForecastUsageMonthHistory_CreatedByUserId]

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageMonthHistory_SourceId] FOREIGN KEY([SourceId])
REFERENCES [Information].[Source] ([SourceId])

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory] CHECK CONSTRAINT [FK_ForecastUsageMonthHistory_SourceId]

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageMonthHistory_MonthId] FOREIGN KEY([MonthId])
REFERENCES [Information].[Month] ([MonthId])

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory] CHECK CONSTRAINT [FK_ForecastUsageMonthHistory_MonthId]

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageMonthHistory_YearId] FOREIGN KEY([YearId])
REFERENCES [Information].[Year] ([YearId])

ALTER TABLE [Supply.X].[ForecastUsageMonthHistory] CHECK CONSTRAINT [FK_ForecastUsageMonthHistory_YearId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageMonthHistory].CreatedByUserId to [Administration.User].[User].UserId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageMonthHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageMonthHistory_CreatedByUserId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageMonthHistory].SourceId to [Information].[Source].SourceId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageMonthHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageMonthHistory_SourceId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageMonthHistory].MonthId to [Information].[Month].MonthId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageMonthHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageMonthHistory_MonthId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageMonthHistory].YearId to [Information].[Year].YearId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageMonthHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageMonthHistory_YearId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryTableSQLGranularityAttributeId, @SQL

SET @SQL = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageMonthLatest](
    [YearId] [bigint] NOT NULL,
    [MonthId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageMonthLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageMonthLatest_MonthId] FOREIGN KEY([MonthId])
REFERENCES [Information].[Month] ([MonthId])

ALTER TABLE [Supply.X].[ForecastUsageMonthLatest] CHECK CONSTRAINT [FK_ForecastUsageMonthLatest_MonthId]

ALTER TABLE [Supply.X].[ForecastUsageMonthLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageMonthLatest_YearId] FOREIGN KEY([YearId])
REFERENCES [Information].[Year] ([YearId])

ALTER TABLE [Supply.X].[ForecastUsageMonthLatest] CHECK CONSTRAINT [FK_ForecastUsageMonthLatest_YearId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageMonthLatest].MonthId to [Information].[Month].MonthId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageMonthLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageMonthLatest_MonthId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageMonthLatest].YearId to [Information].[Year].YearId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageMonthLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageMonthLatest_YearId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestTableSQLGranularityAttributeId, @SQL