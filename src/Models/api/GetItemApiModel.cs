using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRepoWebApp.Models.api
{
    /// <summary>
    /// The result of a get item request via API
    /// </summary>
    public class GetItemApiModel
    {
        #region Public Properties
        /// <summary>
        /// The items id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// The items name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The owner of the item
        /// </summary>
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// The datetime of when the item was updated
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// The type of the item
        /// </summary>
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// The folder id of where the item is located
        /// </summary>
        public int FolderId { get; set; }

        /// <summary>
        /// The id of where the item exists in the content table
        /// </summary>
        public long ContentId { get; set; }

        /// <summary>
        /// The thumbnail of the item
        /// </summary>
        public byte[] Thumbnail { get; set; } = new byte[0];

        #endregion

        #region Constructor

        public GetItemApiModel()
        {
                
        }

        #endregion
    }
}
