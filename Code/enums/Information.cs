namespace enums
{
    public partial class Enums
    {
        public class Information
        {
            public class Commodity
            {
                public string Electricity = "Electricity";
                public string Gas = "Gas";
            }

            public class ContractType
            {
                public string Fixed = "Fixed";
                public string Flex = "Flex";
            }

            public class File
            {
                public class Attribute
                {
                    public string FileName ="File Name";
                    public string ProcessQueueGUID ="Process Queue GUID";
                }

                public class Type
                {
                    public string UsageUpload = "Usage Upload";
                    public string LetterOfAuthority = "Letter Of Authority";
                    public string SupplierContract = "Supplier Contract";
                    public string EMaaSContract = "EMaaS Contract";
                    public string FlexContract = "Flex Contract";
                    public string Invoice = "Invoice";
                    public string SupplierBill = "Supplier Bill";
                }
            }

            public class GridSupplyPoint
            {
                public class Attribute
                {
                    public string GridSupplyPointGroupId = "Grid Supply Point Group Id";
                }
            }

            public class LocalDistributionZone
            {
                public class Attribute
                {
                    public string LocalDistributionZoneCode = "Local Distribution Zone Code";
                }
            }

            public class MeterExemption
            {
                public class Attribute
                {
                    public string MeterExemptionProduct = "Meter Exemption Product";
                    public string MeterExemptionProportion = "Meter Exemption Proportion";
                    public string UseDefaultValue = "Use Default Value?";
                }
            }

            public class MeterTimeswitchCode
            {
                public class Attribute
                {
                    public string MeterTimeswitchRangeStart = "Meter Timeswitch Class Range Start";
                    public string MeterTimeswitchRangeEnd = "Meter Timeswitch Class Range End";
                }
            }

            public class ProfileClass
            {
                public class Attribute
                {
                    public string ProfileClassCode = "Profile Class Code";
                }
            }

            public class RateType
            {
                public string StandingCharge = "Standing Charge";
                public string CapacityCharge = "Capacity Charge";
                public string UnitRate1 = "Unit Rate 1";
                public string UnitRate2 = "Unit Rate 2";
                public string UnitRate3 = "Unit Rate 3";
                public string UnitRate4 = "Unit Rate 4";
                public string UnitRate5 = "Unit Rate 5";
                public string UnitRate6 = "Unit Rate 6";
                public string UnitRate7 = "Unit Rate 7";
                public string UnitRate8 = "Unit Rate 8";
                public string UnitRate9 = "Unit Rate 9";
                public string UnitRate10 = "Unit Rate 10";
                public string ShapeFee = "Shape Fee";
                public string AdminFee = "Admin Fee";
                public string ImbalanceFee = "Imbalance Fee";
                public string RiskFee = "Risk Fee";
                public string GreenPremium = "Green Premium";
                public string OptimisationBenefit = "Optimisation Benefit";
            }

            public class RateUnit
            {
                public string PencePerKiloWattHour = "p/kWh";
                public string PencePerDay = "p/day";
                public string PencePerKiloVoltAmperePerDay = "p/kVa/day";
                public string PoundPerMegaWattHour = "£/MWh";
            }

            public class Source
            {
                public class Attribute
                {
                    public string UserGenerated = "User Generated";
                }
            }

            public class TradeDirection
            {
                public string Buy = "Buy";
                public string Sell = "Sell";
            }

            public class VolumeUnit
            {
                public string KiloWattHour = "kWh";
                public string MegaWatt = "MW";
                public string KiloVoltAmpere = "kVa";
            }
        }
    }
}