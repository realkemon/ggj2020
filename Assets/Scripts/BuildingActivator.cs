using UnityEngine;

public class BuildingActivator : MonoBehaviour
{
    public int playerId;
    public LayerMask layers;
    public float interactionDistance;
    public float groundOffset;
    public float safetyOffset;
    public float viewDegree;

    private ItemGrabber grabber;
    private bool isButtonDown;
    private Animator anim;

    private void Start()
    {
        grabber = GetComponent<ItemGrabber>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {

        if (Input.GetButton("SecondaryInteractPlayer" + playerId))
        {
            RaycastHit hit;
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + groundOffset, transform.position.z);
            Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
            startPosition -= forwardDirection * safetyOffset;

            if (Physics.Raycast(startPosition, forwardDirection, out hit, interactionDistance, layers))
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            else
            {
                Debug.DrawRay(startPosition, transform.TransformDirection(Vector3.forward) * interactionDistance, Color.green);

                Vector3 newDirection = (forwardDirection * Mathf.Cos(viewDegree * Mathf.Deg2Rad) + transform.right * Mathf.Sin(viewDegree * Mathf.Deg2Rad)).normalized;
                if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                    Debug.DrawRay(startPosition, newDirection * hit.distance, Color.blue);
                else
                {
                    Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.green);

                    newDirection = (forwardDirection * Mathf.Cos(-viewDegree * Mathf.Deg2Rad) + transform.right * Mathf.Sin(-viewDegree * Mathf.Deg2Rad)).normalized;
                    if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                        Debug.DrawRay(startPosition, newDirection * hit.distance, Color.blue);
                    else
                    {
                        Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.green);

                        newDirection = (forwardDirection * Mathf.Cos(viewDegree * Mathf.Deg2Rad * 2.0f) + transform.right * Mathf.Sin(viewDegree * Mathf.Deg2Rad * 2.0f)).normalized;
                        if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                            Debug.DrawRay(startPosition, newDirection * hit.distance, Color.blue);
                        else
                        {
                            Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.green);

                            newDirection = (forwardDirection * Mathf.Cos(-viewDegree * Mathf.Deg2Rad * 2.0f) + transform.right * Mathf.Sin(-viewDegree * Mathf.Deg2Rad * 2.0f)).normalized;
                            if (Physics.Raycast(startPosition, newDirection, out hit, interactionDistance, layers))
                                Debug.DrawRay(startPosition, newDirection * hit.distance, Color.blue);
                            else
                            {
                                Debug.DrawRay(startPosition, newDirection * interactionDistance, Color.green);
                            }
                        }
                    }
                }
            }

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<WashingStation>())
                {
                    if (grabber.GetHeldItemType() == Globals.itemTypes.None)
                    {
                        if (hit.collider.gameObject.GetComponent<WashingStation>().WashItem())
                            anim.SetBool("isWashing", true);
                        else
                            anim.SetBool("isWashing", false);
                    }

                }

                if (!isButtonDown && hit.collider.gameObject.GetComponent<Debris>())
                {
                    if (grabber.GetHeldItemType() == Globals.itemTypes.Pickaxe)
                    {
                        hit.collider.gameObject.GetComponent<Debris>().HitWithPickaxe();
                        anim.SetTrigger("hit");
                    }
                }
            }

            isButtonDown = true;
        }
        else
        {
            isButtonDown = false;
            anim.SetBool("isWashing", false);
        }
    }
}