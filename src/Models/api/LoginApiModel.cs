using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRepoWebApp.Models.api
{
    /// <summary>
    /// The result of a login request via API
    /// </summary>
    public class LoginApiModel
    {
        #region Public Properties
        public long UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool Verified { get; set; }
        
        public bool Admin { get; set; }

        #endregion

        #region Constructor

        public LoginApiModel()
        {
        }

        #endregion
    }
}
