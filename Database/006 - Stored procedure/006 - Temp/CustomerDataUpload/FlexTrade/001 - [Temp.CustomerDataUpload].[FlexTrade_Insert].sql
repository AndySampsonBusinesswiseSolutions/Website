USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[FlexTrade_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[FlexTrade_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new FlexTrade into [Temp.CustomerDataUpload].[FlexTrade] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[FlexTrade_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @BasketReference VARCHAR(255),
    @TradeReference VARCHAR(255),
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
    -- 2020-07-25 -> Andrew Sampson -> Added TradeReference
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.CustomerDataUpload].[FlexTrade]
    (
        ProcessQueueGUID,
        RowId,
        BasketReference,
        TradeReference,
        TradeDate,
        TradeProduct,
        Volume,
        Price,
        Direction
    )
    VALUES
    (
        @ProcessQueueGUID,
        @RowId,
        @BasketReference,
        @TradeReference,
        @TradeDate,
        @TradeProduct,
        @Volume,
        @Price,
        @Direction
    )
END
GO
