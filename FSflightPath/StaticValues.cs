using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSflightPath
{
    public static class StaticValues
    {
        public static string rootFolder = KSPUtil.ApplicationRootPath.Replace("\\", "/");
        public static string pluginFolder = rootFolder + "GameData/FSflightPath/";
        public static string pathFolder = pluginFolder + "Paths/";
        public static string pathExtension = ".path";

        // GUI window layers
        public static int flightPathWindowLayer = 5916; // 5 reserved in Firespitter list
        public static int loadWindowLayer = 5917;
    }
}
