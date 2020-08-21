USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeter_GetByContractMeterGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeter_GetByContractMeterGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Get ContractMeter info from [Customer].[ContractMeter] table by GUID
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeter_GetByContractMeterGUID]
    @ContractMeterGUID UNIQUEIDENTIFIER,
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
        ContractMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterGUID
    FROM 
        [Customer].[ContractMeter] 
    WHERE 
        ContractMeterGUID = @ContractMeterGUID
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
