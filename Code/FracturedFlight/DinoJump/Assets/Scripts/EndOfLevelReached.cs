using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PolygonCollider2D))]
public class EndOfLevelReached : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    private void Reset()
    {
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator1.SetBool("EndReached", true);
        animator2.SetBool("EndReached", true);
        animator3.SetBool("EndReached", true);
        UIManager.instance.disablePlayerMovement();
        SaveLevelCollectibles();
    }
    public void SaveLevelCollectibles()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        char levelNumber = currentLevelName[currentLevelName.Length - 1]; // levels are called 'Level 1' and so on up to 8 each world. This gets the number
        string key = "Level_" + levelNumber + "_Collectibles"; // e.g., "Level_1_Collectibles"
        string collectedThisTime = UIManager.instance.getCollectablesFound();
        string alreadyFound = PlayerPrefs.GetString(key, "");

        if(alreadyFound == "") PlayerPrefs.SetString(key, collectedThisTime);
        else PlayerPrefs.SetString(key, orCollectedStrings(alreadyFound, collectedThisTime));
        Debug.Log($"After Save - Key: {key}, New Combined: {collectedThisTime}");
        PlayerPrefs.Save();
    }
    public string orCollectedStrings(string collectedThisTime, string collectedAlready)
    {

        char[] result = new char[3];

        for (int i = 0; i < collectedThisTime.Length; i++)
        {
            if (collectedThisTime[i] == '1' || collectedAlready[i] == '1')
            {
                result[i] = '1';
            }
            else
            {
                result[i] = '0';
            }
        }

        string finalResult = new string(result);
        return finalResult;
    }
}
