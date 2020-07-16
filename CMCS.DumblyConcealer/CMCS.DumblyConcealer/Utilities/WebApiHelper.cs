using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CMCS.Common.Entities.Sys;
using Newtonsoft.Json;

namespace CMCS.DumblyConcealer.Utilities
{
    public class WebApiHelper
    {
        public string HttpApi(string url, string jsonstr, string type)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html,applicaton/xhtml+xml,*/*";
            request.ContentType = "application/json";
            request.Method = type.ToUpper().ToString();
            byte[] buffer = encoding.GetBytes(jsonstr);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }

    /// <summary>
    /// API调用结果(无返回值)
    /// </summary>
    public class ApiBaseResult
    {
        public string targetUrl { get; set; }
        public bool success { get; set; }
        public Error error { get; set; }

        public bool unAuthorizedRequest { get; set; }

        public bool __abp { get; set; }
    }
    /// <summary>
    /// API调用结果(带分页的返回值，集合和数据总条数)
    /// </summary>
    public class ApiPageResult<T> : ApiBaseResult
    {
        public PageResult<T> result { get; set; }
    }
    /// <summary>
    /// API调用结果(只返回集合)
    /// </summary>
    public class ApiListResult<T> : ApiBaseResult
    {
        public ListResult<T> result { get; set; }
    }

    /// <summary>
    /// API调用结果(只返回集合)
    /// </summary>
    public class ApiList<T> : ApiBaseResult
    {
        public List<T> result { get; set; }
    }

    /// <summary>
    /// API调用结果(返回实体)
    /// </summary>
    public class ApiEntityResult<T> : ApiBaseResult
    {
        public T result { get; set; }
    }

    public class PageResult<T>
    {
        public int totalCount { get; set; }
        public IList<T> items { get; set; }
    }

    public class ListResult<T>
    {
        public IList<T> items { get; set; }
    }
}
