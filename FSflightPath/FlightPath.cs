using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    public class FlightPath
    {
        public string pathName = string.Empty;
        public List<FlightPathNode> nodes = new List<FlightPathNode>();
        public bool loops = false;
        public int currentNodeNumber = 0;
        public float positionDeltaTolerance = 500f;
        public float positionMinDeltaTolerance = 5f;
        public float orientationDeltaTolerance = 15f;
        public float speedDeltaTolerance = 20f;
        public float velocityVectorDeltaTolerance = 10f;
        public float minTimeDelta = 0.7f;

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
    }
}
