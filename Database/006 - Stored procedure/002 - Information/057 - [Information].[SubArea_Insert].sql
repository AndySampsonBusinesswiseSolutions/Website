USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[SubArea_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[SubArea_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new profile class into [Information].[SubArea] table
-- =============================================

ALTER PROCEDURE [Information].[SubArea_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SubAreaDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[SubArea] WHERE SubAreaDescription = @SubAreaDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[SubArea]
            (
                CreatedByUserId,
                SourceId,
                SubAreaDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @SubAreaDescription
            )
        END
END
GO
