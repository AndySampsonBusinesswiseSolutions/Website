USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[RateUnit_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[RateUnit_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new Rate Unit into [Information].[RateUnit] table
-- =============================================

ALTER PROCEDURE [Information].[RateUnit_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @RateUnitDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[RateUnit]
    (
        CreatedByUserId,
        SourceId,
        RateUnitDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @RateUnitDescription
    )
END
GO