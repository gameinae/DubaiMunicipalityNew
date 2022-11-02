//
// Copyright(c) 2020 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using UnityEngine;

namespace RealTimeWeather.Managers
{
    /// <summary>
    /// A class for performing operations on packages.
    /// </summary>
    public class PackageManager
    {
        /// <summary>
        /// Imports package at packagePath into the current project.
        /// </summary>
        /// <param name="packagePath">A string value that represents the package path.</param>
        /// <param name="interactive">A bool value depends on which the import package dialog is opened.</param>
        public static void ImportPackage(string packagePath, bool interactive)
        {
#if UNITY_EDITOR
            AssetDatabase.importPackageCompleted += ImportPackageCompleted;
            AssetDatabase.importPackageFailed += ImportPackageFailed;
            AssetDatabase.importPackageStarted += ImportPackageStarted;
            AssetDatabase.ImportPackage(packagePath, interactive);
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// Deletes a directory from a specified path.
        /// </summary>
        /// <param name="directoryPath">A string value that represents the path to the directory.</param>
        public static void DeleteDirectory(string directoryPath)
        {
#if UNITY_EDITOR
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
                AssetDatabase.Refresh();
            }
#endif
        }

        /// <summary>
        /// Callback raised whenever a package import successfully completes.
        /// </summary>
        /// <param name="packageName">A string value that represents the package name.</param>
        private static void ImportPackageCompleted(string packageName)
        {
#if UNITY_EDITOR
            Debug.Log("Import package completed: " + packageName);
            AssetDatabase.importPackageCompleted -= ImportPackageCompleted;
            AssetDatabase.importPackageFailed -= ImportPackageFailed;
#endif
        }

        /// <summary>
        /// Callback raised whenever a package import failed.
        /// </summary>
        /// <param name="packageName">A string value that represents the package name.</param>
        private static void ImportPackageFailed(string packageName, string errorMessage)
        {
#if UNITY_EDITOR
            Debug.Log("Import package failed: " + packageName);
            AssetDatabase.importPackageFailed -= ImportPackageFailed;
            AssetDatabase.importPackageCompleted -= ImportPackageCompleted;
#endif
        }

        /// <summary>
        /// Callback raised whenever a package import starts.
        /// </summary>
        /// <param name="packageName">A string value that represents the package name.</param>
        private static void ImportPackageStarted(string packageName)
        {
#if UNITY_EDITOR
            Debug.Log("Import package started: " + packageName);
            AssetDatabase.importPackageStarted -= ImportPackageStarted;
#endif
        }
    }
}
