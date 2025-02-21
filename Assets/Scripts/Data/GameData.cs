using UniRx;

public enum EGameState
{
    Lobby,
    InGame,
    End
}

public enum EInGameState
{
    None,
    Default,
    Fever,
    SlowTime,
    SuperBooster,
    GameOver
}

public enum EChapterType
{
    Chapter1,
    Chapter2,
    Chapter3,
    Chapter4,
    Chapter5,
    End
}

public class GameData
{
    public ReactiveProperty<EGameState> gameState;
    public EChapterType chapterType;
    public ReactiveProperty<EInGameState> inGameState;
}
