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
using MyRepoWebApp.Models.api;
using MyRepoWebApp.Services;
using Microsoft.AspNetCore.Authentication;

namespace MyRepoWebApp.Controllers
{
    public class APIController : ControllerBase
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected MyRepoWebAppContext _context;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The manager for handling signing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> mSignInManager;

        protected CredentialModel doesUserExistinDB = new CredentialModel();

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
            _context = context;
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
        public Task<ApiResponse<UserIsAdminApiModel>> IsUserAnAdminAsync()
        {

            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
            string isAdmin = string.Empty;


            if (principal == null)
            {
                // Return error
                return Task.FromResult(new ApiResponse<UserIsAdminApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "Claim not found"
                });
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
                return Task.FromResult(new ApiResponse<UserIsAdminApiModel>
                {
                    // Pass back the user details and the if is a admin
                    Response = new UserIsAdminApiModel
                    {
                        Email = principal.Identity.Name,
                        Admin = isAdmin
                    }
                });
            }
            else
            {
                // Return error
                return Task.FromResult(new ApiResponse<UserIsAdminApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User is not authenticated",
                    Code = 401
                });
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

        public Task<ApiResponse<GetFolderApiModel>> GetFolder(long Id)
        {

            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;

            if (principal == null)
            {

                // Return error
                return Task.FromResult(new ApiResponse<GetFolderApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User not found",
                    Code = 404
                });
            }
            if (principal.Identity.IsAuthenticated)
            {
                var data = _context.FolderModel.AsQueryable();

                //get folder id from 
                if (data != null) data = from m in _context.FolderModel
                                 where m.owner == User.Identity.Name
                                 where m.ID == Id
                                 select m;

                if (data.Count() > 0)
                {
                    // Return email and if they are an admin
                    return Task.FromResult(new ApiResponse<GetFolderApiModel>
                    {
                        // Pass back the user details and the if is a admin
                        Response = new GetFolderApiModel
                        {
                            ID = data.FirstOrDefault().ID,
                            Name = data.FirstOrDefault().Name,
                            owner = data.FirstOrDefault().owner,
                            UpdateDate = data.FirstOrDefault().UpdateDate
                        }
                    });
                }                
            }
            else
            {
                // Return error
                return Task.FromResult(new ApiResponse<GetFolderApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User is not authenticated",
                    Code = 401
                });
            }

            // Return error
            return Task.FromResult(new ApiResponse<GetFolderApiModel>()
            {
                // TODO: Localization
                ErrorMessage = "Internal Server Error",
                Code = 500
            });


        }

        #endregion

        #region GetItem

        [Route("api/GetItem/{Id}")]
        [HttpGet]
        /// <summary>
        /// Returns an item using its id as an API
        /// </summary>
        /// <returns></returns>

        public Task<ApiResponse<GetItemApiModel>> GetItem(long Id)
        {

            
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;

            if (principal == null)
            {

                // Return error
                return Task.FromResult(new ApiResponse<GetItemApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User not found",
                    Code = 404
                });
            }
            if (principal.Identity.IsAuthenticated)
            {
                var data = _context.Upload.AsQueryable();

                //get folder id from 
                if (data != null) data = from m in _context.Upload
                       where m.Owner == User.Identity.Name
                       where m.Id == Id
                       select m;

                if (data.Count() > 0)
                {
                    // Return email and if they are an admin
                    return Task.FromResult(new ApiResponse<GetItemApiModel>
                    {
                        // Pass back the user details and the if is a admin
                        Response = new GetItemApiModel
                        {
                            Id = data.FirstOrDefault().Id,
                            Name = data.FirstOrDefault().Name,
                            Owner = data.FirstOrDefault().Owner,
                            UpdateDate = data.FirstOrDefault().UpdateDate,
                            Type = data.FirstOrDefault().Type,
                            ContentId = data.FirstOrDefault().ContentId,
                            FolderId = data.FirstOrDefault().FolderId,
                            Thumbnail = data.FirstOrDefault().Thumbnail
                        }
                    });
                }
            }
            else
            {
                // Return error
                return Task.FromResult(new ApiResponse<GetItemApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "User is not authenticated",
                    Code = 401
                });
            }

            // Return error
            return Task.FromResult(new ApiResponse<GetItemApiModel>()
            {
                // TODO: Localization
                ErrorMessage = "Internal Server Error",
                Code = 500
            });


        }

        #endregion

        #region LoginAsync

        [Route("api/Login/{UserId}/{Password}")]
        [HttpGet]
        /// <summary>
        /// For logging into the application and returns data if the passed userid and password is valid 
        /// </summary>
        /// <returns>  UserId, Verified, IsAdmin, Email</returns>
        public async Task<ApiResponse<LoginApiModel>> LoginAsync(string UserId, string Password)
        {
        
            try
            {
                // Create a claims principle for multiple claim identites 
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;

                // if user is not already authenicated
                if (!principal.Identity.IsAuthenticated)
                {
                    // create a variable for the IQueryable crendtails data
                    var data = _context.CredentialModel.AsQueryable();

                    // check if passed UserId and Password is valid 
                    if (CredentialModelUsernameExists(UserId, Password))
                    {
                        // check their email has been verfied
                        if (IsUserVerified(UserId))
                        {
                            // The passed credentails are valid, creating a claim for this user
                            WriteToLog.writeToLogInformation($@"{UserId} exists, password is valid and is verified, creating a claim for this user!");
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, UserId),
                                new Claim(ClaimTypes.Email, UserId),
                                new Claim("Admin", doesUserExistinDB.Admin.ToString())
                            };
                            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                            var authProperties = new AuthenticationProperties
                            {
                                //TODO: review if persistent login is required
                                //IsPersistent = Credential.RememberMe
                            };

                            WriteToLog.writeToLogInformation($@"{UserId} claim created and siging in to app");

                            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                            // Return email and if they are an admin
                            return new ApiResponse<LoginApiModel>
                            {
                                Code = 200,                                
                                // Pass back the user details and the if is a admin
                                Response = new LoginApiModel
                                {

                                    UserId = data.FirstOrDefault().UserId,
                                    Verified = data.FirstOrDefault().Verified,
                                    Admin = data.FirstOrDefault().Admin,
                                    Email = data.FirstOrDefault().Email                                    
                                }
                            };
                        }
                        // if not verified, report the status message to the page
                        else
                        {
                            // Return error
                            return new ApiResponse<LoginApiModel>()
                            {
                                // TODO: Localization
                                ErrorMessage = "User is not verified",
                                Code = 401
                            };
                        }
                    }
                    // Users credentials are not valid 
                    else
                    {
                        // Return error
                        return new ApiResponse<LoginApiModel>()
                        {
                            // TODO: Localization
                            ErrorMessage = "Users credentials are not valid ",
                            Code = 401
                        };
                    }

                }
                else
                {
                    return new ApiResponse<LoginApiModel>()
                    {
                        // TODO: Localization
                        ErrorMessage = "User is already authenticated",
                        Code = 200
                    };
                }                
            }
            catch (Exception ex)
            {
                WriteToLog.writeToLogInformation($@"********************* login API failed for {UserId}: {ex.Message} {ex.InnerException}");

                return new ApiResponse<LoginApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "Internal Server Error",
                    Code = 500
                };
            }
        }

        /// <summary>
        /// Checks if users email and password is valid. 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        private bool CredentialModelUsernameExists(String Email, String Password)
        {

            doesUserExistinDB = _context.CredentialModel.Where(a => a.Email.Equals(Email)).FirstOrDefault();

            if (doesUserExistinDB != null)
            {

                SecurityService sc = new SecurityService();
                if (sc.VerifyHashedPassword(doesUserExistinDB.Password, Password) == PasswordVerificationResult.Success)
                {
                    WriteToLog.writeToLogInformation($@"{Email} exists in database.");
                    return true;
                }
            }
            WriteToLog.writeToLogInformation($@"{Email} does not exist in database.");
            return false;
        }

        /// <summary>
        /// This methods checks if the users email is verified.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        private bool IsUserVerified(String Email)
        {

            doesUserExistinDB = _context.CredentialModel.Where(a => a.Email.Equals(Email)).FirstOrDefault();

            if (doesUserExistinDB.Verified)
            {
                WriteToLog.writeToLogInformation($@"{Email} is verifed.");
                return true;

            }
            else
            {
                WriteToLog.writeToLogInformation($@"{Email} is not verifed.");
                return false;
            }

        }

        #endregion

        #region LogoffAsync

        [Route("api/Logoff/{UserId}")]
        [HttpGet]
        /// <summary>
        /// Logs a user off via an API
        /// </summary>
        /// <returns></returns>

        public async Task<ApiResponse<LogoffApiModel>> LogoffAsync(string UserId)
        {

            try
            {
                WriteToLog.writeToLogInformation($@"{UserId} claim created and siging in to app");
                
                await HttpContext.SignOutAsync("MyCookieAuth");
               
                return new ApiResponse<LogoffApiModel>
                {
                    Code = 200,
                    // Pass back the user details and the if is a admin
                    Response = new LogoffApiModel
                    {
                            Successful = true
                    }
                };
            }                                                        
            catch (Exception ex)
            {
                WriteToLog.writeToLogInformation($@"********************* logoff API failed for {UserId}: {ex.Message} {ex.InnerException}");

                return new ApiResponse<LogoffApiModel>()
                {
                    // TODO: Localization
                    ErrorMessage = "Internal Server Error",
                    Code = 500
                };
            }
        }
        
        #endregion

    }
}
