USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ForecastAgentAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ForecastAgentAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-22
-- Description:	Insert new ForecastAgent attribute into [DemandForecast].[ForecastAgentAttribute] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ForecastAgentAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ForecastAgentAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [DemandForecast].[ForecastAgentAttribute] WHERE ForecastAgentAttributeDescription = @ForecastAgentAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [DemandForecast].[ForecastAgentAttribute]
            (
                CreatedByUserId,
                SourceId,
                ForecastAgentAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ForecastAgentAttributeDescription
            )
        END
END
GO
