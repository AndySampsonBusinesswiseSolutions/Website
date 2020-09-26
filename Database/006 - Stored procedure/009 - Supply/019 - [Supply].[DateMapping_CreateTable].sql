USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[DateMapping_CreateTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[DateMapping_CreateTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-24
-- Description:	Create new DateMapping table for a granularity for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[DateMapping_CreateTable]
    @MeterId BIGINT,
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[DateMapping]
	(
        DateMappingId BIGINT IDENTITY(1,1) NOT NULL,
        EffectiveFromDateTime DATETIME NOT NULL,
        EffectiveToDateTime DATETIME NOT NULL,
        CreatedDateTime DATETIME NOT NULL,
        CreatedByUserId BIGINT NOT NULL,
        SourceId BIGINT NOT NULL,
        DateId BIGINT,
        MappedDateId BIGINT
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Primary Key
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[DateMapping] ADD CONSTRAINT
	PK_DateMapping PRIMARY KEY CLUSTERED 
	(
	DateMappingId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Defaults
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[DateMapping] ADD CONSTRAINT
	    DF_DateMapping_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[DateMapping] ADD CONSTRAINT
	    DF_DateMapping_EffectiveToDateTime DEFAULT ''9999-12-31'' FOR EffectiveToDateTime'
    EXEC sp_sqlexec @SQL

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[DateMapping] ADD CONSTRAINT
	    DF_DateMapping_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime'
    EXEC sp_sqlexec @SQL

    --Add Foreign Keys
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[DateMapping] ADD CONSTRAINT
	FK_DateMapping_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[DateMapping].CreatedByUserId to [Administration.User].[User].UserId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', 'DateMapping', N'CONSTRAINT', 'FK_DateMapping_CreatedByUserId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[DateMapping] ADD CONSTRAINT
	FK_DateMapping_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[DateMapping].SourceId to [Information].[Source].SourceId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', 'DateMapping', N'CONSTRAINT', 'FK_DateMapping_SourceId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[DateMapping] ADD CONSTRAINT
	FK_DateMapping_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE NO ACTION 
	 ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[DateMapping].DateId to [Information].[Date].DateId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', 'DateMapping', N'CONSTRAINT', 'FK_DateMapping_DateId'
END
GO
