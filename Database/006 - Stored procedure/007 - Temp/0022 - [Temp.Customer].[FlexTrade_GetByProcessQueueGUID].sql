USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[FlexTrade_GetByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[FlexTrade_GetByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-21
-- Description:	Get FlexTrade from [Temp.Customer].[FlexTrade] table by Process Queue GUID by Process Queue GUID
-- =============================================

ALTER PROCEDURE [Temp.Customer].[FlexTrade_GetByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-21 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
        ProcessQueueGUID,
        BasketReference,
        TradeDate,
        TradeProduct,
        Volume,
        Price,
        Direction
    FROM
        [Temp.Customer].[FlexTrade]
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
