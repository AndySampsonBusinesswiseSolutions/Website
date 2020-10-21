USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileAgent_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileAgent_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-08
-- Description:	Insert new ProfileAgent into [DemandForecast].[ProfileAgent] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileAgent_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileAgentGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [DemandForecast].[ProfileAgent] WHERE ProfileAgentGUID = @ProfileAgentGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [DemandForecast].[ProfileAgent]
            (
                CreatedByUserId,
                SourceId,
                ProfileAgentGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProfileAgentGUID
            )
        END
END
GO
