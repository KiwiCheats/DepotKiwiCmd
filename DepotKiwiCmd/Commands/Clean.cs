using System;
using System.Linq;

using DepotKiwiApiCore.Utils;

namespace DepotKiwiCmd.Commands {
    internal class Clean : ICommand {
        public string Name => "clean";
        public ICommand.Parameter[] Parameters => new ICommand.Parameter[0];

        public void Execute(DepotHelper depotHelper, string[] parameters) {
            var depot = depotHelper.Get();

            if (depot is null) {
                Console.WriteLine("[-] Current directory is not a depot.");

                return;
            }

            var currentFiles = depotHelper.GetFiles().ToList();
            var cachedFiles = depot.Files;

            foreach (var name in currentFiles) {
                if (cachedFiles.Any(file => file.Name == name)) {
                    if (!depotHelper.FileMatches(name, cachedFiles.FirstOrDefault(file => file.Name == name)?.Sha256)) {
                        Console.WriteLine($"[*] Syncing: {name}");
                        
                        var buffer = depotHelper.Api.DownloadFile(depot.Id, name);

                        if (buffer is null || !depotHelper.SaveFile(name, buffer)) {
                            Console.WriteLine($"[-] Failed to sync: {name}");
                        }
                    }
                    
                    continue;
                }

                Console.WriteLine($"[*] cleaning loose file: {name}");
                
                if (!depotHelper.DeleteFile(name)) {
                    Console.WriteLine($"[-] Failed to delete: {name}");
                }
            }
        }
    }
}