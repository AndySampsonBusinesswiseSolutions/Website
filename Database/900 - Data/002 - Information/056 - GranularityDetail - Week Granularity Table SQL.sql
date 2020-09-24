USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Table SQL')
DECLARE @ForecastUsageLatestTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Table SQL')

--Week
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '8FD4C63A-84D5-4A03-B488-1A99C2331726')

DECLARE @SQL NVARCHAR(MAX) = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageWeekHistory](
	[ForecastUsageWeekHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[EffectiveFromDateTime] [datetime] NOT NULL,
	[EffectiveToDateTime] [datetime] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
    [YearId] [bigint] NOT NULL,
    [WeekId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
 CONSTRAINT [PK_ForecastUsageWeekHistory] PRIMARY KEY CLUSTERED 
(
	[ForecastUsageWeekHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [Supply]
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory] ADD  CONSTRAINT [DF_ForecastUsageWeekHistory_EffectiveFromDateTime]  DEFAULT (GETUTCDATE()) FOR [EffectiveFromDateTime]

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory] ADD  CONSTRAINT [DF_ForecastUsageWeekHistory_EffectiveToDateTime]  DEFAULT (''9999-12-31'') FOR [EffectiveToDateTime]

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory] ADD  CONSTRAINT [DF_ForecastUsageWeekHistory_CreatedDateTime]  DEFAULT (GETUTCDATE()) FOR [CreatedDateTime]

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageWeekHistory_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [Administration.User].[User] ([UserId])

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory] CHECK CONSTRAINT [FK_ForecastUsageWeekHistory_CreatedByUserId]

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageWeekHistory_SourceId] FOREIGN KEY([SourceId])
REFERENCES [Information].[Source] ([SourceId])

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory] CHECK CONSTRAINT [FK_ForecastUsageWeekHistory_SourceId]

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageWeekHistory_WeekId] FOREIGN KEY([WeekId])
REFERENCES [Information].[Week] ([WeekId])

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory] CHECK CONSTRAINT [FK_ForecastUsageWeekHistory_WeekId]

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageWeekHistory_YearId] FOREIGN KEY([YearId])
REFERENCES [Information].[Year] ([YearId])

ALTER TABLE [Supply.X].[ForecastUsageWeekHistory] CHECK CONSTRAINT [FK_ForecastUsageWeekHistory_YearId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageWeekHistory].CreatedByUserId to [Administration.User].[User].UserId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageWeekHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageWeekHistory_CreatedByUserId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageWeekHistory].SourceId to [Information].[Source].SourceId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageWeekHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageWeekHistory_SourceId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageWeekHistory].WeekId to [Information].[Week].WeekId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageWeekHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageWeekHistory_WeekId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageWeekHistory].YearId to [Information].[Year].YearId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageWeekHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageWeekHistory_YearId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryTableSQLGranularityAttributeId, @SQL

SET @SQL = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageWeekLatest](
    [YearId] [bigint] NOT NULL,
    [WeekId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageWeekLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageWeekLatest_WeekId] FOREIGN KEY([WeekId])
REFERENCES [Information].[Week] ([WeekId])

ALTER TABLE [Supply.X].[ForecastUsageWeekLatest] CHECK CONSTRAINT [FK_ForecastUsageWeekLatest_WeekId]

ALTER TABLE [Supply.X].[ForecastUsageWeekLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageWeekLatest_YearId] FOREIGN KEY([YearId])
REFERENCES [Information].[Year] ([YearId])

ALTER TABLE [Supply.X].[ForecastUsageWeekLatest] CHECK CONSTRAINT [FK_ForecastUsageWeekLatest_YearId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageWeekLatest].WeekId to [Information].[Week].WeekId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageWeekLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageWeekLatest_WeekId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageWeekLatest].YearId to [Information].[Year].YearId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageWeekLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageWeekLatest_YearId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestTableSQLGranularityAttributeId, @SQL