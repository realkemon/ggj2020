using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    public int playerId;
    public LayerMask layers;
    public float interactionDistance;
    public float groundOffset;
    public float throwMultiplier;

    private GameObject heldItem;

    private void Start()
    {
    }

    private void Update()
    {
        if (heldItem == null)
        {
            RaycastHit hit;
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + groundOffset, transform.position.z);
            if (Physics.Raycast(startPosition, transform.TransformDirection(Vector3.forward), out hit, interactionDistance, layers))
            {
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                if (Input.GetButtonDown("InteractPlayer" + playerId))
                {
                    if (hit.collider.gameObject.GetComponent<Item>())
                        GrabItem(hit.collider.gameObject);
                }
            }
            else
            {
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * interactionDistance, Color.yellow);
            }
        }
        else
        {
            if (Input.GetButtonDown("InteractPlayer" + playerId))
                ReleaseItem();
        }
    }

    private void GrabItem(GameObject itemToGrab)
    {
        itemToGrab.GetComponent<Rigidbody>().isKinematic = true;
        itemToGrab.transform.SetParent(gameObject.transform);
        itemToGrab.transform.localPosition = itemToGrab.GetComponent<Item>().carryingPosition;
        itemToGrab.transform.localEulerAngles = Vector3.zero;
        itemToGrab.layer = 12; // Layer "Nothing"
        heldItem = itemToGrab;
    }

    private void ReleaseItem()
    {
        heldItem.transform.SetParent(null);
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        Vector3 throwDirection = transform.TransformDirection(Vector3.forward);
        heldItem.GetComponent<Rigidbody>().AddForce(throwDirection.x * throwMultiplier, throwDirection.y + throwMultiplier, throwDirection.z * throwMultiplier);
        heldItem.layer = 9; // Layer "Items"
        heldItem = null;
    }
}