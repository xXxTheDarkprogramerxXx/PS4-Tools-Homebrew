using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Wrapper
{
    /// <summary>
    /// Implementation of the PS4 Store Api
    /// </summary>
    public class PS4_chihiro_API
    {

        [DllImport("universal")]
        private static extern string DownloadString(string Url);

        /// <summary>
        /// Region Code needs to be Upper case
        /// </summary>
        public static string region = "US";
        /// <summary>
        /// System lanag code can also be used must be lowercase
        /// </summary>
        public static string lang = "en";
        /// <summary>
        /// Age of the person searching 
        /// Default is 999
        /// </summary>
        public static string Age = "999";




        /// <summary>
        /// Get a model by title ID
        /// (Title Id Must include _00)
        /// </summary>
        /// <param name="TitleId">e.g CUSA00053_00</param>
        /// <returns><see cref="Assets.Code.Models.PS4_chihiro_model.PS4_chihiro_model_item"/></returns>
        public static Assets.Code.Models.PS4_chihiro_model.PS4_chihiro_model_item GetGameInfoByTitleId(string TitleId)
        {
            Assets.Code.Models.PS4_chihiro_model.PS4_chihiro_model_item rntItem = new Models.PS4_chihiro_model.PS4_chihiro_model_item();
            string URL = "https://store.playstation.com/store/api/chihiro/00_09_000/titlecontainer/" + region + "/" + lang + "/" + Age + "/" + TitleId + "/";
            //using (WebClient client = new WebClient())
            {
                //add protocols incase sony wants to add them
                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Ssl3;
                //add a header cause sometimes they check this setting 
                //also we can make the Header match the ps4 if need be
                //client.Headers.Add("user-agent", "Only a test!");


                //string xmlfilecontent = client.DownloadString(URL);
                try
                {

                    //Unity not playing fair so ill make a c function
                    //var aesCrypto = new System.Security.Cryptography.AesCryptoServiceProvider();
                    //X509Certificate2 rootcert = new X509Certificate2("Assets/Scripts/adminClient.p12", "xxxxxx");

                    //using (UnityWebRequest www = UnityWebRequest.Get(URL))
                    //{
                    //    www.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
                    //    var request = www.SendWebRequest();

                    //    while (!request.isDone)
                    //    {
                    //        Debug.Log("Download Stat: " + request.progress);
                    //    }
                    //    if (www.isNetworkError || www.isHttpError)
                    //    {
                    //        if (Application.platform == RuntimePlatform.PS4)
                    //        {
                    //            Util.SendMessageToPS4(www.error);
                    //        }
                    //        Debug.Log(www.error);
                    //    }
                    //    else
                    //    {
                    string json = DownloadString(URL);
                    Assets.Code.Models.PS4_chihiro_model.PS4_chihiro_model_item myDeserializedClass = JsonConvert.DeserializeObject<Assets.Code.Models.PS4_chihiro_model.PS4_chihiro_model_item>(json);
                    rntItem = myDeserializedClass;
                    //string savePath = string.Format("{0}/{1}.pdb", Application.persistentDataPath, file_name);
                    //System.IO.File.WriteAllText(savePath, www.downloadHandler.text);



                }
                catch (Exception ex)
                {
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        Assets.Code.MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                    //json changed ???
                }
            }
            return rntItem;
        }

        //        class AcceptAllCertificatesSignedWithASpecificPublicKey : CertificateHandler
        //{
        //    // Encoded RSAPublicKey
        //    private static string PUB_KEY = "30818902818100C4A06B7B52F8D17DC1CCB47362" +
        //        "C64AB799AAE19E245A7559E9CEEC7D8AA4DF07CB0B21FDFD763C63A313A668FE9D764E" +
        //        "D913C51A676788DB62AF624F422C2F112C1316922AA5D37823CD9F43D1FC54513D14B2" +
        //        "9E36991F08A042C42EAAEEE5FE8E2CB10167174A359CEBF6FACC2C9CA933AD403137EE" +
        //        "2C3F4CBED9460129C72B0203010001";

        //        protected override bool ValidateCertificate(byte[] certificateData)
        //        {
        //            X509Certificate2 certificate = new X509Certificate2(certificateData);

        //            string pk = certificate.GetPublicKeyString();

        //            return pk.Equals(PUB_KEY));
        //        }
        //    }


        private static bool MyRemoteCertificateValidationCallback(System.Object sender,
        X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain,
            // look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }


        /// <summary>
        /// Get the image of a product using the image resource
        /// (Title Id Must include _00)
        /// </summary>
        /// <param name="TitleId">e.g CUSA00053_00</param>
        /// <returns>byte[] of the image resource</returns>
        public static byte[] GetImageFromTitleId(string TitleId)
        {
            string URL = "https://store.playstation.com/store/api/chihiro/00_09_000/titlecontainer/" + region + "/" + lang + "/" + Age + "/" + TitleId + "/image";
            //using (WebClient client = new WebClient())
            //{
            //    //add protocols incase sony wants to add them
            //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Ssl3;
            //    //add a header cause sometimes they check this setting 
            //    //also we can make the Header match the ps4 if need be
            //    client.Headers.Add("user-agent", "Only a test!");
            //    try
            //    {
            //        byte[] content = client.DownloadData(URL);
            //        return content;
            //    }
            //    catch (Exception ex)
            //    {
            //        //json changed ???
            //    }
            //}
            try
            {
                var aesCrypto = new System.Security.Cryptography.AesCryptoServiceProvider();
                using (UnityWebRequest www = UnityWebRequest.Get(URL))
                {
                    var request = www.SendWebRequest();
                    while (!request.isDone)
                    {
                        if (www.isNetworkError || www.isHttpError)
                        {
                            return null;
                        }
                        Debug.Log("Download Stat: " + request.progress);
                    }
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        byte[] imgbytes = request.webRequest.downloadHandler.data;
                        return imgbytes;
                        //string savePath = string.Format("{0}/{1}.pdb", Application.persistentDataPath, file_name);
                        //System.IO.File.WriteAllText(savePath, www.downloadHandler.text);
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return null;
        }

    }
}
