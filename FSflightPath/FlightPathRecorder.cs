using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    public class FlightPathRecorder : MonoBehaviour
    {
        [KSPField]
        public string ID = "generic";
        public FlightPath path;
        public bool recording = false;
        public Rigidbody rbody;
        private FlightPathNode newNode = new FlightPathNode();
        //private int currentNodeNumber = 0;
        public GameObject pathMarker;
        public float timeElapsed = 0f;
        // Use this for initialization
        void Start()
        {
            rbody = rigidbody;
        }

        private bool sufficientChange()
        {
            if (Vector3.Distance(path.lastNode.position, newNode.position) < path.positionMinDeltaTolerance || timeElapsed < path.minTimeDelta)
            {
                return false;
            }
            if (Quaternion.Angle(path.lastNode.rotation, newNode.rotation) > path.orientationDeltaTolerance)
            {
                //Debug.Log("orientation fulfilled");
                return true;
            }
            if (Vector3.Angle(path.lastNode.velocity.normalized, newNode.velocity.normalized) > path.velocityVectorDeltaTolerance)
            {
                //Debug.Log("velocity fulfilled");
                return true;
            }
            if (Mathf.Abs(path.lastNode.speed - newNode.speed) > path.speedDeltaTolerance)
            {
                //Debug.Log("speed fulfilled");
                return true;
            }
            if (Vector3.Distance(path.lastNode.position, newNode.position) > path.positionDeltaTolerance)
            {
                //Debug.Log("maxDist fulfilled");
                return true;
            }
            return false;
        }

        public void addCurrentNode()
        {
            path.nodes.Add(new FlightPathNode(newNode));
            //Instantiate(pathMarker, newNode.position, newNode.rotation);
            timeElapsed = 0f;
            Debug.Log("add node");
        }

        public void startRecording()
        {
            Debug.Log("Starting recording");
            rbody = FlightGlobals.ActiveVessel.rigidbody;
            recording = true;
        }

        public void stopRecording()
        {
            Debug.Log("Stopping recording");
            recording = false;
            if (path.nodes.Count > 0)
            {
                float loopDistance = (float)Vector3d.Distance(path.nodes[0].position, path.lastNode.position);
                float timeUsed = loopDistance / path.lastNode.speed;
                Debug.Log("timeToLoop: " + timeUsed);
                path.nodes[0].time = timeUsed;
            }
        }

        public void clearPath()
        {
            path.nodes = new List<FlightPathNode>();
            timeElapsed = 0f;
            updateNewNode();
            addCurrentNode();
            Debug.Log("clear");
        }

        private void updateNewNode()
        {
            timeElapsed += Time.deltaTime;
            newNode.position = rbody.position - FlightGlobals.ActiveVessel.mainBody.position;
            newNode.rotation = rbody.rotation;
            newNode.velocity = rbody.velocity;
            newNode.time = timeElapsed;
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            //Debug.Log("rec FU " + Time.deltaTime);
            if (recording)
            {

                updateNewNode();

                if (path.nodes.Count < 1)
                {
                    Debug.Log("add currentNode as first");
                    addCurrentNode();
                }

                if (sufficientChange())
                {
                    //Debug.Log("sufficientChange");
                    addCurrentNode();
                }
                //Debug.Log("nodes: " + path.nodes.Count + " / newNode " + path.lastNode.position);
            }

        }       
    }
}
