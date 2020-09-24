USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Table SQL')
DECLARE @ForecastUsageLatestTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Table SQL')

--Quarter
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '8029270C-1ECB-43B0-B313-9082890CDC8B')

DECLARE @SQL NVARCHAR(MAX) = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageQuarterHistory](
	[ForecastUsageQuarterHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[EffectiveFromDateTime] [datetime] NOT NULL,
	[EffectiveToDateTime] [datetime] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
    [YearId] [bigint] NOT NULL,
    [QuarterId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
 CONSTRAINT [PK_ForecastUsageQuarterHistory] PRIMARY KEY CLUSTERED 
(
	[ForecastUsageQuarterHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [Supply]
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory] ADD  CONSTRAINT [DF_ForecastUsageQuarterHistory_EffectiveFromDateTime]  DEFAULT (GETUTCDATE()) FOR [EffectiveFromDateTime]

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory] ADD  CONSTRAINT [DF_ForecastUsageQuarterHistory_EffectiveToDateTime]  DEFAULT (''9999-12-31'') FOR [EffectiveToDateTime]

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory] ADD  CONSTRAINT [DF_ForecastUsageQuarterHistory_CreatedDateTime]  DEFAULT (GETUTCDATE()) FOR [CreatedDateTime]

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageQuarterHistory_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [Administration.User].[User] ([UserId])

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory] CHECK CONSTRAINT [FK_ForecastUsageQuarterHistory_CreatedByUserId]

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageQuarterHistory_SourceId] FOREIGN KEY([SourceId])
REFERENCES [Information].[Source] ([SourceId])

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory] CHECK CONSTRAINT [FK_ForecastUsageQuarterHistory_SourceId]

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageQuarterHistory_QuarterId] FOREIGN KEY([QuarterId])
REFERENCES [Information].[Quarter] ([QuarterId])

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory] CHECK CONSTRAINT [FK_ForecastUsageQuarterHistory_QuarterId]

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageQuarterHistory_YearId] FOREIGN KEY([YearId])
REFERENCES [Information].[Year] ([YearId])

ALTER TABLE [Supply.X].[ForecastUsageQuarterHistory] CHECK CONSTRAINT [FK_ForecastUsageQuarterHistory_YearId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageQuarterHistory].CreatedByUserId to [Administration.User].[User].UserId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageQuarterHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageQuarterHistory_CreatedByUserId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageQuarterHistory].SourceId to [Information].[Source].SourceId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageQuarterHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageQuarterHistory_SourceId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageQuarterHistory].QuarterId to [Information].[Quarter].QuarterId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageQuarterHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageQuarterHistory_QuarterId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageQuarterHistory].YearId to [Information].[Year].YearId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageQuarterHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageQuarterHistory_YearId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryTableSQLGranularityAttributeId, @SQL

SET @SQL = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageQuarterLatest](
    [YearId] [bigint] NOT NULL,
    [QuarterId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageQuarterLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageQuarterLatest_QuarterId] FOREIGN KEY([QuarterId])
REFERENCES [Information].[Quarter] ([QuarterId])

ALTER TABLE [Supply.X].[ForecastUsageQuarterLatest] CHECK CONSTRAINT [FK_ForecastUsageQuarterLatest_QuarterId]

ALTER TABLE [Supply.X].[ForecastUsageQuarterLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageQuarterLatest_YearId] FOREIGN KEY([YearId])
REFERENCES [Information].[Year] ([YearId])

ALTER TABLE [Supply.X].[ForecastUsageQuarterLatest] CHECK CONSTRAINT [FK_ForecastUsageQuarterLatest_YearId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageQuarterLatest].QuarterId to [Information].[Quarter].QuarterId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageQuarterLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageQuarterLatest_QuarterId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageQuarterLatest].YearId to [Information].[Year].YearId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageQuarterLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageQuarterLatest_YearId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestTableSQLGranularityAttributeId, @SQL