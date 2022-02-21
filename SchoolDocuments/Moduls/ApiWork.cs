using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using SchoolDocuments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Moduls
{
    class ApiWork
    {
        private static string baseUrl = "https://documents-tasks-api.herokuapp.com/";
        public static async Task<List<Models.Document>> GetAllDocuments()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/documents").GetStringAsync();

            List<Models.Document> documents = JsonConvert.DeserializeObject<List<Models.Document>>(response);

            return documents;
        }
        public static async Task<List<Models.Familiarize>> GetAllFamiliarize(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment($"/{id}").AppendPathSegment("/familiarizes").GetStringAsync();

            List<Models.Familiarize> documents = JsonConvert.DeserializeObject<List<Models.Familiarize>>(response);

            return documents;
        }

        public static async Task<List<User>> GetAllUsers()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/users").GetStringAsync();

            List<User> documents = JsonConvert.DeserializeObject<List<User>>(response);

            return documents;
        }
        public static async Task<List<Template>> GetAllTemplates()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/template").GetStringAsync();

            List<Template> documents = JsonConvert.DeserializeObject<List<Template>>(response);

            return documents;
        }

        public static async void AddTemplate(Template template) => await $"{baseUrl}".AppendPathSegment("/template").PostJsonAsync(template).ReceiveString();
        public static async void SaveTemplate(Template template) => await $"{baseUrl}".AppendPathSegment("/template").PutJsonAsync(template).ReceiveString();
        public static async void AddDocument(Document document) => await $"{baseUrl}".AppendPathSegment("/document").PostJsonAsync(document).ReceiveString();
        public static async void SaveFam(Familiarize familiarize) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/familiarize").AppendPathSegment($"{familiarize.id}").PutJsonAsync(familiarize).ReceiveString();
    }
}
