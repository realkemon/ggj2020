using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    public int playerId;
    public LayerMask layers;
    public float interactionDistance;
    public float groundOffset;
    public float safetyOffset;
    public float viewDegree;
    public float throwMultiplier;
    public GameObject handNode;

    public AudioClip materialAudio;
    public AudioClip pickaxeAudio;
    public AudioClip goldAudio;

    private GameObject heldItem;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (heldItem == null)
        {
            RaycastHit hit;
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + groundOffset, transform.position.z);
            Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
            startPosition -= forwardDirection * safetyOffset;

            if (Physics.Raycast(startPosition, forwardDirection, out hit, interactionDistance, layers))
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            else
            {
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * interactionDistance, Color.yellow);

                Vector3 newDirection = (forwardDirection * Mathf.Cos(viewDegree * Mathf.Deg2Rad) + transform.right * Mathf.Sin(viewDegree * Mathf.Deg2Rad)).normalized;
                if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                    Debug.DrawRay(startPosition, newDirection * hit.distance, Color.red);
                else
                {
                    Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.yellow);

                    newDirection = (forwardDirection * Mathf.Cos(-viewDegree * Mathf.Deg2Rad) + transform.right * Mathf.Sin(-viewDegree * Mathf.Deg2Rad)).normalized;
                    if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                        Debug.DrawRay(startPosition, newDirection * hit.distance, Color.red);
                    else
                    {
                        Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.yellow);

                        newDirection = (forwardDirection * Mathf.Cos(viewDegree * Mathf.Deg2Rad * 2.0f) + transform.right * Mathf.Sin(viewDegree * Mathf.Deg2Rad * 2.0f)).normalized;
                        if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                            Debug.DrawRay(startPosition, newDirection * hit.distance, Color.red);
                        else
                        {
                            Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.yellow);

                            newDirection = (forwardDirection * Mathf.Cos(-viewDegree * Mathf.Deg2Rad * 2.0f) + transform.right * Mathf.Sin(-viewDegree * Mathf.Deg2Rad * 2.0f)).normalized;
                            if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                                Debug.DrawRay(startPosition, newDirection * hit.distance, Color.red);
                            else
                            {
                                Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.yellow);
                            }
                        }
                    }
                }     
            }
        
            if (hit.collider != null && Input.GetButtonDown("InteractPlayer" + playerId))
            {
                if (hit.collider.gameObject.GetComponent<Item>())
                    GrabItem(hit.collider.gameObject);
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
        itemToGrab.layer = 12; // Layer "Nothing"

        if (itemToGrab.GetComponent<Item>().itemType != Globals.itemTypes.Pickaxe)
        {
            itemToGrab.transform.SetParent(gameObject.transform);
            itemToGrab.transform.localPosition = itemToGrab.GetComponent<Item>().carryingPosition;
            itemToGrab.transform.localEulerAngles = Vector3.zero;
            GetComponent<AudioSource>().PlayOneShot(pickaxeAudio);
            anim.SetBool("isCarrying", true);
        }
        else
        {
            itemToGrab.transform.SetParent(handNode.transform);
            itemToGrab.transform.localPosition = itemToGrab.GetComponent<Item>().carryingPosition;
            itemToGrab.transform.localEulerAngles = new Vector3(8.780001f, 73.34901f, 103.74f);
            if (itemToGrab.GetComponent<Item>().itemType == Globals.itemTypes.Gold)
                GetComponent<AudioSource>().PlayOneShot(goldAudio);
            else
                GetComponent<AudioSource>().PlayOneShot(materialAudio);
            anim.SetBool("HasPickaxe", true);
        }

        heldItem = itemToGrab;
    }

    private void ReleaseItem()
    {
        heldItem.transform.SetParent(null);
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        Vector3 throwDirection = transform.TransformDirection(Vector3.forward);
        heldItem.GetComponent<Rigidbody>().AddForce(throwDirection.x * throwMultiplier, throwDirection.y + throwMultiplier, throwDirection.z * throwMultiplier);
        heldItem.layer = 9; // Layer "Items"
        anim.SetBool("isCarrying", false);
        anim.SetBool("HasPickaxe", false);
        heldItem = null;
    }

    public Globals.itemTypes GetHeldItemType()
    {
        if (heldItem)
            return heldItem.GetComponent<Item>().itemType;
        else
            return Globals.itemTypes.None;
    }
}