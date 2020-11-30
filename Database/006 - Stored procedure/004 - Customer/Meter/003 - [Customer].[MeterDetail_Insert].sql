USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new Meter detail into [Customer].[MeterDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[MeterDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterId BIGINT,
    @MeterAttributeId BIGINT,
    @MeterDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[MeterDetail]
    (
        CreatedByUserId,
        SourceId,
        MeterId,
        MeterAttributeId,
        MeterDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @MeterId,
        @MeterAttributeId,
        @MeterDetailDescription
    )
END
GO