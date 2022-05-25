using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
    public class SendEmailResponseModel
    {
        public bool Successful => errorMessage == null;
        public string errorMessage { get; set; }
    }
}
