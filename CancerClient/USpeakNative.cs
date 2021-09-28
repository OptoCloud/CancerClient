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
		private static extern bool Native_StreamMp3(IntPtr ptr, string path, Int32 pathLength);
		[DllImport("USpeakNative.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern Int32 Native_GetAudioFrame(IntPtr ptr, Int32 playerId, Int32 packetTime, IntPtr bufferPtr, Int32 bufferLength);
		[DllImport("USpeakNative.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern Int32 Native_RecodeAudioFrame(IntPtr ptr, IntPtr dataInPtr, Int32 dataInLength, IntPtr bufferPtr, Int32 bufferLength);

		public USpeakNative()
		{
			m_bufferGcHandle = GCHandle.Alloc(m_buffer, GCHandleType.Pinned);
			m_thisPtr = Native_CreateUSpeakLite();
		}
		~USpeakNative()
		{
			Native_DeleteUSpeakLite(m_thisPtr);
			m_bufferGcHandle.Free();
		}

		public bool StreamMp3(string path)
		{
			return Native_StreamMp3(m_thisPtr, path, path.Length);
		}

		public byte[] GetAudioFrame(Int32 playerId, Int32 packetTime)
		{
			lock (m_lock)
			{
				int nRead = Native_GetAudioFrame(m_thisPtr, playerId, packetTime, m_bufferGcHandle.AddrOfPinnedObject(), BufferSize);
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
				GCHandle dataInHandle = GCHandle.Alloc(m_buffer, GCHandleType.Pinned);

				int nRead = Native_RecodeAudioFrame(m_thisPtr, dataInHandle.AddrOfPinnedObject(), dataIn.Length, m_bufferGcHandle.AddrOfPinnedObject(), BufferSize);

				dataInHandle.Free();

				if (nRead <= 0)
				{
					return null;
				}

				byte[] frame = new byte[nRead];
				Buffer.BlockCopy(m_buffer, 0, frame, 0, nRead);

				return frame;
			}
		}

		private const Int32 BufferSize = 1024;

		private readonly IntPtr m_thisPtr;
		private object m_lock = new object();
		private byte[] m_buffer = new byte[BufferSize];
		private GCHandle m_bufferGcHandle;
	}
}
