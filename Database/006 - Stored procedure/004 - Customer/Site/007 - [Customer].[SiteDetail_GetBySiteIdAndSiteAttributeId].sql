USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SiteDetail_GetBySiteIdAndSiteAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SiteDetail_GetBySiteIdAndSiteAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Get SiteDetail info from [Customer].[SiteDetail] table by Site Id and Site Attribute Id
-- =============================================

ALTER PROCEDURE [Customer].[SiteDetail_GetBySiteIdAndSiteAttributeId]
    @SiteId BIGINT,
    @SiteAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SiteDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SiteId,
        SiteAttributeId,
        SiteDetailDescription
    FROM 
        [Customer].[SiteDetail] 
    WHERE 
        SiteId = @SiteId
        AND SiteAttributeId = @SiteAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
