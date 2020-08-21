USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterDetail_GetByContractMeterAttributeIdAndContractMeterDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterDetail_GetByContractMeterAttributeIdAndContractMeterDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ContractMeterDetail info from [Customer].[ContractMeterDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterDetail_GetByContractMeterAttributeIdAndContractMeterDetailDescription]
    @ContractMeterAttributeId BIGINT,
    @ContractMeterDetailDescription VARCHAR(255)
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
        ContractMeterDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterId,
        ContractMeterAttributeId,
        ContractMeterDetailDescription
    FROM 
        [Customer].[ContractMeterDetail] 
    WHERE 
        ContractMeterAttributeId = @ContractMeterAttributeId
        AND ContractMeterDetailDescription = @ContractMeterDetailDescription
END
GO
