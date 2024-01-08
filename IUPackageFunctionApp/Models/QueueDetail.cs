using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUPackageFunctionApp.Models
{
    public class QueueDetail
    {
        public int Id { get; set; }

        public string BlobUri { get; set; } = null!;

        public int SentImageCount { get; set; }

        public DateTime DequeueTime { get; set; }
    }
}
