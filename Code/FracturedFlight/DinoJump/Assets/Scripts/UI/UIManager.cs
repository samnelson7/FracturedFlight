using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject missingEggIcon1;
    public GameObject missingEggIcon2;
    public GameObject missingEggIcon3;
    public GameObject collectedEgg1;
    public GameObject collectedEgg2;
    public GameObject collectedEgg3;
    public GameObject PlayAgain;
    public GameObject NextStage;
    public GameObject ExitStage;
    public GameObject YouDied;
    public GameObject LevelComplete;
    public GameObject ToMove;
    public GameObject ArrowKeys;
    public PlayerMovement playerMovement;
    private float MovementDisplay = 2.0f;
    private bool egg1Found = false;
    private bool egg2Found = false;
    private bool egg3Found = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        ToMove.SetActive(true);
        ArrowKeys.SetActive(true);
        missingEggIcon1.SetActive(true);
        missingEggIcon2.SetActive(true);
        missingEggIcon3.SetActive(true);
        collectedEgg1.SetActive(false);
        collectedEgg2.SetActive(false);
        collectedEgg3.SetActive(false);
        PlayAgain.SetActive(false);
        ExitStage.SetActive(false);
        YouDied.SetActive(false);
        NextStage.SetActive(false);
        LevelComplete.SetActive(false);
    }
    private void Update()
    {
        MovementDisplay -= Time.deltaTime;
        if (MovementDisplay < 0)
        {
            ToMove.SetActive(false);
            ArrowKeys.SetActive(false);
        }
    }
    public void EggCollected(int eggCollected)
    {
        if(eggCollected == 1)
        {
            egg1Found = true;
            missingEggIcon1.SetActive(false);
            collectedEgg1.SetActive(true);
        } else if (eggCollected == 2)
        {
            egg2Found = true;
            missingEggIcon2.SetActive(false);
            collectedEgg2.SetActive(true);
        } else if (eggCollected == 3)
        {
            egg3Found = true;
            missingEggIcon3.SetActive(false);
            collectedEgg3.SetActive(true);
        }
    }
    public void killed(string causeOfDeath)
    {
        PlayAgain.SetActive(true);
        ExitStage.SetActive(true);
        TMP_Text text = YouDied.GetComponentInChildren<TMP_Text>();
        text.text = causeOfDeath;
        YouDied.SetActive(true);
    }
    public void playerMoved()
    {
        ToMove.SetActive(false);
        ArrowKeys.SetActive(false);
    }
    public void disablePlayerMovement()
    {
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.playerCanMove = false;
        NextStage.SetActive(true);
        ExitStage.SetActive(true);
        LevelComplete.SetActive(true);
    }

    public string getCollectablesFound()
    {
        string value = "";
        value += egg1Found ? "1" : "0";
        value += egg2Found ? "1" : "0";
        value += egg3Found ? "1" : "0";
        return value;
    }
}
