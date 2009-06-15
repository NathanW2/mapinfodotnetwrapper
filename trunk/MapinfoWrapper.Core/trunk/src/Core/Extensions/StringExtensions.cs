using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace MapinfoWrapper.Core.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Wraps a string in quotes.
        /// </summary>
        /// <param name="value">The string to wrap in quotes.</param>
        /// <returns>A string wrapped in double quotes.</returns>
        [DebuggerStepThrough]
        public static string InQuotes(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("Value");

            return "\"" + value + "\"";
        }

        /// <summary>
        /// Formats a string with a list of literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="args">The argument list</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        /// <summary>
        /// Formats a string with one literal placeholder.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string text, object arg0)
        {
            return string.Format(text, arg0);
        }

        /// <summary>
        /// Formats a string with two literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <param name="arg1">Argument 1</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string text, object arg0, object arg1)
        {
            return string.Format(text, arg0, arg1);
        }

        /// <summary>
        /// Formats a string with tree literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <returns>The formatted string</returns>
        [DebuggerStepThrough]
        public static string FormatWith(this string text, object arg0, object arg1, object arg2)
        {
            return string.Format(text, arg0, arg1, arg2);
        }
    }
}
