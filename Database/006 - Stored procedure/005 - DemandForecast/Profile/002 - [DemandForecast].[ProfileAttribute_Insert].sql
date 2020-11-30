USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-09
-- Description:	Insert new Profile attribute into [DemandForecast].[ProfileAttribute] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [DemandForecast].[ProfileAttribute]
    (
        CreatedByUserId,
        SourceId,
        ProfileAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProfileAttributeDescription
    )
END
GO