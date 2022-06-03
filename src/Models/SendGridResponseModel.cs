using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
    public class SendGridResponseModel
    {
        //any errors from response
        public List<SendGridReponseErrorModel> Errors { get; set; } = new List<SendGridReponseErrorModel>();

    }
        
}
