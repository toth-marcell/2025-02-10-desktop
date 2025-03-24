using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
namespace _2025_02_10_desktop
{
    public static class API
    {
        static readonly string URL = "http://localhost:8080/api";
        static readonly HttpClient http = new HttpClient();
        public static LoggedInUser ActiveUser { get; private set; }
        public static async Task<List<User>> GetUsers()
        {
            HttpResponseMessage response = await http.GetAsync(URL + "/profiles");
            string body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(body);
        }
        public static async Task Login(string name, string password)
        {
            string json = JsonConvert.SerializeObject(new LoginBody(name, password));
            HttpContent body = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.PostAsync(URL + "/login", body);
            string responseBody = await response.Content.ReadAsStringAsync();
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw new Exception(JsonConvert.DeserializeObject<APIError>(responseBody).Msg);
            }
            ActiveUser = JsonConvert.DeserializeObject<LoggedInUser>(responseBody);
        }
    }
    public class User
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class LoginBody
    {
        public string name { get; set; }
        public string password { get; set; }
        public LoginBody(string name, string password)
        {
            this.name = name;
            this.password = password;
        }
    }
    public class APIError
    {
        public string Msg { get; set; }
    }
    public class LoggedInUser
    {
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
