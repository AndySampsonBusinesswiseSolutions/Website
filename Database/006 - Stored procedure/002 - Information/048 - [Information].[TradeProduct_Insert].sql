USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[TradeProduct_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[TradeProduct_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new Trade Product into [Information].[TradeProduct] table
-- =============================================

ALTER PROCEDURE [Information].[TradeProduct_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TradeProductDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[TradeProduct] WHERE TradeProductDescription = @TradeProductDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[TradeProduct]
            (
                CreatedByUserId,
                SourceId,
                TradeProductDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @TradeProductDescription
            )
        END
END
GO
