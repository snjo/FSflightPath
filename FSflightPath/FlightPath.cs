using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace FSflightPath
{
    public class FlightPath
    {
        public string pathName = "unnamed path"; //string.Empty;        
        public string modelName = "FSflightPath/Models/targetPlane";
        public bool loadCraft = false;
        public List<FlightPathNode> nodes = new List<FlightPathNode>();
        public bool loops = false;
        public int currentNodeNumber = 0;
        public float positionDeltaTolerance = 500f;
        public float positionMinDeltaTolerance = 5f;
        public float orientationDeltaTolerance = 15f;
        public float speedDeltaTolerance = 20f;
        public float velocityVectorDeltaTolerance = 10f;
        public float minTimeDelta = 0.7f;
        public bool goOffrailsAtEnd = true;
        private string output = string.Empty;

        public FlightPathNode Next()
        {
            if (currentNodeNumber >= nodes.Count - 1)
            {
                if (loops)
                {
                    currentNodeNumber = 0;
                }
                else
                {
                    //currentNodeNumber = nodes.Count - 1;				
                }
            }
            else
            {
                currentNodeNumber++;
            }
            if (currentNodeNumber < 0 || nodes.Count < 1) return new FlightPathNode();
            return nodes[currentNodeNumber];
        }

        public FlightPathNode nextNode
        {
            get
            {
                int returnedNodeNumber = 0;
                if (currentNodeNumber < nodes.Count - 1)
                    returnedNodeNumber = currentNodeNumber + 1;
                else
                {
                    if (loops)
                        returnedNodeNumber = 0;
                    else
                        returnedNodeNumber = currentNodeNumber;
                }

                if (currentNodeNumber < nodes.Count)
                    return nodes[returnedNodeNumber];
                else
                    return new FlightPathNode();
            }
        }

        public FlightPathNode lastNode
        {
            get
            {
                if (currentNodeNumber < nodes.Count)
                    return nodes[nodes.Count - 1];
                else
                    return currentNode;
            }
        }

        public int lastNodeNumber
        {
            get
            {
                if (nodes.Count > 0)
                    return nodes.Count - 1;
                else
                    return 0;
            }
        }

        public FlightPathNode currentNode
        {
            get
            {
                if (currentNodeNumber < nodes.Count)
                    return nodes[currentNodeNumber];
                else
                    return new FlightPathNode();
            }
        }

        private void addOutputLine(string newValue)
        {
            output += newValue + "\n";
        }
        private void addOutputLine(Vector3 newValue)
        {
            output += newValue.x + ";" + newValue.y + ";" + newValue.z + "\n";
        }
        private void addOutputLine(Quaternion newValue)
        {
            output += newValue.x + ";" + newValue.y + ";" + newValue.z + ";" + newValue.w + "\n";
        }

        private Vector3 parseVector3(string inString)
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

        private Quaternion parseQuaternion(string inString)
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

        public string serialize()
        {
            output = pathName + "\n";
            addOutputLine(loops.ToString());
            addOutputLine(modelName);
            addOutputLine(goOffrailsAtEnd.ToString());
            addOutputLine(loadCraft.ToString());
            addOutputLine("reserved line 3");
            addOutputLine("reserved line 4");
            addOutputLine("reserved line 5");
            addOutputLine("reserved line 6");            
            foreach (FlightPathNode node in nodes)
            {
                addOutputLine(node.time.ToString());
                addOutputLine(node.position);
                addOutputLine(node.rotation);
                addOutputLine(node.velocity);
            }
            return output;
        }

        public void parseStream(StreamReader stream)
        {
            nodes = new List<FlightPathNode>();
            try
            {
                pathName = stream.ReadLine();
                bool.TryParse(stream.ReadLine(), out loops);
                modelName = stream.ReadLine();
                bool.TryParse(stream.ReadLine(), out goOffrailsAtEnd);
                bool.TryParse(stream.ReadLine(), out loadCraft);
                stream.ReadLine(); // reserved line 3
                stream.ReadLine(); // reserved line 4
                stream.ReadLine(); // reserved line 5
                stream.ReadLine(); // reserved line 6
                while (!stream.EndOfStream)
                {
                    FlightPathNode newNode = new FlightPathNode();
                    float.TryParse(stream.ReadLine(), out newNode.time);
                    newNode.position = parseVector3(stream.ReadLine());
                    newNode.rotation = parseQuaternion(stream.ReadLine());
                    newNode.velocity = parseVector3(stream.ReadLine());
                    nodes.Add(newNode);
                }
            }
            catch (Exception e)
            {
                Debug.Log("importPath error: " + e.ToString());
            }
        }
    }
}
