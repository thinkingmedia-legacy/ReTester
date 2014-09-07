using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ReTester.Annotations;

namespace ReTester
{
    /// <summary>
    /// A test running tool.
    /// </summary>
    public static class ReTest
    {
        /// <summary>
        /// Returns a list of files matching the pattern relative to the project source folder.
        /// </summary>
        public static IEnumerable<string> Files([NotNull] string pPath)
        {
            if (pPath == null)
            {
                throw new ArgumentNullException("pPath");
            }

            DirectoryInfo info = Directory.GetParent(Directory.GetCurrentDirectory());
            string parent = info.Parent == null
                ? info.FullName
                : info.Parent.FullName;

            string project = parent + @"\" + pPath;
            if (File.Exists(project))
            {
                return new[] {project};
            }

            Match m = Regex.Match(pPath, @"^(?<path>.*)(?<pattern>\*\..*)$");
            string path = m.Groups["path"].Success ? m.Groups["path"].Value : "";
            string pattern = m.Groups["pattern"].Success ? m.Groups["pattern"].Value : null;

            project = parent + @"\" + path;

            return pattern == null
                ? Directory.EnumerateFiles(project)
                : Directory.EnumerateFiles(project, pattern);
        }

        /// <summary>
        /// Starts a chainable description of what a test does.
        /// </summary>
        /// <param name="pMessage">A description of what the test does.</param>
        /// <returns>The thing it does.</returns>
        public static DoesThis It([NotNull] string pMessage)
        {
            if (pMessage == null)
            {
                throw new ArgumentNullException("pMessage");
            }

            return new DoesThis(pMessage);
        }

        /// <summary>
        /// Opens and reads all files as text in the path.
        /// </summary>
        public static IEnumerable<string> Open([NotNull] string pPath)
        {
            if (pPath == null)
            {
                throw new ArgumentNullException("pPath");
            }

            return from file in Files(pPath) select File.ReadAllText(file);
        }
    }
}