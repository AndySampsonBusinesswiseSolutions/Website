USE [EMaaS]
GO

DECLARE @SourceGUID UNIQUEIDENTIFIER = '1963766E-F0CC-45E2-9E95-463E015023C9'
DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')

EXEC [Information].[Source_Insert] @CreatedByUserId, @SourceGUID

DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceGUID = @SourceGUID)

EXEC [Information].[SourceDetail_Insert] @CreatedByUserId, @SourceId, @SourceAttributeId, @SourceId

ALTER TABLE [Information].[SourceAttribute] ADD CONSTRAINT
	FK_SourceAttribute_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[SourceAttribute].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'SourceAttribute', N'CONSTRAINT', N'FK_SourceAttribute_SourceId'

UPDATE
    [Administration.User].[User]
SET 
    SourceId = @SourceId,
    CreatedByUserId = @CreatedByUserId
WHERE
    UserId = @CreatedByUserId

UPDATE
    [Information].[SourceAttribute]
SET 
    SourceId = @SourceId
WHERE
    SourceAttributeId = @SourceAttributeId

ALTER TABLE [Information].[SourceAttribute] ALTER COLUMN SourceId BIGINT NOT NULL
GO