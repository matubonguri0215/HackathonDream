

public class PlayerDataProvider
{
    private Player _player;

    private static PlayerDataProvider Instance;
    public void InitProvider(Player player)
    {
        _player=player;
    }

    public static Player GetPlayer()
    {
        return Instance._player;
    }


}