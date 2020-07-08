USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchiveDetail_GetByProcessArchiveDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessArchiveDetail_GetByProcessArchiveDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-08
-- Description:	Get ProcessArchiveDetail info from [System].[ProcessArchiveDetail] table by Process Archive Detail Id
-- =============================================

ALTER PROCEDURE [System].[ProcessArchiveDetail_GetByProcessArchiveDetailId]
    @ProcessArchiveDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ProcessArchiveDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProcessArchiveId,
        ProcessArchiveAttributeId,
        ProcessArchiveDetailDescription
    FROM 
        [System].[ProcessArchiveDetail] 
    WHERE 
        ProcessArchiveDetailId = @ProcessArchiveDetailId
END
GO
