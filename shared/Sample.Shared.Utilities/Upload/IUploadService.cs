using System.Collections.Generic;
using System.IO;

namespace Sample.Shared.Utilities.Upload
{
    public interface IUploadService
    {
        /// <summary>
        /// Uploads the local file.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="localPath">The local path.</param>
        /// <returns></returns>
        string UploadLocalFile(string directoryName, string fileName, string localPath);

        /// <summary>
        /// Uploads the byte array.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="byteArray">The byte array.</param>
        /// <returns></returns>
        string UploadByteArray(string directoryName, string fileName, byte[] byteArray);

        /// <summary>
        /// Uploads the file stream.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <returns></returns>
        string UploadFileStream(string directoryName, string fileName, Stream fileStream);

        /// <summary>
        /// Uploads the base64 string.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="base64String">The base64 string.</param>
        /// <returns></returns>
        string UploadBase64String(string directoryName, string fileName, string base64String);

        /// <summary>
        /// Gets the pre-signed file URL.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="expiryTimeInMinutes">The expiry time in minutes.</param>
        /// <returns></returns>
        string GetPreSignedFileUrl(string fileUrl, double expiryTimeInMinutes);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <returns></returns>
        bool DeleteFile(string fileUrl);

        /// <summary>
        /// Gets the file stream.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <returns></returns>
        Stream GetFileStream(string fileUrl);

        /// <summary>
        /// Gets the file count.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="thumbnailPath">The thumbnail path.</param>
        /// <returns></returns>
        int GetFileCount(string path, string thumbnailPath);

        /// <summary>
        /// Gets the attachments.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="thumnail">if set to <c>true</c> [thumnail].</param>
        /// <returns></returns>
        List<string> GetAllFiles(string path, bool thumnail = false);

        /// <summary>
        /// Deletes all attachments.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        bool DeleteAllFiles(string path);

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        string GetFile(string path);
    }
}
