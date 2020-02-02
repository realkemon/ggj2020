using UnityEngine;
using UnityEngine.UI;

public class WashingStation : MonoBehaviour
{
    public int woodPool;
    public int brickPool;
    public int dirtPool;
    public int goldPool;

    public float washTime;
    public GameObject woodPrefab;
    public GameObject brickPrefab;
    public GameObject dirtPrefab;
    public GameObject goldPrefab;

    public Image countdown;
    public AudioClip washingAudio;

    private GameObject heldItem;
    private int lastCheckedObjectId;
    private float doneWashTime;
    private int materialCount;
    private int[] materialResults;

    private void Start()
    {
        materialResults = new int[woodPool + brickPool + dirtPool + goldPool];
        int count = 0;
        for (int i = 0; i < woodPool; i++)
        {
            materialResults[count] = 0;
            count++;
        }
        for (int i = 0; i < brickPool; i++)
        {
            materialResults[count] = 1;
            count++;
        }
        for (int i = 0; i < dirtPool; i++)
        {
            materialResults[count] = 2;
            count++;
        }
        for (int i = 0; i < goldPool; i++)
        {
            materialResults[count] = 3;
            count++;
        }

        // Shuffle
        for (int t = 0; t < materialResults.Length; t++)
        {
            int tmp = materialResults[t];
            int r = Random.Range(t, materialResults.Length);
            materialResults[t] = materialResults[r];
            materialResults[r] = tmp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == lastCheckedObjectId)
            return;

        if (other.gameObject.GetComponent<Item>())
        {
            lastCheckedObjectId = other.gameObject.GetInstanceID();
            if (!heldItem && CheckRequirements(other.gameObject.GetComponent<Item>().itemType))
                TakeItem(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        lastCheckedObjectId = 0;
        doneWashTime = 0.0f;
        countdown.fillAmount = 0.0f;
        if (other.gameObject == heldItem)
            heldItem = null;
    }

    private bool CheckRequirements(Globals.itemTypes itemType)
    {
        switch (itemType)
        {
            case Globals.itemTypes.Undefined:
                    return true;
            default:
                return false;
        }
    }

    private void TakeItem(GameObject itemToTake)
    { 
        heldItem = itemToTake;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.transform.position = transform.position;
        heldItem.transform.eulerAngles = Vector3.zero;
    }

    public bool WashItem()
    {
        if (!heldItem)
        {
            GetComponent<AudioSource>().Stop();
            return false;
        }

        if (heldItem.GetComponent<Item>().itemType != Globals.itemTypes.Undefined)
        {
            GetComponent<AudioSource>().Stop();
            return false;
        }

        doneWashTime += Time.deltaTime * Globals.currentPlayerSpeedMultiplier;
        countdown.fillAmount = 1.0f / washTime * doneWashTime;
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().PlayOneShot(washingAudio);

        if (doneWashTime >= washTime)
        {
            SwitchItem();
            GetComponent<AudioSource>().Stop();
            return false;
        }

        return true;
    }

    public void StopWashing()
    {
        GetComponent<AudioSource>().Stop();
    }

    private void SwitchItem()
    {
        Destroy(heldItem);

        if (materialCount > materialResults.Length - 1)
        {
            Debug.LogError("Material pool is empty.");
            return;
        }

        GameObject newItem = null;
        switch (materialResults[materialCount])
        {
            case 0:
                newItem = Instantiate(woodPrefab, transform.position, Quaternion.identity) as GameObject;
                break;
            case 1:
                newItem = Instantiate(brickPrefab, transform.position, Quaternion.identity) as GameObject;
                break;
            case 2:
                newItem = Instantiate(dirtPrefab, transform.position, Quaternion.identity) as GameObject;
                break;
            case 3:
                newItem = Instantiate(goldPrefab, transform.position, Quaternion.identity) as GameObject;
                break;
            default:
                Debug.LogError("Item not defined.");
                break;
        }
        newItem.GetComponent<Rigidbody>().isKinematic = true;
        newItem.transform.eulerAngles = transform.rotation.eulerAngles;

        // Reset
        materialCount++;
        countdown.fillAmount = 0.0f;
        doneWashTime = 0.0f;
    }
}