USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[TradeDetail_GetByTradeAttributeIdAndTradeDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[TradeDetail_GetByTradeAttributeIdAndTradeDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get TradeDetail info from [Customer].[TradeDetail] table by Trade Attribute Id and Trade Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[TradeDetail_GetByTradeAttributeIdAndTradeDetailDescription]
    @TradeAttributeId BIGINT,
    @TradeDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        TradeDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        TradeId,
        TradeAttributeId,
        TradeDetailDescription
    FROM 
        [Customer].[TradeDetail] 
    WHERE 
        TradeAttributeId = @TradeAttributeId
        AND TradeDetailDescription = @TradeDetailDescription
END
GO
