using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Netcorex.Localizations;

namespace Netcorex.Common
{
	/// <summary>
	/// Basic Encryption/Decryption (C#)
	/// http://www.deltasblog.co.uk/code-snippets/basic-encryptiondecryption-c/
	/// </summary>
	public static class Encryption
	{
		internal const string DefaultKey = "YqbliAAsooG5210335l90164"; // 192 bits (24 length)


		internal static string Encrypt(string input, string key = DefaultKey)
		{
			if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(key))
			{
				return null;
			}
			try
			{
				byte[] inputArray = Encoding.UTF8.GetBytes(input);
				TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider
					{
						Key = Encoding.UTF8.GetBytes(key),
						Mode = CipherMode.ECB,
						Padding = PaddingMode.PKCS7
					};
				ICryptoTransform cTransform = tripleDes.CreateEncryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
				tripleDes.Clear();
				return Convert.ToBase64String(resultArray, 0, resultArray.Length);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Titles.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return null;
			}
		}

		internal static string Decrypt(string input, string key = DefaultKey)
		{
			if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(key))
			{
				return null;
			}
			try
			{
				byte[] inputArray = Convert.FromBase64String(input);
				TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider
					{
						Key = Encoding.UTF8.GetBytes(key),
						Mode = CipherMode.ECB,
						Padding = PaddingMode.PKCS7
					};
				ICryptoTransform cTransform = tripleDes.CreateDecryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
				tripleDes.Clear();
				return Encoding.UTF8.GetString(resultArray);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Titles.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return null;
			}
		}
	}
}
