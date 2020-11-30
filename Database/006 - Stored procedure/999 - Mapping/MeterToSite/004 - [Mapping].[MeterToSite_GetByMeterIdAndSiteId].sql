USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToSite_GetByMeterIdAndSiteId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToSite_GetByMeterIdAndSiteId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-13
-- Description:	Get MeterToSite info from [Mapping].[MeterToSite] table by Meter Id And Meter Exemption Id
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToSite_GetByMeterIdAndSiteId]
    @MeterId BIGINT,
    @SiteId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-13 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        MeterToSiteId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterId,
        SiteId
    FROM 
        [Mapping].[MeterToSite] 
    WHERE 
        MeterId = @MeterId
        AND SiteId = @SiteId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
