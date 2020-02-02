using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionSite : MonoBehaviour
{
    public int woodRequirement;
    public int brickRequirement;
    public int cementRequirement;
    public int plankRequirement;
    public int goldRequirement;

    public GameObject buildingPrefab;
    public float buildingHeight;

    public Text[] slots;

    public AudioClip growAudio;
    public AudioClip readyAudio;

    public string debugKey;

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

        UpdateRecipe();
    }

    private void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            givenBrick = brickRequirement;
            givenWood = woodRequirement;
            givenCement = cementRequirement;
            givenPlank = plankRequirement;
            givenGold = goldRequirement;
            CheckSum();
        }
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetInstanceID() == lastCheckedObjectId)
            lastCheckedObjectId = 0;
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
        UpdateRecipe();
        StartCoroutine(GrowBuilding());
    }

    private IEnumerator GrowBuilding()
    {
        isGrowing = true;
        GetComponent<AudioSource>().PlayOneShot(growAudio);
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
            //Instantiate(buildingPrefab, transform.position, Quaternion.identity);
            //GetComponent<AudioSource>().PlayOneShot(readyAudio);
            buildingPrefab.SetActive(true);
            Destroy(slots[0].transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    private void UpdateRecipe()
    {
        int count = 0;

        if (goldRequirement > 0)
        {
            slots[count].text = givenGold + "/" + goldRequirement;
            count++;
        }

        if (plankRequirement > 0)
        {
            slots[count].text = givenPlank + "/" + plankRequirement;
            count++;
        }

        if (cementRequirement > 0)
        {
            slots[count].text = givenCement + "/" + cementRequirement;
            count++;
        }

        if (woodRequirement > 0)
        {
            slots[count].text = givenWood + "/" + woodRequirement;
            count++;
        }

        if (brickRequirement > 0)
        {
            slots[count].text = givenBrick + "/" + brickRequirement;
            count++;
        }

        for (int g = count; g < slots.Length; g++)
            slots[g].text = "";
    }
}