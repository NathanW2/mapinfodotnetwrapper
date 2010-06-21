using Mapinfo.Wrapper.Core.Extensions;
using System;
using System.IO;

namespace Mapinfo.Wrapper.Core
{
    public static class Check
    {
        /// <summary>
        /// Checks if a file exists at the supplied location.
        /// </summary>
        /// <param name="path">The path of the file to check.</param>
        public static void FileExists(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("{0} could not be found.".FormatWith(path));
        }

        /// <summary>
        /// Checks if a files extension matches the one that is expected.
        /// </summary>
        /// <param name="path">The path to the file to check.</param>
        /// <param name="expected">The expected file extension.</param>
        public static void CorrectExtension(string path, string expected)
        {
            string extension = Path.GetExtension(path).ToLower();
            if (extension != expected.ToLower())
                throw new ArgumentException("Expected file with extension {1} but was {0}".FormatWith(extension,expected));
        }
    }
}