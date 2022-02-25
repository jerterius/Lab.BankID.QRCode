using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LabMVCQRCode.Models;
using System.IO;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System.Net.Http.Headers;

using System.Drawing;
using ZXing;
using ZXing.QrCode;

using System.Text;
using System.Security.Cryptography;

namespace LabMVCQRCode.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //POST: Home/Auth
        [HttpPost]
        public AuthResponseModel Auth()
        {
            AuthResponseModel authResult;
            AuthRequestModel auth = new AuthRequestModel{endUserIp =  HttpContext.Connection.RemoteIpAddress.ToString()};
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key","secret");
                httpClient.DefaultRequestHeaders.Add("Authorization","Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCIsImtpZCI6Ik1yNS1BVWliZkJpaTdOZDFqQmViYXhib1hXMCJ9.eyJhdWQiOiJodHRwczovL2tyLnNsLnNlcnZpY2VzLmFwaS5rcmFmdHJpbmdlbi5zZS9rci5zbC5zZXJ2aWNlcy50ZXN0IiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMDdlYmIyMzktNTQxZi00OGMyLWEzNTMtMzA4Yjc3OTA4NTAzLyIsImlhdCI6MTY0NTAzMTM4OCwibmJmIjoxNjQ1MDMxMzg4LCJleHAiOjE2NDUwMzUyODgsImFpbyI6IkUyWmdZRGpPSHlIS2J2TG5iVTNxdlVjeU9TKzNBQUE9IiwiYXBwaWQiOiJmMmU0MGJjOS1mMjJkLTRiMWYtYWIxNi0xMDE2Zjk0ODM1ZTMiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8wN2ViYjIzOS01NDFmLTQ4YzItYTM1My0zMDhiNzc5MDg1MDMvIiwib2lkIjoiYmZkZjY1NTktMzQxNS00ZmNmLTk3NDAtMzJjM2NlODFmY2E4IiwicmgiOiIwLkFWMEFPYkxyQng5VXdraWpVekNMZDVDRkEzMkJVbm1pTTk5SWpXUjlhSkJXWVl0ZEFBQS4iLCJyb2xlcyI6WyJLUl9TTF9NZWFzdXJtZW50U2VydmljZXNfQVBJLlJlYWQiLCJLUl9TTF9TaWduaW5nU2VydmljZXNfQVBJLldyaXRlIiwiS1JfU0xfQ3VzdG9tZXJTZXJ2aWNlc19BUEkuUmVhZCIsIktSX1NMX1dvcmtmbG93U2VydmljZXNfQVBJLldyaXRlIiwiS1JfU0xfUHJvZHVjdENhdGFsb2d1ZV9BUEkuUmVhZCJdLCJzdWIiOiJiZmRmNjU1OS0zNDE1LTRmY2YtOTc0MC0zMmMzY2U4MWZjYTgiLCJ0aWQiOiIwN2ViYjIzOS01NDFmLTQ4YzItYTM1My0zMDhiNzc5MDg1MDMiLCJ1dGkiOiJsWHo4ekJXUzJrU29CalZtRzZXbEFBIiwidmVyIjoiMS4wIn0.JbQkGGsBZ520132tv-BEdnUXTUE1qVhryGNGRZAt13gEs_W0utET5xYv1PY2wufLRPqGiCgOqNeN_hbwoFOCvQeG1QIIe6pyCahdu21fhF-PHJv9CUM8kgHahoZE6fMWu9AEv6jnk36pCwPWvDKUDU4l9ejQypq7YAd8vpu133BkI8-gNROReQB6Cl1hsMj84GPtVuJEND-JvTQnzfvAwDkE1d2qYMg6-ueKHd1z0dkPuD1Yc5coZBZeNFdFvY9-A5aNZ_AQx8aFDD4JTJ-eiSAw8lsfdAsyhxzIEHl4NSKe_jFN5nzfdf2B6aqfzoldTeabP1XPsKfB0f0VSr13Ig");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                using (var response = httpClient.PostAsync("https://test-apim.kraftringen.se/bankid/v1/auth", content).Result)
                {
                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(content);
                    Console.WriteLine(apiResponse);
                    authResult = JsonConvert.DeserializeObject<AuthResponseModel>(apiResponse);
                    System.IO.File.WriteAllText (@"QRStartSecret.txt", authResult.QRStartSecret);
                    authResult.QRStartSecret = "secret";
                }
            }


           

            return authResult;
        }
        //POST: Home/RequestQR
        [HttpPost]
        public Byte[] RequestQR(QRCodeModel qRCodeModel)
        {
            Console.WriteLine(String.Format("qrStartToken is : {0}", qRCodeModel.QRStartToken));
            Console.WriteLine(String.Format("qrStartSecret is : {0}", qRCodeModel.QRStartSecret));
            Console.WriteLine(String.Format("qr_time is : {0}", qRCodeModel.QRTime));
            ///Code goes here to create 64 char hex from HMAC SHA 256

            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int order_time = (int)t.TotalSeconds;


            string qr_auth_code = HmacSHA256(qRCodeModel.QRStartSecret, qRCodeModel.QRTime);
            string qrText = String.Join(".", "bankid", qRCodeModel.QRStartToken, qRCodeModel.QRTime, qr_auth_code);


            ///

            Byte[] byteArray;
            var width = 250; // width of the Qr Code   
            var height = 250; // height of the Qr Code   
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var pixelData = qrCodeWriter.Write(qrText);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference   
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB   
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image   
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    // save to stream as PNG   
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = ms.ToArray();
                }
            }


            return byteArray;
        }
        public string HmacSHA256(string key, string data)
        {
            string hash;
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] code = encoder.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(code))
            {
                Byte[] hmBytes = hmac.ComputeHash(encoder.GetBytes(data));
                hash = ToHexString(hmBytes);
            }
            return hash;
        }

        public static string ToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            Console.WriteLine(hex);
            return hex.ToString();

        }
        //PYTHON
        //qr_auth_code = hmac.new(qr_start_secret, qr_time, hashlib.sha256).hexdigest()
        //hmac.new(key.encode('utf-8'), data.encode('utf-8'), hashlib.sha256).hexdigest()


    }
}
