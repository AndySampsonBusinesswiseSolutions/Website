USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ForecastAgentDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ForecastAgentDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-22
-- Description:	Insert new ForecastAgent detail into [DemandForecast].[ForecastAgentDetail] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ForecastAgentDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ForecastAgentId BIGINT,
    @ForecastAgentAttributeId BIGINT,
    @ForecastAgentDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [DemandForecast].[ForecastAgentDetail] WHERE ForecastAgentId = @ForecastAgentId 
        AND ForecastAgentAttributeId = @ForecastAgentAttributeId 
        AND ForecastAgentDetailDescription = @ForecastAgentDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [DemandForecast].[ForecastAgentDetail]
            (
                CreatedByUserId,
                SourceId,
                ForecastAgentId,
                ForecastAgentAttributeId,
                ForecastAgentDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ForecastAgentId,
                @ForecastAgentAttributeId,
                @ForecastAgentDetailDescription
            )
        END
END
GO
