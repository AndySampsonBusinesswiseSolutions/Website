USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterExemptionDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterExemptionDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new MeterExemption detail into [Customer].[MeterExemptionDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[MeterExemptionDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterExemptionId BIGINT,
    @MeterExemptionAttributeId BIGINT,
    @MeterExemptionDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[MeterExemptionDetail] WHERE MeterExemptionId = @MeterExemptionId 
        AND MeterExemptionAttributeId = @MeterExemptionAttributeId 
        AND MeterExemptionDetailDescription = @MeterExemptionDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[MeterExemptionDetail]
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
