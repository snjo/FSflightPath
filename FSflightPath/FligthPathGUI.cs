using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    class FligthPathGUI
    {
        public FlightPathRecorder recorder;
        public FlightPathFollower follower;
        public Rect windowRect = new Rect(100f, 10f, 200f, 100f);
        public int GUIlayer = 2;
        string status = "Standby";

        private void drawWindow(int windowID)
        {
            if (GUI.Button(new Rect(5f, 20f, 60f, 17f), "Record")) recorder.startRecording();
            if (GUI.Button(new Rect(5f, 50f, 60f, 17f), "Stop")) recorder.stopRecording();
            if (GUI.Button(new Rect(5f, 80f, 60f, 17f), "Clear"))
            {
                follower.goOffRails(Vector3.zero);
                recorder.clearPath();
            }
            if (GUI.Button(new Rect(70f, 20f, 60f, 17f), "Play")) follower.startPlayback();
            if (GUI.Button(new Rect(70f, 50f, 60f, 17f), "OffRails")) follower.goOffRails(new Vector3(0f, 0.1f, 0f));
            GUI.DragWindow();
        }

        void OnGUI()
        {
            if (recorder.recording) status = "REC";
            else status = "Standby";
            windowRect = GUI.Window(GUIlayer, windowRect, drawWindow, "Path Recorder: " + status);
        }
    }
}
