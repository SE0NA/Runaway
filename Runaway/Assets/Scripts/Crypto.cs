using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Crypto
{
    private static string PASSWORD = "gCeKm4WNyjRal3AG";    // ��ȣ. 16���� �̻�

    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);    // 8bit ���� ����

    // ��ȣȭ -> Save
    public static string AESEncrypt128(string plain)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

        RijndaelManaged rijndael = new RijndaelManaged();   // ���δ� �˰���

        // ��ȣȭ ��� ����
        rijndael.Mode = CipherMode.CBC;

        // paddingMode: �޽��� ������ ����� ��ȣȭ �۾��� �ʿ��� ���̺��� ª�� �� �������� ä�� ������ ����
        rijndael.Padding = PaddingMode.PKCS7;

        // �н����� Ű ������
        rijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = rijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes= memoryStream.ToArray();
        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    // ��ȣȭ -> Load
    public static string AESDecrypt128(string encrypt)
    {
        byte[] encryptBytes = Convert.FromBase64String(encrypt);

        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.Mode = CipherMode.CBC;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(encryptBytes);
        ICryptoTransform decryptor = rijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }
}
