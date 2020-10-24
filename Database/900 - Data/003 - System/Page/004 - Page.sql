USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '6641A1BF-84C8-48F8-9D79-70D0AB2BB787' --Login
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '342B0397-8DBD-4782-9A96-6C714450264A' --Dashboard
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, 'A78E3EB1-C69E-4DFE-9653-C30ADDD4D3BF' --Data Analysis
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, 'D250779D-2A3C-44A4-B343-9F5C920CED3A' --Budget Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '0FAF0BF4-FE77-4A9D-8FDB-CA81CE2905CC' --Invoice Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '74268A15-79AB-4375-930F-028C99297F38' --Revenue Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, 'B228030E-F3FE-46BF-94DF-2C6C8B1D71F3' --Opportunities Dashboard
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, 'CAA28556-5D67-4D60-A9C2-51C5847221E6' --Pending & Active Opportunities
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '52DB7B9B-6FEB-4BFF-A466-1E6F4F166D0A' --Finished Opportunities
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '37D34C4F-C9BD-48B1-B3C1-1D95B3BF2211' --Opportunity Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '5B34FA8A-F3A5-4D04-8A15-AC7D978DE702' --Create Opportunities
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '714F10C4-ACF3-4409-97A8-C605E8E2FD0C' --Site Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '20C6F1CC-4BA3-41A8-995C-D7024F2585D3' --Contract Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '2B3A955E-BAD8-4D22-80EF-E556CB582093' --Flexible Procurement
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '97E452E6-7C54-49AE-9E3D-6FCC9CC0AB7D' --Supplier Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, 'F252ED3D-E30E-48A3-B298-5709D3A154B2' --Supplier Product Management
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '6B23BB38-0850-4969-B786-B7751EC04442' --My Profile
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, 'B974CFB4-AE27-4A72-A1D7-927B84C5CB62' --Manage Users
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '80B1CC99-7C91-4D07-A541-9D69AC4CC304' --Manage Customers
EXEC [System].[Page_Insert] @CreatedByUserId, @SourceId, '33A6D63F-D9C7-45FA-9BCF-B471F1A3D842' --My Documents