using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    public TMP_Text keyCountText;
    private int currentKeys = 0;
    private int requiredKeys = 3; 

    public void UpdateKeyCount()
    {
        currentKeys++;
        keyCountText.text = currentKeys + " / " + requiredKeys;
    }
}
