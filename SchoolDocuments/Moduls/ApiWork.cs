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
        private static string baseUrl = "https://api.ok654.org/";
        public static CookieJar jar = new CookieJar();
        public static string ID;
        public static CookieSession session = new CookieSession($"{baseUrl}");
        public static async void Login(string id)
        {
            ID = id;
            await $"{baseUrl}".AppendPathSegment("/login").WithCookies(jar).PostUrlEncodedAsync(new
            {
                idToken = id
            }).ReceiveString();
        }
        public static async Task<List<Models.Document>> GetAllDocuments()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/documents").WithCookies(jar).GetStringAsync();

            List<Models.Document> documents = JsonConvert.DeserializeObject<List<Models.Document>>(response);

            return documents;
        }
        public static async Task<List<Models.Document>> GetUserDocuments(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment("/documents").AppendPathSegment("/created").WithCookies(jar).GetStringAsync();

            List<Models.Document> documents = JsonConvert.DeserializeObject<List<Models.Document>>(response);

            return documents;
        }
        public static async Task<Models.File> GetUserDocumentsFile(int id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/document").AppendPathSegment($"/{id}").AppendPathSegment("/file").WithCookies(jar).GetStringAsync();

            Models.File documents = JsonConvert.DeserializeObject<Models.File>(response);

            return documents;
        }

        public static async Task<List<Models.Familiarize>> GetAllFamiliarize(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment("/familiarizes").WithCookies(jar).GetStringAsync();

            List<Models.Familiarize> documents = JsonConvert.DeserializeObject<List<Models.Familiarize>>(response);

            return documents;
        }
        public static async Task<List<Models.Agreement>> GetAllAgreements(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment("/agreements").WithCookies(jar).GetStringAsync();

            List<Models.Agreement> documents = JsonConvert.DeserializeObject<List<Models.Agreement>>(response);

            return documents;
        }
        public static async Task<List<Models.Task>> GetGivenTasks(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment("/tasks").AppendPathSegment("/given").WithCookies(jar).GetStringAsync();

            List<Models.Task> documents = JsonConvert.DeserializeObject<List<Models.Task>>(response);

            return documents;
        }
        public static async Task<List<Models.Task>> GetToDoTasks(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment("/tasks").AppendPathSegment("/todo").WithCookies(jar).GetStringAsync();

            List<Models.Task> documents = JsonConvert.DeserializeObject<List<Models.Task>>(response);

            return documents;
        }

        public static async Task<List<User>> GetAllUsers()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/users").WithCookies(jar).GetStringAsync();

            List<User> documents = JsonConvert.DeserializeObject<List<User>>(response);

            return documents;
        }
        public static async Task<User> GetUserInfo(string id)
        {
            var response = await $"{baseUrl}".AppendPathSegment($"/user/{id}")
                .WithCookies(jar)
                .GetStringAsync();

            User documents = JsonConvert.DeserializeObject<User>(response);

            return documents;
        }

        public static async Task<List<Template>> GetAllTemplates()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/template").WithCookies(jar).GetStringAsync();

            List<Template> documents = JsonConvert.DeserializeObject<List<Template>>(response);

            return documents;
        }

        public static async void Logout() => await $"{baseUrl}".AppendPathSegment("/logout").WithCookies(jar).GetAsync().ReceiveString();
        public static async void DeleteDocument(int id) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment($"{id}").WithCookies(jar).DeleteAsync().ReceiveString();

        public static async void AddTemplate(Template template) => await $"{baseUrl}".AppendPathSegment("/template").WithCookies(jar).PostJsonAsync(template).ReceiveString();
        public static async void AddTask(Models.TaskWithFiles task) => await $"{baseUrl}".AppendPathSegment("/task").WithCookies(jar).PostJsonAsync(task).ReceiveString();
        public static async void PutTaskStatus(int id, PerformerStatus status) => await $"{baseUrl}".AppendPathSegment("/task").AppendPathSegment($"{id}").AppendPathSegment("/status").WithCookies(jar).PutJsonAsync(status).ReceiveString();
        public static async void PutTaskComment(int id, string comment) => await $"{baseUrl}".AppendPathSegment("/task").AppendPathSegment($"{id}").AppendPathSegment("/comment").WithCookies(jar).PutJsonAsync(comment).ReceiveString();
        public static async void DeleteTask(Models.Task task) => await $"{baseUrl}".AppendPathSegment("/task").AppendPathSegment($"{task.id}").WithCookies(jar).DeleteAsync().ReceiveString();
        public static async void SaveRole(User user, Role role) => await $"{baseUrl}".AppendPathSegment("/user").AppendPathSegment($"{user.id}").WithCookies(jar).PutJsonAsync(role).ReceiveString();
        public static async void SaveTemplate(Template template) => await $"{baseUrl}".AppendPathSegment("/template").WithCookies(jar).PutJsonAsync(template).ReceiveString();
        public static async void AddDocument(DocumentWithFile document) => await $"{baseUrl}".AppendPathSegment("/document").WithCookies(jar).PostJsonAsync(document).ReceiveString();
        public static async void AddDocumentToPerformer(DocumentWithFile document, Performer performer) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/perform").AppendPathSegment($"{performer.id}").WithCookies(jar).PostJsonAsync(document).ReceiveString();
        public static async void SaveDocument(Models.File document,int id) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment($"/{id}").WithCookies(jar).PutJsonAsync(document).ReceiveString();
        public static async void SaveFam(Familiarize familiarize) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/familiarize").AppendPathSegment($"{familiarize.id}").WithCookies(jar).PutJsonAsync(familiarize).ReceiveString();
        public static async void SaveFamList(List<int> familiarize) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/familiarize").WithCookies(jar).PutJsonAsync(familiarize).ReceiveString();
        public static async void SaveAgreementWithout(Agreement agreement) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/agreement").AppendPathSegment($"{agreement.id}").WithCookies(jar).PutJsonAsync(agreement.status).ReceiveString();
        public static async void SaveAgreementWith(Agreement agreement) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/agreement").AppendPathSegment($"{agreement.id}").SetQueryParam($"comment={agreement.comment}").WithCookies(jar).PutJsonAsync(agreement.status).ReceiveString();
    }
}
