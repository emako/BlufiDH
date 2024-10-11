using System.Numerics;

namespace DiffieHellman.Test;

internal static class Program
{
    public static void Main()
    {
        const string DH_P = "cf5cf5c38419a724957ff5dd323b9c45c3cdd261eb740f69aa94b8bb1a5c9640" +
           "9153bd76b24222d03274e4725a5406092e9e82e9135c643cae98132b0d95f7d6" +
           "5347c68afc1e677da90e51bbab5f5cf429c291b4ba39c6b2dc5e8c7231e46aa7" +
           "728e87664532cdf547be20c9a3fa8342be6e34371a27c06f7dc0edddd2f86373";
        const string DH_G = "2";
        const int radix = 16;
        const int dhLength = 1024;
        BigInteger dhG = BigInteger.Parse(DH_G);
        BigInteger dhP = RadixBigInteger.Parse(DH_P, radix);

        BlufiDH appDH = new(dhG, dhP, dhLength);
        BlufiDH espDH = new(dhG, dhP, dhLength);

        BigInteger A = appDH.GetPublicKey();
        BigInteger B = espDH.GetPublicKey();

        BigInteger keyA = appDH.GenerateSecretKey(B);
        Console.WriteLine($"APP computed key: {keyA}");

        BigInteger keyB = espDH.GenerateSecretKey(A);
        Console.WriteLine($"ESP computed key: {keyB}");

        if (keyA == keyB)
        {
            Console.WriteLine("Key exchange successful! Both parties have the same shared key.");
        }
        else
        {
            Console.WriteLine("Key exchange failed! The keys do not match.");
        }
    }
}
