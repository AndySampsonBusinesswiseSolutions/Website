USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SubMeterAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SubMeterAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new SubMeter attribute into [Customer].[SubMeterAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[SubMeterAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SubMeterAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[SubMeterAttribute] WHERE SubMeterAttributeDescription = @SubMeterAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[SubMeterAttribute]
            (
                CreatedByUserId,
                SourceId,
                SubMeterAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @SubMeterAttributeDescription
            )
        END
END
GO
