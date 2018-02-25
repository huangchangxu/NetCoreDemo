using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;
using System.Collections;

namespace PaymentCenter.PaymentCore.UcfPay
{
    public static class UcfUtils
    {
        //工具类： 
        //1、MD5处理  Md5Encrypt
        //2、生成唯一流水号
        //3、AES加密、解密处理
        //4、RSA公钥加密、解密、验签处理

        //---------------------------------------MD5处理------------------------------------------------------------

        public static String Md5Encrypt(String strSource)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes("" + strSource);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
            }
            return sTemp;
        }


        //---------------------------------------生成唯一流水号------------------------------------------------------------
        /// <summary>
        /// 生成唯一流水号
        /// </summary>
        /// <param name="merchantId">商户号</param>
        /// <param name="service">接口</param>
        /// <param name="merchantNo">商品名称</param>
        /// <returns></returns>
        public static String createUnRepeatCode(String merchantId, String service, String merchantNo)
        {
            String reqSn = "";
            if ((merchantId == null) || ("".Equals(merchantId)))
                return reqSn;
            if ((service == null) || ("".Equals(service)))
                return reqSn;
            if ((merchantNo == null) || ("".Equals(merchantNo)))
            {
                merchantNo = Convert.ToString(DateTime.Now.Millisecond);
            }
            StringBuilder strBuffer = new StringBuilder();
            String randomVal = Guid.NewGuid().ToString();
            strBuffer.Append(merchantId).Append(service).Append(merchantNo).Append(randomVal);
            reqSn = Md5Encrypt(strBuffer.ToString());
            return reqSn;
        }

        //---------------------------------------AES 处理------------------------------------------------------------  
        /// AES加密
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <returns></returns>
        public static String AESEncrypt(String text, String password)
        {
            String iv = "";
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.ECB;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            byte[] keyBytes = decodeHex(Md5Encrypt(password).ToUpper().ToCharArray());
            rijndaelCipher.Key = keyBytes;

            byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
            rijndaelCipher.IV = new byte[16];
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static String AESDecrypt(String text, String password)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.ECB;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(text.Replace(" ", "+"));

            byte[] keyBytes = decodeHex(Md5Encrypt(password).ToUpper().ToCharArray());
            rijndaelCipher.Key = keyBytes;
            //String iv = "";
            //byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
            //rijndaelCipher.IV = ivBytes;
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

        public static byte[] decodeHex(char[] data)
        {
            int len = data.Length;

            if ((len & 0x1) != 0)
            {
                return null;
            }

            byte[] outdata = new byte[len >> 1];

            int i = 0;
            for (int j = 0; j < len; ++i)
            {
                int f = toDigit(data[j], j) << 4;
                ++j;
                f |= toDigit(data[j], j);
                ++j;
                outdata[i] = (byte)(f & 0xFF);
            }

            return outdata;
        }

        public static int toDigit(char ch, int index)
        {
            int digit = Convert.ToInt32(ch.ToString(), 16);
            return digit;
        }


        //---------------------------------------RSA加密、验签处理------------------------------------------------------------

        /**
           * 根据传入的参数做加密
           * @param srcMsgData 加密用源串
           * @param publicKey 公钥串 
        */

        public static String RSAEncrypt(String srcMsgData, String publicKey)
        {
            byte[] pubKeyBytes = Convert.FromBase64String(publicKey);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            byte[] resultBytes = rsaPub.Encrypt(Encoding.UTF8.GetBytes(srcMsgData), false);
            String result = Convert.ToBase64String(resultBytes);
            return result;
        }


        /**
           * 根据传入的参数做解密
           * @param srcMsgData 解密用源串
           * @param publicKey 公钥串 
        */

        public static String RSADecrypt(String srcMsgData, String publicKey)
        {
            byte[] pubKeyBytes = Convert.FromBase64String(publicKey);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            BigInteger biN = new BigInteger(paraPub.Modulus);
            BigInteger biE = new BigInteger(paraPub.Exponent);

            byte[] dataBytes = Convert.FromBase64String(srcMsgData);
            int kLen = 128;
            int len = dataBytes.Length;
            int cycle = 0;
            if ((len % kLen) == 0)
                cycle = len / kLen;
            else
                cycle = len / kLen + 1;

            MemoryStream output = new MemoryStream();
            ArrayList temp = new ArrayList();
            int blockLen = 0;
            for (int i = 0; i < cycle; i++)
            {
                if (len >= kLen)
                    blockLen = kLen;
                else
                    blockLen = len;

                byte[] context = new byte[blockLen];
                int po = i * kLen;
                Array.Copy(dataBytes, po, context, 0, blockLen);

                BigInteger biText = new BigInteger(context);
                BigInteger biEnText = biText.modPow(biE, biN);

                byte[] buffer = biEnText.getBytes();
                int k = 2;
                while (buffer[k++] != 0) ;
                output.Write(buffer, k, buffer.Length - k);
                len -= blockLen;
            }
            byte[] result = output.ToArray(); //得到加密结果
            output.Close();
            return System.Text.Encoding.UTF8.GetString(result);
        }


        /**
             * 根据传入的参数做验签
             * @param srcMsgData 签名用源串
             * @param signMsgData 响应中给出的签名串
             * @param publicKey 公钥串 
             */

        public static bool RSAVerify(String srcMsgData, String signMsgData, String publicKey)
        {
            String signValue = RSADecrypt(signMsgData, publicKey);
            String signData = Md5Encrypt(srcMsgData).ToLower();
            if (!String.IsNullOrEmpty(signValue) && !String.IsNullOrEmpty(signData))
            {
                if (signValue.Equals(signData))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /**
         * 根据传入的参数做验签
         * @param srcMsgData 签名用源串
         * @param signMsgData 响应中给出的签名串
         * @param publicKey 公钥串 
         */

        public static String getSignData(Dictionary<string, object> dic, String signName)
        {
            Dictionary<String, object> dicAsc = (from d in dic orderby d.Key ascending select d).ToDictionary(
                k => k.Key, v => v.Value);
            if (!String.IsNullOrEmpty(signName))
                dicAsc.Remove(signName);

            String result = "";
            if (dicAsc.Count > 0)
            {
                foreach (KeyValuePair<String, object> kv in dicAsc)
                {
                    String value = (String)kv.Value;
                    value = (null == value) ? "" : value;
                    result = result + "&" + kv.Key + "=" + kv.Value;
                }
                if (!String.IsNullOrEmpty(result))
                {
                    if (result.StartsWith("&"))
                        result = result.Substring(1);
                }
            }
            return result;
        }
        public static String getSignData(Dictionary<String, String> dic, String signName)
        {
            Dictionary<String, String> dicAsc = (from d in dic orderby d.Key ascending select d).ToDictionary(
                k => k.Key, v => v.Value);
            if (!String.IsNullOrEmpty(signName))
                dicAsc.Remove(signName);

            String result = "";
            if (dicAsc.Count > 0)
            {
                foreach (KeyValuePair<String, String> kv in dicAsc)
                {
                    String value = (String)kv.Value;
                    value = (null == value) ? "" : value;
                    result = result + "&" + kv.Key + "=" + kv.Value;
                }
                if (!String.IsNullOrEmpty(result))
                {
                    if (result.StartsWith("&"))
                        result = result.Substring(1);
                }
            }
            return result;
        }
        /// <summary>
        /// 根据传入的参数做验签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="signName"></param>
        /// <returns></returns>
        public static string getSignData<T>(T model, string signName) where T : class
        {
           var dic = model.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(model) == null ? "" : p.GetValue(model).ToString());
           return getSignData(dic, signName);
        }



        /**
             * 根据传入的公钥字符串读取公钥
             * @param pemFileConent 公钥串 
             */

        private static RSAParameters ConvertFromPublicKey(String pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }


        //---------------------------------------------------------------------------------------------------  

    }

}