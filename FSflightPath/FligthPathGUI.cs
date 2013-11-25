using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace FSflightPath
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FligthPathGUI : MonoBehaviour
    {        
        public FlightPathRecorder recorder = new FlightPathRecorder();
        //public List<FlightPathFollower> followers = new List<FlightPathFollower>();
        public List<FollowerObject> followerObjects = new List<FollowerObject>();
        public FollowerObject workPathFollower;
        public int selectedPath = 0;
        public Rect windowRect = new Rect(100f, 10f, 200f, 180f);
        public Rect windowRectLoad = new Rect(100f, 200f, 200f, 180f);
        public Vector2 buttonSize = new Vector2(90f, 20f);
        public Vector2 margin = new Vector2(10f, 5f);
        public float topMargin = 20f;
        private int elementCount = 0;
        private Vector2 windowSize = Vector2.zero;
        private Vector2 currentElementPos = Vector2.zero;
        public int GUIlayer = StaticValues.flightPathWindowLayer;
        public int GUIlayerLoad = StaticValues.loadWindowLayer;
        string status = "Standby";
        public GameObject pathHolder;
        public FlightPath path = new FlightPath();
        public bool showMenu = true;
        private bool showLoadMenu = false;
        private float hideMenuHintCountDown = 0f;
        private Vector2 furthestElement = Vector2.zero;
        private string[] files;
        private float loadMenuLastElementTop = 0f;
        private List<FollowerObject> followerObjectsDeletion = new List<FollowerObject>();

        //public string meshName = "FSflightPath/Models/targetPlane";
        //public GameObject model;
        //public int currentNodeNumber = 0;

        public FollowerObject createFollower(FlightPath inPath, string modelName, string displayName)
        {
            GameObject newObject = new GameObject();
            FollowerObject newFO = new FollowerObject();
            newFO.gameObject = newObject;
            //followerObjects.Add(newFO); handled in the gui load thing
            createModel(newFO, modelName, inPath);
            return newFO;
        }

        public void createModel(FollowerObject newFollowerObject, string modelName, FlightPath inPath) //FlightPathFollower follower,
        {            
            newFollowerObject.gameObject = GameDatabase.Instance.GetModel(modelName);
            if (newFollowerObject.gameObject == null)
            {
                Debug.Log("failed finding model" + modelName);
                return;
            }            
            newFollowerObject.follower.path = inPath;
            newFollowerObject.follower.followerObject = newFollowerObject;
            newFollowerObject.destroyFunction = destroyFollower;
                 
            Rigidbody newRigidBody = newFollowerObject.gameObject.AddComponent<Rigidbody>();            
            newRigidBody.mass = 2.0f;
            newRigidBody.drag = 0.05f;
            newRigidBody.isKinematic = true;
            newFollowerObject.follower.rbody = newRigidBody;            
            newFollowerObject.gameObject.SetActive(true);                                    
            newFollowerObject.follower.followerCollider = newFollowerObject.gameObject.GetComponentInChildren<MeshCollider>();            
        }

        private void newLine()
        {
            currentElementPos.y += buttonSize.y + margin.y;
            elementCount++;
            updateFurthestElement();
        }
        private void newColumn()
        {
            currentElementPos = new Vector2(currentElementPos.x + buttonSize.x + margin.x, topMargin);
            elementCount = 0;
            updateFurthestElement();
        }
        private Rect nextGUIpos
        {
            get
            {
                if (elementCount > 0)
                    currentElementPos.y += margin.y + buttonSize.y;
                else
                    elementCount++;
                updateFurthestElement();
                return new Rect(currentElementPos.x, currentElementPos.y, buttonSize.x, buttonSize.y);
            }
        }

        public void destroyFollower(FollowerObject target)
        {
            Debug.Log("Remove " + target.follower.path.pathName + " from list of " + followerObjects.Count);
            followerObjectsDeletion.Add(target);            
        }

        private void updateFurthestElement()
        {
            furthestElement.x = Mathf.Max(furthestElement.x, currentElementPos.x + buttonSize.x + margin.x);
            furthestElement.y = Mathf.Max(furthestElement.y, currentElementPos.y + buttonSize.y + margin.y);
        }

        public void Start()
        {
            pathHolder = new GameObject("pathHolder");
            //followers.Add(new FlightPathFollower());            
            recorder.path = path;
            //if (followerObjects.Count == 0) followerObjects.Add(createFollower(path, path.modelName, "new path"));            
        }

        private void openLoadMenu()
        {
            showLoadMenu = true;
            windowRectLoad = windowRect;
            windowRectLoad.y += windowRect.height + 10f;
            files = Directory.GetFiles(StaticValues.pathFolder, "*" + StaticValues.pathExtension);
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
                if (workPathFollower != null)
                    workPathFollower.follower.goOffRails(Vector3.zero);
            }
            if (GUI.Button(nextGUIpos, "Add node")) recorder.addCurrentNode();
            path.loops = GUI.Toggle(nextGUIpos, path.loops, "Loop path");
            path.goOffrailsAtEnd = GUI.Toggle(new Rect(margin.x, nextGUIpos.y, buttonSize.x * 2, buttonSize.y), path.goOffrailsAtEnd, "Off Rails at end");

            newLine();
            path.pathName = GUI.TextField(nextGUIpos, path.pathName);
            if (GUI.Button(nextGUIpos, "Save")) PathImportExport.exportPath(path);
            if (GUI.Button(nextGUIpos, "Load")) openLoadMenu();


            newColumn();
            if (GUI.Button(nextGUIpos, "Play"))
            {
                if (workPathFollower == null) workPathFollower = createFollower(path, path.modelName, "work path");
                workPathFollower.follower.startPlayback();
            }
            if (GUI.Button(nextGUIpos, "OffRails") && workPathFollower != null)
                workPathFollower.follower.goOffRails(Vector3.zero);
            /*if (GUI.Button(nextGUIpos, "Create Model"))
            {
                Debug.Log("Create Model pressed");
                if (followers[selectedPath].rbody == null)
                    createModel(followers[selectedPath], meshName);
            }*/
            GUI.Label(nextGUIpos, "Nodes: " + path.nodes.Count);

            windowSize = furthestElement;// + buttonSize + margin;

            if (GUI.Button(new Rect(windowRect.width - 18f, 2f, 16f, 16f), ""))
            {
                showMenu = false;
                hideMenuHintCountDown = 4f;
            }

            updateFurthestElement();

            GUI.DragWindow();
        }

        private void drawLoadWindow(int windowID)
        {
            loadMenuLastElementTop = topMargin;
            foreach (string pathFile in files)
            {
                string[] pathName = pathFile.Split('/');
                if (GUI.Button(new Rect(margin.x, loadMenuLastElementTop, buttonSize.x * 2 + margin.x, buttonSize.y), pathName[pathName.Length-1]))
                {
                    FlightPath newPath = PathImportExport.importPath(pathFile);
                    Debug.Log("imported path from text file");
                    followerObjects.Add(createFollower(newPath, newPath.modelName, newPath.pathName));
                    Debug.Log("added follower object, count: " + followerObjects.Count);
                    if (followerObjects.Count > 0)
                        followerObjects[followerObjects.Count - 1].follower.startPlayback();
                    showLoadMenu = false;
                    //newFollower.startPlayback();
                    //path = PathImportExport.importPath(pathFile);
                }
                loadMenuLastElementTop += buttonSize.y + margin.y;
            }

            if (GUI.Button(new Rect(margin.x, loadMenuLastElementTop + buttonSize.y, buttonSize.x, buttonSize.y), "Cancel"))
                showLoadMenu = false;
            windowRectLoad.height = loadMenuLastElementTop + buttonSize.y * 2 + margin.y;
            GUI.DragWindow();
        }

        void OnGUI()
        {
            if (showMenu)
            {
                if (recorder.recording) status = "REC";
                else status = "Standby";
                windowRect = GUI.Window(GUIlayer, new Rect(windowRect.x, windowRect.y, windowSize.x, windowSize.y + 2f), drawWindow, "Path Recorder: " + status);
            }
            if (showLoadMenu)
            {
                windowRectLoad = GUI.Window(GUIlayerLoad, windowRectLoad, drawLoadWindow, "Load Path");
            }
            else if (hideMenuHintCountDown > 0f)
            {
                GUI.Label(new Rect(windowRect.x, windowRect.y, buttonSize.x * 3, buttonSize.y * 2), "Press Alt+F6 to enable path menu");
            }
        }

        public void FixedUpdate()
        {
            recorder.FixedUpdate();            
            if (hideMenuHintCountDown > 0f) hideMenuHintCountDown -= Time.deltaTime;
            if (workPathFollower != null)
                workPathFollower.follower.FixedUpdate();
            foreach (FollowerObject fO in followerObjects)
            {
                fO.follower.FixedUpdate();
            }
            foreach (FollowerObject deleteObject in followerObjectsDeletion)
            {
                if (deleteObject != workPathFollower)
                {
                    Destroy(deleteObject.gameObject);
                    followerObjects.Remove(deleteObject);
                }
                else
                {
                    workPathFollower.follower.goOffRails(Vector3.zero);
                    Debug.Log("Can't delete work path follower");
                }
                Debug.Log("followerObjects now has " + followerObjects.Count);
            }
            followerObjectsDeletion.Clear();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F6) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.RightShift)))
                showMenu = !showMenu;
        }
    }
}


