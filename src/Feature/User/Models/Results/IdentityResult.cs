using System.Collections.Generic;

namespace User.Models.Results
{
    public class IdentityResult
    {
        public string Token { get; set; } = null;
        public string RefreshToken { get; set; } = null;
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }

        public static IdentityResult EmptyRequestResult()
        {
            return new IdentityResult
            {
                Success = false,
                ErrorMessages = new[] { "Request is empty" }
            };
        }


        public static IdentityResult UserAlreadyExistsResult()
        {
            return new IdentityResult
            {
                Success = false,
                ErrorMessages = new[] {"This email address is already registered"}
            };
        }

        public static IdentityResult ErrorWhenCreatingUserResult()
        {
            return new IdentityResult
            {
                Success = false,
                ErrorMessages = new[] { "A problem occured during creating a new user" }
            };
        }


        public static IdentityResult UserDoesNotExistResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] { "User does not exist" }
            };
        }

        public static IdentityResult InvalidPasswordResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] { "User/password combination is wrong" }
            };
        }


        public static IdentityResult TokenDoesNotExistResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] {"Invalid Token"}
            };
        }

        public static IdentityResult TokenHasNotExpiredResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] { "This Token hasn't expired yet" }
            };
        }

        public static IdentityResult TokenHasExpiredResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] { "This Token has expired" }
            };
        }

        public static IdentityResult InvalidatedTokenResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] { "This Token has been invalidated" }
            };
        }

        public static IdentityResult TokenAlreadyUsedResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] { "This Token has been used" }
            };
        }

        public static IdentityResult TokensDoNotMatchResult()
        {
            return new IdentityResult()
            {
                Success = false,
                ErrorMessages = new[] { "This Token doesn't match this JWT" }
            };
        }
    }
}
