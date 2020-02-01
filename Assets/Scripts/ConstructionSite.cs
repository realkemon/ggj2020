using System.Collections;
using UnityEngine;

public class ConstructionSite : MonoBehaviour
{
    public int woodRequirement;
    public int brickRequirement;
    public int cementRequirement;
    public int plankRequirement;
    public int goldRequirement;

    public GameObject buildingPrefab;
    public float buildingHeight;

    private int givenWood;
    private int givenBrick;
    private int givenCement;
    private int givenPlank;
    private int givenGold;

    private Transform building;
    private float heightPerItem;
    private bool isGrowing;
    private int lastCheckedObjectId;

    private void Start()
    {
        building = transform.GetChild(0);
        building.localPosition = new Vector3(0.0f, -buildingHeight, 0.0f);
        int totalItemCount = woodRequirement + brickRequirement + cementRequirement + plankRequirement + goldRequirement;
        heightPerItem = buildingHeight / totalItemCount;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == lastCheckedObjectId)
            return;

        if (other.gameObject.GetComponent<Item>())
        {
            lastCheckedObjectId = other.gameObject.GetInstanceID();
            if (!isGrowing && CheckRequirements(other.gameObject.GetComponent<Item>().itemType))
                TakeItem(other.gameObject);
        }
    }

    private bool CheckRequirements(Globals.itemTypes itemType)
    {
        switch (itemType)
        {
            case Globals.itemTypes.Brick:
                if (givenBrick < brickRequirement)
                    return true;
                else
                    return false;
            case Globals.itemTypes.Cement:
                if (givenCement < cementRequirement)
                    return true;
                else
                    return false;
            case Globals.itemTypes.Gold:
                if (givenGold < goldRequirement)
                    return true;
                else
                    return false;
            case Globals.itemTypes.Plank:
                if (givenPlank < plankRequirement)
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
            case Globals.itemTypes.Brick:
                givenBrick++;
                break;
            case Globals.itemTypes.Cement:
                givenCement++;
                break;
            case Globals.itemTypes.Gold:
                givenGold++;
                break;
            case Globals.itemTypes.Plank:
                givenPlank++;
                break;
            case Globals.itemTypes.Wood:
                givenWood++;
                break;
            default:
                Debug.LogError("Wrong item.");
                break;
        }

        Destroy(itemToTake);
        StartCoroutine(GrowBuilding());
    }

    private IEnumerator GrowBuilding()
    {
        isGrowing = true;
        int step = 0;
        while (step <= 30)
        {
            building.Translate(new Vector3(0.0f, heightPerItem / 30, 0.0f));
            step++;
            yield return new WaitForSeconds(0.033333f);
        }
        isGrowing = false;
        CheckSum();
    }

    private void CheckSum()
    {
        if (givenBrick == brickRequirement &&
            givenCement == cementRequirement &&
            givenGold == goldRequirement &&
            givenPlank == plankRequirement &&
            givenWood == woodRequirement)
        {
            Instantiate(buildingPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}