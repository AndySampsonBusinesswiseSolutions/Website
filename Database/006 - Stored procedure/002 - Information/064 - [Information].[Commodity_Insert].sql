USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Commodity_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[Commodity_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new profile class into [Information].[Commodity] table
-- =============================================

ALTER PROCEDURE [Information].[Commodity_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @CommodityDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[Commodity] WHERE CommodityDescription = @CommodityDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[Commodity]
            (
                CreatedByUserId,
                SourceId,
                CommodityDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @CommodityDescription
            )
        END
END
GO
