using System;

namespace MyRepoWebApp.Models.api
{
    /// <summary>
    /// The result of a login request or get user profile details request via API
    /// </summary>
    public class GetFolderApiModel
    {
        #region Public Properties

        public long ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string owner { get; set; } = string.Empty;

        public DateTime UpdateDate { get; set; }
    

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public GetFolderApiModel()
        {

        }
        #endregion
    }
}
