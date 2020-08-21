USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ReferenceVolumeAttribute_GetByReferenceVolumeAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ReferenceVolumeAttribute_GetByReferenceVolumeAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ReferenceVolumeAttribute info from [Customer].[ReferenceVolumeAttribute] table by ReferenceVolumeAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[ReferenceVolumeAttribute_GetByReferenceVolumeAttributeDescription]
    @ReferenceVolumeAttributeDescription VARCHAR(255),
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
        ReferenceVolumeAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ReferenceVolumeAttributeDescription
    FROM 
        [Customer].[ReferenceVolumeAttribute] 
    WHERE 
        ReferenceVolumeAttributeDescription = @ReferenceVolumeAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
