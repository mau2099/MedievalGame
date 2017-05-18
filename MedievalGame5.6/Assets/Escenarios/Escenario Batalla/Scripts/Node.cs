using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private List<Node> arcs;
    private bool _visited;
    private GameObject me;

    private int _gCost;
    private int _hCost;

    public List<Node> Arcs
    {
        get
        {
            return arcs;
        }

        set
        {
            arcs = value;
        }
    }
    public bool Visited
    {
        get
        {
            return _visited;
        }

        set
        {
            _visited = value;
        }
    }
    public GameObject Me
    {
        get
        {
            return me;
        }

        set
        {
            me = value;
        }
    }

    public int gCost
    {
        get
        {
            return _gCost;
        }

        set
        {
            _gCost = value;
        }
    }

    public int hCost
    {
        get
        {
            return _hCost;
        }

        set
        {
            _hCost = value;
        }
    }
    public int fCost
    {
        get
        {
            return _hCost + _gCost;
        }
    }
    public int posX { get; set; }
    public int posZ { get; set; }

    public Node parent { get; set; }
    public bool walkable;
    void Start()
    {
        me = this.gameObject;
    }

    public Node()
    {

    }
    public void Initialize()
    {
        arcs = new List<Node>();
    }

    public void AddArc(Node n)
    {
        if (n != null)
        {
            n.walkable = true;
            arcs.Add(n);
        }
    }

    public void Update()
    {
        for (int i = 0; i < arcs.Count; i++)
        {
            Debug.DrawLine(me.transform.position, arcs[i].me.transform.position);
        }

    }
}