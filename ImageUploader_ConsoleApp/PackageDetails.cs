﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMProcess.ImageUploaderApp
{
    public class PackageDetails
    {
        public string BlobURI { get; set; }
        public string SeriesID { get; set; }
        public int FilesCount { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}
