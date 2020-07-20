USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[FlexTrade_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[FlexTrade_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new FlexTrade into [Temp.Customer].[FlexTrade] table
-- =============================================

ALTER PROCEDURE [Temp.Customer].[FlexTrade_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @BasketReference VARCHAR(255),
    @TradeDate VARCHAR(255),
    @TradeProduct VARCHAR(255),
    @Volume VARCHAR(255),
    @Price VARCHAR(255),
    @Direction VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-20 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[FlexTrade]
    (
        ProcessQueueGUID,
        BasketReference,
        TradeDate,
        TradeProduct,
        Volume,
        Price,
        Direction
    )
    VALUES
    (
        @ProcessQueueGUID,
        @BasketReference,
        @TradeDate,
        @TradeProduct,
        @Volume,
        @Price,
        @Direction
    )
END
GO
