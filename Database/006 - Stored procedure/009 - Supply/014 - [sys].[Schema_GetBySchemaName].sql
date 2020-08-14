USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[sys].[Schema_GetBySchemaName]'))
    BEGIN
        EXEC('CREATE PROCEDURE [sys].[Schema_GetBySchemaName] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Get Schema info from [sys].[Schemas] by SchemaName
-- =============================================

ALTER PROCEDURE [sys].[Schema_GetBySchemaName]
    @SchemaName VARCHAR(255)
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
        schema_id,
        principal_id
    FROM
        [sys].[schemas]
    WHERE
        name = @SchemaName
END
GO
