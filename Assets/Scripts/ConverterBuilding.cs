using System.Collections;
using UnityEngine;

public class ConverterBuilding : MonoBehaviour
{
    public int dirtRequirement;
    public int woodRequirement;

    public float waitTime;
    public GameObject resultItemPrefab;

    private int givenDirt;
    private int givenWood;

    private Transform producePosition;
    private bool isProducing;

    private void Start()
    {
        producePosition = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>())
        {
            if (!isProducing && CheckRequirements(other.gameObject.GetComponent<Item>().itemType))
                TakeItem(other.gameObject);
        }
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
        yield return new WaitForSeconds(waitTime);
        Instantiate(resultItemPrefab, producePosition.position, Quaternion.identity);

        // Reset
        givenDirt = 0;
        givenWood = 0;
        isProducing = false;
    }
}