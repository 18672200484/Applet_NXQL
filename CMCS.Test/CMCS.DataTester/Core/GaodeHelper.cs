using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CMCS.DataTester.Core
{
    /// <summary>
    /// 高德地图调用帮助类
    /// 更多详情请参考 高德api
    /// </summary>
    public class GaodeHelper
    {
        //高德平台申请的秘钥
        public static string SecretKey = "948487e0d2ee833a3fd957ce02c27a76";

        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static string GetGeocode(string address, string city)
        {
            string geocodeUrl = "http://restapi.amap.com/v3/geocode/geo?address={Address}&city={City}&output=json&key={SecretKey}"
                .Replace("{SecretKey}", SecretKey)
                .Replace("{Address}", address)
                .Replace("{City}", city);

            string geocode = WebClientDownloadInfoToString(geocodeUrl);
            geocode = GetLatitudeAndLongitude(geocode);
            return geocode;
        }

        /// <summary>
        /// 根据经纬度获取位置
        /// </summary>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static string GetLocation(string location)
        {
            string geocodeUrl = "http://restapi.amap.com/v3/geocode/regeo?key={SecretKey}&location={Location}&radius=1000&extensions=all&batch=false&roadlevel=0"
                .Replace("{SecretKey}", SecretKey)
                .Replace("{Location}", location);

            string geocode = WebClientDownloadInfoToString(geocodeUrl);
            geocode = GetLocationStr(geocode);
            return geocode;
        }

        /// <summary>
        /// 获取城市之间的距离
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="beginCity"></param>
        /// <param name="end"></param>
        /// <param name="endCity"></param>
        /// <returns></returns>
        public static string GetDistance(string begin, string beginCity, string end, string endCity)
        {
            string origin = GetGeocode(begin, beginCity);
            string destination = GetGeocode(end, endCity);
            string driveUri = "http://restapi.amap.com/v3/direction/driving?key={SecretKey}&origin={Origin}&destination={Destination}"
                .Replace("{SecretKey}", SecretKey)
                .Replace("{Origin}", origin)
                .Replace("{Destination}", destination);

            string result = WebClientDownloadInfo(driveUri);
            //var gd = Newtonsoft.Json.JsonConvert.DeserializeObject<GaodeReturn>(result);
            return result;
        }

        private static string WebClientDownloadInfo(string uri)
        {
            string result = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/xml;charset=UTF-8";
                result = wc.DownloadString(uri);
            }
            return result;
        }

        /// <summary>
        /// 模拟请求
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string WebClientDownloadInfoToString(string uri)
        {
            string result = string.Empty;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/xml;charset=UTF-8";
                result = wc.DownloadString(uri);
            }
            return result;
        }

        /// <summary>
        /// 解析返回的经纬度信息
        /// </summary>
        /// <param name="GeocodeJsonFormat"></param>
        /// <returns></returns>
        private static string GetLatitudeAndLongitude(string GeocodeJsonFormat)
        {
            JObject o = JObject.Parse(GeocodeJsonFormat);
            string geocodes = (string)o["geocodes"][0]["location"];
            return geocodes;
        }

        /// <summary>
        /// 解析返回的位置信息
        /// </summary>
        /// <param name="GeocodeJsonFormat"></param>
        /// <returns></returns>
        private static string GetLocationStr(string GeocodeJsonFormat)
        {
            JObject o = JObject.Parse(GeocodeJsonFormat);
            string geocodes = (string)o["regeocode"]["formatted_address"];
            return geocodes;
        }
    }
}
