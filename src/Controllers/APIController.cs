using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyRepoWebApp.Models;
using System.Security.Claims;
using System.Linq;
using MyRepoWebApp.Services.Logger;
using System;
using MyRepoWebApp.Data;
using System.Net.Http;
using MyRepoWebApp.Models.api;

namespace MyRepoWebApp.Controllers
{
    public class APIController : ControllerBase
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected MyRepoWebAppContext mContext;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The manager for handling signing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> mSignInManager;

        /// <summary>
        /// To store the folder data for 
        /// </summary>
        protected IList<Models.FolderModel> Folder { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        /// <param name="signInManager">The Identity sign in manager</param>
        /// <param name="userManager">The Identity user manager</param>
        public APIController(MyRepoWebAppContext context)
        {
            mContext = context;
        }

        #endregion

        #region IsUserAuthenticated

        [Route("api/IsUserAuthenticated")]
        [HttpGet]
        /// <summary>
        /// Returns if the user is authenticated 
        /// </summary>
        /// <returns></returns>
        /// 
        public IActionResult IsUserAuthenticated()
        {

            // Create list of empty errors
            var errors = new List<string>();

            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;


            if (null != principal)
            {
                foreach (Claim claim in principal.Claims)
                {
                    WriteToLog.writeToLogInformation("CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value + "</br>");
                }
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(true);
            }

            return Ok(false);
        }

        #endregion

        #region IsUserAnAdminAsync

        [Route("api/IsUserAnAdminAsync")]
        [HttpGet]
        /// <summary>
        /// Returns if the user is an admin if they are authenticated
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<UserIsAdminApiModel>> IsUserAnAdminAsync()
        {

            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            string isAdmin = string.Empty;


            if (principal == null)
            {
                // Return error
                return new ApiResponse<UserIsAdminApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "Claim not found"
                };
            }
            if (principal.Identity.IsAuthenticated)
            {

                foreach (Claim claim in principal.Claims)
                {
                    if (claim.Type == "Admin")
                    {
                        isAdmin = claim.Value;
                    }
                };

                // Return email and if they are an admin
                return new ApiResponse<UserIsAdminApiModel>
                {
                    // Pass back the user details and the if is a admin
                    Response = new UserIsAdminApiModel
                    {
                        Email = principal.Identity.Name,
                        Admin = isAdmin
                    }
                };
            }
            else
            {
                // Return error
                return new ApiResponse<UserIsAdminApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User is not authenticated",
                    Code = 401
                };
            }
        }

        #endregion

        #region GetFolder

        [Route("api/GetFolder/{Id}")]
        [HttpGet]
        /// <summary>
        /// Returns a folder using its id
        /// </summary>
        /// <returns></returns>

        public async Task<ApiResponse<GetFolderApiModel>> GetFolder(long Id)
        {

            string folderId = string.Empty;
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;

            if (principal == null)
            {

                // Return error
                return new ApiResponse<GetFolderApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User not found",
                    Code = 404
                };
            }
            if (principal.Identity.IsAuthenticated)
            {
                var data = mContext.FolderModel.AsQueryable();

                //get folder id from 
                data = from m in mContext.FolderModel
                                 where m.owner == User.Identity.Name
                                 where m.ID == Id
                                 select m;

                // Return email and if they are an admin
                return new ApiResponse<GetFolderApiModel>
                {
                    // Pass back the user details and the if is a admin
                    Response = new GetFolderApiModel
                    {
                        ID = data.FirstOrDefault().ID,
                        Name = data.FirstOrDefault().Name,
                        owner = data.FirstOrDefault().owner,
                        UpdateDate = data.FirstOrDefault().UpdateDate                               
                     }
                };                
            }
            else
            {
                // Return error
                return new ApiResponse<GetFolderApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User is not authenticated",
                    Code = 401
                };
            }

            // Return error
            return new ApiResponse<GetFolderApiModel>()
            {
                // TODO: Localization
                ErrorMessage = "Internal Server Error",
                Code = 500
            };


        }

        #endregion

    }
}
