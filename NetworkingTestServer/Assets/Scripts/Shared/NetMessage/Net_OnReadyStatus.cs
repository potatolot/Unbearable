[System.Serializable]
public class Net_OnReadyStatus : NetMsg
{
    public Net_OnReadyStatus()
    {
        OperationCode = NetOP.OnReadyStatus;
    }

    public string Username { set; get; }
    public bool Status { set; get; }
}
