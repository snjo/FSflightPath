using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    public class FollowerObject
    {
        public GameObject gameObject; // gGameObject added fro db
        public FlightPathFollower follower = new FlightPathFollower();
        public delegate void fOdelegate(FollowerObject fO);
        public fOdelegate destroyFunction;

        public void Destroy()
        {
            destroyFunction(this);
        }
    }
}
