using System.Collections.Generic;

namespace Entity
{
    public partial class System
    {
        public partial class API
        {
            public class CreateFiveMinuteForecast
            {
                public class Configuration
                {
                    private long _APIId;
                    public long APIId
                    {
                        get { return _APIId; }
                        set { _APIId = value; }
                    }

                    private string _APIGUID;
                    public string APIGUID
                    {
                        get { return _APIGUID; }
                        set { _APIGUID = value; }
                    }

                    private string _Password;
                    public string Password
                    {
                        get { return _Password; }
                        set { _Password = value; }
                    }

                    private string _HostEnvironment;
                    public string HostEnvironment
                    {
                        get { return _HostEnvironment; }
                        set { _HostEnvironment = value; }
                    }

                    private string _GranularityCode;
                    public string GranularityCode
                    {
                        get { return _GranularityCode; }
                        set { _GranularityCode = value; }
                    }

                    private Dictionary<long, List<long>> _NonStandardGranularityDateDictionary;
                    public Dictionary<long, List<long>> NonStandardGranularityDateDictionary
                    {
                        get { return _NonStandardGranularityDateDictionary; }
                        set { _NonStandardGranularityDateDictionary = value; }
                    }

                    private List<long> _StandardGranularityTimePeriodList;
                    public List<long> StandardGranularityTimePeriodList
                    {
                        get { return _StandardGranularityTimePeriodList; }
                        set { _StandardGranularityTimePeriodList = value; }
                    }

                    private Dictionary<long, Dictionary<long, List<long>>> _TimePeriodToMappedTimePeriodDictionary;
                    public Dictionary<long, Dictionary<long, List<long>>> TimePeriodToMappedTimePeriodDictionary
                    {
                        get { return _TimePeriodToMappedTimePeriodDictionary; }
                        set { _TimePeriodToMappedTimePeriodDictionary = value; }
                    }

                    public Configuration(long APIId_, string APIGUID_, string Password_, string HostEnvironment_, string GranularityCode_, 
                        Dictionary<long, List<long>> NonStandardGranularityDateDictionary_,
                        List<long> StandardGranularityTimePeriodList_,
                        Dictionary<long, Dictionary<long, List<long>>> TimePeriodToMappedTimePeriodDictionary_)
                    {
                        this.APIId = APIId_;
                        this.APIGUID = APIGUID_;
                        this.Password = Password_;
                        this.HostEnvironment = HostEnvironment_;
                        this.GranularityCode = GranularityCode_;
                        this.NonStandardGranularityDateDictionary = NonStandardGranularityDateDictionary_;
                        this.StandardGranularityTimePeriodList = StandardGranularityTimePeriodList_;
                        this.TimePeriodToMappedTimePeriodDictionary = TimePeriodToMappedTimePeriodDictionary_;
                    }
                }
            }
        }
    }
}