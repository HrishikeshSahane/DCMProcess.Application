using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace IUPackageFunctionApp
{
    public class IUPackageFunctionApp
    {
        [FunctionName("IUPackageFunctionApp")]
        public async Task Run([QueueTrigger("%queue-name%", Connection = "AzureWebJobsStorage")]PackageDetails packageDetails, ILogger log)
        {
            Console.WriteLine( packageDetails.BlobURI);
            FunctionHelper functionHelper= new FunctionHelper();
            ////await Task.Run(()=> functionHelper.ProcessQueueMessage(packageDetails));
            functionHelper.ProcessQueueMessage(packageDetails);
        }
    }
}
