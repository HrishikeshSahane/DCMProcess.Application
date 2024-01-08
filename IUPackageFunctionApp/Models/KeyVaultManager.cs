using Azure.Security.KeyVault.Secrets;
using IUPackageFunctionApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUPackageFunctionApp.Models
{
    public class KeyVaultManager : IKeyVaultManager
    {
        public static SecretClient _secretClient;
        public KeyVaultManager(SecretClient secretClient)
        {
            _secretClient = secretClient;


        }

        public string GetSecret(string secretName)
        {
            try
            {
                Uri keyVaultUri = new Uri("https://iukey-vault.vault.azure.net/");
                KeyVaultSecret keyValueSecret = _secretClient.GetSecret(secretName);

                return keyValueSecret.Value;
            }
            catch
            {
                throw;
            }
        }
    }
}
