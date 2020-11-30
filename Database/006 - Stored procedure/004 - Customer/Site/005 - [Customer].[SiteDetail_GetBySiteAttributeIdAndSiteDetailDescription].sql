USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get SiteDetail info from [Customer].[SiteDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[SiteDetail_GetBySiteAttributeIdAndSiteDetailDescription]
    @SiteAttributeId BIGINT,
    @SiteDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

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
        SiteAttributeId = @SiteAttributeId
        AND SiteDetailDescription = @SiteDetailDescription
END
GO
