using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class SystemSchema
        {
            public partial class Application
            {
                public void LaunchApplication(object data, string APIGUID, long APIId, string hostEnvironment, string fileName)
                {
                    var jsonObject = JObject.Parse(data.ToString());
                    if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(APIGUID, APIId, hostEnvironment, jsonObject))
                    {
                        return;
                    }

                    ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                    startInfo.Arguments = JsonConvert.SerializeObject(data.ToString());
                    System.Diagnostics.Process.Start(startInfo);
                }
            }
        }
    }
}