USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractMeterToProduct_GetByContractMeterIdAndProductId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractMeterToProduct_GetByContractMeterIdAndProductId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get ContractMeterToProduct info from [Mapping].[ContractMeterToProduct] table by ContractMeter Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractMeterToProduct_GetByContractMeterIdAndProductId]
    @ContractMeterId BIGINT,
    @ProductId BIGINT,
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
        ContractMeterToProductId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterId,
        ProductId
    FROM 
        [Mapping].[ContractMeterToProduct]
    WHERE 
        ContractMeterId = @ContractMeterId
        AND ProductId = @ProductId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
