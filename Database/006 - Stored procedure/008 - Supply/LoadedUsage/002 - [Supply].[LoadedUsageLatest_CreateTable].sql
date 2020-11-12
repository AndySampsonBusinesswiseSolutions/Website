USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[LoadedUsageLatest_CreateTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[LoadedUsageLatest_CreateTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-11
-- Description:	Create new LoadedUsageLatest table for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[LoadedUsageLatest_CreateTable]
    @MeterId BIGINT,
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-11 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[LoadedUsageLatest]
	(
        DateId BIGINT NOT NULL,
        TimePeriodId BIGINT NOT NULL,
        Usage DECIMAL(18,10) NOT NULL
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL

    --Add Foreign Keys
    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsageLatest] ADD CONSTRAINT
    FK_LoadedUsageLatest_DateId FOREIGN KEY
    (
    DateId
    ) REFERENCES [Information].[Date]
    (
    DateId
    ) ON UPDATE NO ACTION 
    ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[LoadedUsageLatest].DateId to [Information].[Date].DateId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', N'LoadedUsageLatest', N'CONSTRAINT', 'FK_LoadedUsageLatest_DateId'

    SET @SQL = N'
    USE [EMaaS]
    
    ALTER TABLE [' + @SchemaName + '].[LoadedUsageLatest] ADD CONSTRAINT
    FK_LoadedUsageLatest_TimePeriodId FOREIGN KEY
    (
    TimePeriodId
    ) REFERENCES [Information].[TimePeriod]
    (
    TimePeriodId
    ) ON UPDATE NO ACTION 
    ON DELETE NO ACTION'
    EXEC sp_sqlexec @SQL

    SET @v = N'Foreign Key constraint joining [' + @SchemaName + '].[LoadedUsageLatest].DateId to [Information].[TimePeriod].TimePeriodId'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', @SchemaName, N'TABLE', N'LoadedUsageLatest', N'CONSTRAINT', 'FK_LoadedUsageLatest_TimePeriodId'
END
GO