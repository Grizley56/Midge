using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Midge.API.Models
{
	[Serializable]
	public class AudioStreamInfo
	{
		[JsonProperty("crypt_key")]
		public string Key { get; set; }
		[JsonProperty("crypt_iv")]
		public string IV { get; set; }
		[JsonProperty("sample_rate")]
		public int SampleRate { get; set; }
		[JsonProperty("channel")]
		public AudioChannel Channel { get; set; }
		[JsonProperty("alg")]
		public string Algorithm { get; set; }

		public AudioStreamInfo(string key, string iv, int sampleRate, AudioChannel channel, string algorithm)
		{
			Key = key;
			IV = iv;
			SampleRate = sampleRate;
			Channel = channel;
			Algorithm = algorithm;
		}

		public AudioStreamInfo()
		{
		}
	}
}
