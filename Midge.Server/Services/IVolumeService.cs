namespace Midge.Server.Services
{
	public interface IVolumeService
	{
		int Volume { get; set; }
		bool IsMute { get; set; }
	}
}
