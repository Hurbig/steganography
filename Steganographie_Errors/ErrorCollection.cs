using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganographie_Errors
{
    /// <summary>
    /// Returns you an error to display or log
    /// </summary>
    public static class ErrorCollection
    {
        private const string _PleaseReportBug = "Please report the bug to a developer.";
        private static readonly Dictionary<int, Error> _Errors = new Dictionary<int, Error>
                                                                 {
                                                                     {1, new Error("Null-Reference", "The object you wanted to use was empty.", _PleaseReportBug)},
                                                                     {2, new Error("Incomplete Information", "The information you added is not complete.", "Please type in all required information.") },
                                                                     {3, new Error("Something went wrong", "Something went wrong in this process", "If you can reproduce this, please report it to a developer. Otherwise simply try it again.") },
                                                                     {4, new Error("Invalid Path", "The path you entered is invalid or is not an image.", "Please enter a valid path to an image file.") },
                                                                     {5, new Error("Invalid Path", "The path you entered to open the picture is the same as the path to save the picture", "Enter two different pathes to encrypt the picture correctly") },
                                                                     {6, new Error("Out of Memory", "A system out of memory exception occured.", "Use a smaller sized image or less text.") }
                                                                 };
        /// <summary>
        /// Returns the error with the information to the number
        /// </summary>
        /// <param name="errorNumber">Number of the error</param>
        /// <returns></returns>
        public static Error GetError(int errorNumber)
        {
            if(_Errors.ContainsKey(errorNumber))
            {
                _Errors[errorNumber].IsErrorValid = true;
                return _Errors[errorNumber];
            }
                
            return new Error("Error not found", "No error description found", "No solution for this error.");
        }
    }
}
