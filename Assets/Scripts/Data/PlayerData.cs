using UniRx;

public class PlayerData
{
    public int life;
    public int maxLife;
    public float hp;
    public float maxHP;

    public PlayerData()
    {
        maxLife = 2;
        maxHP = 100f;

        life = maxLife;
        hp = maxHP;
    }
}
