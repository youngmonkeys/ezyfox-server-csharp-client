using System;
using System.Security.Cryptography;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.security
{
	public class EzyAsyCrypt
	{
        private readonly byte[] privateKey;

        protected EzyAsyCrypt(Builder builder)
        {
            this.privateKey = builder._privateKey;
        }

        public byte[] decrypt(byte[] message)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(convertToPrivateKeyParameters());
                return rsa.Decrypt(message, RSAEncryptionPadding.Pkcs1);
            }
        }

        private RSAParameters convertToPrivateKeyParameters()
        {
            RSAParameters privateKeyParameters = new RSAParameters();
            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKey, out _);
                privateKeyParameters.Modulus = rsa.ExportParameters(true).Modulus;
                privateKeyParameters.Exponent = rsa.ExportParameters(true).Exponent;
                privateKeyParameters.D = rsa.ExportParameters(true).D;
                privateKeyParameters.P = rsa.ExportParameters(true).P;
                privateKeyParameters.Q = rsa.ExportParameters(true).Q;
                privateKeyParameters.DP = rsa.ExportParameters(true).DP;
                privateKeyParameters.DQ = rsa.ExportParameters(true).DQ;
                privateKeyParameters.InverseQ = rsa.ExportParameters(true).InverseQ;
            }

            return privateKeyParameters;
        }

        public static Builder builder()
        {
            return new Builder();
        }

        public class Builder : EzyBuilder<EzyAsyCrypt>
        {
            public byte[] _privateKey;

            public Builder privateKey(byte[] privateKey)
            {
                this._privateKey = privateKey;
                return this;
            }

            public EzyAsyCrypt build()
            {
                return new EzyAsyCrypt(this);
            }
        }
    }
}

