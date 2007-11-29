using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace LocalMiniTrayApp
{

    public static class IconManager
    {
        //The dictionary that actually stores the icons
        private static Dictionary<string, Icon> iconsdict;

        /// Must be immeadiately called, initializes the dictionary
        public static void Initialize()
        {
            iconsdict = new Dictionary<string, Icon>();
        }

        /// Allows the addition of a custom icon and specific key
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

        /// Add an icon associated with a file
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

        /// Returns an icon associated with a file
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
