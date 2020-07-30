USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply.Meter].[ForecastUsage' + @Granularity + 'Latest_CreateTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply.Meter].[ForecastUsage' + @Granularity + 'Latest_CreateTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Create new ForecastUsageLatest table for a granularity for MeterId
-- =============================================

ALTER PROCEDURE [Supply.Meter].[ForecastUsage' + @Granularity + 'Latest_CreateTable]
    @MeterId BIGINT,
    @Granularity VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.Meter' + @MeterId
    DECLARE @TableName NVARCHAR(255) = 'ForecastUsage' + @Granularity + 'Latest'
    DECLARE @RequiresDateColumn BIT = (SELECT IsTimePeriod FROM [Information].[Granularity] WHERE GranularityDescription = @Granularity)
    DECLARE @ForeignKeyName NVARCHAR(255)
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(255) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[' + @TableName + ']
	(
        ' + @Granularity + 'Id BIGINT NOT NULL,'

    IF @RequiresDateColumn = 1
        BEGIN
            SET @SQL = @SQL + 'DateId BIGINT NOT NULL,'
        END
        
    SET @SQL = @SQL + 'Usage DECIMAL(18,10)
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Granularity Foreign Key
    SET @ForeignKeyName = 'FK_' + @TableName + '_' + @Granularity + 'Id'
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
	' + @ForeignKeyName + ' FOREIGN KEY
	(
	' + @Granularity + 'Id
	) REFERENCES [Information].[' + @Granularity + ']
	(
	' + @Granularity + 'Id
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[' + @TableName + '].' + @Granularity + 'Id to [Information].[' + @Granularity + '].' + @Granularity + 'Id'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', @TableName, N'CONSTRAINT', @ForeignKeyName

    --Add Date Foreign Key if required
    IF @RequiresDateColumn = 1
        BEGIN
            SET @ForeignKeyName = 'FK_' + @TableName + '_DateId'
            SET @SQL = N'
            USE [EMaaS]
            
            ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CONSTRAINT
            ' + @ForeignKeyName + ' FOREIGN KEY
            (
            DateId
            ) REFERENCES [Information].[Date]
            (
            DateId
            ) ON UPDATE NO ACTION 
            ON DELETE NO ACTION'
            EXEC sp_sqlexec @SQL

            SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[' + @TableName + '].DateId to [Information].[Date].DateId'
            EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', @TableName, N'CONSTRAINT', @ForeignKeyName
        END
END
GO
