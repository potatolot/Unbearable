[System.Serializable]
public class Net_JoinGame : NetMsg
{
    public Net_JoinGame()
    {
        OperationCode = NetOP.JoinGame;
    }

    public string Username { set; get; }
    public string Room { set; get; }
}
