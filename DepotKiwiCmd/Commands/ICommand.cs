using DepotKiwiCmd.Utils;

namespace DepotKiwiCmd.Commands {
    internal interface ICommand {
        internal class Parameter {
            internal Parameter(string name, string description) {
                Name = name;
                Description = description;
            }
            
            internal string Name { get; init; }
            internal string Description { get; init; }
        }
        
        string Name { get; }
        Parameter[] Parameters { get; }

        void Execute(DepotHelper depotHelper, string[] parameters);
    }
}