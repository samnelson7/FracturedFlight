using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] public string SceneToLoad;
    [SerializeField] public GameObject egg1Collected;
    [SerializeField] public GameObject egg2Collected;
    [SerializeField] public GameObject egg3Collected;
    public HoverColorChange hoverColorChange;
    private void Start()
    {
        if (egg1Collected != null && egg2Collected != null && egg3Collected != null) 
        {
            egg1Collected.SetActive(false);
            egg2Collected.SetActive(false);
            egg3Collected.SetActive(false);
            loadCollectables();
        }
    }
    public void OpenScene()
    {
        if (SceneToLoad == "")
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }else if (SceneToLoad == "Next")
        {
            string currentLevelName = SceneManager.GetActiveScene().name;
            string[] parts = currentLevelName.Split(' ');
            int currentLevelNumber = int.Parse(parts[parts.Length - 1]) + 1;
            if (currentLevelNumber == '8')
            {
                SceneManager.LoadSceneAsync("Level Selector");
            }
            else
            {
                SceneManager.LoadSceneAsync("Level " + currentLevelNumber);
            }
        }
        else if (SceneToLoad == "Exit")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadSceneAsync(SceneToLoad);
        }
    }
    public void loadCollectables()
    {
        string[] parts = SceneToLoad.Split(' ');
        int currentLevelNumber = int.Parse(parts[parts.Length - 1]);
        bool previousLevelCompleted = isPreviousLevelCompleted(currentLevelNumber);
        if (!previousLevelCompleted) setLevelButtonState(false);
        string key = "Level_" + currentLevelNumber + "_Collectibles"; // e.g., "Level_1_Collectibles"
        string collectedEggs = PlayerPrefs.GetString(key, "000");
        Debug.Log($"After Save - Key: {key}, CollectedEggs: {collectedEggs}");
        for (int i = 0; i < 3; i++)
        {
            if (i == 0 && collectedEggs[i] == '1') egg1Collected.SetActive(true);
            if (i == 1 && collectedEggs[i] == '1') egg2Collected.SetActive(true);
            if (i == 2 && collectedEggs[i] == '1') egg3Collected.SetActive(true);

        }
    }
    public bool isPreviousLevelCompleted(int currentLevelNumber)
    {
        if (int.Parse(currentLevelNumber.ToString()) <= 1) return true;

        return PlayerPrefs.HasKey($"Level_{currentLevelNumber - 1}_Collectibles");
    }
    public void setLevelButtonState(bool isInteractable)
    {
        // Disable the button's click functionality
        GetComponent<Button>().interactable = isInteractable;

        
        if (hoverColorChange != null)
        {
            Debug.Log("entered != null");
            hoverColorChange.isLocked = !isInteractable; // Disable hover effect if level is locked
        }

        // Try to find the child object named 'LevelFrame'
        Transform frame = transform.Find("LevelFrame");
        if (frame != null)
        {
            Image[] images = frame.GetComponentsInChildren<Image>(includeInactive: true);
            foreach (Image img in images)
            {
                if (!img.gameObject.name.Contains("Collect"))
                {
                    Color currentColor = img.color;
                    Color32 grey = new Color32(128, 128, 128, (byte)(currentColor.a * 255));
                    img.color = isInteractable ? Color.white : grey;
                }
            }
        }
    }
}
