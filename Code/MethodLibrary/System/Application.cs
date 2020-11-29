using Newtonsoft.Json.Linq;
using System.Diagnostics;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class SystemSchema
        {
            public class Application
            {
                public void LaunchApplication(object data, string applicationGUID, long applicationId, string hostEnvironment, string fileName)
                {
                    var jsonObject = JObject.Parse(data.ToString());
                    if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(applicationGUID, applicationId, hostEnvironment, jsonObject))
                    {
                        return;
                    }

                    //Create new ApplicationRun entry
                    var applicationRunId = 0L;

                    if(hostEnvironment.Contains("Debug"))
                    {
                        DebugApplication(applicationGUID, applicationRunId.ToString());
                    }
                    else
                    {
                        //Get base folder of ConsoleApplications
                        //var consoleApplicationBaseFolder = string.Empty;

                        //Get additional folder of specific application
                        //var applicationLocation = string.Empty;
                        //var applicationLocationApplicationAttributeId = 0L;
                        
                        //Pass ApplicationRun Id through to the application as an argument
                        ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                        startInfo.Arguments = applicationRunId.ToString();
                        System.Diagnostics.Process.Start(startInfo);
                    }
                }

                private void DebugApplication(string applicationGUID, string arguments)
                {
                    var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
                    
                    if(applicationGUID == systemAPIGUIDEnums.UploadFileAPI)
                    {
                        // var program = new UploadFileAppProgram.Program();
                        // program.Main(arguments);
                    }
                }
            }
        }
    }
}