using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using DepotKiwi.Models;

namespace DepotKiwiCmd.Utils {
    internal class DepotHelper {
        internal DepotHelper(string api, string directory) {
            Api = new Api(api);
            _directory = directory;
        }

        internal Api Api { get; }

        internal IEnumerable<string> GetFiles() {
            var kiwiDepotFolder = GetKiwiDepotFolder();
            
            foreach (var file in Directory.GetFiles(_directory, "*.*", SearchOption.AllDirectories)) {
                if ((File.GetAttributes(file) & FileAttributes.Directory) != 0)
                    continue;
                
                if (Path.GetDirectoryName(file) == kiwiDepotFolder)
                    continue;

                string name;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    name = file.Substring(_directory.Length + 1).Replace("\\", "/");
                }
                else {
                    name = file.Substring(_directory.Length + 1);
                }

                yield return name;
            }
        }

        internal bool Create(string id) {
            try {
                Directory.CreateDirectory(GetKiwiDepotFolder());

                File.WriteAllText(GetKiwiDepotPath(".depot"), id);

                return true;
            }
            catch {
                return false;
            }
        }

        internal Depot Get() {
            var path = GetKiwiDepotPath(".depot");

            if (!File.Exists(path)) {
                return null;
            }

            var id = File.ReadAllText(path);

            return Api.GetDepot(id);
        }

        internal FileStream GetFile(string name) {
            try {
                return File.OpenRead(Path.Join(_directory, name));
            }
            catch {
                return null;
            }
        }
        
        internal bool SaveFile(string name, byte[] buffer) {
            try {
                var path = Path.Join(_directory, name);

                var directory = Path.GetDirectoryName(path);

                if (directory is null)
                    return false;

                Directory.CreateDirectory(directory);

                using var writer = new BinaryWriter(File.Create(path), Encoding.ASCII);

                writer.Write(buffer);

                return true;
            }
            catch {
                return false;
            }
        }
        
        internal bool DeleteFile(string name) {
            try {
                File.Delete(Path.Join(_directory, name));

                return true;
            }
            catch {
                return false;
            }
        }

        internal bool FileMatches(string name, string hash) {
            var path = Path.Join(_directory, name);

            if (!File.Exists(path))
                return false;

            try {
                using var stream = File.OpenRead(path);

                return CalculateSha256(stream) == hash;
            }
            catch {
                return false;
            }
        }

        private string GetKiwiDepotPath(string file) {
            return Path.Join(GetKiwiDepotFolder(), file);
        }

        private string GetKiwiDepotFolder() {
            return Path.Join(_directory, ".kiwidepot");
        }
        
        private static string CalculateSha256(FileStream file) {
            using var crypt = new SHA256Managed();

            var builder = new StringBuilder();

            foreach (var b in crypt.ComputeHash(file)) {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        
        private readonly string _directory;
    }
}