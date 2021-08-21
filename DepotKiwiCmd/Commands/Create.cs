using System;
using System.Linq;
using DepotKiwiCmd.Utils;

namespace DepotKiwiCmd.Commands {
    internal class Create : ICommand {
        public string Name => "create";

        public ICommand.Parameter[] Parameters {
            get {
                return new [] {
                    new ICommand.Parameter("name:depot name", "Depot name.")
                };
            }
        }

        public void Execute(DepotHelper depotHelper, string[] parameters) {
            var response = depotHelper.Api.CreateRepository(parameters[0]);

            if (response is null) {
                Console.WriteLine("[-] Network issue.");

                return;
            }
            
            if (!response.Success) {
                Console.WriteLine($"[-] {response.Message}");
                
                return;
            }

            var repository = depotHelper.Api.ListRepositories().FirstOrDefault(x => x.Name == parameters[0]);

            if (repository is null) {
                Console.WriteLine("[-] Network issue.");

                return;
            }

            if (!depotHelper.Create(repository.Id)) {
                Console.WriteLine("[-] Failed to setup repository targets.");

                return;
            }
            
            Console.WriteLine("[*] Successfully created repository.");
        }
    }
}