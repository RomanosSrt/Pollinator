using System;
using System.Collections.Generic;
using System.Text;

namespace YpenService.Models.Ypen
{
    public class Crs
    {
        public string type { get; set; } = string.Empty;
        public Properties properties { get; set; } = new Properties();
    }
}
