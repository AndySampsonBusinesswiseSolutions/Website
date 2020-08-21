USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get BasketDetail info from [Customer].[BasketDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[BasketDetail_GetByBasketAttributeIdAndBasketDetailDescription]
    @BasketAttributeId BIGINT,
    @BasketDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        BasketDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        BasketId,
        BasketAttributeId,
        BasketDetailDescription
    FROM 
        [Customer].[BasketDetail] 
    WHERE 
        BasketAttributeId = @BasketAttributeId
        AND BasketDetailDescription = @BasketDetailDescription
END
GO
