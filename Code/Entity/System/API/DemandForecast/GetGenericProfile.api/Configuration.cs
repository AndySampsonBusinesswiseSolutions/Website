namespace Entity
{
    public partial class System
    {
        public partial class API
        {
            public class GetGenericProfile
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

                    public Configuration(long APIId_, string APIGUID_, string Password_, string HostEnvironment_)
                    {
                        this.APIId = APIId_;
                        this.APIGUID = APIGUID_;
                        this.Password = Password_;
                        this.HostEnvironment = HostEnvironment_;
                    }
                }
            }
        }
    }
}