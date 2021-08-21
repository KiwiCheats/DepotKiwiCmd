using System;

namespace DepotKiwiCmd.Exceptions {
    public class KiwiCheatsException : Exception {
        protected KiwiCheatsException(string message) : base(message) { }
    }
}