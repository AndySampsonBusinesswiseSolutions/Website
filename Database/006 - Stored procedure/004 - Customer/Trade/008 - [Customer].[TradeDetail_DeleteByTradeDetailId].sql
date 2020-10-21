USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[TradeDetail_DeleteByTradeDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[TradeDetail_DeleteByTradeDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Delete Trade detail from [Customer].[TradeDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[TradeDetail_DeleteByTradeDetailId]
    @TradeDetailId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [Customer].[TradeDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        TradeDetailId = @TradeDetailId
END
GO
