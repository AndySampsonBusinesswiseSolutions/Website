USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToMeterExemptionToMeterExemptionProduct_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToMeterExemptionToMeterExemptionProduct_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a MeterToMeterExemption to a MeterExemptionProduct into [Mapping].[MeterToMeterExemptionToMeterExemptionProduct] table
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToMeterExemptionToMeterExemptionProduct_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterToMeterExemptionId BIGINT,
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

    INSERT INTO [Mapping].MeterToMeterExemptionToMeterExemptionProduct
    (
        CreatedByUserId,
        SourceId,
        MeterToMeterExemptionId,
        MeterExemptionProductId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @MeterToMeterExemptionId,
        @MeterExemptionProductId                
    )
END
GO