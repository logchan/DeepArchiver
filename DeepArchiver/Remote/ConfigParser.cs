using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;

namespace DeepArchiver.Remote {
    public static class ConfigParser {
        public static RemoteService Parse(string config) {
            if (config.StartsWith("AWS;")) {
                return ParseAws(config);
            }
            if (config.StartsWith("LOCAL;")) {
                return ParseLocal(config);
            }

            throw new Exception("Invalid config: can't determine identifier");
        }

        private static LocalFileRemote ParseLocal(string config) {
            return new LocalFileRemote {
                Root = config.Substring(6)
            };
        }

        /// <summary>
        /// Parse AWS config
        /// </summary>
        /// <param name="config">AWS;access-key-id;secret-access-key;bucket-name;bucket-region</param>
        private static AwsRemote ParseAws(string config) {
            var args = config.Substring(4).Split(';');
            if (args.Length != 4) {
                throw new Exception("Expected format: AWS;access-key-id;secret-access-key;bucket-name;bucket-region");
            }

            var keyId = args[0];
            var secret = args[1];
            var bucket = args[2];
            var regionName = args[3];

            return new AwsRemote(keyId, secret, bucket, ParseRegion(regionName));
        }

        private static RegionEndpoint ParseRegion(string name) {
            var field = typeof(RegionEndpoint).GetField(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (field == null || field.FieldType != typeof(RegionEndpoint)) {
                throw new Exception($"Unable to parse region \"{name}\"");
            }

            return (RegionEndpoint) field.GetValue(null);
        }
    }
}
