using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Start Menu")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;
    public GameObject charSelectMenu;
    public GameObject notAvalable;
    public GameObject fadeOut;

    [Header("In Game Menu")]
    public GameObject pauseMenu;
    public GameObject HUD;
    public GameObject dialogueBox;
    public GameObject ready;
    public GameObject go;
    public GameObject youWin;
    public GameObject youLose;

    [Header("Loading Screen")]
    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    private bool isPaused = false;
    private bool canPause = true;
    private bool isDialogue = false;

    private void Awake()
    {
        GameManager.serviceLocator.ProvideService(this);
    }

    public void MenuNext()
    {
        SoundManager.Instance.PlaySound("menuNext", 1.0f);
    }

    public void MenuBack()
    {
        SoundManager.Instance.PlaySound("menuBack", 1.0f);
    }

    public void MenuStart()
    {
        SoundManager.Instance.PlaySound("gameStart", 1.0f);
    }

    public void MenuHover()
    {
        SoundManager.Instance.PlaySound("menuSelection", 1.0f);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsDialogue()
    {
        return isDialogue;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            fadeOut.SetActive(false);
            BackToMainMenu();
        }
        else
        {
            StartOfScene();
        }
    }
    public void StartOfScene()
    {
        InGameSwitch("HUD");
        StartCoroutine(ReadyToFight());
    }

    private IEnumerator ReadyToFight()
    {
        canPause = false;
        SoundManager.Instance.StopMusic();
        Time.timeScale = 0;
        ready.SetActive(true);
        int startAudio = Random.Range(0, 2);
        Debug.Log(startAudio);
        if(startAudio == 0)
            SoundManager.Instance.PlaySound("getReady", 1.0f);
        else
            SoundManager.Instance.PlaySound("getReady2", 1.0f);
        yield return new WaitForSecondsRealtime(3.0f);
        ready.SetActive(false);
        go.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        go.SetActive(false);
        Time.timeScale = 1;
        SoundManager.Instance.PlayMusic("stage1Music");
        canPause = true;
    }

    public void Pause()
    {
        if (!canPause)
            return;
        SoundManager.Instance.PlaySound("menuPause", 1.0f);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        //GameManager.Instance.SetGameState(GameState.PAUSED);
        InGameSwitch("Pause");
        Time.timeScale = 0; // Make sure Input System Package setting "Update Mode" is set to "Dynamic Update", otherwise it will not work
    }

    public void Unpause()
    {
        SoundManager.Instance.PlaySound("menuPause", 1.0f);
        if (!isPaused)
            return;
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        //GameManager.Instance.SetGameStateByContext();
        InGameSwitch("HUD");
    }

    public void OpenDialogue(string name, string[] text)
    {
        dialogueBox.GetComponent<Animator>().Play("OpenDialogue");
        //GameManager.Instance.SetGameState(GameState.DIALOGUE);
        isPaused = false; isDialogue = true;
        //playerController.SetSpeedZero();
        //Time.timeScale = 0;
        InGameSwitch("Dialogue");
        //dialogueName.SetText(name);
        //dialogue.SetText(text);
        //dialogue.StartDialogue();
    }

    public void CloseDialogue()
    {
        dialogueBox.GetComponent<Animator>().Play("CloseDialogue");
        //GameManager.Instance.SetGameStateByContext();
        isPaused = false; isDialogue = false;
        //Time.timeScale = 1;
        Invoke("CloseDialogueDelay", 0.3f);
    }

    private void CloseDialogueDelay()
    {
        InGameSwitch("HUD");
        //openDialogue.DoneTalking();
    }

    public void NextDialogue()
    {
        //dialogue.Click();
    }

    public void OpenYouWin()
    {
        StartCoroutine(YouWin());
    }

    private IEnumerator YouWin()
    {
        Time.timeScale = 0;
        InGameSwitch("YouWin");
        yield return new WaitForSecondsRealtime(1.0f);
        InGameSwitch("HUD");
        Time.timeScale = 1;
        SendToMainMenu("Start");
    }

    public void OpenYouLose()
    {
        StartCoroutine(YouLose());
    }

    private IEnumerator YouLose()
    {
        Time.timeScale = 0;
        InGameSwitch("YouLose");
        yield return new WaitForSecondsRealtime(1.0f);
        InGameSwitch("HUD");
        Time.timeScale = 1;
        SendToMainMenu("Start");
    }

    public void InGameSwitch(string ui)
    {
        switch (ui)
        {
            case "HUD":
                HUD.SetActive(true);
                pauseMenu.SetActive(false);
                dialogueBox.SetActive(false);
                LoadingScreen.SetActive(false);
                youWin.SetActive(false);
                youLose.SetActive(false);
                break;
            case "Pause":
                HUD.SetActive(false);
                pauseMenu.SetActive(true);
                youWin.SetActive(false);
                youLose.SetActive(false);
                break;
            case "Dialogue":
                HUD.SetActive(true);
                pauseMenu.SetActive(false);
                dialogueBox.SetActive(true);
                break;
            case "YouWin":
                HUD.SetActive(true);
                pauseMenu.SetActive(false);
                dialogueBox.SetActive(true);
                youWin.SetActive(true);
                youLose.SetActive(false);
                break;
            case "YouLose":
                HUD.SetActive(true);
                pauseMenu.SetActive(false);
                dialogueBox.SetActive(true);
                youWin.SetActive(false);
                youLose.SetActive(true);
                break;
            default:
                break;
        }
    }


    public void MoveToScene(string SceneName)
    {
        //GameManager.Instance.ChangeScene(SceneName);
        StartCoroutine(LoadNextScene(SceneName));
    }

    private IEnumerator LoadNextScene(string SceneName)
    {
        LoadingScreen.SetActive(true);
        LoadingBarFill.fillAmount = 0;
        //yield return new WaitForSeconds(1.0f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName); //does not change the game state, need to fix
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
        //MoveToScene(SceneName); //might conflict with loading bar
    }

    public void SendToMainMenu(string SceneName)
    {
        SoundManager.Instance.PlayMusic("menuMusic");
        //GameManager.Instance.SetGameState(GameState.MAIN_MENU);
        Time.timeScale = 1;
        //GameManager.Instance.ChangeScene(SceneName);
        MoveToScene(SceneName);
    }

    public void NewGame(string SceneName)
    {
        Debug.Log("Starting new game!");
        StartCoroutine(NewGameFade(SceneName));

    }

    private IEnumerator NewGameFade(string SceneName)
    {

        fadeOut.SetActive(true);
        LoadingScreen.SetActive(true);
        fadeOut.GetComponent<Animator>().Play("MenuFade");
        LoadingBarFill.fillAmount = 0;
        SoundManager.Instance.StopMusic();
        yield return new WaitForSeconds(1.0f);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
        //MoveToScene(SceneName); //might conflict with loading bar
    }

    public void OpenCredits()
    {
        MenuSwitch("Credits");
    }

    public void OpenControls()
    {
        MenuSwitch("Controls");
    }

    public void BackToMainMenu()
    {
        MenuSwitch("Main Menu");
    }

    public void OpenSettings()
    {
        MenuSwitch("Settings");
    }

    public void OpenCharSelect()
    {
        SoundManager.Instance.PlayMusic("characterSelect");
        MenuSwitch("CharSelect");
    }

    public void CloseCharSelect()
    {
        SoundManager.Instance.PlayMusic("menuMusic");
        MenuSwitch("Main Menu");
    }

    public void NotAvalable()
    {
        StartCoroutine(NoSaveData());
        Debug.Log("We don't have a save system to load from yet!");
    }

    private IEnumerator NoSaveData()
    {
        notAvalable.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        notAvalable.SetActive(false);
    }

    private void MenuSwitch(string ui)
    {
        switch (ui)
        {
            case "Controls":
                controlsMenu.SetActive(true);
                creditsMenu.SetActive(false);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                LoadingScreen.SetActive(false);
                charSelectMenu.SetActive(false);
                break;
            case "Credits":
                controlsMenu.SetActive(false);
                creditsMenu.SetActive(true);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                LoadingScreen.SetActive(false);
                charSelectMenu.SetActive(false);
                break;
            case "Main Menu":
                controlsMenu.SetActive(false);
                creditsMenu.SetActive(false);
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
                LoadingScreen.SetActive(false);
                charSelectMenu.SetActive(false);
                break;
            case "Settings":
                controlsMenu.SetActive(false);
                creditsMenu.SetActive(false);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(true);
                LoadingScreen.SetActive(false);
                charSelectMenu.SetActive(false);
                break;
            case "CharSelect":
                controlsMenu.SetActive(false);
                creditsMenu.SetActive(false);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                LoadingScreen.SetActive(false);
                charSelectMenu.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
