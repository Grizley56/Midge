namespace Midge.Client.Mobile.Core
{
	public interface IDecrypter
	{
		byte[] Decrypt(byte[] data);

		byte[] Key { get; }
		byte[] IV { get; }
	}
}