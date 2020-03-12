[System.Serializable]
public class Net_OnJoinGame : NetMsg
{
    public Net_OnJoinGame()
    {
        OperationCode = NetOP.OnJoinGame;
    }

    public byte Success { set; get; }
    public string Information { set; get; }
    public int ConnectionID { set; get; }
    public string Username { set; get; }
    public string Discriminator { set; get; }
    public string Token { set; get; }
}
