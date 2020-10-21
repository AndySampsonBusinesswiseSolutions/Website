USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterAttribute_GetByContractMeterAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterAttribute_GetByContractMeterAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Get ContractMeterAttribute info from [Customer].[ContractMeterAttribute] table by ContractMeterAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterAttribute_GetByContractMeterAttributeDescription]
    @ContractMeterAttributeDescription VARCHAR(255),
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
        ContractMeterAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterAttributeDescription
    FROM 
        [Customer].[ContractMeterAttribute] 
    WHERE 
        ContractMeterAttributeDescription = @ContractMeterAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
