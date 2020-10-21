USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileAgentAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileAgentAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-08
-- Description:	Insert new ProfileAgent attribute into [DemandForecast].[ProfileAgentAttribute] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileAgentAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileAgentAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [DemandForecast].[ProfileAgentAttribute] WHERE ProfileAgentAttributeDescription = @ProfileAgentAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [DemandForecast].[ProfileAgentAttribute]
            (
                CreatedByUserId,
                SourceId,
                ProfileAgentAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProfileAgentAttributeDescription
            )
        END
END
GO
