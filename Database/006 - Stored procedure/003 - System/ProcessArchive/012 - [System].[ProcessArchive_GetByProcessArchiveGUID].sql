USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchive_GetByProcessArchiveGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessArchive_GetByProcessArchiveGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get ProcessArchive info from [System].[ProcessArchive] table by GUID
-- =============================================

ALTER PROCEDURE [System].[ProcessArchive_GetByProcessArchiveGUID]
    @ProcessArchiveGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-17 -> Andrew Sampson -> Adjusted stored procedure name to correctly identify which GUID is being used
    -- 2020-07-08 -> Andrew Sampson -> Removed EffectiveDatetime parameter
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ProcessArchiveId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProcessArchiveGUID
    FROM 
        [System].[ProcessArchive] 
    WHERE 
        ProcessArchiveGUID = @ProcessArchiveGUID
END
GO
