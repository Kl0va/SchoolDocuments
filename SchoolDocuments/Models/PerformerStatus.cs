﻿using Newtonsoft.Json;
using SchoolDocuments.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    [JsonConverter(typeof(PerformStatusConv))]
    enum PerformerStatus
    {
        Waiting, InProgress, Completed
    }
}
