USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[ProfileClassAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[ProfileClassAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Insert new Profile Class attribute into [Information].[ProfileClassAttribute] table
-- =============================================

ALTER PROCEDURE [Information].[ProfileClassAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileClassAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[ProfileClassAttribute] WHERE ProfileClassAttributeDescription = @ProfileClassAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[ProfileClassAttribute]
            (
                CreatedByUserId,
                SourceId,
                ProfileClassAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProfileClassAttributeDescription
            )
        END
END
GO
