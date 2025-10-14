namespace VehicleRegistrationService.Certificates;

using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

internal sealed class SigningIssuerCertificate : IDisposable
{
    private readonly RSA rsa = RSA.Create();
    private readonly Lazy<RsaSecurityKey> lazySecurityKey;
    private bool disposed;

    public SigningIssuerCertificate()
    {
        lazySecurityKey = new Lazy<RsaSecurityKey>(CreateRsaSecurityKey);
    }

    public RsaSecurityKey GetIssuerSigningKey()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        return lazySecurityKey.Value;
    }

    private RsaSecurityKey CreateRsaSecurityKey()
    {
        var privateXmlKey = File.ReadAllText("./private_key.xml");
        rsa.FromXmlString(privateXmlKey);

        return new RsaSecurityKey(rsa);
    }

    public void Dispose()
    {
        if (!disposed)
        {
            rsa.Dispose();
            disposed = true;
        }
    }
}