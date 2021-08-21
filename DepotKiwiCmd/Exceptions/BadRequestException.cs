using System.Net;

namespace DepotKiwiCmd.Exceptions {
    public class BadRequestException : KiwiCheatsException {
        public BadRequestException(HttpStatusCode statusCode, string message) : base(message) {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; init; }
    }
}