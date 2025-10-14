namespace VehicleRegistrationService.Certificates;

using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

internal sealed class SigningAudienceCertificate : IDisposable
{
    private readonly RSA rsa = RSA.Create();
    private readonly Lazy<SigningCredentials> lazySigningCredentials;
    private bool disposed;

    public SigningAudienceCertificate()
    {
        lazySigningCredentials = new Lazy<SigningCredentials>(CreateSigningCredentials);
    }

    public SigningCredentials GetAudienceSigningKey()
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        return lazySigningCredentials.Value;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var privateXmlKey = File.ReadAllText("./private_key.xml");
        rsa.FromXmlString(privateXmlKey);

        return new SigningCredentials(
            key: new RsaSecurityKey(rsa),
            algorithm: SecurityAlgorithms.RsaSha256);
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