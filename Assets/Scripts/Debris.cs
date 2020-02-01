using UnityEngine;

public class Debris : MonoBehaviour
{
    public int startSize;
    public GameObject undefinedPrefab;
    public GameObject smallSize;
    public GameObject mediumSize;
    public GameObject bigSize;

    private int currentSize;
    private BoxCollider trigger;

    private void Start()
    {
        currentSize = startSize;
        trigger = GetComponent<BoxCollider>();
        AdjustSize();
    }

    public void HitWithPickaxe()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x + Random.value <= 0.5f ? trigger.size.x / 2 : -trigger.size.x / 2, transform.position.y + 0.5f, transform.position.z + Random.value <= 0.5f ? trigger.size.z / 2: -trigger.size.z / 2);
        GameObject newItem = Instantiate(undefinedPrefab, transform.position + spawnPosition, Quaternion.identity) as GameObject;
        newItem.GetComponent<Rigidbody>().AddForce(Random.Range(-50.0f, 50.0f), 80.0f, Random.Range(-50.0f, 50.0f));
        currentSize--;
        AdjustSize();

        if (currentSize == 0)
            Destroy(gameObject);
    }

    private void AdjustSize()
    {
        if (currentSize > 5)
        {
            smallSize.SetActive(false);
            mediumSize.SetActive(false);
            bigSize.SetActive(true);
            trigger.size = new Vector3(2.3f, 2.0f, 2.3f);
        }
        else if (currentSize > 3)
        {
            smallSize.SetActive(false);
            mediumSize.SetActive(true);
            bigSize.SetActive(false);
            trigger.size = new Vector3(1.7f, 2.0f, 1.7f);
        }
        else
        {
            smallSize.SetActive(true);
            mediumSize.SetActive(false);
            bigSize.SetActive(false);
            trigger.size = new Vector3(1.1f, 2.0f, 1.1f);
        }
    }
}