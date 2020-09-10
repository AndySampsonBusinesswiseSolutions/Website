USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[Profile_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[Profile_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-09
-- Description:	Insert new Profile into [DemandForecast].[Profile] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[Profile_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [DemandForecast].[Profile] WHERE ProfileGUID = @ProfileGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [DemandForecast].[Profile]
            (
                CreatedByUserId,
                SourceId,
                ProfileGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProfileGUID
            )
        END
END
GO
