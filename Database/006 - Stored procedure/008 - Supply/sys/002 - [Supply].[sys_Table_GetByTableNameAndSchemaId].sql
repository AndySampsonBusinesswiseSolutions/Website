USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[sys_Table_GetByTableNameAndSchemaId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[sys_Table_GetByTableNameAndSchemaId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Get Table info from [sys].[Tables] by TableName and SchemaId
-- =============================================

ALTER PROCEDURE [Supply].[sys_Table_GetByTableNameAndSchemaId]
    @TableName VARCHAR(255),
    @SchemaId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
        name,
        object_id,
        principal_id,
        schema_id,
        parent_object_id,
        type,
        type_desc,
        create_date,
        modify_date,
        is_ms_shipped,
        is_published,
        is_schema_published,
        lob_data_space_id,
        filestream_data_space_id,
        max_column_id_used,
        lock_on_bulk_load,
        uses_ansi_nulls,
        is_replicated,
        has_replication_filter,
        is_merge_published,
        is_sync_tran_subscribed,
        has_unchecked_assembly_data,
        text_in_row_limit,
        large_value_types_out_of_row, 
        is_tracked_by_cdc,
        lock_escalation,
        lock_escalation_desc,
        is_filetable,
        is_memory_optimized,
        durability,
        durability_desc,
        temporal_type,
        temporal_type_desc,
        history_table_id,
        is_remote_data_archive_enabled,
        is_external,
        history_retention_period,
        history_retention_period_unit,
        history_retention_period_unit_desc,
        is_node,
        is_edge
    FROM
        [sys].[Tables]
    WHERE
        name = @TableName
        AND schema_id = @SchemaId
END
GO
