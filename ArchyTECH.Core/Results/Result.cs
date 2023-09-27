namespace ArchyTECH.Core.Results
{
    public class Result
    {
        public Result(){}
        public Result(bool success)
        {
            Success = success;
        }

        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        /// <summary>
        /// (Optional) error id 
        /// </summary>
        public string? ErrorId { get; set; }

        /// <summary>
        /// Whether or not request was successful 
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Status message for the request
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// A title for the result
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Meant to line up with JQuery validation of new { InvalidPropertyName: InvalidMessage} to provide client side validation error messages
        /// </summary>
        public Dictionary<string, string> Errors { get; set; } = new();

        /// <summary>
        /// Create a quick way to create a success message
        /// </summary>
        /// <param name="message">Helpful message that can be presented to UI</param>
        /// <returns>Successful Result with message</returns>
        public static Result Successful(string message)
        {
            return new Result(true, message);
        }

        /// <summary>
        /// Create a quick way to create a success message with some data
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="message">Helpful message that can be presented to UI</param>
        /// <param name="data">Object that could be used by UI</param>
        /// <returns>Successful Result with message and data</returns>
        public static DataResult<T> Successful<T>(string message, T data)
        {
            return new DataResult<T>(true, message)
            {
                Data = data
            };
        }

        /// <summary>
        /// Create a quick way to create a failure message
        /// </summary>
        /// <param name="message">Helpful message that can be presented to UI</param>
        /// <param name="errorId">Optional property for tracking errors</param>
        /// <returns>Failed Result with message</returns>
        public static Result Failed(string message, string? errorId = null)
        {
            return new Result(false, message)
            {
                ErrorId = errorId
            };
        }

        /// <summary>
        /// Create a quick way to create a failure message with some data
        /// </summary>
        /// <typeparam name="T">Type of data result</typeparam>
        /// <param name="message">Helpful message that can be presented to UI</param>
        /// <param name="data">Object that could be used by UI</param>
        /// <param name="errorId">Optional property for tracking errors</param>
        /// <returns>Failed Result with message and data</returns>
        public static DataResult<T> Failed<T>(string message, T data, string? errorId = null)
        {
            return new DataResult<T>(false, message)
            {
                ErrorId = errorId,
                Data = data
            };
        } 
        /// <summary>
        /// Create a quick way to create a failure message with for a DataResult
        /// </summary>
        /// <typeparam name="T">Type of data result</typeparam>
        /// <param name="message">Helpful message that can be presented to UI</param>
        public static DataResult<T> Failed<T>(string message)
        {
            return new DataResult<T>(false, message);
        }
    }
}