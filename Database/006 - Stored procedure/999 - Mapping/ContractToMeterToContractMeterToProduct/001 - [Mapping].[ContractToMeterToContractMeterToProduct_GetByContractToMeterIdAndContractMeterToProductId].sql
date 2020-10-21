USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToMeterToContractMeterToProduct_GetByContractToMeterIdAndContractMeterToProductId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToMeterToContractMeterToProduct_GetByContractToMeterIdAndContractMeterToProductId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get ContractToMeterToContractMeterToProduct info from [Mapping].[ContractToMeterToContractMeterToProduct] table by ContractToMeter Id And ContractMeterToProduct Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToMeterToContractMeterToProduct_GetByContractToMeterIdAndContractMeterToProductId]
    @ContractToMeterId BIGINT,
    @ContractMeterToProductId BIGINT,
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
        ContractToMeterToContractMeterToProductId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractToMeterId,
        ContractMeterToProductId
    FROM 
        [Mapping].[ContractToMeterToContractMeterToProduct]
    WHERE 
        ContractToMeterId = @ContractToMeterId
        AND ContractMeterToProductId = @ContractMeterToProductId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
