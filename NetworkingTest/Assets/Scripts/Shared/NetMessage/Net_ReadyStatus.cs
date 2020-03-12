[System.Serializable]
public class Net_ReadyStatus : NetMsg
{
    public Net_ReadyStatus()
    {
        OperationCode = NetOP.ReadyStatus;
    }

    public string Username { set; get; }
    public bool Status { set; get; }
}
