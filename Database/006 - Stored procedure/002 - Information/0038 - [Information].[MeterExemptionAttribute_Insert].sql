USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterExemptionAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterExemptionAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-23
-- Description:	Insert new Local Distribution Zone attribute into [Information].[MeterExemptionAttribute] table
-- =============================================

ALTER PROCEDURE [Information].[MeterExemptionAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterExemptionAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-23 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[MeterExemptionAttribute] WHERE MeterExemptionAttributeDescription = @MeterExemptionAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[MeterExemptionAttribute]
            (
                CreatedByUserId,
                SourceId,
                MeterExemptionAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @MeterExemptionAttributeDescription
            )
        END
END
GO
