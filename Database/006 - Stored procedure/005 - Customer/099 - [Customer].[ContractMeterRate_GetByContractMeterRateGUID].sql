USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterRate_GetByContractMeterRateGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterRate_GetByContractMeterRateGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ContractMeterRate info from [Customer].[ContractMeterRate] table by GUID
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterRate_GetByContractMeterRateGUID]
    @ContractMeterRateGUID UNIQUEIDENTIFIER,
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
        ContractMeterRateId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterRateGUID
    FROM 
        [Customer].[ContractMeterRate] 
    WHERE 
        ContractMeterRateGUID = @ContractMeterRateGUID
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
