USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ApplicationToProcessArchiveDetail_GetByApplicationId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ApplicationToProcessArchiveDetail_GetByApplicationId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Get ApplicationToProcessArchiveDetail info from [Mapping].[ApplicationToProcessArchiveDetail] table
-- =============================================

ALTER PROCEDURE [Mapping].[ApplicationToProcessArchiveDetail_GetByApplicationId]
    @ApplicationId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ApplicationToProcessArchiveDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ApplicationId,
        ProcessArchiveDetailId
    FROM 
        [Mapping].[ApplicationToProcessArchiveDetail]
    WHERE
        ApplicationId = @ApplicationId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
