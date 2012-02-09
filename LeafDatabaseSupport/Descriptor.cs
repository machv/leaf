using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedType(Format.UserDefined,
        MaxByteSize = 8000)]
public struct Descriptor : IBinarySerialize, INullable
{
	public bool IsNull { get; private set; }

	public double[] descriptor;

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

		var str = Convert.ToString(s);
		var strDesc = str.Split(':');

		var desc = new double[strDesc.Length];

		for (var i = 0; i < strDesc.Length; i++)
		{
			desc[i] = Double.Parse(strDesc[i]);
		}

		return new Descriptor { descriptor = desc };
	}

	public SqlString Distance(Descriptor desc)
	{
		var sum = this.descriptor.Select((t, i) => Math.Abs(t - desc.descriptor[i])).Sum();

		return (sum/this.descriptor.Length).ToString();
	}

	public SqlString Distance(SqlString desc)
	{
		var d = Descriptor.Parse(desc);

		var sum = this.descriptor.Select((t, i) => Math.Abs(t - d.descriptor[i])).Sum();

		return (sum / this.descriptor.Length).ToString();
	}

	public void Read(System.IO.BinaryReader r)
	{
		Parse(r.ReadString()).descriptor.CopyTo(this.descriptor,0);
	}

	public void Write(System.IO.BinaryWriter w)
	{
		w.Write(this.ToString());
	}
}

