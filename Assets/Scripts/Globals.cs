using UnityEngine;

public class Globals : MonoBehaviour
{
    public static float currentPlayerSpeedMultiplier = 1.0f;

    public enum itemTypes
    {
        None,
        Undefined,
        Wood,
        Brick,
        Dirt,
        Cement,
        Plank,
        Gold,
        Pickaxe
    }
}