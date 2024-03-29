﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Report
{
    public class CsvResultDto(byte[] content, string filename)
    {
        public byte[] Content { get; set; } = content;
        public string Filename { get; set; } = filename;
        public string ContentType { get; set; } = "text/csv";
    }
}
