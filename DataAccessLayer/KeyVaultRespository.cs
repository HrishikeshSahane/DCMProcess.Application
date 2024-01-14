using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMProcess.DataAccessLayer
{
    public class KeyVaultRespository
    {
        SecretClient _secretClient;
        public KeyVaultRespository()
        {
            _secretClient = new SecretClient(new Uri("https://dcmprocesskeyvault.vault.azure.net/"), new DefaultAzureCredential());
        }


        public string GetKey(string key)
        {
            string value = String.Empty;
            try
            {
                value = _secretClient.GetSecret(key).Value.Value;
                return value;
            }
            catch (Exception ex)
            {
                return value;
            }
        }
    }
}
