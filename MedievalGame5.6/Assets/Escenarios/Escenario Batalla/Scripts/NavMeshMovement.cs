using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovement : MonoBehaviour
{
    public NavMeshAgent agentArcher;
    public NavMeshAgent agentInfantry;
    public GameObject selectedObject;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        RaycastHit rayHitInfo;

        if (Input.GetButtonDown("Fire1"))
        {
            bool encontroCollider = Physics.Raycast(ray, out rayHitInfo);
            //Avienta un rayo, si choca contra un collider, entonces entra y valida que objeto es
            if (encontroCollider)
            {

                //Debug.Log("Encontro colider");
                //Obtiene el game object que golpeo
                GameObject hitGameObject = rayHitInfo.transform.root.gameObject;

                if (hitGameObject.tag != MedievalObjects.goBattlefield)
                {

                    //Debug.Log("NavMeshObject: " + rayHitInfo.transform.root.gameObject.name);
                    SelectObject(hitGameObject);
                }
            }
            else
            {
                ClearSelection();
            }
        }
    }

    /// <summary>
    /// Entra cuando un rayo encontro un collider
    /// </summary>
    /// <param name="obj"></param>
    void SelectObject(GameObject obj)
    {
        //Si no habia un objeto seleccionado, lo asigna
        if (selectedObject == null)
        {
            selectedObject = obj;
        }
        else
        {
            //Si ya habia un objeto seleccionado y es el mismo, no hace nada
            if (obj == selectedObject)
                return;
            //En caso de ser diferencia, lo reasigna
            else
                selectedObject = obj;
        }
    }


    void ClearSelection()
    {
        if (selectedObject == null)
            return;

        selectedObject = null;
    }
}
