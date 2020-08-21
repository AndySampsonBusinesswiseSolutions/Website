USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToSupplier_GetByContractIdAndSupplierId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToSupplier_GetByContractIdAndSupplierId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get ContractToSupplier info from [Mapping].[ContractToSupplier] table by Contract Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToSupplier_GetByContractIdAndSupplierId]
    @ContractId BIGINT,
    @SupplierId BIGINT,
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
        ContractToSupplierId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractId,
        SupplierId
    FROM 
        [Mapping].[ContractToSupplier]
    WHERE 
        ContractId = @ContractId
        AND SupplierId = @SupplierId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
