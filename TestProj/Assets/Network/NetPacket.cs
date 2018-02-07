using System;
using System.Net.Sockets;
//using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace UnityNetwork
{
	/// <summary>
	/// 网络数据包
	/// </summary>
	public class NetPacket
	{
		public const int INT32_LEN = 4;	//整型占用字节数
		public const int headerLength = 4;	//包头占用字节数
		public const int max_body_length = 512;	//包体最大字节数

		public int bodyLength { get; set;} // 当前数据长度

		// 包体总长度
		public int Length
		{
			get { return headerLength + bodyLength;}
		}
			
		public byte[] bytes{ get; set;}

		public Socket socket;
		public int readLength { get; set;}

		public NetPacket()
		{
			readLength = 0;
			bodyLength = 0;
			bytes = new byte[headerLength + max_body_length];
		}

		public NetPacket(NetPacket p)
		{
			bodyLength = p.bodyLength;
			bytes = new byte[p.bytes.Length];
			p.bytes.CopyTo (bytes, 0);
			readLength = p.readLength;
			socket = p.socket;
		}

		public void Reset ()
		{
			readLength = 0;
			bodyLength = 0;
		}

		public void BeginWrite (string msg)
		{
			bodyLength = 0;
			WriteString (msg);
		}

		public void WriteInt (int number)
		{
			if (bodyLength + INT32_LEN > max_body_length) 
			{
				return;
			}
			byte[] bs = System.BitConverter.GetBytes (number);
			bs.CopyTo (bytes, Length);
			bodyLength += INT32_LEN;
		}

		public void WriteString (string str)
		{
			int len = System.Text.Encoding.UTF8.GetByteCount (str);
			this.WriteInt (len);
			if (bodyLength + len > max_body_length) 
			{
				return;
			}
			System.Text.Encoding.UTF8.GetBytes (str, 0, str.Length, bytes, Length);
			bodyLength += len;
		}

		public void WriteStream (byte[] bs) 
		{
			WriteInt (bs.Length);
			if (bodyLength + bs.Length > max_body_length) 
			{
				return;
			}
			bs.CopyTo (bytes, Length);
			bodyLength += bs.Length;
		}

		public void WriteObject<T>(T t)
		{
			byte[] bs = Serialize<T> (t);
			WriteStream (bs);
		}



		public void BeginRead (out string msg)
		{
			bodyLength = 0;
			ReadString (out msg);
		}


		public void ReadInt (out int number)
		{
			number = 0;
			if (bodyLength + INT32_LEN > max_body_length) 
			{
				return;
			}

			number = System.BitConverter.ToInt32 (bytes, Length);
			bodyLength += INT32_LEN;
		}

		public void ReadString (out string str)
		{
			str = "";
			int len = 0;
			ReadInt (out len);
			if (bodyLength + len > max_body_length) 
			{
				return;
			}

			str = System.Text.Encoding.UTF8.GetString (bytes, Length, len);
			bodyLength += len;
		}

		public byte[] ReadStream ()
		{
			int size = 0;
			ReadInt (out size);
			if (bodyLength + size > max_body_length) 
			{
				return null;
			}
			byte[] bs = new byte[size];
			for (int i = 0; i < size; i++) 
			{
				bs[i] = bytes[Length + i];
			}
			bodyLength += size;
			return bs;
		}

		public T ReadObject<T>()
		{
			byte[] bs = ReadStream ();
			if (bs == null) {
				return default(T);
			}

			return Deserialize<T> (bs);
		}

		public void EncodeHeader()
		{
			byte[] bs = System.BitConverter.GetBytes (bodyLength);
			bs.CopyTo (bytes, 0);
		}

		public void DecodeHeader()
		{
			bodyLength = System.BitConverter.ToInt32 (bytes, 0);
		}

		public byte[] Serialize<T>(T t)
		{
			using (MemoryStream stream = new MemoryStream ()) 
			{
				try
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(stream, t);
					stream.Seek(0, SeekOrigin.Begin);
					return stream.ToArray();
				} 
				catch(Exception ex) 
				{
					Console.WriteLine (ex.Message);
					return null;
				}
			}
		}

		public T Deserialize<T>(byte[] bs)
		{
			using (MemoryStream stream = new MemoryStream ()) 
			{
				try
				{
					BinaryFormatter bf = new BinaryFormatter();
					T t = (T)bf.Deserialize(stream);
					return t;
				}
				catch(Exception ex) 
				{
					Console.Write ("Deserialize: " + ex.Message);
					return default(T);
				}
			}
		}
	}
}
