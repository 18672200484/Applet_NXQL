using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CMCS.DumblyConcealer.Utilities
{
    /// <summary>
    /// 高德地图调用帮助类
    /// 更多详情请参考 高德api
    /// </summary>
    public class GaodeHelper
    {
        //高德平台申请的秘钥
        public static string SecretKey = "948487e0d2ee833a3fd957ce02c27a76";

        //地球半径，单位米
        private const double EARTH_RADIUS = 6378137;

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

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lat1">第一点纬度</param>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <param name="lng2">第二点经度</param>
        /// <returns></returns>
        public static double GetTwoPointDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
    }
}
