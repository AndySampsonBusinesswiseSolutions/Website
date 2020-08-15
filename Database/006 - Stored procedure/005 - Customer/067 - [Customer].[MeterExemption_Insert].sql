USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterExemption_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterExemption_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new MeterExemption into [Customer].[MeterExemption] table
-- =============================================

ALTER PROCEDURE [Customer].[MeterExemption_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterExemptionGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[MeterExemption] WHERE MeterExemptionGUID = @MeterExemptionGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[MeterExemption]
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
