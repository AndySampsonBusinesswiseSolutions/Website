USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToContractType_GetByContractIdAndContractTypeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToContractType_GetByContractIdAndContractTypeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Get ContractToContractType info from [Mapping].[ContractToContractType] table by Contract Id and Contract Type Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToContractType_GetByContractIdAndContractTypeId]
    @ContractId BIGINT,
    @ContractTypeId BIGINT,
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
        ContractToContractTypeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractId,
        ContractTypeId
    FROM 
        [Mapping].[ContractToContractType] 
    WHERE 
        ContractId = @ContractId
        AND ContractTypeId = @ContractTypeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
