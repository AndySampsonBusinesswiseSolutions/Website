USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-09
-- Description:	Insert new Profile detail into [DemandForecast].[ProfileDetail] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileId BIGINT,
    @ProfileAttributeId BIGINT,
    @ProfileDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [DemandForecast].[ProfileDetail] WHERE ProfileId = @ProfileId 
        AND ProfileAttributeId = @ProfileAttributeId 
        AND ProfileDetailDescription = @ProfileDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [DemandForecast].[ProfileDetail]
            (
                CreatedByUserId,
                SourceId,
                ProfileId,
                ProfileAttributeId,
                ProfileDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProfileId,
                @ProfileAttributeId,
                @ProfileDetailDescription
            )
        END
END
GO
