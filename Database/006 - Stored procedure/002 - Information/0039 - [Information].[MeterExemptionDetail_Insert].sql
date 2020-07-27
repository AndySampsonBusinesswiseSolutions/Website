USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterExemptionDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterExemptionDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-23
-- Description:	Insert new Meter Exemption detail into [Information].[MeterExemptionDetail] table
-- =============================================

ALTER PROCEDURE [Information].[MeterExemptionDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterExemptionId BIGINT,
    @MeterExemptionAttributeId BIGINT,
    @MeterExemptionDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-23 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[MeterExemptionDetail] WHERE MeterExemptionId = @MeterExemptionId 
        AND MeterExemptionAttributeId = @MeterExemptionAttributeId 
        AND MeterExemptionDetailDescription = @MeterExemptionDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[MeterExemptionDetail]
            (
                CreatedByUserId,
                SourceId,
                MeterExemptionId,
                MeterExemptionAttributeId,
                MeterExemptionDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @MeterExemptionId,
                @MeterExemptionAttributeId,
                @MeterExemptionDetailDescription
            )
        END
END
GO
