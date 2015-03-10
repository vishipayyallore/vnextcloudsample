using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudSample.Entities
{
    public class Friend : TableEntity
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
    }
}
