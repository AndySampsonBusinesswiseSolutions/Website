USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[APIToProcessArchiveDetail_GetByAPIId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[APIToProcessArchiveDetail_GetByAPIId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-08
-- Description:	Get APIToProcessArchiveDetail info from [Mapping].[APIToProcessArchiveDetail] table
-- =============================================

ALTER PROCEDURE [Mapping].[APIToProcessArchiveDetail_GetByAPIId]
    @APIId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        APIToProcessArchiveDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        APIId,
        ProcessArchiveDetailId
    FROM 
        [Mapping].[APIToProcessArchiveDetail]
    WHERE
        APIId = @APIId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
