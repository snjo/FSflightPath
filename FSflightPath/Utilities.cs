using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    public static class Utilities
    {
        public static string rootFolder = KSPUtil.ApplicationRootPath.Replace("\\", "/");        
        public static string pluginFolder = rootFolder + "GameData/FSflightPath/";
        public static string pathFolder = pluginFolder + "Paths/";
        public static string CraftPath = pluginFolder + "Craft/";
        public static string craftExtension = ".crf";
        public static string pathExtension = ".path";
        public static string fallbackModelName = "FSflightPath/Models/targetPlane";
        public static int craftFileFormat = 1;

        // GUI window layers
        public static int flightPathWindowLayer = 5916; // 5 reserved in Firespitter list
        public static int loadWindowLayer = 5917;

        public static string Vector3ToString(Vector3 inValue)
        {
            return (String.Concat(inValue.x.ToString(), ";", inValue.y.ToString(), ";", inValue.z.ToString()));
        }

        public static string QuaternionToString(Quaternion inValue)
        {
            return (String.Concat(inValue.x.ToString(), ";", inValue.y.ToString(), ";", inValue.z.ToString(), ";", inValue.w.ToString()));
        }

        public static Quaternion parseQuaternion(string inString)
        {
            string[] floatStrings = inString.Split(';');
            Quaternion result = new Quaternion();
            if (floatStrings.Length == 4)
            {
                float.TryParse(floatStrings[0], out result.x);
                float.TryParse(floatStrings[1], out result.y);
                float.TryParse(floatStrings[2], out result.z);
                float.TryParse(floatStrings[3], out result.w);
            }
            else
            {
                Debug.Log("Couldn't parse " + inString + " to Quaternion");
            }
            return result;
        }

        public static Vector3 parseVector3(string inString)
        {
            string[] floatStrings = inString.Split(';');
            Vector3 result = Vector3.zero;
            if (floatStrings.Length == 3)
            {
                float.TryParse(floatStrings[0], out result.x);
                float.TryParse(floatStrings[1], out result.y);
                float.TryParse(floatStrings[2], out result.z);
            }
            else
            {
                Debug.Log("Couldn't parse " + inString + " to Vector3");
            }
            return result;
        }
    }
}
