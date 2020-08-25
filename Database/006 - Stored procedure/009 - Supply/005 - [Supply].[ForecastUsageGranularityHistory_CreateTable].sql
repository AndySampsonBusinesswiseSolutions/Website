USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[ForecastUsageGranularityHistory_CreateTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[ForecastUsageGranularityHistory_CreateTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new ForecastUsageHistory table for a granularity for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[ForecastUsageGranularityHistory_CreateTable]
    @MeterId BIGINT,
    @GranularityCode VARCHAR(255),
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
    DECLARE @TableName NVARCHAR(255) = 'ForecastUsage' + @GranularityCode + 'History'
    DECLARE @RequiresDateColumn BIT = (SELECT IsTimePeriod FROM [Information].[Granularity] WHERE GranularityCode = @GranularityCode)
    DECLARE @KeyName NVARCHAR(255)
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[' + @TableName + ']
	(
        ForecastUsage' + @GranularityCode + 'HistoryId BIGINT IDENTITY(1,1) NOT NULL,
        EffectiveFromDateTime DATETIME NOT NULL,
        EffectiveToDateTime DATETIME NOT NULL,
        CreatedDateTime DATETIME NOT NULL,
        CreatedByUserId BIGINT NOT NULL,
        SourceId BIGINT NOT NULL,
        TimePeriodId BIGINT NOT NULL,'

    IF @RequiresDateColumn = 1
        BEGIN
            SET @SQL = @SQL + 'DateId BIGINT NOT NULL,'
        END
        
    SET @SQL = @SQL + 'Usage DECIMAL(18,10) NOT NULL
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Primary Key
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	PK_' + @TableName + ' PRIMARY KEY CLUSTERED 
	(
	' + @TableName + 'Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Defaults
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	    DF_' + @TableName + '_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	    DF_' + @TableName + '_EffectiveToDateTime DEFAULT ''9999-12-31'' FOR EffectiveToDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	    DF_' + @TableName + '_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime'
    EXEC sp_sqlexec @SQL

    --Add Foreign Keys
    SET @KeyName = 'FK_' + @TableName + '_CreatedByUserId'
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	' + @KeyName + ' FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[' + @TableName + '].CreatedByUserId to [Administration.User].[User].UserId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', @TableName, N'CONSTRAINT', @KeyName

    SET @KeyName = 'FK_' + @TableName + '_SourceId'
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	' + @KeyName + ' FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[' + @TableName + '].SourceId to [Information].[Source].SourceId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', @TableName, N'CONSTRAINT', @KeyName

    SET @KeyName = 'FK_' + @TableName + '_' + @GranularityCode + 'Id'
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	' + @KeyName + ' FOREIGN KEY
	(
	TimePeriodId
	) REFERENCES [Information].[TimePeriod]
	(
	TimePeriodId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[' + @TableName + '].' + @GranularityCode + 'Id to [Information].[TimePeriod].TimePeriodId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', @TableName, N'CONSTRAINT', @KeyName

    IF @RequiresDateColumn = 1
        BEGIN
            SET @KeyName = 'FK_' + @TableName + '_DateId'
            SET @SQL = N'
            USE [EMaaS]
            
            ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
            ' + @KeyName + ' FOREIGN KEY
            (
            DateId
            ) REFERENCES [Information].[Date]
            (
            DateId
            ) ON UPDATE NO ACTION 
            ON DELETE NO ACTION'
            EXEC sp_sqlexec @SQL

            SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[' + @TableName + '].DateId to [Information].[Date].DateId'
            EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', @TableName, N'CONSTRAINT', @KeyName
        END
END
GO
