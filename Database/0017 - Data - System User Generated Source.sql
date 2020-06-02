USE [EMaaS]
GO

EXEC [Information].[Source_Insert] '743E21EE-2185-45D4-9003-E35060B751E2', 'User Generated', 0

DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = 'User Generated')
DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId AND SourceTypeEntityId = 0)

ALTER TABLE [Information].[SourceType] ADD CONSTRAINT
	FK_SourceType_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[SourceType].CreatedByUserId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'SourceType', N'CONSTRAINT', N'FK_SourceType_SourceId'

UPDATE
    [Administration.User].[User]
SET 
    SourceId = @SourceId,
    CreatedByUserId = @UserId
WHERE
    UserId = @UserId

UPDATE
    [Information].[SourceType]
SET 
    SourceId = @SourceId
WHERE
    SourceTypeId = @SourceTypeId

ALTER TABLE [Information].[SourceType] ALTER COLUMN SourceId BIGINT NOT NULL
GO