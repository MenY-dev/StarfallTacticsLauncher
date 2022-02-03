using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class SfBinaryWriter : IDisposable
    {
        public Stream Stream { get; protected set; }

        public Encoding Encoding { get; set; } = Encoding.ASCII;

        protected BinaryWriter Writer { get; set; }

        protected bool CreatedFromBytes { get; }


        public SfBinaryWriter(Stream stream)
        {
            Stream = stream;
            Writer = new BinaryWriter(stream, Encoding, true);
        }

        public SfBinaryWriter(byte[] buffer) : this(new MemoryStream(buffer))
        {
            CreatedFromBytes = true;
        }

        public virtual void WriteText(string text) => WriteText(text, false, Encoding);

        public virtual void WriteText(string text, bool writeTextSize) => WriteText(text, writeTextSize, Encoding);

        public virtual void WriteText(string text, Encoding encoding) => WriteText(text, false, encoding);

        public virtual void WriteText(string text, bool writeTextSize, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(text);

            if (writeTextSize == true)
            {
                Stream.Write(BitConverter.GetBytes((ushort)buffer.Length), 0, 2);
            }

            Stream.Write(buffer, 0, buffer.Length);
        }

        public virtual void WriteByte(byte value) => Writer.Write(value);

        public virtual void WriteInt16(short value) => Writer.Write(value);

        public virtual void WriteInt32(int value) => Writer.Write(value);

        public virtual void WriteInt64(long value) => Writer.Write(value);

        public virtual void WriteUInt16(ushort value) => Writer.Write(value);

        public virtual void WriteUInt32(uint value) => Writer.Write(value);

        public virtual void WriteUInt64(ulong value) => Writer.Write(value);

        public virtual void WriteSingle(float value) => Writer.Write(value);

        public virtual void WriteDouble(double value) => Writer.Write(value);

        public virtual void WriteDecimal(decimal value) => Writer.Write(value);

        public virtual void WriteChar(char value) => Writer.Write(value);

        public virtual void Write(byte[] buffer) => Writer.Write(buffer);

        public virtual void Write(byte[] buffer, int index, int count) => Writer.Write(buffer, index, count);


        public void Dispose()
        {
            if (CreatedFromBytes == true)
                Stream.Dispose();

            Stream = null;
            Writer.Dispose();
        }
    }
}
