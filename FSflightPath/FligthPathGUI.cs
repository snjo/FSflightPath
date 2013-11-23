using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FligthPathGUI : MonoBehaviour
    {
        public FlightPathRecorder recorder = new FlightPathRecorder();
        public FlightPathFollower follower = new FlightPathFollower();
        public Rect windowRect = new Rect(100f, 10f, 200f, 180f);
        public int GUIlayer = 2;
        string status = "Standby";
        public GameObject pathHolder;
        public FlightPath path = new FlightPath();

        public string meshName = "FSflightPath/models/targetPlane";
        public GameObject model;
        //public int currentNodeNumber = 0;

        public void createModel(string modelName)
        {
            Debug.Log("Finding model");
            model = GameDatabase.Instance.GetModel(meshName);
            //model.transform.position = part.transform.position + new Vector3(0f, 2f, 2f);
            Debug.Log("adding RB");
            Rigidbody newRigidBody = model.AddComponent<Rigidbody>();
            Debug.Log("RB values");
            newRigidBody.mass = 2.0f;
            newRigidBody.drag = 0.05f;
            newRigidBody.isKinematic = true;            
            
            Debug.Log("set active");
            model.SetActive(true);
            Debug.Log("add follower");
            follower = model.AddComponent<FlightPathFollower>();
            Debug.Log("edit follower");
            follower.path = path;
            follower.rbody = newRigidBody;
            Debug.Log("disable collider");
            follower.collider = model.GetComponentInChildren<MeshCollider>();
            //rbody = newRigidBody;
            //newRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            //FSstaticMeshCollisionHandler colliderHandler = model.AddComponent<FSstaticMeshCollisionHandler>();
            //colliderHandler.thisCollider = model.collider;
            //model.AddComponent<physicalObject>();
            //db.debugMessage("FS: mesh pos == " + model.transform.position);
            //db.debugMessage("FS: part pos == " + part.transform.position);
        }

        public void Start()
        {
            pathHolder = new GameObject("pathHolder");
            recorder.path = path;            
        }

        private void drawWindow(int windowID)
        {
            if (GUI.Button(new Rect(5f, 20f, 60f, 17f), "Record")) recorder.startRecording();
            if (GUI.Button(new Rect(5f, 50f, 60f, 17f), "Stop")) recorder.stopRecording();
            if (GUI.Button(new Rect(5f, 80f, 60f, 17f), "Clear"))
            {                
                recorder.clearPath();
                follower.goOffRails(Vector3.zero);
            }
            if (GUI.Button(new Rect(70f, 20f, 60f, 17f), "Play")) follower.startPlayback();
            if (GUI.Button(new Rect(70f, 50f, 60f, 17f), "OffRails")) follower.goOffRails(new Vector3(0f, 0.1f, 0f));
            if (GUI.Button(new Rect(70f, 80f, 60f, 17f), "Create Model"))
            {
                Debug.Log("Create Model pressed");
                if (follower.rbody == null)
                    createModel(meshName);
            }
            
            GUI.Label(new Rect(5f, 110f, 180f, 20f), "Nodes: " + path.nodes.Count);

            //if (recorder.rbody != null)
            //{
            //    GUI.Label(new Rect(5f, 110f, 300f, 20f), "RL: " + recorder.rbody.position);
            //    GUI.Label(new Rect(5f, 130f, 300f, 20f), "RG: " + (recorder.rbody.position - FlightGlobals.ActiveVessel.mainBody.transform.position));
            //}
            //if (follower.rbody != null)
            //{
            //    GUI.Label(new Rect(5f, 140f, 300f, 20f), "FG: " + (follower.rbody.position - FlightGlobals.ActiveVessel.mainBody.transform.position));
            //}
            GUI.DragWindow();
        }

        void OnGUI()
        {
            if (recorder.recording) status = "REC";
            else status = "Standby";
            windowRect = GUI.Window(GUIlayer, windowRect, drawWindow, recorder.ID + " Path Recorder: " + status);
        }

        public void FixedUpdate()
        {
            recorder.FixedUpdate();
        }

        public void Update()
        {
            
        }
    }
}
