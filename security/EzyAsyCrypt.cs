using System.Security.Cryptography;
using System.Text;
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
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(Encoding.UTF8.GetString(privateKey));
                return rsa.Decrypt(message, false);
            }
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
