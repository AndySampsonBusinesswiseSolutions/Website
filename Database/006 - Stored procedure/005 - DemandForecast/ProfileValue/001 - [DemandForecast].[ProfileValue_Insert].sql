USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileValue_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileValue_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-09
-- Description:	Insert new ProfileValue into [DemandForecast].[ProfileValue] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileValue_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileValue DECIMAL(19, 19)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [DemandForecast].[ProfileValue]
    (
        CreatedByUserId,
        SourceId,
        ProfileValue
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProfileValue
    )
END
GO