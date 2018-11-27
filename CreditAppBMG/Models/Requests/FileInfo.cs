﻿
namespace CreditAppBMG.Models.Requests
{
    public class FileInfo
    {
        /// <summary>
        /// File at a public URL location
        /// </summary>
        public URLFileInfo documentURL { get; set; }
        /// <summary>
        /// The documentID as returned from the transient document creation API
        /// </summary>
        public string transientDocumentId { get; set; }

        /// <summary>
        /// The name of a library document that is available to the sender,
        /// </summary>
        public string libraryDocumentName { get; set; }

        /// <summary>
        /// The ID for a library document that is available to the sender,
        /// </summary>
        public string libraryDocumentId { get; set; }
    }
}
