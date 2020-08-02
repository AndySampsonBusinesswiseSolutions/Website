USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[EstimatedAnnualUsage_CreateTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[EstimatedAnnualUsage_CreateTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new EstimatedAnnualUsage table for a granularity for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[EstimatedAnnualUsage_CreateTable]
    @MeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-30 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.Meter' + CONVERT(NVARCHAR, @MeterId)
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[EstimatedAnnualUsage]
	(
        EstimatedAnnualUsageId BIGINT NOT NULL,
        EffectiveFromDateTime DATETIME NOT NULL,
        EffectiveToDateTime DATETIME NOT NULL,
        CreatedDateTime DATETIME NOT NULL,
        CreatedByUserId BIGINT NOT NULL,
        SourceId BIGINT NOT NULL,
        Usage DECIMAL(18,10) NOT NULL
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Primary Key
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[EstimatedAnnualUsage] ADD CONSTRAINT
	PK_EstimatedAnnualUsage PRIMARY KEY CLUSTERED 
	(
	EstimatedAnnualUsageId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Defaults
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[EstimatedAnnualUsage] ADD CONSTRAINT
	    DF_EstimatedAnnualUsage_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[EstimatedAnnualUsage] ADD CONSTRAINT
	    DF_EstimatedAnnualUsage_EffectiveToDateTime DEFAULT ''9999-12-31'' FOR EffectiveToDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[EstimatedAnnualUsage] ADD CONSTRAINT
	    DF_EstimatedAnnualUsage_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime'
    EXEC sp_sqlexec @SQL

    --Add Foreign Keys
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[EstimatedAnnualUsage] ADD CONSTRAINT
	FK_EstimatedAnnualUsage_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[EstimatedAnnualUsage].CreatedByUserId to [Administration.User].[User].UserId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', 'EstimatedAnnualUsage', N'CONSTRAINT', 'FK_EstimatedAnnualUsage_CreatedByUserId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[EstimatedAnnualUsage] ADD CONSTRAINT
	FK_EstimatedAnnualUsage_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[EstimatedAnnualUsage].SourceId to [Information].[Source].SourceId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', 'EstimatedAnnualUsage', N'CONSTRAINT', 'FK_EstimatedAnnualUsage_SourceId'
END
GO
