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

    class X509Key
    {
        private byte[] key;
        private readonly AlgorithmId algid;
        private int unusedBits = 0;
        private BitArray bitStringKey = null;
        protected byte[] encodedKey;

        public X509Key(byte[] modulus, byte[] exponent)
        {
            this.algid = new AlgorithmId();
            DerOutputStream var4 = new DerOutputStream();
            var4.putInteger(new BigInteger(modulus, true, true));
            var4.putInteger(new BigInteger(exponent, true, true));
            byte[] var5 = (new DerValue((byte)48, var4.toByteArray())).toByteArray();
            this.setKey(new BitArray(var5.Length * 8, var5));
        }

        protected void setKey(BitArray var1)
        {
            this.bitStringKey = (BitArray)var1.clone();
            this.key = var1.toByteArray();
            int var2 = var1.length() % 8;
            this.unusedBits = var2 == 0 ? 0 : 8 - var2;
        }

        public byte[] getEncoded()
        {
            DerOutputStream var2 = new DerOutputStream();
            encode(var2);
            return var2.toByteArray();
        }

        private void encode(DerOutputStream stream)
        {
            encode(stream, this.getKey());
        }

        private void encode(
            DerOutputStream var0,
            BitArray var2
        )
        {
            DerOutputStream var3 = new DerOutputStream();
            algid.encode(var3);
            var3.putUnalignedBitString(var2);
            var0.write((byte)48, var3);
        }

        private BitArray getKey()
        {
            this.bitStringKey = new BitArray(this.key.Length * 8 - this.unusedBits, this.key);
            return (BitArray)this.bitStringKey.clone();
        }
    }

    public class DerValue
    {
        public byte tag;
        protected DerInputBuffer buffer;
        public readonly DerInputStream data;
        private int length;

        public DerValue(byte var1, byte[] var2) : this(var1, var2, true)
        {
        }

        public DerValue(byte var1, byte[] var2, bool var3)
        {
            this.tag = var1;
            this.buffer = new DerInputBuffer((byte[])var2.Clone(), var3);
            this.length = var2.Length;
            this.data = new DerInputStream(this.buffer);
            this.data.mark(int.MaxValue);
        }

        public byte[] toByteArray()
        {
            DerOutputStream var1 = new DerOutputStream();
            this.encode(var1);
            this.data.reset();
            return var1.toByteArray();
        }

        public void encode(DerOutputStream var1)
        {
            var1.write(this.tag);
            var1.putLength(this.length);
            if (this.length > 0)
            {
                byte[] var2 = new byte[this.length];
                this.buffer.reset();
                if (this.buffer.read(var2) != this.length)
                {
                    throw new IOException("short DER value read (encode)");
                }

                var1.write(var2);
            }
        }
    }

    public class DerInputStream
    {
        private readonly DerInputBuffer buffer;

        public DerInputStream(DerInputBuffer var1)
        {
            this.buffer = var1;
            this.buffer.mark(int.MaxValue);
        }

        public void mark(int var1)
        {
            this.buffer.mark(var1);
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
        private readonly bool allowBER;

        public DerInputBuffer(byte[] var1, bool var2)
        {
            this.allowBER = var2;
            this.stream = new MemoryStream(var1);
        }

        public int read(byte[] bytes)
        {
            return this.stream.Read(bytes);
        }

        public void mark(int readAheadLimit)
        {
            this._mark = this.stream.Position;
        }

        public void reset()
        {
            this.stream.Position = this._mark;
        }
    }

    class AlgorithmId
    {
        private readonly ObjectIdentifier algid = RSAEncryption_oid;

        private static readonly ObjectIdentifier RSAEncryption_oid =
            new ObjectIdentifier(
                new byte[] { 42, 0x86, 72, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 }
            );

        public void encode(DerOutputStream var1)
        {
            DerOutputStream var2 = new DerOutputStream();
            DerOutputStream var3 = new DerOutputStream();
            var2.putOID(this.algid);
            var2.putNull();
            var3.write((byte)48, var2);
            var1.write(var3.toByteArray());
        }
    }

    public class ObjectIdentifier
    {

        private readonly byte[] encoding;

        public ObjectIdentifier(byte[] encoding)
        {
            this.encoding = encoding;
        }

        public void encode(DerOutputStream var1)
        {
            var1.write(6, this.encoding);
        }
    }

    public class BitArray
    {
        private readonly byte[] repn;
        private readonly int _length;

        private BitArray(BitArray var1)
        {
            this._length = var1._length;
            this.repn = (byte[])var1.repn.Clone();
        }

        public BitArray(int var1, byte[] var2)
        {
            if (var1 < 0)
            {
                throw new ArgumentException("Negative length for BitArray");
            }
            else if (var2.Length * 8 < var1)
            {
                throw new ArgumentException("Byte array too short to represent bit array of given length");
            }
            else
            {
                this._length = var1;
                int var3 = (var1 + 8 - 1) / 8;
                int var4 = var3 * 8 - var1;
                byte var5 = (byte)(255 << var4);
                this.repn = new byte[var3];
                Array.Copy(var2, this.repn, var3);
                if (var3 > 0)
                {
                    byte[] var10000 = this.repn;
                    var10000[var3 - 1] &= var5;
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

        public void write(byte var1, byte[] var2)
        {
            this.write(var1);
            this.putLength(var2.Length);
            this.write(var2, 0, var2.Length);
        }

        public void write(byte[] b, int off, int len)
        {
            stream.Write(b, off, len);
        }

        public void write(byte var1, DerOutputStream var2)
        {
            this.write(var1);
            int count = var2.count();
            this.putLength(count);
            this.write(var2.toByteArray(), 0, count);
        }

        public void writeInt(int value)
        {
            stream.Write(EzyBytes.getBytes(value));
        }

        public void putInteger(BigInteger var1)
        {
            this.write(2);
            byte[] var2 = var1.ToByteArray();
            EzyBytes.swapBytes(var2);
            this.putLength(var2.Length);
            this.write(var2, 0, var2.Length);
        }

        public void putNull()
        {
            this.write(5);
            this.putLength(0);
        }

        public void putUnalignedBitString(BitArray var1)
        {
            byte[] var2 = var1.toByteArray();
            this.write(3);
            this.putLength(var2.Length + 1);
            this.write((byte)(var2.Length * 8 - var1.length()));
            this.write(var2);
        }

        public void putOID(ObjectIdentifier var1)
        {
            var1.encode(this);
        }

        public void putLength(int var1)
        {
            if (var1 < 128)
            {
                this.write((byte)var1);
            }
            else if (var1 < 256)
            {
                this.write(-127);
                this.write((byte)var1);
            }
            else if (var1 < 65536)
            {
                this.write(-126);
                this.write((byte)(var1 >> 8));
                this.write((byte)var1);
            }
            else if (var1 < 16777216)
            {
                this.write(-125);
                this.write((byte)(var1 >> 16));
                this.write((byte)(var1 >> 8));
                this.write((byte)var1);
            }
            else
            {
                this.write(-124);
                this.write((byte)(var1 >> 24));
                this.write((byte)(var1 >> 16));
                this.write((byte)(var1 >> 8));
                this.write((byte)var1);
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
