using System;
using System.IO;
using Gx.Records;
using System.Xml.Serialization;

namespace Gx.CLI
{
	public class RecordSetParser
	{
		public int Parse(String file) {
			return Parse (new FileStream(file, FileMode.Open));
		}
		
		public int Parse(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(RecordSet));
			RecordSet records = (RecordSet) serializer.Deserialize(stream);
			return records.Records.Count;
		}
	}
}

