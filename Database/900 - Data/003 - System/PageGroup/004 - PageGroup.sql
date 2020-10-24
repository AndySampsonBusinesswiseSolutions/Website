USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, 'E461FB50-E8DE-4EA5-9CE8-35441DFBAE0D' --Dashboard
EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, '868666CA-E93F-4ADA-AC06-23D72FC31E8E' --Finance
EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, '4896A4A3-124E-4BF1-BDA2-7DAAB95303D1' --Energy Opportunities
EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, 'C27E016D-D483-4A9E-8B41-F4D3634B0501' --Portfolio Management
EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, '85703311-C99D-446D-9007-E8A2207A7F37' --Supplier Management
EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, '68D8143E-B4E7-4B55-A0DA-A3BA34FAB89A' --Account Management
EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, '144E4143-1EB0-47F4-89C9-F613315ED685' --My Basket
EXEC [System].[PageGroup_Insert] @CreatedByUserId, @SourceId, '0E75486F-66DC-4A0C-BC5F-F1F31E1EBBF8' --Logout