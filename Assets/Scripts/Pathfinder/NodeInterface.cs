using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Pathfinder
{
    interface NodeInterface {
  
        int iGridX //X Position in the Node Array
        {
            get;
            set;
        }
        int iGridY //Y Position in the Node Array
        {
            get;
            set;
        }

        bool bIsWall //Tells the program if this node is being obstructed.
        { 
            get;
            set;
        }
        Vector3 vPosition //The world position of the node.
        {
            get;
            set;
        }

        Node ParentNode //For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.
        {
            get;
            set;
        }

        int igCost //The cost of moving to the next square.
        {
            get;
            set;
        }
        int ihCost //The distance to the goal from this node.
        {
            get;
            set;
        }

        int timeCost
        {
            get;
            set;
        }
        int calculateTimeCost();

        int FCost(); //Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

    }
}