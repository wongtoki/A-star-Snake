using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{

    public Vector2 nodePos;
    public Node parent;
    public int gScore;
    public int hScore;
    public int fScore;
    public GameObject tile;
}

public class PlayerPathFinder : MonoBehaviour
{

    public List<Node> openSet;
    public List<Node> closedSet;
    public int routeCost;

    public Vector3[] FindPath(Vector2 startPos, Vector2 destination, Vector2[] tails)
    {

        closedSet = new List<Node>();
        openSet = new List<Node>();

        Node startNode = new Node
        {

            nodePos = startPos,
            gScore = 0,

        };



        closedSet.Add(startNode);


        Node current = startNode;

        while (true)
        {

            Node[] neighbours;
            //Get adjacent tiles 
            neighbours = GetNeighbours(current, tails);

            //Add node to openset
            foreach (Node n in neighbours)
            {


                if (ListContain(openSet, n) || ListContain(closedSet, n))
                {

                    continue;

                }


                n.hScore = (int)(Mathf.Abs(n.nodePos.x - destination.x) + Mathf.Abs(n.nodePos.y - destination.y));
                n.fScore = n.gScore + n.hScore;
                n.parent = current;
                openSet.Add(n);

            }

            //Find the node with the lowest f score in the open set
            int lowF = int.MaxValue;
            Node closedNode = null;
            foreach (Node open in openSet)
            {

                if (open.fScore < lowF)
                {

                    lowF = open.fScore;
                    closedNode = open;

                }

            }

            //Check if all nodes have the same f score
            bool sameFscore = false;
            foreach (Node f in openSet)
            {

                if (lowF != f.fScore)
                {
                    sameFscore = false;
                    break;

                }
                else
                {

                    sameFscore = true;

                }

            }

            if (sameFscore)
            {
                //Find the most recent node(Highest G score)
                int HighG = 0;
                foreach (Node g in openSet)
                {

                    if (g.gScore > HighG)
                    {

                        HighG = g.gScore;
                        closedNode = g;

                    }

                }
            }

            //Add it to the closed set.
            current = closedNode;

            openSet.Remove(closedNode);
            closedSet.Add(closedNode);

            if (openSet.Count == 0)
            {

                Debug.Log("Can't find a route.");
                return null;

            }

            if (current.nodePos == new Vector2(destination.x, destination.y))
            {

                break;

            }


        }

        routeCost = 0;

        Vector3[] waypoints;
        List<Vector3> reversedWaypoints = new List<Vector3>();
        Node currentNode = closedSet[closedSet.Count - 1];
        reversedWaypoints.Add(currentNode.nodePos);
        routeCost += currentNode.gScore;

        while (currentNode.nodePos != startPos)
        {

            reversedWaypoints.Add(currentNode.parent.nodePos);
            routeCost += currentNode.gScore;
            currentNode = currentNode.parent;

        }

        waypoints = reversedWaypoints.ToArray();

        Array.Reverse(waypoints);

        return waypoints;

    }

    Node[] GetNeighbours(Node currentNode, Vector2[] tails)
    {
        Node[] nodes;
        nodes = new Node[4];

        for (int i = 0; i < nodes.Length; i++)
        {

            nodes[i] = new Node();

        }


        nodes[0].nodePos = currentNode.nodePos + new Vector2(-1, 0);


        nodes[1].nodePos = currentNode.nodePos + new Vector2(0, 1);


        nodes[2].nodePos = currentNode.nodePos + new Vector2(1, 0);


        nodes[3].nodePos = currentNode.nodePos + new Vector2(0, -1);

        //Calculate tail gscore
        foreach (Node nodePos in nodes)
        {

            bool isOnTail = false;
            int pos = 0;
            for (int i = 0; i < tails.Length; i++)
            {

                if (nodePos.nodePos == tails[i])
                {

                    pos = i;
                    isOnTail = true;
                    break;

                }

            }

            if (isOnTail)
            {

                nodePos.gScore = currentNode.gScore + (tails.Length - pos)*100;

            }
            else
            {

                nodePos.gScore = currentNode.gScore + 1;
            }

        }


        List<Node> walkableNodes = new List<Node>();
        foreach (Node n in nodes)
        {
            bool isWalkable = true;
            Grid grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();

            if (n.nodePos.x >= grid.width || n.nodePos.x <= -grid.width)
            {

                isWalkable = false;

            }

            if (n.nodePos.y >= grid.height || n.nodePos.y <= -grid.height)
            {

                isWalkable = false;

            }


            if (isWalkable)
                walkableNodes.Add(n);

        }


        return walkableNodes.ToArray();
    }


    bool ListContain(List<Node> list, Node node)
    {
        foreach (Node n in list)
        {

            if (n.nodePos == node.nodePos)
            {

                return true;

            }

        }

        return false;

    }

 

}
