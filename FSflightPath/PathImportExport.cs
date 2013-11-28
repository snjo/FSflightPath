using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace FSflightPath
{
    public static class PathImportExport
    {
        public static void exportPath(FlightPath path)
        {            
            Debug.Log("FSflightPath: Saving path " + path.pathName);
            StreamWriter writer = new StreamWriter(Utilities.pathFolder + path.pathName + Utilities.pathExtension);            
            writer.WriteLine(path.serialize());
            writer.Close();            
        }

        public static FlightPath importPath(string fileName)
        {
            Debug.Log("Importing path " + fileName);
            FlightPath newPath = new FlightPath();
            StreamReader reader = new StreamReader(fileName);

            newPath.parseStream(reader);

            return newPath;
        }
    }
}
