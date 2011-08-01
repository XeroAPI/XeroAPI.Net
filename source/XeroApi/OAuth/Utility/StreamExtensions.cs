using System.IO;
using System.Net;

namespace DevDefined.OAuth.Utility
{
  public static class StreamExtensions
  {
    public static string ReadToEnd(this Stream stream)
    {
        if (!stream.CanRead)
        {
            throw new EndOfStreamException("The stream cannot be read");
        }

        if (stream.CanSeek)
        {
            stream.Seek(0, 0);
        }

        var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static string ReadToEnd(this WebResponse response)
    {
      return response.GetResponseStream().ReadToEnd();
    }

    public static void CopyTo(this Stream fromStream, Stream toStream)
    {
        // Implementation taken from http://stackoverflow.com/questions/230128/best-way-to-copy-between-two-stream-instances-c

        // 1k buffer
        byte[] buffer = new byte[1024];

        while (true)
        {
            int read = fromStream.Read(buffer, 0, buffer.Length);

            if (read <= 0)
                return;

            toStream.Write(buffer, 0, read);
        }
    }
  }
}