using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUPackageFunctionApp.Interfaces
{
    public  interface IKeyVaultManager
    {
        public string GetSecret(string secretName);
    }
}
