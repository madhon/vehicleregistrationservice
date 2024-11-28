namespace VehicleRegistrationService.Certificates;

using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

internal sealed class SigningAudienceCertificate : IDisposable
{
    private readonly RSA rsa = RSA.Create();

    public SigningCredentials GetAudienceSigningKey()
    {
        var privateXmlKey = File.ReadAllText("./private_key.xml");
        rsa.FromXmlString(privateXmlKey);

        return new SigningCredentials(
            key: new RsaSecurityKey(rsa),
            algorithm: SecurityAlgorithms.RsaSha256);
    }

    public void Dispose()
    {
        rsa?.Dispose();
    }
}