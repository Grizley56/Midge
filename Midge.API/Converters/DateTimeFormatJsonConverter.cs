using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Midge.API.Converters
{
	public class DateTimeFormatJsonConverter: JsonConverter<DateTime>
	{
		public readonly string Format;

		public DateTimeFormatJsonConverter(string format)
		{
			Format = format;
		}
		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString(Format));
		}

		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			string dt = (string) reader.Value;
			return DateTime.ParseExact(dt, Format, CultureInfo.InvariantCulture);
		}

	}
}
