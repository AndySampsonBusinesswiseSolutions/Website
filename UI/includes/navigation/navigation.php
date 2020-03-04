<link rel="stylesheet" href="/includes/navigation/navigation.css">

<header class="fusion-header-wrapper">
    <div class="fusion-header-v1 fusion-mobile-logo-1  fusion-mobile-menu-design-modern">
        <div class="fusion-header">
            <div class="fusion-row">
                <div class="fusion-logo" data-margin-top="20px" data-margin-bottom="0px" data-margin-left="0px" data-margin-right="0px">
                    <a class="fusion-logo-link" href="https://www.businesswisesolutions.co.uk/">
                        <!-- standard logo -->
                        <img src="https://www.businesswisesolutions.co.uk/wp-content/uploads/2018/05/bws-logo-white.png" srcset="https://www.businesswisesolutions.co.uk/wp-content/uploads/2018/05/bws-logo-white.png 1x" alt="Businesswise Solutions Logo" retina_logo_url="" class="fusion-standard-logo">
                    </a>
                </div>
                <?php if ($PAGE_TITLE == "Login") { ?>
                    <div class="topnav">
                        <div class="login-container">
                            <form method="post" action='/Internal/Dashboard/'>
                                <input type="text" placeholder="Email Address" name="username" required>
                                <input type="text" placeholder="Password" name="psw" required>
                                <button type="submit" class="login-button">Login</button>
                                <a href="/Internal/ForgottenPassword/" style="color: white;">Forgotten Password?</a>
                            </form>
                        </div>
                    </div>
                <?php } ?>
                <?php if ($PAGE_TITLE == "Forgotten Password") { ?>
                <?php } ?>
                <?php if ($PAGE_TITLE != "Login" && $PAGE_TITLE != "Forgotten Password") { ?>
                    <nav class="fusion-main-menu" aria-label="Main Menu">
                        <ul id="menu-main-menu" class="fusion-menu">
                            <li style="padding-right: 25px;"><a href="/Internal/Dashboard/" class="fusion-flex-link fusion-bar-highlight"><i class="fas fa-home" title="Dashboard"></i></a></li>
                            <li style="padding-right: 25px;"><a href="#" class="fusion-flex-link fusion-bar-highlight" aria-haspopup="true"><i class="fas fa-coins"></i><span class="fusion-caret"></span><span>Finance</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/Finance/BillValidation/"><span>Bill Validation</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/Finance/Commissions/"><span>Commissions</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/Finance/VarianceAnalysis/"><span>Variance Analysis</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#" class="fusion-flex-link fusion-bar-highlight" aria-haspopup="true"><i class="fas fa-cog"></i><span class="fusion-caret"></span><span>Energy Efficiency</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/OpportunitiesDashboard/"><span>Opportunities Dashboard</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/Pending&ActiveOpportunities/"><span>Pending & Active Opportunities</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/FinishedOpportunities/"><span>Finished Opportunities</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/EnergyEfficiency/OpportunityManagement/"><span>Opportunity Management</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#" class="fusion-flex-link fusion-bar-highlight" aria-haspopup="true"><i class="fas fa-list-alt"></i><span class="fusion-caret"></span><span>Portfolio Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/PortfolioManagement/SiteManagement/"><span>Site Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/PortfolioManagement/ContractManagement/"><span>Contract Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/PortfolioManagement/ViewFlexPosition/"><span>View Flex Position</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/PortfolioManagement/ViewMeterConsumption/"><span>View Meter Consumption</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#" class="fusion-flex-link fusion-bar-highlight" aria-haspopup="true"><i class="fas fa-user"></i><span class="fusion-caret"></span><span>Account Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/MyProfile/"><span>My Profile</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/ManageUsers/"><span>Manage Users</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/ManageCustomers/"><span>Manage Customers</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/AccountManagement/MyDocuments/"><span>My Documents</span></a></li>
                                </ul>
                            </li>
                            <li style="padding-right: 25px;"><a href="#" class="fusion-flex-link fusion-bar-highlight" aria-haspopup="true"><i class="fas fa-lightbulb"></i><span class="fusion-caret"></span><span>Supplier Management</span><span class="fusion-caret"><i class="fas fa-chevron-down"></i></span></a>
                                <ul class="sub-menu">
                                    <li class="submenu-item"><a href="/Internal/SupplierManagement/SupplierManagement/"><span>Supplier Management</span></a></li>
                                    <li class="submenu-item"><a href="/Internal/SupplierManagement/SupplierProductManagement/"><span>Supplier Product Management</span></a></li>
                                </ul>
                            </li>
                            <li><a href="/" class="fusion-flex-link fusion-bar-highlight"><i class="fas fa-sign-out-alt" title="Logout"></i></a></li>
                        </ul>
                    </nav>
                <?php } ?>
            </div>
        </div>
    </div>
    <div class="fusion-clearfix"></div>
</header>