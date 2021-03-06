namespace Manzana.WebApi.MiddleWares
{
    /// <summary>
    /// Error response model
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Error response constructor
        /// </summary>
        /// <param name="message"></param>
        public ErrorResponse(string message)
        {
            Message = message;
        }
    }
}
