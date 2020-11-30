USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ForecastAgent_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ForecastAgent_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-25
-- Description:	Insert new ForecastAgent into [DemandForecast].[ForecastAgent] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ForecastAgent_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ForecastAgentGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-25 -> Andrew Sampson -> Initial development of script
    -- 2020-09-22 -> Andrew Sampson -> Updated to use Entity\Attribute\Detail structure
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [DemandForecast].[ForecastAgent]
    (
        CreatedByUserId,
        SourceId,
        ForecastAgentGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ForecastAgentGUID
    )
END
GO
