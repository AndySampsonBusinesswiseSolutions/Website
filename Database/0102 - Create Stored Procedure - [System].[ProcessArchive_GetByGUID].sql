USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchive_GetByGUID]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessArchive_GetByGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get ProcessArchive info from [System].[ProcessArchive] table by GUID
-- =============================================

ALTER PROCEDURE [System].[ProcessArchive_GetByGUID]
    @ProcessArchiveGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
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
        GUID
    FROM 
        [System].[ProcessArchive] 
    WHERE 
        GUID = @ProcessArchiveGUID
END
GO
