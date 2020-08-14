USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[LoadedUsage_CreateTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[LoadedUsage_CreateTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new ForecastUsageHistory table for a granularity for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[LoadedUsage_CreateTable]
    @MeterId BIGINT,
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-30 -> Andrew Sampson -> Initial development of script
    -- 2020-08-14 -> Andrew Sampson -> Added MeterType parameter
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[LoadedUsage]
	(
        LoadedUsageId BIGINT NOT NULL,
        EffectiveFromDateTime DATETIME NOT NULL,
        EffectiveToDateTime DATETIME NOT NULL,
        CreatedDateTime DATETIME NOT NULL,
        CreatedByUserId BIGINT NOT NULL,
        SourceId BIGINT NOT NULL,
        DateId BIGINT NOT NULL,
        TimePeriodId BIGINT NOT NULL,
        UsageTypeId BIGINT NOT NULL,
        Usage DECIMAL(18,10) NOT NULL
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Primary Key
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
	PK_LoadedUsage PRIMARY KEY CLUSTERED 
	(
	LoadedUsageId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Defaults
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
	    DF_LoadedUsage_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
	    DF_LoadedUsage_EffectiveToDateTime DEFAULT ''9999-12-31'' FOR EffectiveToDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
	    DF_LoadedUsage_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime'
    EXEC sp_sqlexec @SQL

    --Add Foreign Keys
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
	FK_LoadedUsage_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[LoadedUsage].CreatedByUserId to [Administration.User].[User].UserId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', N'LoadedUsage', N'CONSTRAINT', 'FK_LoadedUsage_CreatedByUserId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
	FK_LoadedUsage_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[LoadedUsage].SourceId to [Information].[Source].SourceId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', N'LoadedUsage', N'CONSTRAINT', 'FK_LoadedUsage_SourceId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
    FK_LoadedUsage_DateId FOREIGN KEY
    (
    DateId
    ) REFERENCES [Information].[Date]
    (
    DateId
    ) ON UPDATE NO ACTION 
    ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[LoadedUsage].DateId to [Information].[Date].DateId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', N'LoadedUsage', N'CONSTRAINT', 'FK_LoadedUsage_DateId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
    FK_LoadedUsage_TimePeriodId FOREIGN KEY
    (
    TimePeriodId
    ) REFERENCES [Information].[TimePeriod]
    (
    TimePeriodId
    ) ON UPDATE NO ACTION 
    ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[LoadedUsage].DateId to [Information].[TimePeriod].TimePeriodId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', N'LoadedUsage', N'CONSTRAINT', 'FK_LoadedUsage_TimePeriodId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsage] ADD CONSTRAINT
    FK_LoadedUsage_UsageTypeId FOREIGN KEY
    (
    UsageTypeId
    ) REFERENCES [Information].[UsageType]
    (
    UsageTypeId
    ) ON UPDATE NO ACTION 
    ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[LoadedUsage].DateId to [Information].[UsageType].UsageTypeId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', N'LoadedUsage', N'CONSTRAINT', 'FK_LoadedUsage_UsageTypeId'
END
GO
