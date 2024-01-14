using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMProcess.ImageUploaderApp
{
    public class Program
    {

        public static void Main(string[] args)
        {
            string storageconnectionString= "DefaultEndpointsProtocol=https;AccountName=dcmprocesscontainer;AccountKey=ns9OQoQ1LooyBXHD6OYsWKLyjZL53HUoyhz5tw5X6xK5CIYq5Qu+wqD9OiOH15ddX5K77lxiuhgh+ASt64oB5Q==;EndpointSuffix=core.windows.net";
            string storageconnectionKey= "ns9OQoQ1LooyBXHD6OYsWKLyjZL53HUoyhz5tw5X6xK5CIYq5Qu+wqD9OiOH15ddX5K77lxiuhgh+ASt64oB5Q==";
            ImageUploader.ReadFiles(storageconnectionString, storageconnectionKey);
        }


    }
}