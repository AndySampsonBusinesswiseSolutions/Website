USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Insert new mapping of an ContractToMeterToContractMeterToProduct to a ContractMeterRateToRateType into [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractToMeterToContractMeterToProductId BIGINT,
    @ContractMeterRateToRateTypeId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractToMeterToContractMeterToProductToContractMeterRateToRateType
    (
        CreatedByUserId,
        SourceId,
        ContractToMeterToContractMeterToProductId,
        ContractMeterRateToRateTypeId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractToMeterToContractMeterToProductId,
        @ContractMeterRateToRateTypeId
    )
END
GO