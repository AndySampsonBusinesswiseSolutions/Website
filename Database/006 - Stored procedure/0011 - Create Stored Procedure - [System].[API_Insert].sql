USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[API_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[API_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new API into [System].[API] table
-- =============================================

ALTER PROCEDURE [System].[API_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @APIGUID UNIQUEIDENTIFIER
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

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[API] WHERE APIGUID = @APIGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [System].[API]
            (
                CreatedByUserId,
                SourceId,
                APIGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @APIGUID
            )
        END
END
GO
