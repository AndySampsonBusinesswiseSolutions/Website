USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[RateType_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[RateType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new Rate Type into [Information].[RateType] table
-- =============================================

ALTER PROCEDURE [Information].[RateType_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @RateTypeCode VARCHAR(255),
    @RateTypeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- 2020-08-26 -> Andrew Sampson -> Added RateTypeCode parameter
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[RateType] WHERE RateTypeCode = @RateTypeCode
        AND RateTypeDescription = @RateTypeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[RateType]
            (
                CreatedByUserId,
                SourceId,
                RateTypeCode,
                RateTypeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @RateTypeCode,
                @RateTypeDescription
            )
        END
END
GO
