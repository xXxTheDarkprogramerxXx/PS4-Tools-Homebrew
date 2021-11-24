using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Assets.Code.Wrapper
{
    /// <summary>
    /// Implementation of the PS4 Store Api
    /// </summary>
    public class PS4_chihiro_API
    {
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
            string URL = "https://store.playstation.com/store/api/chihiro/00_09_000/titlecontainer/" + region + "/" + lang + "/"+Age +"/" + TitleId + "/";
            using (WebClient client = new WebClient())
            {
                //add protocols incase sony wants to add them
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Ssl3;
                //add a header cause sometimes they check this setting 
                //also we can make the Header match the ps4 if need be
                client.Headers.Add("user-agent", "Only a test!");


                string xmlfilecontent = client.DownloadString(URL);
                try
                {
                    string json = xmlfilecontent;
                    Assets.Code.Models.PS4_chihiro_model.PS4_chihiro_model_item myDeserializedClass = JsonConvert.DeserializeObject<Assets.Code.Models.PS4_chihiro_model.PS4_chihiro_model_item>(json);
                    rntItem = myDeserializedClass;
                }
                catch (Exception ex)
                {
                    //json changed ???
                }
            }
            return rntItem;
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
            using (WebClient client = new WebClient())
            {
                //add protocols incase sony wants to add them
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Ssl3;
                //add a header cause sometimes they check this setting 
                //also we can make the Header match the ps4 if need be
                client.Headers.Add("user-agent", "Only a test!"); 
                try
                {
                    byte[] content = client.DownloadData(URL);
                    return content;
                }
                catch (Exception ex)
                {
                    //json changed ???
                }
            }


            return null;
        }

    }
}
