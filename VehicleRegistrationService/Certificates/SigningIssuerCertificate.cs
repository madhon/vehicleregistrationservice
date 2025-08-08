namespace VehicleRegistrationService.Certificates;

using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

internal sealed class SigningIssuerCertificate : IDisposable
{
    private readonly RSA rsa = RSA.Create();

    public RsaSecurityKey GetIssuerSigningKey()
    {
        var publicXmlKey = File.ReadAllText("./public_key.xml");
        rsa.FromXmlString(publicXmlKey);

        return new RsaSecurityKey(rsa);
    }

    public void Dispose()
    {
        rsa?.Dispose();
    }
}