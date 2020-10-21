USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractMeterRateToRateType_GetByContractMeterRateIdAndRateTypeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractMeterRateToRateType_GetByContractMeterRateIdAndRateTypeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get ContractMeterRateToRateType info from [Mapping].[ContractMeterRateToRateType] table by ContractMeterRate Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractMeterRateToRateType_GetByContractMeterRateIdAndRateTypeId]
    @ContractMeterRateId BIGINT,
    @RateTypeId BIGINT,
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
        ContractMeterRateToRateTypeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterRateId,
        RateTypeId
    FROM 
        [Mapping].[ContractMeterRateToRateType]
    WHERE 
        ContractMeterRateId = @ContractMeterRateId
        AND RateTypeId = @RateTypeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
