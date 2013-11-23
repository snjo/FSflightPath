using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FSflightPath
{
    public class FlightPathNode
    {
        public Vector3d position;
        public Quaternion rotation;
        public Vector3 velocity;
        public float time;

        public float speed
        {
            get
            {
                return velocity.magnitude;
            }
        }

        public FlightPathNode(Vector3 _position, Quaternion _rotation, Vector3 _velocity, float _time)
        {
            position = _position;
            rotation = _rotation;
            velocity = _velocity;
            time = _time;
        }

        public FlightPathNode(FlightPathNode node)
        {
            position = node.position;
            rotation = node.rotation;
            velocity = node.velocity;
            time = node.time;
        }

        public FlightPathNode()
        {
        }
    }
}
