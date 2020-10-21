USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileAgentDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileAgentDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-08
-- Description:	Insert new ProfileAgent detail into [DemandForecast].[ProfileAgentDetail] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileAgentDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileAgentId BIGINT,
    @ProfileAgentAttributeId BIGINT,
    @ProfileAgentDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [DemandForecast].[ProfileAgentDetail] WHERE ProfileAgentId = @ProfileAgentId 
        AND ProfileAgentAttributeId = @ProfileAgentAttributeId 
        AND ProfileAgentDetailDescription = @ProfileAgentDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [DemandForecast].[ProfileAgentDetail]
            (
                CreatedByUserId,
                SourceId,
                ProfileAgentId,
                ProfileAgentAttributeId,
                ProfileAgentDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProfileAgentId,
                @ProfileAgentAttributeId,
                @ProfileAgentDetailDescription
            )
        END
END
GO
