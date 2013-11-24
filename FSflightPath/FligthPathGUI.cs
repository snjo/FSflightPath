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
        public Vector2 buttonSize = new Vector2(90f, 20f);
        public Vector2 margin = new Vector2(10f, 5f);
        public float topMargin = 20f;
        private int elementCount = 0;
        private Vector2 windowSize = Vector2.zero;
        private Vector2 currentElementPos = Vector2.zero;
        public int GUIlayer = 2;
        string status = "Standby";
        public GameObject pathHolder;
        public FlightPath path = new FlightPath();
        public bool showMenu = true;
        private float hideMenuHintCountDown = 0f;

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

        private void newLine()
        {
            currentElementPos.y += buttonSize.y + margin.y;
            elementCount++;
        }
        private void newColumn()
        {
            currentElementPos = new Vector2(currentElementPos.x + buttonSize.x + margin.x, topMargin);
            elementCount = 0;
        }
        private Rect nextGUIpos
        {
            get
            {
                if (elementCount > 0)
                    currentElementPos.y += margin.y + buttonSize.y;
                else
                    elementCount++;
                return new Rect(currentElementPos.x, currentElementPos.y, buttonSize.x, buttonSize.y);
            }
        }

        public void Start()
        {
            pathHolder = new GameObject("pathHolder");
            recorder.path = path;            
        }

        private void drawWindow(int windowID)
        {            
            currentElementPos = new Vector2(margin.x, topMargin);
            elementCount = 0;
            if (GUI.Button(nextGUIpos, "Record")) recorder.startRecording();
            if (GUI.Button(nextGUIpos, "Stop")) recorder.stopRecording();
            if (GUI.Button(nextGUIpos, "Clear"))
            {                
                recorder.clearPath();
                follower.goOffRails(Vector3.zero);
            }
            if (GUI.Button(nextGUIpos, "Add node")) recorder.addCurrentNode();

            newColumn();
            if (GUI.Button(nextGUIpos, "Play"))
            {
                if (follower.rbody == null) createModel(meshName);
                follower.startPlayback();                
            }
            if (GUI.Button(nextGUIpos, "OffRails")) follower.goOffRails(Vector3.zero);
            if (GUI.Button(nextGUIpos, "Create Model"))
            {
                Debug.Log("Create Model pressed");
                if (follower.rbody == null)
                    createModel(meshName);
            }
            GUI.Label(nextGUIpos, "Nodes: " + path.nodes.Count);

            windowSize = currentElementPos + buttonSize + margin;            

            if (GUI.Button(new Rect(windowRect.width - 18f, 2f, 16f, 16f), ""))
            {
                showMenu = false;
                hideMenuHintCountDown = 4f;
            }


            GUI.DragWindow();
        }

        void OnGUI()
        {
            if (showMenu)
            {
                if (recorder.recording) status = "REC";
                else status = "Standby";
                windowRect = GUI.Window(GUIlayer, new Rect(windowRect.x, windowRect.y, windowSize.x, windowSize.y + 2f), drawWindow, recorder.ID + " Path Recorder: " + status);
            }
            else if (hideMenuHintCountDown > 0f)
            {
                GUI.Label(new Rect(windowRect.x, windowRect.y, buttonSize.x*3, buttonSize.y*2), "Press F12 to enable path menu");
            }
        }

        public void FixedUpdate()
        {
            recorder.FixedUpdate();
            if (hideMenuHintCountDown > 0f) hideMenuHintCountDown -= Time.deltaTime;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12) && !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
                showMenu = !showMenu;
        }
    }
}
