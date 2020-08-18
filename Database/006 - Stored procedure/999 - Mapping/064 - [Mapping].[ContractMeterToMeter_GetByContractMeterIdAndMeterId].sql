USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractMeterToMeter_GetByContractMeterIdAndMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractMeterToMeter_GetByContractMeterIdAndMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Get ContractMeterToMeter info from [Mapping].[ContractMeterToMeter] table by ContractMeter Id
-- =============================================

ALTER PROCEDURE [Mapping].[ContractMeterToMeter_GetByContractMeterIdAndMeterId]
    @ContractMeterId BIGINT,
    @MeterId BIGINT,
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
        ContractMeterToMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ContractMeterId,
        MeterId
    FROM 
        [Mapping].[ContractMeterToMeter]
    WHERE 
        ContractMeterId = @ContractMeterId
        AND MeterId = @MeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
