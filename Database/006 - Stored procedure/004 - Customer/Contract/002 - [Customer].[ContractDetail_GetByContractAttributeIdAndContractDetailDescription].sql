USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractDetail_GetByContractAttributeIdAndContractDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractDetail_GetByContractAttributeIdAndContractDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get ContractDetail info from [Customer].[ContractDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[ContractDetail_GetByContractAttributeIdAndContractDetailDescription]
    @ContractAttributeId BIGINT,
    @ContractDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ContractDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractId,
        ContractAttributeId,
        ContractDetailDescription
    FROM 
        [Customer].[ContractDetail] 
    WHERE 
        ContractAttributeId = @ContractAttributeId
        AND ContractDetailDescription = @ContractDetailDescription
END
GO
