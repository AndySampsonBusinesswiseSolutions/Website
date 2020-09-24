USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Table SQL')
DECLARE @ForecastUsageLatestTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Table SQL')

--Half Hour
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = 'CEA433FB-5327-4747-95CB-0FEFD1D2AD6B')

DECLARE @SQL NVARCHAR(MAX) = N'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageHalfHourHistory](
	[ForecastUsageHalfHourHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[EffectiveFromDateTime] [datetime] NOT NULL,
	[EffectiveToDateTime] [datetime] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
    [DateId] [bigint] NOT NULL,
	[TimePeriodId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
 CONSTRAINT [PK_ForecastUsageHalfHourHistory] PRIMARY KEY CLUSTERED 
(
	[ForecastUsageHalfHourHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [Supply]
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory] ADD  CONSTRAINT [DF_ForecastUsageHalfHourHistory_EffectiveFromDateTime]  DEFAULT (getutcdate()) FOR [EffectiveFromDateTime]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory] ADD  CONSTRAINT [DF_ForecastUsageHalfHourHistory_EffectiveToDateTime]  DEFAULT (''9999-12-31'') FOR [EffectiveToDateTime]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory] ADD  CONSTRAINT [DF_ForecastUsageHalfHourHistory_CreatedDateTime]  DEFAULT (getutcdate()) FOR [CreatedDateTime]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageHalfHourHistory_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [Administration.User].[User] ([UserId])

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory] CHECK CONSTRAINT [FK_ForecastUsageHalfHourHistory_CreatedByUserId]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageHalfHourHistory_TimePeriodId] FOREIGN KEY([TimePeriodId])
REFERENCES [Information].[TimePeriod] ([TimePeriodId])

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory] CHECK CONSTRAINT [FK_ForecastUsageHalfHourHistory_TimePeriodId]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageHalfHourHistory_SourceId] FOREIGN KEY([SourceId])
REFERENCES [Information].[Source] ([SourceId])

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory] CHECK CONSTRAINT [FK_ForecastUsageHalfHourHistory_SourceId]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageHalfHourHistory_DateId] FOREIGN KEY([DateId])
REFERENCES [Information].[Date] ([DateId])

ALTER TABLE [Supply.X].[ForecastUsageHalfHourHistory] CHECK CONSTRAINT [FK_ForecastUsageHalfHourHistory_DateId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageHalfHourHistory].CreatedByUserId to [Administration.User].[User].UserId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageHalfHourHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageHalfHourHistory_CreatedByUserId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageHalfHourHistory].TimePeriodId to [Information].[TimePeriod].TimePeriodId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageHalfHourHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageHalfHourHistory_TimePeriodId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageHalfHourHistory].SourceId to [Information].[Source].SourceId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageHalfHourHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageHalfHourHistory_SourceId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageHalfHourHistory].DateId to [Information].[Date].DateId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageHalfHourHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageHalfHourHistory_DateId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryTableSQLGranularityAttributeId, @SQL

SET @SQL = N'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageHalfHourLatest](
    [DateId] [bigint] NOT NULL,
	[TimePeriodId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageHalfHourLatest_TimePeriodId] FOREIGN KEY([TimePeriodId])
REFERENCES [Information].[TimePeriod] ([TimePeriodId])

ALTER TABLE [Supply.X].[ForecastUsageHalfHourLatest] CHECK CONSTRAINT [FK_ForecastUsageHalfHourLatest_TimePeriodId]

ALTER TABLE [Supply.X].[ForecastUsageHalfHourLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageHalfHourLatest_DateId] FOREIGN KEY([DateId])
REFERENCES [Information].[Date] ([DateId])

ALTER TABLE [Supply.X].[ForecastUsageHalfHourLatest] CHECK CONSTRAINT [FK_ForecastUsageHalfHourLatest_DateId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageHalfHourLatest].TimePeriodId to [Information].[TimePeriod].TimePeriodId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageHalfHourLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageHalfHourLatest_TimePeriodId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageHalfHourLatest].DateId to [Information].[Date].DateId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageHalfHourLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageHalfHourLatest_DateId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestTableSQLGranularityAttributeId, @SQL