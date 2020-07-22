USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[ProfileClassDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[ProfileClassDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Insert new profile class detail into [Information].[ProfileClassDetail] table
-- =============================================

ALTER PROCEDURE [Information].[ProfileClassDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileClassId BIGINT,
    @ProfileClassAttributeId BIGINT,
    @ProfileClassDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[ProfileClassDetail] WHERE ProfileClassId = @ProfileClassId 
        AND ProfileClassAttributeId = @ProfileClassAttributeId 
        AND ProfileClassDetailDescription = @ProfileClassDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[ProfileClassDetail]
            (
                CreatedByUserId,
                SourceId,
                ProfileClassId,
                ProfileClassAttributeId,
                ProfileClassDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProfileClassId,
                @ProfileClassAttributeId,
                @ProfileClassDetailDescription
            )
        END
END
GO
