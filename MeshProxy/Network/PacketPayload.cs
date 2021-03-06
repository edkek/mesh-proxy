﻿using System;
using System.Text;
using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;

namespace MeshProxy.Network
{
	public static class PacketPayload
	{
		public struct Reject
		{
			public string Reason { get; }

			public Reject(string reason)
			{
				this.Reason = reason;
			}

			public byte[] Compile()
			{
				var json = JsonConvert.SerializeObject(this);
				var jsonData = Encoding.UTF8.GetBytes(json);

				var compiled = new byte[jsonData.Length + 2];
				compiled[0] = 0x00;
				compiled[1] = 0x01;

				Array.Copy(jsonData, 0, compiled, 2, jsonData.Length);

				return compiled;
			}
		}

		public struct Handshake
		{
			public string Name { get; }
			public string[] KnownPeers { get; }
			public string Version { get; }

			public Handshake(string name, string version, string[] knownPeers)
			{
				this.Name = name;
				this.KnownPeers = knownPeers;
				this.Version = version;
			}

			public byte[] Compile()
			{
				var json = JsonConvert.SerializeObject(this);
				var jsonData = Encoding.UTF8.GetBytes(json);

				var compiled = new byte[jsonData.Length + 2];
				compiled[0] = 0x00;
				compiled[1] = 0x00;

				Array.Copy(jsonData, 0, compiled, 2, jsonData.Length);

				return compiled;
			}
		}

		public struct PacketForward
		{
			public byte[] data { get; private set; }
			public LinkLayers type { get; private set; }

			public PacketForward(RawCapture capture)
			{
				this.data = capture.Data;
				this.type = capture.LinkLayerType;
			}

            public PacketForward(LinkLayers type, byte[] data) {
                this.data = data;
                this.type = type;
            }

			public byte[] Compile()
			{
				//var json = JsonConvert.SerializeObject(this);

                //Console.WriteLine(json);

				//var jsonData = Encoding.UTF8.GetBytes(json);

                var compiled = new byte[data.Length + 1 + 2];
				compiled[0] = 0x01;
				compiled[1] = 0x03;
                compiled[2] = (byte)type;

                Array.Copy(data, 0, compiled, 3, data.Length);
				//Array.Copy(jsonData, 0, compiled, 2, jsonData.Length);

				return compiled;
			}
		}
	}
}