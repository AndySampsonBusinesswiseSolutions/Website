USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterRateAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterRateAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new contract meter rate attribute into [Customer].[ContractMeterRateAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterRateAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterRateAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[ContractMeterRateAttribute] WHERE ContractMeterRateAttributeDescription = @ContractMeterRateAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[ContractMeterRateAttribute]
            (
                CreatedByUserId,
                SourceId,
                ContractMeterRateAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ContractMeterRateAttributeDescription
            )
        END
END
GO
