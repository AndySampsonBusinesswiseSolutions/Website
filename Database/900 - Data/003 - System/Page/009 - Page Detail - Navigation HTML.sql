USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageAttributeId BIGINT = (SELECT PageAttributeId FROM [System].[PageAttribute] WHERE PageAttributeDescription = 'Navigation HTML')

DECLARE @PageId BIGINT = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'A78E3EB1-C69E-4DFE-9653-C30ADDD4D3BF') --Data Analysis
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/Finance/DataAnalysis/"><span>Eagle Eye<i style="font-size: 10px; vertical-align: text-top;" class="fas fa-trademark"></i></span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'D250779D-2A3C-44A4-B343-9F5C920CED3A') --Budget Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/Finance/BudgetManagement/"><span>Budget Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '0FAF0BF4-FE77-4A9D-8FDB-CA81CE2905CC') --Invoice Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/Finance/BillValidation/"><span>Invoice Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '74268A15-79AB-4375-930F-028C99297F38') --Revenue Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/Finance/Commissions/"><span>Revenue Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'B228030E-F3FE-46BF-94DF-2C6C8B1D71F3') --Opportunities Dashboard
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/EnergyEfficiency/OpportunitiesDashboard/"><span>Opportunities Dashboard</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'CAA28556-5D67-4D60-A9C2-51C5847221E6') --Pending & Active Opportunities
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/"><span>Pending & Active Opportunities</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '52DB7B9B-6FEB-4BFF-A466-1E6F4F166D0A') --Finished Opportunities
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/EnergyEfficiency/FinishedOpportunities/"><span>Finished Opportunities</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '37D34C4F-C9BD-48B1-B3C1-1D95B3BF2211') --Opportunity Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/EnergyEfficiency/OpportunityManagement/"><span>Opportunity Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '714F10C4-ACF3-4409-97A8-C605E8E2FD0C') --Site Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/PortfolioManagement/SiteManagement/"><span>Site Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '20C6F1CC-4BA3-41A8-995C-D7024F2585D3') --Contract Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/PortfolioManagement/ContractManagement/"><span>Contract Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '2B3A955E-BAD8-4D22-80EF-E556CB582093') --Flexible Procurement
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/PortfolioManagement/ViewFlexPosition/"><span>Flexible Procurement</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '97E452E6-7C54-49AE-9E3D-6FCC9CC0AB7D') --Supplier Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/SupplierManagement/SupplierManagement/"><span>Supplier Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'F252ED3D-E30E-48A3-B298-5709D3A154B2') --Supplier Product Management
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/SupplierManagement/SupplierProductManagement/"><span>Supplier Product Management</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '6B23BB38-0850-4969-B786-B7751EC04442') --My Profile
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/AccountManagement/MyProfile/"><span>My Profile</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = 'B974CFB4-AE27-4A72-A1D7-927B84C5CB62') --Manage Users
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/AccountManagement/ManageUsers/"><span>Manage Users</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '80B1CC99-7C91-4D07-A541-9D69AC4CC304') --Manage Customers
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/AccountManagement/ManageCustomers/"><span>Manage Customers</span></a></li>'

SET @PageId = (SELECT PageId FROM [System].[Page] WHERE PageGUID = '33A6D63F-D9C7-45FA-9BCF-B471F1A3D842') --My Documents
EXEC [System].[PageDetail_Insert] @CreatedByUserId, @SourceId, @PageId, @PageAttributeId, '<li class="submenu-item"><a href="/Internal/AccountManagement/MyDocuments/"><span>My Documents</span></a></li>'