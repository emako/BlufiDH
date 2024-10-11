using System.Diagnostics;
using System.Numerics;

namespace DiffieHellman;

/// <summary>
/// Ported from https://github.com/EspressifApp/EspBlufiForAndroid/blob/master/lib-blufi/src/main/java/blufi/espressif/security/BlufiDH.java
/// </summary>
public class BlufiDH
{
    private const string TAG = "BlufiDH";

    /// <summary>
    /// Public values p for the Diffie-Hellman exchange
    /// </summary>
    protected readonly BigInteger mP = default;

    public BigInteger GetP() => mP;

    /// <summary>
    /// Public values g for the Diffie-Hellman exchange
    /// </summary>
    protected readonly BigInteger mG = default;

    public BigInteger GetG() => mG;

    /// <summary>
    /// Private key used in calculations
    /// </summary>
    protected BigInteger mPrivateKey = default;

    public BigInteger GetPrivateKey() => mPrivateKey;

    /// <summary>
    /// Intermediate result aka public key
    /// A = g^a mod p
    /// </summary>
    protected BigInteger mPublicKey = default;

    public BigInteger GetPublicKey() => mPublicKey;

    /// <summary>
    /// The shared secret key, initialized to -1
    /// </summary>
    protected BigInteger mSecretKey = -1;

    public BigInteger GetSecretKey() => mSecretKey;

    /// <summary>
    /// Constructor that initializes with provided public values 'g' and 'p'
    /// </summary>
    /// <param name="g">g for the Diffie-Hellman exchange</param>
    /// <param name="p">p for the Diffie-Hellman exchange</param>
    /// <param name="length"></param>
    public BlufiDH(BigInteger g, BigInteger p, int length)
    {
        mG = g;
        mP = p;

        GenerateKeys(mP, mG, length);
    }

    /// <summary>
    /// Constructor that initializes with provided public values 'g' and 'p'
    /// </summary>
    /// <param name="g">g for the Diffie-Hellman exchange</param>
    /// <param name="p">p for the Diffie-Hellman exchange</param>
    public BlufiDH(BigInteger g, BigInteger p) : this(g, p, 1024)
    {
    }

    /// <summary>
    /// Default constructor that generates random public values 'g' and 'p'
    /// </summary>
    public BlufiDH() : this(RandomStore.Shared.Next() % 96557, RandomStore.Shared.Next() % 1405695061, 1024)
    {
    }

    /// <summary>
    /// Method to generate the shared secret key using another party's public key 'B'
    /// </summary>
    /// <param name="y">Another party's public key 'B'</param>
    /// <returns>Shared secret key</returns>
    public BigInteger GenerateSecretKey(BigInteger y)
    {
        try
        {
            // Calculate the shared secret key
            // Key = B^a mod p
            mSecretKey = BigInteger.ModPow(y, mPrivateKey, mP);
            return mSecretKey;
        }
        catch (Exception e)
        {
            Debug.WriteLine(TAG + ": " + e);
        }
        return default;
    }

    private void GenerateKeys(BigInteger p, BigInteger g, int length)
    {
        try
        {
            if (length > 0)
            {
                byte[] bytes = new byte[length / 8];
                RandomStore.Shared.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F;
                mPrivateKey = new BigInteger(bytes);
            }
            else
            {
                mPrivateKey = RandomStore.Shared.Next() % 7321;
            }

            // A = g^a mod p
            mPublicKey = BigInteger.ModPow(g, mPrivateKey, p);
        }
        catch (Exception e)
        {
            Debug.WriteLine(TAG + ": " + e);
        }
    }
}

file static class RandomStore
{
#if NETSTANDARD
    public static Random Shared { get; } = new Random();

#else
    public static Random Shared => Random.Shared;
#endif
}
