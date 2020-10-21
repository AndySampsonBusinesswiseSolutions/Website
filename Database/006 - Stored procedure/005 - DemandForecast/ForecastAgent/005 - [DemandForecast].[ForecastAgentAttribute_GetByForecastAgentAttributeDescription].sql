USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ForecastAgentAttribute_GetByForecastAgentAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ForecastAgentAttribute_GetByForecastAgentAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-22
-- Description:	Get ForecastAgentAttribute info from [DemandForecast].[ForecastAgentAttribute] table by ForecastAgentAttributeDescription
-- =============================================

ALTER PROCEDURE [DemandForecast].[ForecastAgentAttribute_GetByForecastAgentAttributeDescription]
    @ForecastAgentAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ForecastAgentAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ForecastAgentAttributeDescription
    FROM 
        [DemandForecast].[ForecastAgentAttribute] 
    WHERE 
        ForecastAgentAttributeDescription = @ForecastAgentAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
