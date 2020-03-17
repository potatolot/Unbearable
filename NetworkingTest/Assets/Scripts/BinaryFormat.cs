using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

static public class BinaryFormat
{
	static public void Format(byte[] buffer, NetMsg msg)
    {
		new BinaryFormatter().Serialize(new MemoryStream(buffer) , msg);
	}
	static public NetMsg DeFormat(byte[] recBuffer)
	{
		return (NetMsg)new BinaryFormatter().Deserialize(new MemoryStream(recBuffer));
	}
}
