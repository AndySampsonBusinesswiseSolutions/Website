USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[BasketToContractMeter_GetByBasketIdAndContractMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[BasketToContractMeter_GetByBasketIdAndContractMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get BasketToContractMeter info from [Mapping].[BasketToContractMeter] table by Basket Id and ContractMeter Id
-- =============================================

ALTER PROCEDURE [Mapping].[BasketToContractMeter_GetByBasketIdAndContractMeterId]
    @BasketId BIGINT,
    @ContractMeterId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        BasketToContractMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        BasketId,
        ContractMeterId
    FROM 
        [Mapping].[BasketToContractMeter]
    WHERE 
        BasketId = @BasketId
        AND ContractMeterId = @ContractMeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
