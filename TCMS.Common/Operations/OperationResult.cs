using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.Operations
{
    public class OperationResult
    {
        public bool IsSuccessful { get; private set; }
        public List<string> Messages { get; private set; }

        protected OperationResult(bool isSuccessful, List<string>? messages = null)
        {
            IsSuccessful = isSuccessful;
            Messages = messages ?? [];
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
        public bool IsSuccessful { get; private set; }
        public T Data { get; private set; }
        public List<string> Errors { get; private set; }

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

