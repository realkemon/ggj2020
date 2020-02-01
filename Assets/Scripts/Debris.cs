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
        GameObject newItem = Instantiate(undefinedPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity) as GameObject;
        newItem.GetComponent<Rigidbody>().AddForce(Random.Range(50.0f, 80.0f), 100.0f, Random.Range(-40.0f, 50.0f));
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