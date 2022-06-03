using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
    public class SendEmailDetails
    {
        public string fromName { get; set; } = string.Empty;
        public string fromEmail{ get; set; } = string.Empty;
        public string toName{ get; set; } = string.Empty;
        public string toEmail{ get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string content{ get; set; } = string.Empty;
        public bool isHTML{ get; set; } 


    }
}
