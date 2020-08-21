USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterRateAttribute_GetByContractMeterRateAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterRateAttribute_GetByContractMeterRateAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ContractMeterRateAttribute info from [Customer].[ContractMeterRateAttribute] table by ContractMeterRateAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterRateAttribute_GetByContractMeterRateAttributeDescription]
    @ContractMeterRateAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ContractMeterRateAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterRateAttributeDescription
    FROM 
        [Customer].[ContractMeterRateAttribute] 
    WHERE 
        ContractMeterRateAttributeDescription = @ContractMeterRateAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
