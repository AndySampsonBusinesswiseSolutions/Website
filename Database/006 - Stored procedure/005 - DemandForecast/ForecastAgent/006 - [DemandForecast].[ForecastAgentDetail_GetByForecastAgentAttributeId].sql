USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ForecastAgentDetail_GetByForecastAgentAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ForecastAgentDetail_GetByForecastAgentAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-22
-- Description:	Get ForecastAgentDetail info from [DemandForecast].[ForecastAgentDetail] table by ForecastAgent Attribute Id
-- =============================================

ALTER PROCEDURE [DemandForecast].[ForecastAgentDetail_GetByForecastAgentAttributeId]
    @ForecastAgentAttributeId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ForecastAgentDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ForecastAgentId,
        ForecastAgentAttributeId,
        ForecastAgentDetailDescription
    FROM 
        [DemandForecast].[ForecastAgentDetail] 
    WHERE 
        ForecastAgentAttributeId = @ForecastAgentAttributeId
END
GO
