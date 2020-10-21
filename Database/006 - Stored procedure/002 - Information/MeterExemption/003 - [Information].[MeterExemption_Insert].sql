USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[MeterExemption_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[MeterExemption_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-23
-- Description:	Insert new Meter Exemption into [Information].[MeterExemption] table
-- =============================================

ALTER PROCEDURE [Information].[MeterExemption_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterExemptionGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-23 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[MeterExemption] WHERE MeterExemptionGUID = @MeterExemptionGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[MeterExemption]
            (
                CreatedByUserId,
                SourceId,
                MeterExemptionGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @MeterExemptionGUID
            )
        END
END
GO
