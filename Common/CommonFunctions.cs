using _AbsoPickUp.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace _AbsoPickUp.Common
{
    public static class CommonFunctions
    {
        public static string GenerateAccessToken(string userId, string deviceToken)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                  new Claim("UserId",userId),
                  new Claim("DeviceToken", deviceToken)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("QWERTY123456789012MaMina")), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public static string getUserId(this ClaimsPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
            {
                return "";
            }
            Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == "UserId");
            return identityClaim.Value.ToString();
        }

        public static int getFourDigitCode()
        {
            return new Random().Next(1000, 9999);
        }

        public static string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename[(filename.LastIndexOf("\\") + 1)..];
            return filename;
        }

        public static string RenameFileName(string filename)
        {
            var outFileName = "";
            if (filename != null || filename != "")
            {
                var extension = System.IO.Path.GetExtension(filename);
                outFileName = Guid.NewGuid().ToString() + extension;
            }
            return outFileName;

        }

        public static string EncryptPassword(string password)
        {
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            string strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        public static string Decrypt(string encryptpwd)
        {
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string decryptpwd = new string(decoded_char);
            return decryptpwd;
        }

        public static DistanceMatrixAPIResponse GoogleDistanceMatrixAPI(string source, string destination)
        {
            DistanceMatrixAPIResponse dmapir = new DistanceMatrixAPIResponse();

            var keyString = HttpUtility.UrlEncode(GlobalVariables.API_KEY); // passing API key
            string urlRequest = @"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins=place_id:" + HttpUtility.UrlEncode(source);
            urlRequest = urlRequest + "&destinations=place_id:" + HttpUtility.UrlEncode(destination);
            urlRequest += "&mode='driving'&sensor=false";
            urlRequest = urlRequest + "&key=" + keyString;

            WebRequest request = WebRequest.Create(urlRequest);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string resp = reader.ReadToEnd();
            var resJSON = JsonConvert.DeserializeObject<GoogleAPIResponseViewModel>(resp);
            dmapir.element = resJSON.Rows.FirstOrDefault().Elements.FirstOrDefault();
            dmapir.SourceAddress = resJSON.OriginAddresses.FirstOrDefault();
            dmapir.DestinationAddress = resJSON.DestinationAddresses.FirstOrDefault();
            reader.Close();
            dataStream.Close();
            response.Close();
            return dmapir;
        }
        public static DistanceMatrixAPIResponse GoogleDistanceMatrixAPILatLon(string olat, string olon, string dlat, string dlon)
        {
            DistanceMatrixAPIResponse dmapir = new DistanceMatrixAPIResponse();

            var keyString = HttpUtility.UrlEncode(GlobalVariables.API_KEY); // passing API key
            string urlRequest = @"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric";
            urlRequest = urlRequest + "&origins=" + HttpUtility.UrlEncode(olat+","+olon);
            urlRequest = urlRequest + "&destinations=" + HttpUtility.UrlEncode(dlat + "," + dlon);
            urlRequest += "&mode=driving&sensor=false";
            urlRequest = urlRequest + "&key=" + keyString;

            WebRequest request = WebRequest.Create(urlRequest);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string resp = reader.ReadToEnd();
            var resJSON = JsonConvert.DeserializeObject<GoogleAPIResponseViewModel>(resp);
            dmapir.element = resJSON.Rows.FirstOrDefault().Elements.FirstOrDefault();
            dmapir.SourceAddress = resJSON.OriginAddresses.FirstOrDefault();
            dmapir.DestinationAddress = resJSON.DestinationAddresses.FirstOrDefault();
            reader.Close();
            dataStream.Close();
            response.Close();
            return dmapir;
        }
        public static double GetDistanceFromLatLong(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist *= 1.609344;
                }
                else if (unit == 'N')
                {
                    dist *= 0.8684;
                }
                return (dist);
            }
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public static string GetAppStatusMessage(int statusId, string reason = "")
        {
            if (statusId == 1) return ResponseMessages.msgDocsIncomplete;
            if (statusId == 2) return ResponseMessages.msgDocUploadSuccess;
            if (statusId == 4) return ResponseMessages.msgDocsRejected + reason;
            return string.Empty;
        }
    }
}
