namespace MapinfoWrapper.Core
{
    using System;
    using System.IO;
    using Extensions;

    public static class Check
    {
        public static void FileExists(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("{0} could not be found, please check the path of the table.".FormatWith(path));
        }

        public static void CorrectExtension(string path, string expected)
        {
            string extension = Path.GetExtension(path).ToLower();
            if (extension != expected.ToLower())
                throw new ArgumentException("Expected file with extension .tab but was {0}".FormatWith(extension));
        }
    }
}