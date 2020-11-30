USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Table SQL')
DECLARE @ForecastUsageLatestTableSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Table SQL')

--Date
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '71C54EC0-6415-42D4-9C8C-6D8B8513F2FE')

DECLARE @SQL NVARCHAR(MAX) = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageDateHistory](
	[ForecastUsageDateHistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
    [DateId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
 CONSTRAINT [PK_ForecastUsageDateHistory] PRIMARY KEY CLUSTERED 
(
	[ForecastUsageDateHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [Supply]
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageDateHistory] ADD  CONSTRAINT [DF_ForecastUsageDateHistory_CreatedDateTime]  DEFAULT (GETUTCDATE()) FOR [CreatedDateTime]

ALTER TABLE [Supply.X].[ForecastUsageDateHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageDateHistory_CreatedByUserId] FOREIGN KEY([CreatedByUserId])
REFERENCES [Administration.User].[User] ([UserId])

ALTER TABLE [Supply.X].[ForecastUsageDateHistory] CHECK CONSTRAINT [FK_ForecastUsageDateHistory_CreatedByUserId]

ALTER TABLE [Supply.X].[ForecastUsageDateHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageDateHistory_SourceId] FOREIGN KEY([SourceId])
REFERENCES [Information].[Source] ([SourceId])

ALTER TABLE [Supply.X].[ForecastUsageDateHistory] CHECK CONSTRAINT [FK_ForecastUsageDateHistory_SourceId]

ALTER TABLE [Supply.X].[ForecastUsageDateHistory]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageDateHistory_DateId] FOREIGN KEY([DateId])
REFERENCES [Information].[Date] ([DateId])

ALTER TABLE [Supply.X].[ForecastUsageDateHistory] CHECK CONSTRAINT [FK_ForecastUsageDateHistory_DateId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageDateHistory].CreatedByUserId to [Administration.User].[User].UserId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageDateHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageDateHistory_CreatedByUserId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageDateHistory].SourceId to [Information].[Source].SourceId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageDateHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageDateHistory_SourceId''
EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageDateHistory].DateId to [Information].[Date].DateId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageDateHistory'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageDateHistory_DateId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryTableSQLGranularityAttributeId, @SQL

SET @SQL = N'
USE [EMaaS]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [Supply.X].[ForecastUsageDateLatest](
    [DateId] [bigint] NOT NULL,
	[Usage] [decimal](18, 10) NOT NULL
) ON [Supply]

ALTER TABLE [Supply.X].[ForecastUsageDateLatest]  WITH CHECK ADD  CONSTRAINT [FK_ForecastUsageDateLatest_DateId] FOREIGN KEY([DateId])
REFERENCES [Information].[Date] ([DateId])

ALTER TABLE [Supply.X].[ForecastUsageDateLatest] CHECK CONSTRAINT [FK_ForecastUsageDateLatest_DateId]

EXEC sys.sp_addextendedproperty @name=N''MS_Description'', @value=N''Foreign Key constraint joining [Supply.X].[ForecastUsageDateLatest].DateId to [Information].[Date].DateId'' , @level0type=N''SCHEMA'',@level0name=N''Supply.X'', @level1type=N''TABLE'',@level1name=N''ForecastUsageDateLatest'', @level2type=N''CONSTRAINT'',@level2name=N''FK_ForecastUsageDateLatest_DateId''
'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestTableSQLGranularityAttributeId, @SQL