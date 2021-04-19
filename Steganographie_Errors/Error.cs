using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Steganographie_Errors
{
    /// <summary>
    /// Contains detailed information about an error.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Meaningful title of the error
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Describes the error detailed
        /// </summary>
        public string ErrorDescription { get; private set; }
        /// <summary>
        /// Proposes a solution for the error
        /// </summary>
        public string ErrorSolution { get; private set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public bool IsErrorValid { get; internal set; }

        /// <summary>
        /// Create a new instance of the error object
        /// </summary>
        /// <param name="title">Meaningful title of the error</param>
        /// <param name="description">Describes the error detailed</param>
        /// <param name="solution">Proposes a solution for the error</param>
        public Error(string title, string description, string solution)
        {
            Title = title;
            ErrorDescription = description;
            ErrorSolution = solution;
        }

        public Error(string title, string description, string solution, string errorMessage)
            :this(title, description, solution)
        {
            ErrorMessage = errorMessage;
        }
    }
}
