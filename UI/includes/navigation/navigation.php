<link rel="stylesheet" href="/includes/navigation/navigation.css">
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
                <?php if ($PAGE_TITLE == "Forgotten Password") { ?>
                <?php } ?>
                <?php if ($PAGE_TITLE != "Login" && $PAGE_TITLE != "Forgotten Password") { ?>
                    <nav class="fusion-main-menu" aria-label="Main Menu">
                        <ul id="menu-main-menu" class="fusion-menu">
                            <li style="padding-right: 25px;"><a href="/Internal/Dashboard/"><i class="fas fa-home" title="Dashboard"></i></a></li>
                            <li style="padding-right: 25px;"><a href="#"><i class="fas fa-coins"></i><span class="fusion-caret"></span><span>Finance</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/Finance/DataAnalysis/"><span>Eagle Eye<i style="font-size: 10px; vertical-align: text-top;" class="fas fa-trademark"></i></span></a></li>    
                                    <li class="submenu-item"><a href="/Internal/Finance/BudgetManagement/"><span>Budget Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/Finance/BillValidation/"><span>Invoice Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/Finance/Commissions/"><span>Revenue Management</span></a></li>                                    
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#"><i class="fas fa-cog"></i><span class="fusion-caret"></span><span>Energy Opportunities</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/OpportunitiesDashboard/"><span>Opportunities Dashboard</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/"><span>Pending & Active Opportunities</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/FinishedOpportunities/"><span>Finished Opportunities</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/OpportunityManagement/"><span>Opportunity Management</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#"><i class="fas fa-list-alt"></i><span class="fusion-caret"></span><span>Portfolio Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/PortfolioManagement/SiteManagement/"><span>Site Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/PortfolioManagement/ContractManagement/"><span>Contract Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/PortfolioManagement/ViewFlexPosition/"><span>Flexible Procurement</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#"><i class="fas fa-lightbulb"></i><span class="fusion-caret"></span><span>Supplier Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/SupplierManagement/SupplierManagement/"><span>Supplier Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/SupplierManagement/SupplierProductManagement/"><span>Supplier Product Management</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#"><i class="fas fa-user"></i><span class="fusion-caret"></span><span>Account Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/MyProfile/"><span>My Profile</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/ManageUsers/"><span>Manage Users</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/ManageCustomers/"><span>Manage Customers</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/MyDocuments/"><span>My Documents</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 15px;">
                                <a href="/Internal/AccountManagement/MyDocuments/">
                                    <i class="fas fa-shopping-basket" title="Download Basket (2 items)"></i>
                                    <span style="font-size: 10px; padding-top: 15px; color: black;">(2)</span>
                                </a>
                            </li>
                            <li style="padding-right: 35px;"></li>
                            <li><a href="/"><i class="fas fa-sign-out-alt" title="Logout"></i></a></li>
                        </ul>
                    </nav>
                <?php } ?>
            </div>
        </div>
    </div>
    <div class="fusion-clearfix"></div>
</header>

<script src="/login.js"></script>