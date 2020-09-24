USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Table SQL')
DECLARE @ForecastUsageLatestTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Table SQL')

--Five Minute
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '4D55BB09-9F8F-4AB6-917E-23B1D09E71AD')

DECLARE @SQL NVARCHAR(MAX) = N'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageFiveMinuteHistory](
	[ForecastUsageFiveMinuteHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[EffectiveFromDateTime] [datetime] NOT NULL,
	[EffectiveToDateTime] [datetime] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
    [DateId] [bigint] NOT NULL,
	[TimePeriodId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
 CONSTRAINT [PK_ForecastUsageFiveMinuteHistory] PRIMARY KEY CLUSTERED 
(
	[ForecastUsageFiveMinuteHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [Supply]
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory] ADD  CONSTRAINT [DF_ForecastUsageFiveMinuteHistory_EffectiveFromDateTime]  DEFAULT (getutcdate()) FOR [EffectiveFromDateTime]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory] ADD  CONSTRAINT [DF_ForecastUsageFiveMinuteHistory_EffectiveToDateTime]  DEFAULT (''9999-12-31'') FOR [EffectiveToDateTime]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory] ADD  CONSTRAINT [DF_ForecastUsageFiveMinuteHistory_CreatedDateTime]  DEFAULT (getutcdate()) FOR [CreatedDateTime]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [Administration.User].[User] ([UserId])

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory] CHECK CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_CreatedByUserId]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_TimePeriodId] FOREIGN KEY([TimePeriodId])
REFERENCES [Information].[TimePeriod] ([TimePeriodId])

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory] CHECK CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_TimePeriodId]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_SourceId] FOREIGN KEY([SourceId])
REFERENCES [Information].[Source] ([SourceId])

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory] CHECK CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_SourceId]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_DateId] FOREIGN KEY([DateId])
REFERENCES [Information].[Date] ([DateId])

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteHistory] CHECK CONSTRAINT [FK_ForecastUsageFiveMinuteHistory_DateId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageFiveMinuteHistory].CreatedByUserId to [Administration.User].[User].UserId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageFiveMinuteHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageFiveMinuteHistory_CreatedByUserId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageFiveMinuteHistory].TimePeriodId to [Information].[TimePeriod].TimePeriodId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageFiveMinuteHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageFiveMinuteHistory_TimePeriodId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageFiveMinuteHistory].SourceId to [Information].[Source].SourceId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageFiveMinuteHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageFiveMinuteHistory_SourceId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageFiveMinuteHistory].DateId to [Information].[Date].DateId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageFiveMinuteHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageFiveMinuteHistory_DateId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryTableSQLGranularityAttributeId, @SQL

SET @SQL = N'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageFiveMinuteLatest](
    [DateId] [bigint] NOT NULL,
	[TimePeriodId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageFiveMinuteLatest_TimePeriodId] FOREIGN KEY([TimePeriodId])
REFERENCES [Information].[TimePeriod] ([TimePeriodId])

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteLatest] CHECK CONSTRAINT [FK_ForecastUsageFiveMinuteLatest_TimePeriodId]

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageFiveMinuteLatest_DateId] FOREIGN KEY([DateId])
REFERENCES [Information].[Date] ([DateId])

ALTER TABLE [Supply.X].[ForecastUsageFiveMinuteLatest] CHECK CONSTRAINT [FK_ForecastUsageFiveMinuteLatest_DateId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageFiveMinuteLatest].TimePeriodId to [Information].[TimePeriod].TimePeriodId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageFiveMinuteLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageFiveMinuteLatest_TimePeriodId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageFiveMinuteLatest].DateId to [Information].[Date].DateId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageFiveMinuteLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageFiveMinuteLatest_DateId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestTableSQLGranularityAttributeId, @SQL