USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetByContractToMeterToContractMeterToProductIdAndContractMeterRateToRateTypeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetByContractToMeterToContractMeterToProductIdAndContractMeterRateToRateTypeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get ContractToMeterToContractMeterToProductToContractMeterRateToRateType info from [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] table by ContractToMeterToContractMeterToProduct Id And ContractMeterRateToRateType Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetByContractToMeterToContractMeterToProductIdAndContractMeterRateToRateTypeId]
    @ContractToMeterToContractMeterToProductId BIGINT,
    @ContractMeterRateToRateTypeId BIGINT,
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
        ContractToMeterToContractMeterToProductToContractMeterRateToRateTypeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractToMeterToContractMeterToProductId,
        ContractMeterRateToRateTypeId
    FROM 
        [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType]
    WHERE 
        ContractToMeterToContractMeterToProductId = @ContractToMeterToContractMeterToProductId
        AND ContractMeterRateToRateTypeId = @ContractMeterRateToRateTypeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
