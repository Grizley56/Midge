using System;
using Midge.Server.Services;

namespace Midge.Server
{
	public class BroadcastSettings: IEquatable<BroadcastSettings>
	{
		public int SampleRate { get; }
		public AudioChannel Channel { get; }
		public int BitDepth { get; }

		public BroadcastSettings(int sampleRate, AudioChannel channel, int bitDepth)
		{
			SampleRate = sampleRate;
			Channel = channel;
			BitDepth = bitDepth;
		}

		public bool Equals(BroadcastSettings other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return SampleRate == other.SampleRate && Channel == other.Channel && BitDepth == other.BitDepth;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((BroadcastSettings) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = SampleRate;
				hashCode = (hashCode * 397) ^ (int) Channel;
				hashCode = (hashCode * 397) ^ BitDepth;
				return hashCode;
			}
		}
	}
}