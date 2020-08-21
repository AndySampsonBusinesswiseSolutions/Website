USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ReferenceVolumeDetail_GetByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ReferenceVolumeDetail_GetByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Get ReferenceVolumeDetail info from [Customer].[ReferenceVolumeDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[ReferenceVolumeDetail_GetByReferenceVolumeAttributeIdAndReferenceVolumeDetailDescription]
    @ReferenceVolumeAttributeId BIGINT,
    @ReferenceVolumeDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

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
        ReferenceVolumeAttributeId = @ReferenceVolumeAttributeId
        AND ReferenceVolumeDetailDescription = @ReferenceVolumeDetailDescription
END
GO
