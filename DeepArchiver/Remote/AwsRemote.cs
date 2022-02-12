using Amazon;
using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace DeepArchiver.Remote {
    public sealed class AwsRemote : RemoteService {

        private readonly AmazonS3Client _client;
        private readonly string _bucket;

        public AwsRemote(string keyId, string secret, string bucket, RegionEndpoint region) {
            _client = new AmazonS3Client(keyId, secret, region);
            _bucket = bucket;
        }

        protected override async Task Upload(string path, string hash, Action<int> progressCallback) {
            var transfer = new TransferUtility(_client);
            var request = new TransferUtilityUploadRequest {
                BucketName = _bucket,
                FilePath = path,
                Key = hash,
                PartSize = 20 * (1 << 20),
                StorageClass = S3StorageClass.DeepArchive
            };
            request.Metadata.Add("path", path);
            request.Metadata.Add("hash", hash);

            var currentProgress = 0;
            request.UploadProgressEvent += (sender, e) => {
                var progress = (int) (e.TransferredBytes * 100 / e.TotalBytes);
                if (progress > currentProgress) {
                    currentProgress = progress;
                    progressCallback(progress);
                }
            };

            await transfer.UploadAsync(request);
        }

        protected override async Task Delete(string hash) {
            await _client.DeleteObjectAsync(new DeleteObjectRequest {
                BucketName = _bucket,
                Key = hash,
            });
        }
    }
}
