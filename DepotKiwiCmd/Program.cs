using System;
using System.Linq;

using DepotKiwiCmd.Commands;
using DepotKiwiCmd.Utils;

namespace DepotKiwiCmd {
    internal static class Program {
        private static void ShowHelp() {
            Console.WriteLine("[*] usage: DepotKiwiCmd.exe <endpoint> <command>");
            
            foreach (var command in _commands) {
                Console.Write($"[*] {command.Name}");

                foreach (var parameter in command.Parameters) {
                    Console.Write($" <{parameter.Name}>");
                }

                Console.WriteLine();
            }
        }

        internal static void Main(string[] args) {
            if (args.Length < 2) {
                ShowHelp();

                return;
            }

            var api = args[0];
            var parameters = args.Skip(2).ToArray();
            
            foreach (var command in _commands) {
                if (command.Name == args[1]) {
                    if (command.Parameters.Length != parameters.Length) {
                        Console.WriteLine($"[-] Invalid parameter count!");

                        return;
                    }

                    command.Execute(new DepotHelper(api, Environment.CurrentDirectory), args.Skip(1).ToArray());

                    return;
                }
            }

            ShowHelp();
        }

        private static ICommand[] _commands = {
            new Create(),
            new Clean(),
            new Pull(),
            new Push()
        };
    }
}