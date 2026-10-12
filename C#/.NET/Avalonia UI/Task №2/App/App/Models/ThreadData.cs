using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class ThreadData
    {
        public int ThreadId { get; set; }
        public DateTime Time { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
