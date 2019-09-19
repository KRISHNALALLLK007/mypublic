namespace Sample.Shared.Utilities.Upload
{
    public class AwsParams
    {
        public string AccessKeyId { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string BucketName { get; set; }
        public string BucketUrl { get; set; }
        public string BaseUrl { get; set; }

        public AwsParams()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsParams" /> class.
        /// </summary>
        /// <param name="accessKeyId">The access key identifier.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="region">The region.</param>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="bucketUrl">The bucket URL.</param>
        public AwsParams(string accessKeyId, string secretKey, string region, string bucketName, string baseUrl, string bucketUrl)
        {
            AccessKeyId = accessKeyId;
            SecretKey = secretKey;
            Region = region;
            BucketName = bucketName;
            BaseUrl = baseUrl;
            BucketUrl = bucketUrl;
        }
    }
}
