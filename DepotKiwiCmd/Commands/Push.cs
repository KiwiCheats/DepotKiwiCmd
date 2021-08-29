using System;
using System.Linq;

using DepotKiwiApiCore.Utils;

namespace DepotKiwiCmd.Commands {
    internal class Push : ICommand {
        public string Name => "push";
        public ICommand.Parameter[] Parameters => new ICommand.Parameter[0];

        public void Execute(DepotHelper depotHelper, string[] parameters) {
            var depot = depotHelper.Get();

            if (depot is null) {
                Console.WriteLine("[-] Current directory is not a depot.");

                return;
            }

            var currentFiles = depotHelper.GetFiles().ToList();
            var cachedFiles = depot.Files;

            {
                foreach (var file in cachedFiles) {
                    if (currentFiles.Contains(file.Name))
                        continue;
                    
                    Console.WriteLine($"[*] Removing: {file.Name}");

                    var response = depotHelper.Api.DeleteFile(depot.Id, file.Name);

                    if (!response.Success) {
                        Console.WriteLine($"[-] {response.Message}");
                    }
                }
            }

            foreach (var name in depotHelper.GetFiles()) {
                var existingFile = depot.Files.FirstOrDefault(x => x.Name == name);

                if (existingFile is null || !depotHelper.FileMatches(name, existingFile.Sha256)) {
                    Console.WriteLine($"[*] pushing file: {name}");

                    using var file = depotHelper.GetFile(name);

                    var buffer = new byte[file.Length];
                    
                    file.Read(buffer);
                    
                    var response = depotHelper.Api.UploadFile(depot.Id, name, buffer);

                    if (!response.Success) {
                        Console.WriteLine($"[-] {response.Message}");

                        return;
                    }
                }
            }
        }
    }
}