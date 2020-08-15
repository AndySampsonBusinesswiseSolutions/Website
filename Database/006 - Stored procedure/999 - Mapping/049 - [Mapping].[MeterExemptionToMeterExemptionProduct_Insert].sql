USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterExemptionToMeterExemptionProduct_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterExemptionToMeterExemptionProduct_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a MeterExemption to a MeterExemptionProduct into [Mapping].[MeterExemptionToMeterExemptionProduct] table
-- =============================================

ALTER PROCEDURE [Mapping].[MeterExemptionToMeterExemptionProduct_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterExemptionId BIGINT,
    @MeterExemptionProductId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Mapping].[MeterExemptionToMeterExemptionProduct] WHERE MeterExemptionId = @MeterExemptionId
        AND MeterExemptionProductId = @MeterExemptionProductId 
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Mapping].MeterExemptionToMeterExemptionProduct
            (
                CreatedByUserId,
                SourceId,
                MeterExemptionId,
                MeterExemptionProductId
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @MeterExemptionId,
                @MeterExemptionProductId                
            )
        END
END
GO
