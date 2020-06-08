USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get ProcessArchiveDetail info from [System].[ProcessArchiveDetail] table by ProcessArchive ID and ProcessArchive Attribute ID
-- =============================================

ALTER PROCEDURE [System].[ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId]
    @ProcessArchiveID BIGINT,
    @ProcessArchiveAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

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
        ProcessArchiveId = @ProcessArchiveId
        AND ProcessArchiveAttributeId = @ProcessArchiveAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
