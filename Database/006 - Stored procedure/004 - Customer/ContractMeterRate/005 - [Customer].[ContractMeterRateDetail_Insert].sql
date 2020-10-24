USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterRateDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterRateDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Insert new ContractMeterRate detail into [Customer].[ContractMeterRateDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterRateDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterRateId BIGINT,
    @ContractMeterRateAttributeId BIGINT,
    @ContractMeterRateDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[ContractMeterRateDetail]
    (
        CreatedByUserId,
        SourceId,
        ContractMeterRateId,
        ContractMeterRateAttributeId,
        ContractMeterRateDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractMeterRateId,
        @ContractMeterRateAttributeId,
        @ContractMeterRateDetailDescription
    )
END
GO