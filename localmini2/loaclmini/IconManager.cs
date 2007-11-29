using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace LocalMiniTrayApp
{
    /// <summary>
    /// Static Class IconManager, a wrapper around a dictionary to hold file Icons.
    /// Written in C# by Dominick O'Dierno 
    /// odiernod@gmail.com 
    /// August 2007 
    /// for use with the DOMercury Application Launcher, http://www.odierno.com/DOMercury
    /// </summary>
    public static class IconManager
    {
        //The dictionary that actually stores the icons
        private static Dictionary<string, Icon> iconsdict;

        /// <summary>
        /// Must be immeadiately called, initializes the dictionary
        /// </summary>
        public static void Initialize()
        {
            iconsdict = new Dictionary<string, Icon>();
        }

        /// <summary>
        /// Allows the addition of a custom icon and specific key
        /// </summary>
        /// <param name="key">key to be used to retrieve this specific icon</param>
        /// <param name="icon">the icon to be retrieved</param>
        public static void AddCustomIcon(string key, Icon icon)
        {
            //Check if key already exists, if not, add key, icon pair
            if (!iconsdict.ContainsKey(key))
            {
                try
                {
                    iconsdict.Add(key, icon);
                }
                catch
                {
                    //Do Nothing
                }
            }
        }

        /// <summary>
        /// Add an icon associated with a file
        /// </summary>
        /// <param name="filename">the path to the file with which to add the icon</param>
        public static void AddIcon(string filename)
        {
            //Create a key from the filename
            string key = GetKey(filename);
            //If the key does not exist, add the icon to the repository
            if (!iconsdict.ContainsKey(key))
            {
                try
                {
                    //Call ExtractAssociatedIcon, the function that makes all the magic happen
                    iconsdict.Add(key, Icon.ExtractAssociatedIcon(filename));
                }
                catch
                {
                    //Do Nothing
                }
            }
        }

        /// <summary>
        /// Returns an icon associated with a file
        /// </summary>
        /// <param name="filename">the filename to match the icon</param>
        /// <returns>The icon that matches the file type</returns>
        public static Icon GetIcon(string filename)
        {
            Icon tmp;
            iconsdict.TryGetValue(GetKey(filename), out tmp);
            return tmp;
        }

        /// <summary>
        /// Generates a key based on the file name to use to store with the icon.  A key generation function is used 
        /// so that one icon which is used for multiple files of the same type (such as word documents) will only be
        /// stored once.
        /// </summary>
        /// <param name="path">the filename from which a key is being generated</param>
        /// <returns></returns>
        private static string GetKey(string path)
        {
            //if path is a directory, return the key as "folder"
            if (Directory.Exists(path))
            {
                return "folder";
            }
            //If path is an existing file, return the extension as the key
            else if (File.Exists(path))
            {
                string ext = Path.GetExtension(path).ToLower();
                //.exe, .lnk, .ico, and .icn often have individual icons even though they are the same file type
                //Therefore they must be treated specially
                if (ext == ".exe" || ext == ".lnk" || ext == ".ico" || ext == ".icn")
                {
                    //return the specific path as the key
                    return Path.GetFileName(path);
                }
                else
                {
                    return ext;
                }
            }
            //If the path is no known file or folder, return itself as the key
            else return path;
        }

    }

}
