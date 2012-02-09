using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.SqlServer.Server;

public class Storedfunctions
{
	[SqlFunction]
	public static SqlBoolean IsClose(Descriptor input, Descriptor fromDb, SqlDouble precision)
	{
		return input.Distance(fromDb) <= precision;
	}
}

[Serializable]
[SqlUserDefinedType(Format.UserDefined,
        MaxByteSize = 8000)]
public struct Descriptor : IBinarySerialize, INullable
{
	public bool IsNull { get; private set; }

	private double[] descriptor;

	public static Descriptor Null
	{
		get
		{
			var desc = new Descriptor { IsNull = true };
			return (desc);
		}
	}

	public override string ToString()
	{
		if (this.IsNull)
		{
			return "NULL";
		}

		var sb = new StringBuilder(this.descriptor.Length);
		sb.Append(this.descriptor[0]);

		for (var i = 1; i < this.descriptor.Length; i++)
		{
			sb.Append(':').Append(this.descriptor[i]);
		}

		return sb.ToString();
	}


	public static Descriptor Parse(SqlString s)
	{
		if (s.IsNull)
		{
			return Null;
		}

		var desc = new Descriptor();

		var str = Convert.ToString(s);
		var strDesc = str.Split(":".ToCharArray());

		desc.descriptor = new double[strDesc.Length];

		for (var i = 0; i < strDesc.Length; i++)
		{
			desc.descriptor[i] = Double.Parse(strDesc[i]);
		}

		return desc;
	}

	public SqlDouble Distance(Descriptor desc)
	{
		var sum = this.descriptor.Select((t, i) => Math.Abs(t - desc.descriptor[i])).Sum();

		return (sum/this.descriptor.Length);
	}

	public void Read(System.IO.BinaryReader r)
	{
		var str = Convert.ToString(r.ReadString());
		var strDesc = str.Split(":".ToCharArray());

		this.descriptor = new double[strDesc.Length];

		for (var i = 0; i < strDesc.Length; i++)
		{
			descriptor[i] = Double.Parse(strDesc[i]);
		}
	}

	public void Write(System.IO.BinaryWriter w)
	{
		w.Write(this.ToString());
	}
}
