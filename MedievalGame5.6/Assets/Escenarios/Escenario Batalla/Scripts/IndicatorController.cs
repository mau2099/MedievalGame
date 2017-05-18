using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{

    NavMeshMovement mm;

    public GameObject battlefield;
    // Use this for initialization
    void Start()
    {
        //Obtiene el game object que selecciona el objecto
        mm = GameObject.FindObjectOfType<NavMeshMovement>();
        battlefield = GameObject.FindGameObjectWithTag(MedievalObjects.goBattlefield);

    }

    // Update is called once per frame
    void Update()
    {

        //Significa que tiene un game object seleccionado
        if (mm.selectedObject != null)
        {
            RaycastHit rayHitInfo;
            //Obtiene la posicion del pedacito de campo donde esta el gameobject seleccionado
            //Debug.Log("mm.selected object: " + mm.selectedObject.gameObject.name);
            Ray ray = new Ray(mm.selectedObject.transform.position + new Vector3(0, 1, 0), Vector3.down);

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
            //Ray ray = Physics.Raycast(ray) 

            bool encontroCollider = Physics.Raycast(ray, out rayHitInfo);

            //Debug.Log("encontroCollider: " + encontroCollider);
            //Avienta un rayo en el eje Y, debe encontrar la posicion del pedacito de campo
            if (encontroCollider)
            {

                //Debug.Log("object: " + rayHitInfo.collider.gameObject.name);
                //Obtiene el game object que golpeo
                GameObject hitGameObject = rayHitInfo.collider.gameObject;
                //Debug.Log("Posicion de campito: ");
                //Debug.Log("x: " + hitGameObject.transform.position.x);
                //Debug.Log("y: " + hitGameObject.transform.position.y);
                //Debug.Log("z: " + hitGameObject.transform.position.z);

            }

            //Con el objecto seleccionado, se hace algo dependiendo del tipo de objeto
            switch (mm.selectedObject.tag)
            {
                case MedievalObjects.goArcherName:

                    break;
                default:
                    break;
            }
        }
    }
}
