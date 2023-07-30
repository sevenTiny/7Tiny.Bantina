﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SevenTiny.Bantina.Net.Http
{
    public abstract class HttpHelper
    {
        private static string CommonProcess(CommonRequestArgs commonRequestArgs, Func<HttpClient, Byte[]> func)
        {
            var handler = new HttpClientHandler() { UseCookies = true };
            using (HttpClient client = new HttpClient(handler))
            {
                if (commonRequestArgs.Headers != null)
                {
                    foreach (KeyValuePair<string, string> header in commonRequestArgs.Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                        client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
                    }
                }
                if (commonRequestArgs.Timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, commonRequestArgs.Timeout);
                }
                Byte[] bytes = func(client);
                return Encoding.UTF8.GetString(bytes);
            }
        }
        private static async Task<string> CommonProcessAsync(CommonRequestArgs commonRequestArgs, Func<HttpClient, Task<Byte[]>> func)
        {
            using (HttpClient client = new HttpClient())
            {
                if (commonRequestArgs.Headers != null)
                {
                    foreach (KeyValuePair<string, string> header in commonRequestArgs.Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                if (commonRequestArgs.Timeout > 0)
                {
                    client.Timeout = new TimeSpan(0, 0, commonRequestArgs.Timeout);
                }
                Byte[] bytes = await func(client);
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public static string Get(GetRequestArgs args) => CommonProcess(args, client => client.GetByteArrayAsync(args.Url).Result);
        public static async Task<string> GetAsync(GetRequestArgs args)
        {
            return await CommonProcessAsync(args, client => client.GetByteArrayAsync(args.Url));
        }

        public static string Post(PostRequestArgs args)
        {
            return CommonProcess(args, client =>
            {
                using (HttpContent content = new StringContent(args.Data ?? string.Empty, args.Encoding ?? Encoding.UTF8))
                {
                    if (args.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(args.ContentType);
                    }
                    using (HttpResponseMessage responseMessage = client.PostAsync(args.Url, content).Result)
                    {
                        return responseMessage.Content.ReadAsByteArrayAsync().Result;
                    }
                }
            });
        }
        public static async Task<string> PostAsync(PostRequestArgs args)
        {
            return await CommonProcessAsync(args, client =>
            {
                using (HttpContent content = new StringContent(args.Data ?? string.Empty, args.Encoding ?? Encoding.UTF8))
                {
                    if (args.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(args.ContentType);
                    }
                    using (HttpResponseMessage responseMessage = client.PostAsync(args.Url, content).Result)
                    {
                        return responseMessage.Content.ReadAsByteArrayAsync();
                    }
                }
            });
        }

        public static string Put(PutRequestArgs args)
        {
            return CommonProcess(args, client =>
            {
                using (HttpContent content = new StringContent(args.Data ?? string.Empty, args.Encoding ?? Encoding.UTF8))
                {
                    if (args.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(args.ContentType);
                    }
                    using (HttpResponseMessage responseMessage = client.PutAsync(args.Url, content).Result)
                    {
                        return responseMessage.Content.ReadAsByteArrayAsync().Result;
                    }
                }
            });
        }
        public static async Task<string> PutAsync(PutRequestArgs args)
        {
            return await CommonProcessAsync(args, client =>
            {
                using (HttpContent content = new StringContent(args.Data ?? string.Empty, args.Encoding ?? Encoding.UTF8))
                {
                    if (args.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(args.ContentType);
                    }
                    using (HttpResponseMessage responseMessage = client.PutAsync(args.Url, content).Result)
                    {
                        return responseMessage.Content.ReadAsByteArrayAsync();
                    }
                }
            });
        }

        public static string Delete(DeleteRequestArgs args)
        {
            return CommonProcess(args, client =>
            {
                using (HttpContent content = new StringContent(args.Data ?? string.Empty, args.Encoding ?? Encoding.UTF8))
                {
                    if (args.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(args.ContentType);
                    }
                    using (HttpResponseMessage responseMessage = client.DeleteAsync(args.Url).Result)
                    {
                        return responseMessage.Content.ReadAsByteArrayAsync().Result;
                    }
                }
            });
        }
        public static async Task<string> DeleteAsync(DeleteRequestArgs args)
        {
            return await CommonProcessAsync(args, client =>
            {
                using (HttpContent content = new StringContent(args.Data ?? string.Empty, args.Encoding ?? Encoding.UTF8))
                {
                    if (args.ContentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(args.ContentType);
                    }
                    using (HttpResponseMessage responseMessage = client.DeleteAsync(args.Url).Result)
                    {
                        return responseMessage.Content.ReadAsByteArrayAsync();
                    }
                }
            });
        }
    }
}