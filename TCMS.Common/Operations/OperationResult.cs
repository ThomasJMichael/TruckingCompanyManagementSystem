using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TCMS.Common.Operations
{
    public class OperationResult
    {
        [JsonPropertyName("isSuccessful")]
        public bool IsSuccessful { get; set; }
        [JsonPropertyName("messages")]
        public List<string> Messages { get; set; } = new List<string>();
        public OperationResult() { }

        protected OperationResult(bool isSuccessful, List<string>? messages = null)
        {
            IsSuccessful = isSuccessful;
            Messages = messages ?? [];
        }

        public override string ToString()
        {
            return $"IsSuccessful: {IsSuccessful}, Messages: {string.Join(", ", Messages)}";
        }


        public static OperationResult Success()
        {
            return new OperationResult(true);
        }

        public static OperationResult Failure(IEnumerable<string> messages)
        {
            return new OperationResult(false, messages.ToList());
        }

        public static OperationResult UserNotFound()
        {
            return new OperationResult(false, ["User not found"]);
        }

        public static OperationResult UserAlreadyExists()
        {
            return new OperationResult(false, ["User already exists"]);
        }

        public static OperationResult PasswordsDoNotMatch()
        {
            return new OperationResult(false, ["New password and confirmation password do not match."]);
        }

        public static OperationResult InvalidUsernamePassword()
        {
            return new OperationResult(false, ["Invalid username or password."]);
        }

        public static OperationResult MiscError(params string[] messages)
        {
            return new OperationResult(false, [.. messages]);
        }

        public static OperationResult RoleNotFound()
        {
            return new OperationResult(false, ["Role not found"]);
        }
    }

    public class OperationResult<T>
    {
        [JsonPropertyName("isSuccessful")]
        public bool IsSuccessful { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
        [JsonPropertyName("messages")]
        public List<string> Errors { get; set; } = new List<string>();

        public OperationResult() { }

        protected OperationResult(bool success, T data, List<string>? errors = null)
        {
            IsSuccessful = success;
            Data = data;
            Errors = errors ?? [];
        }

        public static OperationResult<T> Success(T data) => new(true, data);

        public static OperationResult<T> Failure(List<string> errors) => new(false, default, errors);
    }

}

