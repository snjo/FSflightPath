using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    class FlightPathFollower : MonoBehaviour
    {
        public FlightPath path;
        public bool playback = false;
        public float countDown = 0f;
        public float progress = 0f;
        //public int currentNodeNumber = 0;

        public void goOffRails(Vector3 angularVelocity)
        {
            rigidbody.isKinematic = false;
            playback = false;
            if (path.nodes.Count > 0)
                rigidbody.velocity = Vector3.Lerp(path.nextNode.velocity, path.currentNode.velocity, progress);
            rigidbody.angularVelocity = angularVelocity;
        }

        public void startPlayback()
        {
            if (path.nodes.Count <= 0) return;
            rigidbody.isKinematic = true;
            playback = true;
            path.currentNodeNumber = 0;
            countDown = path.nextNode.time;
            transform.position = path.currentNode.position;
            transform.rotation = path.currentNode.rotation;
        }

        // Use this for initialization
        void Start()
        {
            rigidbody.isKinematic = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                startPlayback();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                goOffRails(new Vector3(0.1f, 1f, 0.5f));
            }
        }

        void FixedUpdate()
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
                transform.position = Vector3.Lerp(path.nextNode.position, path.currentNode.position, progress);
                transform.rotation = Quaternion.Lerp(path.nextNode.rotation, path.currentNode.rotation, progress);
            }
        }
    }
}
