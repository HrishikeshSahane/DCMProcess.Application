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
            string storageconnectionString= ImageUploader.GetKeyVaultSecretValue("DCMProcess-StorageConnectionString");
            string storageconnectionKey= ImageUploader.GetKeyVaultSecretValue("DCMProcess-StorageConnectionKey");
            ImageUploader.ReadFiles(storageconnectionString, storageconnectionKey);
        }


    }
}