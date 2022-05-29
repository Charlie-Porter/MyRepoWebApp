using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
    public class SendEmailDetails
    {
        public string fromName{ get; set; }
        public string fromEmail{ get; set; }
        public string toName{ get; set; }
        public string toEmail{ get; set; }
        public string subject { get; set; }
        public string content{ get; set; }
        public bool isHTML{ get; set; }


    }
}
