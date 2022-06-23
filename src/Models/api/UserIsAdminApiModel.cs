namespace MyRepoWebApp.Models.api
{
    /// <summary>
    /// The result of a login request or get user profile details request via API
    /// </summary>
    public class UserIsAdminApiModel
    {
        #region Public Properties

        /// <summary>
        /// The authentication token used to stay authenticated through future requests
        /// </summary>
        /// <remarks>The Token is only provided when called from the login methods</remarks>
        public string Token { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users username
        /// </summary>
        public string Admin { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserIsAdminApiModel()
        {

        }

        #endregion
    }
}
