using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLevelSaveData : MonoBehaviour
{
    [SerializeField] private int numberOfLevels = 8;

    [ContextMenu("Clear Level Collectible Preferences")]
    public void ClearAllLevelCollectibles()
    {
        for (int i = 1; i <= numberOfLevels; i++)
        {
            string key = $"Level_{i}_Collectibles";
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        PlayerPrefs.Save();
    }
}
