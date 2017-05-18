using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BattlefieldController : MonoBehaviour
{
    public Material SelectedMaterial;
    public Material UnSelectedMaterial;

    public enum States
    {
        Idle = 0,
        SelectMovement = 1,
        Moving = 2,
        SelectTargetAttack = 3,
        Attacking = 4,
        TakeDamage = 5,
        Die = 6
    }

    public States state = States.Idle;
    public Animator animatorArcher;
    public int rows = 11;
    public int cols = 11;
    public int scale = 1;
    public GameObject fieldPrefab;
    public bool createBattlefield = false;

    public NavMeshMovement selection;

    public int maxPathLength = 5;

    /// <summary>
    /// 
    /// </summary>
    public Node[,] field;
    List<Node> finalPath;

    Node actualNode;
    // Use this for initialization
    void Start()
    {
        state = States.Idle;
        field = new Node[cols, rows];
        if (createBattlefield)
        {
            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < cols; x++)
                {
                    GameObject auxField;
                    Vector3 pos = new Vector3(x, 1, z);
                    auxField = Instantiate(fieldPrefab, pos, Quaternion.identity, this.transform);
                    Node auxNode = auxField.GetComponent<Node>();
                    auxNode.posX = x;
                    auxNode.posZ = z;
                    field[x, z] = auxNode;
                    auxField.name = "fieldF" + x + "_" + z;
                }
            }

            //Se obtienen los campos de a lado de cada campo
            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < cols; x++)
                {
                    field[x, z].Initialize();

                    field[x, z].AddArc(x > 0 ? field[x - 1, z] : null);
                    field[x, z].AddArc(x == cols - 1 ? null : field[x + 1, z]);
                    field[x, z].AddArc(z == rows - 1 ? null : field[x, z + 1]);
                    field[x, z].AddArc(z > 0 ? field[x, z - 1] : null);
                }
            }
        }

        //Invoke("BuildPath", 2);
    }

    // Update is called once per frame

    int actualNodeIndex = -1;
    void Update()
    {
        switch (state)
        {
            case States.Idle:
                break;
            case States.SelectMovement:
                SelectTargetToMove();
                break;
            case States.Moving:
                MovingCharacer();
                break;
            case States.SelectTargetAttack:
                Debug.Log("Select Target attack movement");
                break;
            case States.Attacking:
                break;
            case States.TakeDamage:
                break;
            case States.Die:
                break;
            default:
                break;
        }

    }

    void BuildPath(Node a, Node b)
    {


        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++)
            {

                field[x, z].Me.GetComponent<Renderer>().material = UnSelectedMaterial;
                field[x, z].Visited = false;
            }
        }

        List<Node> closedNodes;
        List<Node> openedNodes;

        openedNodes = new List<Node>();
        closedNodes = new List<Node>();
        openedNodes.Add(a);
        finalPath = new List<Node>();
        FindAStarPath2(openedNodes, closedNodes, maxPathLength, a, b);
        if (finalPath != null && finalPath.Count > 0)
        {
            for (int i = 0; i < finalPath.Count; i++)
            {
                finalPath[i].Me.GetComponent<Renderer>().material = SelectedMaterial;
                //Debug.Log(finalPath[i].Me);
            }
        }
    }


    bool FindAStarPath(Node initNode, Node finalNode, List<Node> finalList, int hops)
    {

        if (hops == 0)
            return false;

        if (initNode.Visited)
            return false;

        initNode.Visited = true;

        if (initNode == finalNode)
        {
            finalList.Add(initNode);
            return true;
        }

        for (int i = 0; i < initNode.Arcs.Count; i++)
        {
            bool found = FindAStarPath(initNode.Arcs[i], finalNode, finalList, hops - 1);
            if (found)
            {
                finalList.Add(initNode);
                return true;
            }
        }

        initNode.Visited = false;

        return false;
    }


    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();


        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(Mathf.RoundToInt(nodeA.Me.transform.position.x) - Mathf.RoundToInt(nodeB.Me.transform.position.x));
        int dstZ = Mathf.Abs(Mathf.RoundToInt(nodeA.Me.transform.position.z) - Mathf.RoundToInt(nodeB.Me.transform.position.z));
        return (10 * dstZ) + (10 * dstX);
    }

    void FindAStarPath2(List<Node> _opened, List<Node> _closed, int hops, Node initNode, Node endNode)
    {
        if (hops == -1)
            return;

        int minFCost = _opened.Min(x => x.fCost);
        int minHCost = _opened.Min(x => x.hCost);

        Node current = _opened.FirstOrDefault(x => x.fCost <= minFCost && x.hCost <= minHCost);

        _opened.Remove(current);
        _closed.Add(current);

        if (current == endNode)
        {
            finalPath = GetFinalPath(initNode, endNode);
            return;
        }

        //for (int i = 0; i < current.Arcs.Count; i++)

        List<Node> neighbours = CheckWalkablesNodes(current);
        for (int i = 0; i < neighbours.Count; i++)
        {
            Node neighbour = current.Arcs[i];
            if (!neighbour.walkable || _closed.Contains(neighbour))
                continue;
            int newCostToArc = current.gCost + GetDistance(current, neighbour);
            if (newCostToArc < current.Arcs[i].gCost || !_opened.Contains(neighbour))
            {
                neighbour.gCost = newCostToArc;
                neighbour.hCost = GetDistance(neighbour, endNode);
                neighbour.parent = current;
                if (!_opened.Contains(neighbour))
                    _opened.Add(neighbour);
            }
        }

        if (_opened.Count > 0)
        {
            FindAStarPath2(_opened, _closed, hops - 1, initNode, endNode);

        }
    }

    List<Node> GetFinalPath(Node initNode, Node endNode)
    {
        List<Node> _finalPath = new List<Node>();

        Node currentNode = endNode;

        do
        {
            _finalPath.Add(currentNode);
            if (currentNode != initNode)
                currentNode = currentNode.parent;

        } while (currentNode != initNode);

        _finalPath.Reverse();

        return _finalPath;
    }

    void MovingCharacer()
    {
        if (actualNodeIndex == -1)
        { // startig
            actualNodeIndex = 0;
            actualNode = finalPath[actualNodeIndex];
        }
        Vector3 posToLook = actualNode.transform.position;
        posToLook.y = selection.selectedObject.transform.position.y;
        selection.selectedObject.transform.LookAt(posToLook);
        CharacterController control = selection.selectedObject.GetComponent<CharacterController>();

        control.Move(0.012f * selection.selectedObject.transform.TransformDirection(Vector3.forward));
        float d = Vector3.Distance(selection.selectedObject.gameObject.transform.position, posToLook);

        selection.selectedObject.gameObject.SendMessage("SetWalking", SendMessageOptions.DontRequireReceiver);
        if (d < 0.5f)
        {
            actualNodeIndex++;
            if (actualNodeIndex >= finalPath.Count)
            {
                state = States.SelectMovement;
                actualNodeIndex = -1;
                selection.selectedObject.transform.position = posToLook;
                selection.selectedObject.gameObject.SendMessage("SetIdle", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                actualNode = finalPath[actualNodeIndex];
            }
        }
    }

    void SelectTargetToMove()
    {
        if (selection.selectedObject != null)
        {
            RaycastHit rayHitInfo;
            //Obtiene la posicion del pedacito de campo donde esta el gameobject seleccionado
            //Debug.Log("mm.selected object: " + selection.sele1ctedObject.gameObject.name);
            Ray ray = new Ray(selection.selectedObject.transform.position + new Vector3(0, 1, 0), Vector3.down);

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
            //Ray ray = Physics.Raycast(ray) 

            bool encontroCollider = Physics.Raycast(ray, out rayHitInfo);

            //Debug.Log("encontroCollider: " + encontroCollider);
            //Avienta un rayo en el eje Y, debe encontrar la posicion del pedacito de campo
            if (encontroCollider)
            {

                Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);

                Debug.DrawRay(ray1.origin, ray1.direction * 1000, Color.cyan);
                RaycastHit rayHitInfo1;

                bool encontroCollider1 = Physics.Raycast(ray1, out rayHitInfo1);

                //Avienta un rayo, si choca contra un collider, entonces entra y valida que objeto es
                if (encontroCollider1)
                {
                    //Debug.Log("encontroCollider1: " + encontroCollider1 + " " + rayHitInfo1.collider.gameObject);

                    if (rayHitInfo.collider.gameObject != selection.selectedObject)
                    {
                        if (rayHitInfo.collider.gameObject != rayHitInfo1.collider.gameObject && rayHitInfo1.collider.gameObject.GetComponent<Node>() != null)
                        {

                            BuildPath(
                                rayHitInfo.collider.gameObject.GetComponent<Node>(),
                                rayHitInfo1.collider.gameObject.GetComponent<Node>()
                                );

                            if (Input.GetButtonDown("Fire1"))
                            {
                                //Se selecciono la ruta, empieza a avanzar
                                state = States.Moving;
                            }

                        }
                    }
                }
            }

        }
    }

    List<Node> CheckWalkablesNodes(Node initNode)
    {
        int x = Mathf.RoundToInt(initNode.gameObject.transform.position.x);
        int z = Mathf.RoundToInt(initNode.gameObject.transform.position.z);
        //Se obtienen los campos de a lado de cada campo
        initNode.Initialize();


        initNode.AddArc(x > 0 ? field[x - 1, z] : null);
        initNode.AddArc(x == cols - 1 ? null : field[x + 1, z]);
        initNode.AddArc(z == rows - 1 ? null : field[x, z + 1]);
        initNode.AddArc(z > 0 ? field[x, z - 1] : null);

        for (int i = 0; i < initNode.Arcs.Count; i++)
        {
            Node neighbour = initNode.Arcs[i];
            RaycastHit rayHitInfo;
            //Obtiene la posicion del pedacito de campo donde esta el gameobject seleccionado
            //Debug.Log("mm.selected object: " + selection.sele1ctedObject.gameObject.name);
            Ray ray = new Ray(neighbour.Me.transform.position + new Vector3(0, 0, 0), Vector3.up);

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            //Ray ray = Physics.Raycast(ray) 

            bool encontroCollider = Physics.Raycast(ray, out rayHitInfo);

            if (encontroCollider && rayHitInfo.collider.gameObject != selection.selectedObject)
            {
                Debug.Log("Colider walkable" + neighbour);
                if (rayHitInfo.collider.gameObject.GetComponent<Node>() == null)
                {
                    neighbour.walkable = false;
                    initNode.Arcs[i] = neighbour;
                }
            }
        }

        return initNode.Arcs;
    }
}
