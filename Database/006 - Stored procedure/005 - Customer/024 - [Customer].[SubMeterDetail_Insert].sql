USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SubMeterDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SubMeterDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new SubMeter detail into [Customer].[SubMeterDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[SubMeterDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SubMeterId BIGINT,
    @SubMeterAttributeId BIGINT,
    @SubMeterDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[SubMeterDetail] WHERE SubMeterId = @SubMeterId 
        AND SubMeterAttributeId = @SubMeterAttributeId 
        AND SubMeterDetailDescription = @SubMeterDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[SubMeterDetail]
            (
                CreatedByUserId,
                SourceId,
                SubMeterId,
                SubMeterAttributeId,
                SubMeterDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @SubMeterId,
                @SubMeterAttributeId,
                @SubMeterDetailDescription
            )
        END
END
GO
