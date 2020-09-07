using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TcpSocket
{
    public Action<string> ClientReceived;
    public Action<byte[]> ClientReceivedByte;

    private Socket m_socket;
    private byte[] m_data;
    private bool m_isServer;

    public TcpSocket(Socket socket, int dataLength, bool isServer)
    {
        m_socket = socket;
        m_data = new byte[dataLength];
        m_isServer = isServer;
    }


    public void ClientReceive()
    {
        if (ClientConnected)
            m_socket.BeginReceive(m_data, 0, m_data.Length, SocketFlags.None, new AsyncCallback(ClientEndReceiver), null);
    }

    public void ClientEndReceiver(IAsyncResult result)
    {
        int recevieLength = m_socket.EndReceive(result);

        string dataStr = System.Text.Encoding.UTF8.GetString(m_data, 0, recevieLength);
    
        if (ClientReceived != null)
            ClientReceived(dataStr);

    }


    public void ClientSend(byte[] data)
    {
        m_socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ClientSendEnd), null);
    }

    public void ClientSendEnd(IAsyncResult result)
    {
        m_socket.EndSend(result);
    }

    public void ClientConnect(string ip, int port)
    {
        m_socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(ClientEndConnect), null);
    }

    public void ClientEndConnect(IAsyncResult result)
    {
        if (result.IsCompleted)
        {
            Debug.Log("client connect success");
        }

        m_socket.EndConnect(result);
    }

    public void DisConnect()
    {
        //m_socket.Shutdown();
        m_socket.Disconnect(true);
        m_socket.Close();

    }

    public bool ClientConnected
    {
        get
        {
            return m_socket.Connected && !m_socket.Poll(10, SelectMode.SelectRead);
        }
    }


    public static byte[] ConvertDoubleArrayToBytes(List<float> matrix)
    {
        if (matrix == null)
        {
            return new byte[0];
        }
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryWriter bw = new BinaryWriter(stream);
            foreach (var item in matrix)
            {
                bw.Write(item);
            }
            return stream.ToArray();
        }
    }

    static List<float> ConvertBytesToFloatArray(byte[] matrix)
    {
        if (matrix == null)
            return null;

        List<float> result = new List<float>();
        using (var br = new BinaryReader(new MemoryStream(matrix)))
        {
            var ptCount = matrix.Length / 4;
            for (int i = 0; i < ptCount; i++)
            {
                result.Add(br.ReadSingle());
            }
            return result;
        }
    }

    public static byte[] ConvertDoubleArrayToBytes(List<int> matrix)
    {
        if (matrix == null)
        {
            return new byte[0];
        }
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryWriter bw = new BinaryWriter(stream);
            foreach (var item in matrix)
            {
                bw.Write(item);
            }
            return stream.ToArray();
        }
    }

    static List<int> ConvertBytesToIntArray(byte[] matrix)
    {
        if (matrix == null)
            return null;

        List<int> result = new List<int>();
        using (var br = new BinaryReader(new MemoryStream(matrix)))
        {
            var ptCount = matrix.Length / 4;
            for (int i = 0; i < ptCount; i++)
            {
                result.Add(br.ReadInt32());
            }
            return result;
        }
    }
}
