using System.Security.Cryptography;
using System.Text;

var kernelPublicKey = "";
var kernelPrivateKey = "";

var agentPublicKey = "";
var agentPrivateKey = "";

var textToEncrypt = "Coco";
var clearBytes = Encoding.UTF8.GetBytes(textToEncrypt);
var cipherBytes = Array.Empty<byte>();
var cipherSignature = Array.Empty<byte>();

// Generate the Kernel Key Pair
using (var rsa = new RSACryptoServiceProvider())
{
    try
    {
        kernelPrivateKey = rsa.ExportPkcs8PrivateKeyPem();
        kernelPublicKey = rsa.ExportRSAPublicKeyPem();
    }
    finally
    {
        // Make sure to clear the RSA key pair from memory
        rsa.PersistKeyInCsp = false;
    }
}

// Generate the Agent Key Pair
using (var rsa = new RSACryptoServiceProvider())
{
    try
    {
        agentPrivateKey = rsa.ExportPkcs8PrivateKeyPem();
        agentPublicKey = rsa.ExportRSAPublicKeyPem();
    }
    finally
    {
        // Make sure to clear the RSA key pair from memory
        rsa.PersistKeyInCsp = false;
    }
}

// FROM Kernel to Agent
// encrypt with Agent Public Key
// sign with Kernel Private Key

using (var rsa = new RSACryptoServiceProvider())
{
    rsa.ImportFromPem(agentPublicKey);
    cipherBytes = rsa.Encrypt(clearBytes, RSAEncryptionPadding.Pkcs1);
    rsa.PersistKeyInCsp = false;
}
using (var rsa = new RSACryptoServiceProvider())
{
    rsa.ImportFromPem(kernelPrivateKey);
    cipherSignature = rsa.SignData(cipherBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    rsa.PersistKeyInCsp = false;
}

Console.WriteLine("Cipher Text: {0}", Convert.ToBase64String(cipherBytes));
// Verify with Kernel Public Key
using (var rsa = new RSACryptoServiceProvider())
{
    rsa.ImportFromPem(kernelPublicKey);
    if (rsa.VerifyData(cipherBytes, cipherSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
    {
        Console.WriteLine("VERIFIED");
    }

    rsa.PersistKeyInCsp = false;
}

// Decrypt with Agent Private
using (var rsa = new RSACryptoServiceProvider())
{
    rsa.ImportFromPem(agentPrivateKey);
    var x = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
    var s = Encoding.UTF8.GetString(x);
    Console.WriteLine("Clear Text: {0}", s);
    rsa.PersistKeyInCsp = false;
}
