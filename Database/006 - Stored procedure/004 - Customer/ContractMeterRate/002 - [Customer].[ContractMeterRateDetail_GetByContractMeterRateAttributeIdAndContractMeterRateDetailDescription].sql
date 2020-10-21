USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterRateDetail_GetByContractMeterRateAttributeIdAndContractMeterRateDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterRateDetail_GetByContractMeterRateAttributeIdAndContractMeterRateDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ContractMeterRateDetail info from [Customer].[ContractMeterRateDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterRateDetail_GetByContractMeterRateAttributeIdAndContractMeterRateDetailDescription]
    @ContractMeterRateAttributeId BIGINT,
    @ContractMeterRateDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ContractMeterRateDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterRateId,
        ContractMeterRateAttributeId,
        ContractMeterRateDetailDescription
    FROM 
        [Customer].[ContractMeterRateDetail] 
    WHERE 
        ContractMeterRateAttributeId = @ContractMeterRateAttributeId
        AND ContractMeterRateDetailDescription = @ContractMeterRateDetailDescription
END
GO
