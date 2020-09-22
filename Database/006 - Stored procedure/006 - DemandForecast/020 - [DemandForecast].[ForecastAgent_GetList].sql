USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ForecastAgent_GetList]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ForecastAgent_GetList] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-19
-- Description:	Get ForecastAgent info from [DemandForecast].[ForecastAgent] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ForecastAgent_GetList]
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-19 -> Andrew Sampson -> Initial development of script
    -- 2020-09-22 -> Andrew Sampson -> Updated to use Entity\Attribute\Detail structure
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ForecastAgentId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ForecastAgentGUID
    FROM 
        [DemandForecast].[ForecastAgent] 
    WHERE 
        @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
