using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingActivator : MonoBehaviour
{
    public int playerId;
    public LayerMask layers;
    public float interactionDistance;
    public float groundOffset;


    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetButton("SecondaryInteractPlayer" + playerId))
        {
            RaycastHit hit;
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + groundOffset, transform.position.z);
            if (Physics.Raycast(startPosition, transform.TransformDirection(Vector3.forward), out hit, interactionDistance, layers))
            {
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);

                if (hit.collider.gameObject.GetComponent<WashingStation>())
                    hit.collider.gameObject.GetComponent<WashingStation>().WashItem();
            }
            else
            {
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * interactionDistance, Color.blue);
            }  
        }
    }
}