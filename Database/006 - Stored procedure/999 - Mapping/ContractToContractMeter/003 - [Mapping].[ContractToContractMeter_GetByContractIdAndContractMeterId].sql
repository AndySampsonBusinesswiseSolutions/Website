USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToContractMeter_GetByContractIdAndContractMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToContractMeter_GetByContractIdAndContractMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Get ContractToContractMeter info from [Mapping].[ContractToContractMeter] table by Contract Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToContractMeter_GetByContractIdAndContractMeterId]
    @ContractId BIGINT,
    @ContractMeterId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-16 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ContractToContractMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractId,
        ContractMeterId
    FROM 
        [Mapping].[ContractToContractMeter]
    WHERE 
        ContractId = @ContractId
        AND ContractMeterId = @ContractMeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
