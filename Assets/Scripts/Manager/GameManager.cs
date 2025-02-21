using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData gameData;
    public SaveData saveData;

    void Awake()
    {
        InitProperty();
        SetFrameRate();
    }

    void InitProperty()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion

        gameData = new GameData();

        if (saveData == null)
            saveData = new SaveData();
        else
            LoadData();

        gameData.inGameState = new ReactiveProperty<EInGameState>(EInGameState.None);
        gameData.gameState = new ReactiveProperty<EGameState>(EGameState.Lobby);

        gameData.inGameState.Subscribe(state => ObserveInGameState(state));
        gameData.gameState.Subscribe(state => ObserveGameState(state));
    }

    void SetFrameRate()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#else
        QualitySettings.vSyncCount = 1;
#endif
    }

    void ObserveInGameState(EInGameState state)
    {
        AudioManager.instance.ChangeBGM_InGame(state);

        CheckGameOver(state);
    }

    void ObserveGameState(EGameState state)
    {
        AudioManager.instance.ChangeBGM_Game(state);

        switch (state)
        {
            case EGameState.Lobby:
                SetLobby();
                break;
            case EGameState.InGame:
                SetInGame();
                break;
            case EGameState.End:
                SetEnd();
                break;
        }
    }

    void SetLobby()
    {
        LobbyManager.instance.mushroom.SetMushroomText(saveData.mushroom);
    }

    void SetInGame()
    {
        gameData.inGameState.Value = EInGameState.Default;
    }

    void SetEnd()
    {

    }

    void CheckGameOver(EInGameState state)
    {
        if (state == EInGameState.GameOver)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        AddMushroom();
        ResultController.instance.ShowResultDirecting();
    }

    public void NextChapter()
    {
        gameData.chapterType = (EChapterType)(gameData.chapterType + 1);

        #region Background
        BackgroundController.instance.ConvertBackground(gameData.chapterType);
        #endregion

        #region Paritlce
        ScreenParticleController.instance.ConvertParitlce(gameData.chapterType);
        #endregion

        #region Progress
        ProgressController.instance.ResetProgressValue();
        HUD.instance.progress.SetMaxValue();
        #endregion

        #region BGM
        AudioManager.instance.ChangeBGM_Chapter(gameData.chapterType);
        #endregion
    }

    public void ChangeInGameState(EInGameState state)
    {
        if (gameData.inGameState.Value == state)
            return;

        gameData.inGameState.Value = state;
    }

    void AddMushroom()
    {
        int unit = 100;

        saveData.mushroom += (int)ProgressController.instance.ProgressValue.Value / unit;
    }

    void SaveData()
    {
        string jsonData = JsonUtility.ToJson(saveData, true);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, jsonData);
    }

    void LoadData()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsonData = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(jsonData);
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene(SceneDefine.LOBBY);
    }

    public void GoToInGame()
    {
        SceneManager.LoadScene(SceneDefine.INGAME);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
