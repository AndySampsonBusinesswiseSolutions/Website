USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @PageHeaderAttributeId BIGINT = (SELECT PageHeaderAttributeId FROM [System].[PageHeaderAttribute] WHERE PageHeaderAttributeDescription = 'HTML')

DECLARE @PageHeaderId BIGINT = (SELECT PageHeaderId FROM [System].[PageHeader] WHERE PageHeaderGUID = 'EB7B2CCF-B1C7-46D5-A4E9-9068D04E1E59') --Standard
EXEC [System].[PageHeaderDetail_Insert] @CreatedByUserId, @SourceId, @PageHeaderId, @PageHeaderAttributeId, '<link rel="stylesheet" href="/includes/navigation/navigation.css">
<link rel="stylesheet" href="/includes/base/base.css">

<header class="fusion-header-wrapper">
    <div class="fusion-header-v1 fusion-mobile-logo-1 fusion-mobile-menu-design-modern">
        <div class="fusion-header">
            <div class="fusion-row">
                <div class="fusion-logo">
                    <a class="fusion-logo-link" href="https://www.businesswisesolutions.co.uk/" style="float: left;">
                        <img class="fusion-standard-logo" srcset="https://cdn.foleon.com/upload/28150/unknown-10.3e4603022681.jpeg">
                    </a>
                </div>
                <nav class="fusion-main-menu" aria-label="Main Menu">
                    <ul id="menu-main-menu" class="fusion-menu">
                    </ul>
                </nav>
            </div>
        </div>
    </div>
    <div class="fusion-clearfix"></div>
</header>

<script src="/login.js"></script>'