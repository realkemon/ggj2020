using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConverterBuilding : MonoBehaviour
{
    public int dirtRequirement;
    public int woodRequirement;

    public float waitTime;
    public GameObject resultItemPrefab;

    public Text[] slots;
    public Image countdown;

    public AudioClip buildingAudio;

    private int givenDirt;
    private int givenWood;

    private Transform producePosition;
    private bool isProducing;
    private int lastCheckedObjectId;

    private void Start()
    {
        producePosition = transform.GetChild(0);
        slots[0].transform.parent.gameObject.SetActive(true);
        UpdateRecipe();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == lastCheckedObjectId)
            return;

        if (other.gameObject.GetComponent<Item>())
        {
            lastCheckedObjectId = other.gameObject.GetInstanceID();
            if (!isProducing && CheckRequirements(other.gameObject.GetComponent<Item>().itemType))
                TakeItem(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetInstanceID() == lastCheckedObjectId)
            lastCheckedObjectId = 0;
    }

    private bool CheckRequirements(Globals.itemTypes itemType)
    {
        switch (itemType)
        {
            case Globals.itemTypes.Dirt:
                if (givenDirt < dirtRequirement)
                    return true;
                else
                    return false;
            case Globals.itemTypes.Wood:
                if (givenWood < woodRequirement)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }

    private void TakeItem(GameObject itemToTake)
    {
        switch (itemToTake.GetComponent<Item>().itemType)
        {
            case Globals.itemTypes.Dirt:
                givenDirt++;
                break;
            case Globals.itemTypes.Wood:
                givenWood++;
                break;
            default:
                Debug.LogError("Wrong item.");
                break;
        }

        Destroy(itemToTake);
        CheckSum();
        UpdateRecipe();
    }

    private void CheckSum()
    {
        if (givenDirt == dirtRequirement &&
            givenWood == woodRequirement)
        {
            StartCoroutine(ProduceResultItem());
        }
    }

    private IEnumerator ProduceResultItem()
    {
        isProducing = true;
        float currentTime = 0.0f;
        GetComponent<AudioSource>().PlayOneShot(buildingAudio);
        while (currentTime < waitTime)
        {
            currentTime += 0.01f;
            countdown.fillAmount = 1.0f / waitTime * currentTime;
            yield return new WaitForSeconds(0.01f);
        }
        Instantiate(resultItemPrefab, producePosition.position, Quaternion.identity);

        // Reset
        givenDirt = 0;
        givenWood = 0;
        isProducing = false;
        countdown.fillAmount = 0.0f;
        UpdateRecipe();
    }

    private void UpdateRecipe()
    {
        if (isProducing)
        {
            slots[0].text = "";
            slots[1].text = "";
            return;
        }

        if (dirtRequirement > 0)
        {
            slots[1].text = dirtRequirement - givenDirt + "/" + dirtRequirement;
            slots[0].text = "for 1x Cement";
        }
        else if (woodRequirement > 0)
        {
            slots[1].text = woodRequirement - givenWood + "/" + woodRequirement;
            slots[0].text = "for 1x Plank";
        }
    }
}