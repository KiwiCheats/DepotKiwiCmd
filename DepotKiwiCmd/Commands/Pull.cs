using System;

using DepotKiwiApiCore.Utils;

namespace DepotKiwiCmd.Commands {
    internal class Pull : ICommand {
        public string Name => "pull";
        public ICommand.Parameter[] Parameters => new ICommand.Parameter[0];

        public void Execute(DepotHelper depotHelper, string[] parameters) {
            var depot = depotHelper.Get();

            if (depot is null) {
                Console.WriteLine("[-] Current directory is not a depot.");

                return;
            }

            foreach (var file in depot.Files) {
                var path = depotHelper;
                
                if (path.FileMatches(file.Name, file.Sha256))
                    continue;
                
                Console.WriteLine($"[*] syncing file: {file.Name}");

                var buffer = depotHelper.Api.DownloadFile(depot.Id, file.Name);

                if (buffer is null) {
                    Console.WriteLine($"[-] Failed to download file: {file.Name}");

                    return;
                }

                if (!depotHelper.SaveFile(file.Name, buffer)) {
                    Console.WriteLine($"[-] Failed to save file: {file.Name}");

                    return;
                }
            }
        }
    }
}