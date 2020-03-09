[System.Serializable]
public class Net_CreateAccount : NetMsg
{
    public Net_CreateAccount()
    {
        OperationCode = NetOP.CreateAccount;
    }

    public string Username { set; get; }
    public string Password { set; get; }
    public string Email { set; get; }
}
