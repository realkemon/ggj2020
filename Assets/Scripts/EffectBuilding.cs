using UnityEngine;

public class EffectBuilding : MonoBehaviour
{
    public float playerSpeedMultiplier;

    private void Start()
    {
        Globals.currentPlayerSpeedMultiplier += playerSpeedMultiplier;
    }
}