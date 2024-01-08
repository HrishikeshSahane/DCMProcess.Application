using System;
using System.Collections.Generic;

namespace DCMProcess.DataAccessLayer;

public partial class QueueDetail
{
    public int Id { get; set; }

    public string BlobUri { get; set; } = null!;

    public int SentImageCount { get; set; }

    public DateTime DequeueTime { get; set; }
}
