using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    public static class CraftLoader
    {
        public static List<PartValue> getParts(Vessel vessel)
        {
            List<PartValue> partList = new List<PartValue>();
            Vector3 rootPosition;
            Quaternion rootRotation;
            Transform referenceFrame = new GameObject().transform;
            Transform localTransform = new GameObject().transform;
            localTransform.parent = referenceFrame;
            if (vessel != null)
            {
                if (vessel.parts.Count > 0)
                {
                    Debug.Log("vessel parts: " + vessel.parts.Count);
                    rootPosition = vessel.parts[0].transform.position;
                    rootRotation = vessel.parts[0].transform.rotation; 
                    Quaternion worldUp = Quaternion.Euler((vessel.rigidbody.position - vessel.mainBody.position).normalized);
                    referenceFrame.rotation = vessel.transform.rotation;
                    foreach (Part part in vessel.parts)
                    {
                        PartValue newPartValue = new PartValue();
                        newPartValue.position = part.transform.position - rootPosition;                        
                        newPartValue.scale = part.scaleFactor;                        
                        localTransform.rotation = part.transform.rotation;
                        newPartValue.rotation = part.transform.localRotation;
                        newPartValue.partName = part.name.Split(' ')[0];
                        newPartValue.model = findPartModel(newPartValue.partName).model;
                        partList.Add(newPartValue);
                    }
                    Debug.Log("partList count: " + partList.Count);
                }
            }
            return partList;
        }

        public static GameObject assembleCraft(string craftName)
        {
            GameObject craft = new GameObject();
            List<PartValue> pvList = getParts(FlightGlobals.ActiveVessel);
            foreach (PartValue pv in pvList)
            {
                Debug.Log("Part: " + pv.partName);
                pv.model.SetActive(true);
                pv.model.transform.parent = craft.transform;
                pv.model.transform.localPosition = pv.position;
                pv.model.transform.localRotation = pv.rotation;
                //pv.model.transform.localScale = new Vector3(pv.scale, pv.scale, pv.scale);
                //Debug.Log("Part: " + pv.position);
                //Debug.Log("Part: " + pv.rotation);
                //Debug.Log("Part: " + pv.scale);
            }
            return craft;
        }

        public static PartValue findPartModel(string partName)
        {
            UrlDir.UrlConfig[] cfg = GameDatabase.Instance.GetConfigs("PART");
            //Debug.Log("looping through " + cfg.Length);
            for (int i = 0; i < cfg.Length; i++)
            {
                if (partName == cfg[i].name)
                {
                    //Debug.Log("found this part: " + cfg[i].url);
                    float scale = 0.1337f;
                    float.TryParse(cfg[i].config.GetValue("scale"), out scale);
                    //Debug.Log("scale: " + scale);
                    string modelPath = cfg[i].parent.parent.url + "/" + "model";
                    //Debug.Log("model path: " + modelPath);
                    GameObject newModel = GameDatabase.Instance.GetModel(modelPath);
                    if (newModel == null)
                    {
                        //Debug.Log("model load error, fetching first model available");
                        newModel = GameDatabase.Instance.GetModelIn(cfg[i].parent.parent.url);
                        return new PartValue(newModel, scale);
                    }
                    else
                    {
                        //Debug.Log("newModel not null");
                        return new PartValue(newModel, scale);
                    }
                }
            }
            Debug.Log("Finding model " + partName + " failed, returning blank GameObject");
            return new PartValue(new GameObject(), 1f);
        }
    }

    public class PartValue
    {
        public string partName;
        public GameObject model;
        public Vector3 position;
        public Quaternion rotation;
        public Quaternion attachRotation;
        public float scale;

        public PartValue(GameObject _model, float _scale)
        {
            model = _model;
            scale = _scale;
        }

        public PartValue()
        {
        }
    }
}
