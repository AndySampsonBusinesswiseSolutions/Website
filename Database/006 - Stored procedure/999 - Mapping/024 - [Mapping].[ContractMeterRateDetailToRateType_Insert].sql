USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractMeterRateDetailToRateType_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractMeterRateDetailToRateType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new mapping of a ContractMeterRateDetail to a RateType into [Mapping].[ContractMeterRateDetailToRateType] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractMeterRateDetailToRateType_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterRateDetailId BIGINT,
    @RateTypeId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractMeterRateDetailToRateType
    (
        CreatedByUserId,
        SourceId,
        ContractMeterRateDetailId,
        RateTypeId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractMeterRateDetailId,
        @RateTypeId
    )
END
GO
