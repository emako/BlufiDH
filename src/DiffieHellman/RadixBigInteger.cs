using System.Numerics;

namespace DiffieHellman;

/// <summary>
/// Extension of the <see cref="BigInteger"/> class that allows parsing a string representation of a number in a given radix.
/// </summary>
public readonly struct RadixBigInteger
{
    /// <summary>
    /// Parses a string representation of a number into a BigInteger,
    /// supporting both decimal (base 10) and hexadecimal (base 16) formats.
    /// </summary>
    /// <param name="value">The string representation of the number to parse.</param>
    /// <param name="radix">The base of the number system; 10 for decimal and 16 for hexadecimal. Default is 10.</param>
    /// <returns>A BigInteger that represents the parsed value.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid radix value is provided.</exception>
    public static BigInteger Parse(string value, int radix = 10)
    {
        return radix switch
        {
            10 => BigInteger.Parse(value),
            16 => new BigInteger(ToByteArray(value)),
            _ => throw new ArgumentException("Invalid radix value. Only 10 and 16 are supported."),
        };

        static byte[] ToByteArray(string hexString)
        {
            hexString = hexString.Trim();

            // Ensure the string length is even
            if (hexString.Length % 2 != 0)
            {
                hexString = "0" + hexString;
            }

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
