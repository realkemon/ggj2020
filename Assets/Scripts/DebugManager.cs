using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public ConstructionSite[] constructionSites;

    private void Update()
    {
        for (int n = 0; n < constructionSites.Length; n++)
        {
            if (constructionSites[n] != null && Input.GetKeyDown((n + 1).ToString()))
                constructionSites[n].FinishBuilding();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}