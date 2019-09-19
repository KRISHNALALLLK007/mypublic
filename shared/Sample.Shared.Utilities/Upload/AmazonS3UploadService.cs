using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Sample.Shared.Utilities.Upload
{
    public class AmazonS3UploadService : IUploadService
    {
        private readonly ILogger _logger = null;
        private AwsParams _awsParams = null;
        private readonly IServiceProvider _serviceProvider = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonS3UploadService" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="serviceProvider">The service provider</param>
        public AmazonS3UploadService(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            _logger = loggerFactory.CreateLogger(typeof(AmazonS3UploadService));
            _serviceProvider = serviceProvider;
            InitializeAwsParameters();
        }

        /// <summary>
        /// Sets the AWS S3 Specific Parameters
        /// </summary>
        /// <param name="awsParameter"></param>
        public void SetAwsParameters(AwsParams awsParameter)
        {
            _awsParams = awsParameter;
        }

        /// <summary>
        /// Sets the AWS S3 Specific Parameters
        /// </summary>
        /// <param name="accessKeyId"></param>
        /// <param name="secretKey"></param>
        /// <param name="region"></param>
        /// <param name="bucketName"></param>
        public void SetAwsParameters(string accessKeyId, string secretKey, string region, string bucketName, string baseUrl, string bucketUrl)
        {
            _awsParams = new AwsParams();
            _awsParams.AccessKeyId = accessKeyId;
            _awsParams.SecretKey = secretKey;
            _awsParams.Region = region;
            _awsParams.BucketName = bucketName;
            _awsParams.BaseUrl = baseUrl;
            _awsParams.BucketUrl = bucketUrl;
        }

        /// <summary>
        /// Uploads the local file.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="localPath">The local path.</param>
        /// <returns></returns>
        public string UploadLocalFile(string directoryName, string fileName, string localPath)
        {
            _logger.LogTrace("Entering UploadLocalFile in AmazonS3UploadService");
            try
            {
                var fileStream = new MemoryStream(File.ReadAllBytes(localPath));
                string s3FileName = @fileName;
                var response = UploadFileToAmazonS3(fileStream, directoryName, s3FileName);
                LogUploadStatus(fileName, response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("UploadLocalFile failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Uploads the base64 string.
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public string UploadBase64String(string directoryName, string fileName, string base64String)
        {
            _logger.LogTrace("Entering UploadBase64String in AmazonS3UploadService");
            try
            {
                var fileStream = new MemoryStream(Convert.FromBase64String(base64String));
                string s3FileName = @fileName;
                var response = UploadFileToAmazonS3(fileStream, directoryName, s3FileName);
                LogUploadStatus(fileName, response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("UploadBase64String failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Uploads the byte array.
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public string UploadByteArray(string directoryName, string fileName, byte[] byteArray)
        {
            _logger.LogTrace("Entering UploadByteArray in AmazonS3UploadService");
            try
            {
                Stream fileStream = new MemoryStream(byteArray);
                string s3FileName = @fileName;
                var response = UploadFileToAmazonS3(fileStream, directoryName, s3FileName);
                LogUploadStatus(fileName, response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("UploadByteArray failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Uploads the file stream.
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public string UploadFileStream(string directoryName, string fileName, Stream fileStream)
        {
            _logger.LogTrace("Entering UploadFileStream in AmazonS3UploadService");
            try
            {
                string s3FileName = @fileName;
                var response = UploadFileToAmazonS3(fileStream, directoryName, s3FileName);
                LogUploadStatus(fileName, response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("UploadFileStream failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Gets the pre signed file URL.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="expiryTimeInMinutes">The expiry time in minutes.</param>
        /// <returns></returns>
        public string GetPreSignedFileUrl(string fileUrl, double expiryTimeInMinutes)
        {
            _logger.LogTrace("Entering GetPreSignedFileUrl in AmazonS3UploadService");
            try
            {
                if (!string.IsNullOrWhiteSpace(fileUrl) && expiryTimeInMinutes > 0)
                {
                    string fileNameInS3 = GetAwsS3FileName(ref fileUrl);
                    // Getting bucket name from fileUrl
                    var bucketName = GetAwsS3BucketName(fileUrl, fileNameInS3);

                    var regionEndPoint = RegionEndpoint.GetBySystemName(_awsParams.Region);
                    //Initializing AmazonS3Client with AWS credentials
                    IAmazonS3 s3Client = new AmazonS3Client(_awsParams.AccessKeyId, _awsParams.SecretKey, regionEndPoint);
                    var expiryUrlRequest = new GetPreSignedUrlRequest();
                    //Setting GetPreSignedUrlRequest object
                    expiryUrlRequest.BucketName = bucketName.TrimEnd('/');
                    expiryUrlRequest.Key = fileNameInS3;
                    expiryUrlRequest.Expires = DateTime.UtcNow.AddMinutes(expiryTimeInMinutes);
                    expiryUrlRequest.Protocol = Protocol.HTTPS;
                    //Getting Pre-signed file URL
                    string preSignedUrl = s3Client.GetPreSignedURL(expiryUrlRequest);
                    _logger.LogInformation("Pre-signed URL received: " + preSignedUrl);
                    return preSignedUrl;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("GetPreSignedFileUrl failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Deletes the file from amazon s3.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <returns></returns>
        public bool DeleteFile(string fileUrl)
        {
            _logger.LogTrace("Entering DeleteFile in AmazonS3UploadService");
            try
            {
                if (!string.IsNullOrWhiteSpace(fileUrl))
                {
                    var isPresignedUrl = IsPresignedUrl(fileUrl, _awsParams.BucketUrl.TrimEnd('/'));
                    var fileNameInS3 = isPresignedUrl ? fileUrl.Replace(_awsParams.BucketUrl, string.Empty) : GetAwsS3FileName(ref fileUrl);

                    // Getting bucket name from fileUrl
                    var bucketName = GetAwsS3BucketName(fileUrl, fileNameInS3);

                    var regionEndPoint = RegionEndpoint.GetBySystemName(_awsParams.Region);
                    // Initializing AmazonS3Client with AWS credentials
                    IAmazonS3 s3Client = new AmazonS3Client(_awsParams.AccessKeyId, _awsParams.SecretKey, regionEndPoint);
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = isPresignedUrl ? _awsParams.BucketName : bucketName,
                        Key = fileNameInS3
                    };
                    // Delete file from S3
                    var response = s3Client.DeleteObjectAsync(deleteObjectRequest);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("DeleteFile failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Gets the file stream.
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public Stream GetFileStream(string fileUrl)
        {
            _logger.LogTrace("Entering GetFileStream in AmazonS3UploadService");
            try
            {
                if (!string.IsNullOrWhiteSpace(fileUrl))
                {
                    string fileNameInS3 = GetAwsS3FileName(ref fileUrl);
                    // Getting bucket name from fileUrl
                    var bucketName = GetAwsS3BucketName(fileUrl, fileNameInS3);

                    var regionEndPoint = RegionEndpoint.GetBySystemName(_awsParams.Region);
                    // Initializing AmazonS3Client with AWS credentials
                    IAmazonS3 s3Client = new AmazonS3Client(_awsParams.AccessKeyId, _awsParams.SecretKey, regionEndPoint);
                    var utility = new TransferUtility(s3Client);
                    return utility.OpenStream(bucketName, fileNameInS3);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("GetFileStream failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Gets the attachment count.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int GetFileCount(string path, string thumbnailPath)
        {
            _logger.LogTrace("Entering GetFileCount in AmazonS3UploadService");
            try
            {
                var regionEndPoint = RegionEndpoint.GetBySystemName(_awsParams.Region);
                IAmazonS3 client = new AmazonS3Client(_awsParams.AccessKeyId, _awsParams.SecretKey, regionEndPoint);
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = _awsParams.BucketName,
                    Prefix = path.TrimEnd('/') + "/",
                };
                var response = client.ListObjectsAsync(request).GetAwaiter().GetResult();
                var count = response.S3Objects.Select(x => x.Key).Count();
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("GetFileCount failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Gets the attachments.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="thumnail">if set to <c>true</c> [thumnail].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<string> GetAllFiles(string path, bool thumnail = false)
        {
            _logger.LogTrace("Entering GetAllFiles in AmazonS3UploadService");
            try
            {
                var fileUrls = new List<string>();
                var regionEndPoint = RegionEndpoint.GetBySystemName(_awsParams.Region);
                using (IAmazonS3 client = new AmazonS3Client(_awsParams.AccessKeyId, _awsParams.SecretKey, regionEndPoint))
                {
                    ListObjectsRequest request = new ListObjectsRequest
                    {
                        BucketName = _awsParams.BucketName,
                        Prefix = path.TrimEnd('/') + "/",
                        Delimiter = path,
                    };
                    var response = client.ListObjectsAsync(request).GetAwaiter().GetResult();
                    var df = response.S3Objects;
                    df.ForEach(x =>
                    {
                        fileUrls.Add(GetFileUrl(_awsParams.BucketName, x.Key, _awsParams.BaseUrl));
                    });

                    return fileUrls;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("GetAllFiles failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">GetFile failed in AmazonS3UploadService</exception>
        public string GetFile(string path)
        {
            _logger.LogTrace("Entering GetFile in AmazonS3UploadService");
            try
            {
                var fileUrl = GetFileUrl(_awsParams.BucketName, path, _awsParams.BaseUrl);
                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("GetFile failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Deletes all attachments.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">DeleteAllFiles failed in AmazonS3UploadService</exception>
        public bool DeleteAllFiles(string path)
        {
            _logger.LogTrace("Entering DeleteAllFiles in AmazonS3UploadService");
            try
            {
                var files = GetAllFiles(path);
                bool result = false;
                if (files == null || (files != null && !files.Any()))
                {
                    result = true;
                }
                foreach (var file in files)
                {
                    result = DeleteFile(file);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("DeleteAllFiles failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Determines whether [is presigned URL] [the specified file URL].
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>
        ///   <c>true</c> if [is presigned URL] [the specified file URL]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPresignedUrl(string fileUrl, string baseUrl)
        {
            return fileUrl.Contains(baseUrl);
        }

        /// <summary>
        /// Uploads the file to amazon s3.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="subDirectoryInBucket">The sub directory in bucket.</param>
        /// <param name="fileName">The file name in s3.</param>
        /// <returns></returns>
        string UploadFileToAmazonS3(Stream fileStream, string subDirectoryInBucket, string fileName)
        {
            _logger.LogTrace("Entering UploadFileToAmazonS3 in AmazonS3UploadService");
            try
            {
                var regionEndPoint = RegionEndpoint.GetBySystemName(_awsParams.Region);

                if (fileStream != null && !string.IsNullOrWhiteSpace(subDirectoryInBucket) && !string.IsNullOrWhiteSpace(fileName))
                {
                    IAmazonS3 s3Client = new AmazonS3Client(_awsParams.AccessKeyId, _awsParams.SecretKey, regionEndPoint);
                    var utility = new TransferUtility(s3Client);
                    var bucketName = _awsParams.BucketName + "/" + subDirectoryInBucket;

                    var request = new TransferUtilityUploadRequest();
                    request.Key = fileName; //file name with root
                    request.InputStream = fileStream;
                    request.BucketName = bucketName.TrimEnd('/');
                    request.StorageClass = S3StorageClass.ReducedRedundancy;
                    utility.Upload(request); //Uploading to S3

                    return GetFileUrl(bucketName, fileName, _awsParams.BaseUrl);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //    throw new InvalidOperationException("UploadFileToAmazonS3 failed in AmazonS3UploadService");
            }
            return null;
        }

        /// <summary>
        /// Gets the file URL.
        /// </summary>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="region">Region of the bucket.</param>
        /// <param name="subdirectory">The subdirectory.</param>
        /// <returns></returns>
        string GetFileUrl(string bucketName, string fileName, string baseUrl)
        {
            _logger.LogTrace("Entering GetFileUrl in AmazonS3UploadService");
            try
            {
                bucketName = bucketName.TrimStart('/');
                var fileUrl = new StringBuilder();
                fileUrl.Append(baseUrl)
                    .Append(bucketName)
                    .Append("/")
                    .Append(fileName);
                _logger.LogDebug("S3 File URL: " + fileUrl);
                return fileUrl.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("GetFileUrl failed in AmazonS3UploadService");
            }
        }

        /// <summary>
        /// Logs the upload status
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="response"></param>
        void LogUploadStatus(string fileName, string response)
        {
            if (response != null)
            {
                _logger.LogInformation("File " + fileName + " successfully uploaded to S3.");
            }
            else
            {
                _logger.LogInformation("File " + fileName + " could not be uploaded to S3.");
            }
        }

        /// <summary>
        /// Gets the AWS S3 Bucket Name
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="fileNameInS3"></param>
        /// <returns></returns>
        string GetAwsS3BucketName(string fileUrl, string fileNameInS3)
        {
            var bucketName = fileUrl.Replace(_awsParams.BaseUrl, string.Empty).ToString();
            bucketName = bucketName.Replace("/" + fileNameInS3, string.Empty);
            return bucketName;
        }

        /// <summary>
        /// Gets the AWS S3 File Name
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        static string GetAwsS3FileName(ref string fileUrl)
        {
            //Getting file name from fileUrl
            return fileUrl.Split('?').FirstOrDefault().Split('/').Last();
        }

        /// <summary>
        ///// Initializes the AWS S3 Configuration Parameters
        /// </summary>
        void InitializeAwsParameters()
        {
            var configuration = (IConfigurationRoot)_serviceProvider.GetService(typeof(IConfigurationRoot));
            var amazonS3Section = configuration.GetSection("AmazonS3");
            var awsAccessKeyId = amazonS3Section.GetValue<string>("AccessKeyId");
            var awsSecretKey = amazonS3Section.GetValue<string>("SecretKey");
            var awsS3BucketName = amazonS3Section.GetValue<string>("BucketName");
            var awsBucketRegion = amazonS3Section.GetValue<string>("Region");
            var awsBaseUrl = amazonS3Section.GetValue<string>("BaseUrl");
            var awsBucketUrl = amazonS3Section.GetValue<string>("BucketUrl");
            SetAwsParameters(awsAccessKeyId, awsSecretKey, awsBucketRegion, awsS3BucketName, awsBaseUrl, awsBucketUrl);
        }
    }
}
