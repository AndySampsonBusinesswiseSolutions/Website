USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageGroupAttributeId BIGINT = (SELECT PageGroupAttributeId FROM [System].[PageGroupAttribute] WHERE PageGroupAttributeDescription = 'Navigation HTML')

DECLARE @PageGroupId BIGINT = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = 'E461FB50-E8DE-4EA5-9CE8-35441DFBAE0D') --Dashboard
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 25px;"><a href="/Internal/Dashboard/"><i class="fas fa-home" title="Dashboard"></i></a></li>'

SET @PageGroupId = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = '868666CA-E93F-4ADA-AC06-23D72FC31E8E') --Finance
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 25px;"><a href="#"><i class="fas fa-coins"></i><span class="fusion-caret"></span><span>Finance</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a></li>'

SET @PageGroupId = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = '4896A4A3-124E-4BF1-BDA2-7DAAB95303D1') --Energy Opportunities
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 25px;"><a href="#"><i class="fas fa-cog"></i><span class="fusion-caret"></span><span>Energy Opportunities</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a></li>'

SET @PageGroupId = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = 'C27E016D-D483-4A9E-8B41-F4D3634B0501') --Portfolio Management
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 25px;"><a href="#"><i class="fas fa-list-alt"></i><span class="fusion-caret"></span><span>Portfolio Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a></li>'

SET @PageGroupId = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = '85703311-C99D-446D-9007-E8A2207A7F37') --Supplier Management
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 25px;"><a href="#"><i class="fas fa-lightbulb"></i><span class="fusion-caret"></span><span>Supplier Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a></li>'

SET @PageGroupId = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = '68D8143E-B4E7-4B55-A0DA-A3BA34FAB89A') --Account Management
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 25px;"><a href="#"><i class="fas fa-user"></i><span class="fusion-caret"></span><span>Account Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a></li>'

SET @PageGroupId = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = '144E4143-1EB0-47F4-89C9-F613315ED685') --My Basket
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 15px;"><a href="/Internal/AccountManagement/MyDocuments/"><i class="fas fa-shopping-basket" title="Download Basket (2 items)"></i><span style="font-size: 10px; padding-top: 15px; color: black;">(2)</span></a></li>'

SET @PageGroupId = (SELECT PageGroupId FROM [System].[PageGroup] WHERE PageGroupGUID = '0E75486F-66DC-4A0C-BC5F-F1F31E1EBBF8') --Logout
EXEC [System].[PageGroupDetail_Insert] @CreatedByUserId, @SourceId, @PageGroupId, @PageGroupAttributeId, '<li style="padding-right: 35px;"></li><li><a href="/"><i class="fas fa-sign-out-alt" title="Logout"></i></a></li>'