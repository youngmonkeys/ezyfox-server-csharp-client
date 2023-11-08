using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.security
{
    public class EzyKeyPairGentor
    {
        private readonly int keySize;

        protected EzyKeyPairGentor(Builder builder)
        {
            this.keySize = builder._keySize;
        }

        public EzyKeyPair generate()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
            {
                RSAParameters publicKeyParameters = rsa.ExportParameters(false);
                X509Key x509Key = new X509Key(
                    publicKeyParameters.Modulus,
                    publicKeyParameters.Exponent
                );
                byte[] publicKeyBytes = x509Key.getEncoded();
                byte[] privateKeyBytes = Encoding.UTF8.GetBytes(rsa.ToXmlString(true));
                return new EzyKeyPair(privateKeyBytes, publicKeyBytes);
            }
        }

        public static Builder builder()
        {
            return new Builder();
        }

        public class Builder : EzyBuilder<EzyKeyPairGentor>
        {
            public int _keySize = 2048;

            public Builder keySize(int keySize)
            {
                this._keySize = keySize;
                return this;
            }

            public EzyKeyPairGentor build()
            {
                return new EzyKeyPairGentor(this);
            }
        }
    }

    public class X509Key
    {
        private byte[] key;
        private readonly AlgorithmId algid;
        private int unusedBits;
        private BitArray bitStringKey;
        private byte[] encodedKey;

        public X509Key(byte[] modulus, byte[] exponent)
        {
            this.algid = new AlgorithmId();
            DerOutputStream outputStream = new DerOutputStream();
            outputStream.putInteger(new BigInteger(modulus, true, true));
            outputStream.putInteger(new BigInteger(exponent, true, true));
            byte[] bytes = (new DerValue((byte)48, outputStream.toByteArray())).toByteArray();
            this.setKey(new BitArray(bytes.Length * 8, bytes));
        }

        protected void setKey(BitArray bitArray)
        {
            this.bitStringKey = (BitArray)bitArray.clone();
            this.key = bitArray.toByteArray();
            int remaining = bitArray.length() % 8;
            this.unusedBits = remaining == 0 ? 0 : 8 - remaining;
        }

        public byte[] getEncoded()
        {
            DerOutputStream outputStream = new DerOutputStream();
            encode(outputStream);
            return outputStream.toByteArray();
        }

        private void encode(DerOutputStream stream)
        {
            encode(stream, this.getKey());
        }

        private void encode(DerOutputStream outputStream, BitArray key)
        {
            DerOutputStream tmp = new DerOutputStream();
            algid.encode(tmp);
            tmp.putUnalignedBitString(key);
            outputStream.write(DerValue.tag_Sequence, tmp);
        }

        private BitArray getKey()
        {
            this.bitStringKey = new BitArray(this.key.Length * 8 - this.unusedBits, this.key);
            return this.bitStringKey.clone();
        }
    }

    public class DerValue
    {
        private byte tag;
        private DerInputBuffer buffer;
        private readonly DerInputStream data;
        private int length;

        public readonly static byte tag_BitString = 0x03;
        public readonly static byte tag_Integer = 0x02;
        public readonly static byte tag_ObjectId = 0x06;
        public readonly static byte tag_Null = 0x05;
        public readonly static byte tag_Sequence = 0x30;

        public DerValue(byte tag, byte[] data)
        {
            this.tag = tag;
            this.buffer = new DerInputBuffer((byte[])data.Clone());
            this.length = data.Length;
            this.data = new DerInputStream(this.buffer);
            this.data.mark();
        }

        public byte[] toByteArray()
        {
            DerOutputStream outputStream = new DerOutputStream();
            this.encode(outputStream);
            this.data.reset();
            return outputStream.toByteArray();
        }

        public void encode(DerOutputStream outputStream)
        {
            outputStream.write(this.tag);
            outputStream.putLength(this.length);
            if (this.length > 0)
            {
                byte[] value = new byte[this.length];
                this.buffer.reset();
                if (this.buffer.read(value) != this.length)
                {
                    throw new IOException("short DER value read (encode)");
                }
                outputStream.write(value);
            }
        }
    }

    public class DerInputStream
    {
        private readonly DerInputBuffer buffer;

        public DerInputStream(DerInputBuffer data)
        {
            this.buffer = data;
            this.buffer.mark();
        }

        public void mark()
        {
            this.buffer.mark();
        }

        public void reset()
        {
            this.buffer.reset();
        }
    }

    public class DerInputBuffer
    {
        private long _mark;
        private readonly MemoryStream stream;

        public DerInputBuffer(byte[] buf)
        {
            this.stream = new MemoryStream(buf);
        }

        public int read(byte[] bytes)
        {
            return this.stream.Read(bytes);
        }

        public void mark()
        {
            this._mark = this.stream.Position;
        }

        public void reset()
        {
            this.stream.Position = this._mark;
        }
    }

    public class AlgorithmId
    {
        private readonly ObjectIdentifier algid = RSAEncryption_oid;

        private static readonly ObjectIdentifier RSAEncryption_oid =
            new ObjectIdentifier(
                new byte[] { 42, 0x86, 72, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 }
            );

        public void encode(DerOutputStream var1)
        {
            DerOutputStream bytes = new DerOutputStream();
            DerOutputStream tmp = new DerOutputStream();
            bytes.putOID(this.algid);
            bytes.putNull();
            tmp.write(DerValue.tag_Sequence, bytes);
            var1.write(tmp.toByteArray());
        }
    }

    public class ObjectIdentifier
    {

        private readonly byte[] encoding;

        public ObjectIdentifier(byte[] encoding)
        {
            this.encoding = encoding;
        }

        public void encode(DerOutputStream outputStream)
        {
            outputStream.write(DerValue.tag_ObjectId, this.encoding);
        }
    }

    public class BitArray
    {
        private readonly byte[] repn;
        private readonly int _length;

        private BitArray(BitArray ba)
        {
            this._length = ba._length;
            this.repn = (byte[])ba.repn.Clone();
        }

        public BitArray(int length, byte[] a)
        {
            if (length < 0)
            {
                throw new ArgumentException("Negative length for BitArray");
            }
            else if (a.Length * 8 < length)
            {
                throw new ArgumentException("Byte array too short to represent bit array of given length");
            }
            else
            {
                this._length = length;
                int repLength = (length + 8 - 1) / 8;
                int unusedBits = repLength * 8 - length;
                byte bitMask = (byte)(255 << unusedBits);
                this.repn = new byte[repLength];
                Array.Copy(a, this.repn, repLength);
                if (repLength > 0)
                {
                    repn[repLength - 1] &= bitMask;
                }

            }
        }

        public byte[] toByteArray()
        {
            return (byte[])this.repn.Clone();
        }

        public int length()
        {
            return this._length;
        }

        public BitArray clone()
        {
            return new BitArray(this);
        }
    }

    public class DerOutputStream
    {
        private readonly MemoryStream stream = new MemoryStream();

        public void write(byte b)
        {
            stream.WriteByte(b);
        }

        public void write(sbyte b)
        {
            stream.WriteByte((byte)b);
        }

        public void write(byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        public void write(byte tag, byte[] buf)
        {
            this.write(tag);
            this.putLength(buf.Length);
            this.write(buf, 0, buf.Length);
        }

        public void write(byte[] b, int off, int len)
        {
            stream.Write(b, off, len);
        }

        public void write(byte tag, DerOutputStream outputStream)
        {
            this.write(tag);
            int count = outputStream.count();
            this.putLength(count);
            this.write(outputStream.toByteArray(), 0, count);
        }

        public void putInteger(BigInteger value)
        {
            this.write(DerValue.tag_Integer);
            byte[] buf = value.ToByteArray();
            EzyBytes.swapBytes(buf);
            this.putLength(buf.Length);
            this.write(buf, 0, buf.Length);
        }

        public void putNull()
        {
            this.write(DerValue.tag_Null);
            this.putLength(0);
        }

        public void putUnalignedBitString(BitArray var1)
        {
            byte[] bits = var1.toByteArray();
            this.write(DerValue.tag_BitString);
            this.putLength(bits.Length + 1);
            this.write((byte)(bits.Length * 8 - var1.length()));
            this.write(bits);
        }

        public void putOID(ObjectIdentifier oid)
        {
            oid.encode(this);
        }

        public void putLength(int len)
        {
            if (len < 128)
            {
                this.write((byte)len);

            }
            else if (len < (1 << 8))
            {
                this.write((byte)0x081);
                this.write((byte)len);

            }
            else if (len < (1 << 16))
            {
                this.write((byte)0x082);
                this.write((byte)(len >> 8));
                this.write((byte)len);

            }
            else if (len < (1 << 24))
            {
                this.write((byte)0x083);
                this.write((byte)(len >> 16));
                this.write((byte)(len >> 8));
                this.write((byte)len);

            }
            else
            {
                this.write((byte)0x084);
                this.write((byte)(len >> 24));
                this.write((byte)(len >> 16));
                this.write((byte)(len >> 8));
                this.write((byte)len);
            }
        }

        public int count()
        {
            return (int)stream.Length;
        }

        public byte[] toByteArray()
        {
            return stream.ToArray();
        }
    }
}
