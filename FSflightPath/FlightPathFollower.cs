using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    public class FlightPathFollower// : MonoBehaviour
    {
        public FlightPath path;
        public bool playback = false;
        public float countDown = 0f;
        public float progress = 0f;
        public Rigidbody rbody;
        public Collider followerCollider;
        public GameObject parentObject;
        //public bool goOffrailsAtEnd = true;
        public FollowerObject followerObject;

        public void goOffRails(Vector3 angularVelocity)
        {
            playback = false;
            if (rbody == null) return;
            rbody.isKinematic = false;            
            if (path.nodes.Count > 0)
                rbody.velocity = Vector3.Lerp(path.nextNode.velocity, path.currentNode.velocity, progress);
            rbody.angularVelocity = angularVelocity;
        }

        public void startPlayback()
        {
            if (rbody == null)
            {
                Debug.Log("FPF sP: no rbody");
                return;
            }
            if (path == null)
            {
                Debug.Log("FPF sP: no path");
                return;
            }
            if (path.nodes.Count <= 0)
            {
                Debug.Log("No path nodes in path " + path.pathName);
                return;
            }            
            rbody.isKinematic = true;            
            playback = true;
            path.currentNodeNumber = 0;
            countDown = path.nextNode.time;
            rbody.transform.position = path.currentNode.position + FlightGlobals.ActiveVessel.mainBody.position;
            rbody.transform.rotation = path.currentNode.rotation;
            if (followerCollider != null)
            {                
                //followerCollider.isTrigger = false;
            }
        }

        public void FixedUpdate()
        {
            if (playback)
            {
                if (path.nodes.Count < 1) return;
                countDown -= Time.deltaTime;
                if (countDown < 0f)
                {
                    path.Next();
                    countDown = path.nextNode.time;
                }
                progress = countDown / path.nextNode.time;
                //Debug.Log("cur " + path.currentNodeNumber + " / " + progress);
                if (!path.loops && path.currentNodeNumber > 0)
                {
                    if (path.currentNodeNumber >= path.lastNodeNumber)
                    {
                        if (path.goOffrailsAtEnd)
                            goOffRails(Vector3.zero);
                        else
                            followerObject.Destroy();
                    }
                }
                rbody.transform.position = Vector3.Lerp(path.nextNode.position + FlightGlobals.ActiveVessel.mainBody.position, path.currentNode.position + FlightGlobals.ActiveVessel.mainBody.position, progress);
                rbody.transform.rotation = Quaternion.Lerp(path.nextNode.rotation, path.currentNode.rotation, progress);
            }
        }
    }
}
