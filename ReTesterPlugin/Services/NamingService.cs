﻿using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace ReTesterPlugin.Services
{
    public static class NamingService
    {
        /// <summary>
        /// The regex for matching a test project name.
        /// </summary>
        private static readonly Regex _testProjectRegex = new Regex("^(.*)Tests$");

        /// <summary>
        /// Converts a project name into the test project.
        /// </summary>
        public static string ProjectToTestProject([NotNull] string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            return isTestProjectName(pName)
                ? pName
                : string.Format("{0}Tests", pName);
        }

        /// <summary>
        /// Converts a test project name into the source project name.
        /// </summary>
        public static string TestProjectToProject([NotNull] string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            if (!isTestProjectName(pName))
            {
                return pName;
            }

            Match m = _testProjectRegex.Match(pName);
            return m.Groups[0].Value;
        }

        /// <summary>
        /// True if the project name follows a test project naming convention.
        /// </summary>
        public static bool isTestProjectName(string pName)
        {
            return _testProjectRegex.IsMatch(pName);
        }
    }
}