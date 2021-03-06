﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConverterBuilding : MonoBehaviour
{
    public int dirtRequirement;
    public int woodRequirement;

    public float waitTime;
    public GameObject resultItemPrefab;
    public Animator buildingAnimator;

    public int usedClipIndex;
    public Text[] slots;
    public Image countdown;
    public GameObject[] uiElements;

    private int givenDirt;
    private int givenWood;

    private Transform producePosition;
    private bool isProducing;
    private int lastCheckedObjectId;

    private void Start()
    {
        producePosition = transform.GetChild(0);
        buildingAnimator.SetBool("isWorking", true);
        slots[0].transform.parent.transform.parent.gameObject.SetActive(true);
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
        UpdateRecipe();
        SoundManager.instance.PlayItemAcceptSound();
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
        float currentTime = 0.0f;
        SoundManager.instance.PlayConverterUsedSound(usedClipIndex);
        buildingAnimator.SetBool("isBeingUsed", true);
        for (int b = 0; b < uiElements.Length; b++)
            uiElements[b].SetActive(false);
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
        buildingAnimator.SetBool("isBeingUsed", false);
        countdown.fillAmount = 0.0f;
        for (int b = 0; b < uiElements.Length; b++)
            uiElements[b].SetActive(true);
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
            slots[1].text = givenDirt + "/" + dirtRequirement;
            slots[0].text = "= 1";
        }
        else if (woodRequirement > 0)
        {
            slots[1].text = givenWood + "/" + woodRequirement;
            slots[0].text = "= 1";
        }
    }
}