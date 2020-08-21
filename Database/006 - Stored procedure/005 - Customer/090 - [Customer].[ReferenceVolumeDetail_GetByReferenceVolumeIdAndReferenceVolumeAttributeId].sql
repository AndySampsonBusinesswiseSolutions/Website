USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ReferenceVolumeDetail_GetByReferenceVolumeIdAndReferenceVolumeAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ReferenceVolumeDetail_GetByReferenceVolumeIdAndReferenceVolumeAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ReferenceVolumeDetail info from [Customer].[ReferenceVolumeDetail] table by ReferenceVolume Id and ReferenceVolume Attribute Id
-- =============================================

ALTER PROCEDURE [Customer].[ReferenceVolumeDetail_GetByReferenceVolumeIdAndReferenceVolumeAttributeId]
    @ReferenceVolumeId BIGINT,
    @ReferenceVolumeAttributeId BIGINT,
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
        ReferenceVolumeDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ReferenceVolumeId,
        ReferenceVolumeAttributeId,
        ReferenceVolumeDetailDescription
    FROM 
        [Customer].[ReferenceVolumeDetail] 
    WHERE 
        ReferenceVolumeId = @ReferenceVolumeId
        AND ReferenceVolumeAttributeId = @ReferenceVolumeAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
