using System;
using System.Runtime.InteropServices;

namespace CancerClient.USpeak
{
	public class USpeakNative
	{
		[DllImport("USpeakNative.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr Native_CreateUSpeakLite();
		[DllImport("USpeakNative.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void Native_DeleteUSpeakLite(IntPtr ptr);
		[DllImport("USpeakNative.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool Native_StreamMp3(IntPtr ptr, string path, Int32 length);
		[DllImport("USpeakNative.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private static extern Int32 Native_GetAudioFrame(IntPtr ptr, byte[] data, Int32 dataLength, UInt32 actorNr, UInt32 packetTime);
		[DllImport("USpeakNative.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private static extern Int32 Native_RecodeAudioFrame(IntPtr ptr, byte[] dataIn, Int32 dataInLength, byte[] dataOut, Int32 dataOutLength);

		public USpeakNative()
		{
			m_thisPtr = Native_CreateUSpeakLite();
		}
		~USpeakNative()
		{
			Native_DeleteUSpeakLite(m_thisPtr);
		}

		public bool StreamMp3(string path)
		{
			return Native_StreamMp3(m_thisPtr, path, path.Length);
		}

		public byte[] GetAudioFrame(UInt32 actorNr, UInt32 packetTime)
		{
			lock (m_lock)
			{
				int nRead = Native_GetAudioFrame(m_thisPtr, m_buffer, m_buffer.Length, actorNr, packetTime);
				if (nRead <= 0)
				{
					return null;
				}

				byte[] frame = new byte[nRead];

				Buffer.BlockCopy(m_buffer, 0, frame, 0, nRead);

				return frame;
			}
		}

		public byte[] RecodeAudioFrame(byte[] dataIn)
		{
			lock (m_lock)
			{
				int nRead = Native_RecodeAudioFrame(m_thisPtr, dataIn, dataIn.Length, m_buffer, m_buffer.Length);
				if (nRead <= 0)
				{
					return null;
				}

				byte[] frame = new byte[nRead];

				Buffer.BlockCopy(m_buffer, 0, frame, 0, nRead);

				return frame;
			}
		}

		private readonly IntPtr m_thisPtr;
		private object m_lock = new object();
		private byte[] m_buffer = new byte[1022];
	}
}
