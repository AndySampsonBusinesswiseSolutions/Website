USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ReferenceVolume_GetByReferenceVolumeGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ReferenceVolume_GetByReferenceVolumeGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ReferenceVolume info from [Customer].[ReferenceVolume] table by GUID
-- =============================================

ALTER PROCEDURE [Customer].[ReferenceVolume_GetByReferenceVolumeGUID]
    @ReferenceVolumeGUID UNIQUEIDENTIFIER,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ReferenceVolumeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ReferenceVolumeGUID
    FROM 
        [Customer].[ReferenceVolume] 
    WHERE 
        ReferenceVolumeGUID = @ReferenceVolumeGUID
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
