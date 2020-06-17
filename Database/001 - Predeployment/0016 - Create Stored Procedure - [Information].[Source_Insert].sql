USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Source_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Information].[Source_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new user into [Information].[Source] table
-- =============================================

ALTER PROCEDURE [Information].[Source_Insert]
    @CreatedByUserId BIGINT,
    @SourceTypeId BIGINT,
    @SourceTypeEntityId BIGINT
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

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId 
        AND SourceTypeEntityId = @SourceTypeEntityId 
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[Source]
            (
                CreatedByUserId,
                SourceTypeId,
                SourceTypeEntityId
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceTypeId,
                @SourceTypeEntityId
            )
        END
END
GO
