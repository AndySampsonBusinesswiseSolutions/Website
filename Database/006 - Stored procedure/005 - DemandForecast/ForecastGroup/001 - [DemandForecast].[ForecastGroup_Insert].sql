USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ForecastGroup_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ForecastGroup_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-25
-- Description:	Insert new ForecastGroup into [DemandForecast].[ForecastGroup] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ForecastGroup_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ForecastGroupDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [DemandForecast].[ForecastGroup]
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ForecastGroupDescription
    )
END
GO