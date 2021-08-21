namespace DepotKiwiCmd.Exceptions {
    public class BadDecodeException : KiwiCheatsException {
        protected BadDecodeException(string message) : base(message) { }
    }
}