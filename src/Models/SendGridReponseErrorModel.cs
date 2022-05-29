using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRepoWebApp.Models
{
    /// <summary>
    /// A Response to the SendGrid SendMessge call
    /// </summary>
    public class SendGridReponseErrorModel
    {
        /// <summary>
        /// The error message
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// The field inside the email details that was in the error is related to
        /// </summary>        
        public string field { get; set; }

        /// <summary>
        /// Useful inform
        /// </summary>
        public string help { get; set; }
    }
}

