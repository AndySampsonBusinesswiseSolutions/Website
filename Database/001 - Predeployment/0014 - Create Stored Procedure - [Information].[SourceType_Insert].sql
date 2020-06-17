USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[SourceType_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Information].[SourceType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new user into [Information].[SourceType] table
-- =============================================

ALTER PROCEDURE [Information].[SourceType_Insert]
    @CreatedByUserId BIGINT,
    @SourceTypeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-17 -> Andrew Sampson -> Updated as part of code refactor
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription 
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[SourceType]
            (
                CreatedByUserId,
                SourceTypeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceTypeDescription
            )
        END
END
GO
