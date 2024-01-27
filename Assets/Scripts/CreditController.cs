using UnityEngine;

public class CreditController : MonoBehaviour
{
    public static void SetCreditMenuVisible()
    {
        PlayerPrefs.SetInt("CreditMenuVisible", 1);
    }

    public static bool IsCreditMenuVisible() => PlayerPrefs.GetInt("CreditMenuVisible", 0) == 1;
}