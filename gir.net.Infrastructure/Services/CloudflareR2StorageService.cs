using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using gir.net.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace gir.net.Infrastructure.Services;

public class CloudflareR2StorageService : IImageStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _publicUrlPrefix;
    private readonly ILogger<CloudflareR2StorageService> _logger;

    public CloudflareR2StorageService(IConfiguration configuration, ILogger<CloudflareR2StorageService> logger)
    {
        _logger = logger;
        
        var accountId = configuration["R2_ACCOUNT_ID"];
        var accessKey = configuration["R2_ACCESS_KEY"];
        var secretKey = configuration["R2_SECRET_KEY"];
        _bucketName = configuration["R2_BUCKET_NAME"] ?? "gir-dev";
        _publicUrlPrefix = configuration["R2_PUBLIC_URL_PREFIX"] ?? "";

        if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
        {
            _logger.LogWarning("Cloudflare R2 credentials are not fully configured. Image uploads will fail.");
            
            // Initialize with dummy values so DI doesn't fail, but it will fail on use
            var credentials = new BasicAWSCredentials("dummy", "dummy");
            _s3Client = new AmazonS3Client(credentials, new AmazonS3Config { ServiceURL = "https://dummy.r2.cloudflarestorage.com" });
            return;
        }

        var credentialsForS3 = new BasicAWSCredentials(accessKey, secretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(credentialsForS3, config);
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
    {
        try
        {
            // Generate a unique filename to prevent overwriting
            var uniqueFileName = $"{Guid.NewGuid()}-{fileName}";

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = uniqueFileName,
                InputStream = imageStream,
                ContentType = contentType,
                DisablePayloadSigning = true
            };

            await _s3Client.PutObjectAsync(request);

            // If a custom public URL prefix is provided (like a custom domain), use it.
            // Otherwise, construct a default URL (Note: R2 buckets need to be made public or have a custom domain for direct access)
            if (!string.IsNullOrEmpty(_publicUrlPrefix))
            {
                return $"{_publicUrlPrefix.TrimEnd('/')}/{uniqueFileName}";
            }
            
            // Fallback (may not work directly unless bucket access is configured)
            return $"https://{_bucketName}.r2.cloudflarestorage.com/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload image {FileName} to Cloudflare R2", fileName);
            throw;
        }
    }
}
