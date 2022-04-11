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
        public static async Task<List<Models.Agreement>> GetAllAgreements(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment($"/{id}").AppendPathSegment("/agreements").GetStringAsync();

            List<Models.Agreement> documents = JsonConvert.DeserializeObject<List<Models.Agreement>>(response);

            return documents;
        }
        public static async Task<List<Models.Task>> GetGivenTasks(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment($"/{id}").AppendPathSegment("/tasks").AppendPathSegment("/given").GetStringAsync();

            List<Models.Task> documents = JsonConvert.DeserializeObject<List<Models.Task>>(response);

            return documents;
        }
        public static async Task<List<Models.Task>> GetToDoTasks(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment($"/{id}").AppendPathSegment("/tasks").AppendPathSegment("/todo").GetStringAsync();

            List<Models.Task> documents = JsonConvert.DeserializeObject<List<Models.Task>>(response);

            return documents;
        }

        public static async Task<List<User>> GetAllUsers()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/users").GetStringAsync();

            List<User> documents = JsonConvert.DeserializeObject<List<User>>(response);

            return documents;
        }
        public static async Task<User> GetUserInfo(string id)
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/user").AppendPathSegment($"/{id}").GetStringAsync();

            User documents = JsonConvert.DeserializeObject<User>(response);

            return documents;
        }
        
        public static async Task<List<Template>> GetAllTemplates()
        {
            var response = await $@"{baseUrl}".AppendPathSegment("/template").GetStringAsync();

            List<Template> documents = JsonConvert.DeserializeObject<List<Template>>(response);

            return documents;
        }

        public static async void AddTemplate(Template template) => await $"{baseUrl}".AppendPathSegment("/template").PostJsonAsync(template).ReceiveString();
        public static async void AddTask(Models.Task task) => await $"{baseUrl}".AppendPathSegment("/task").PostJsonAsync(task).ReceiveString();
        public static async void PutTaskStatus(int id,PerformerStatus status) => await $"{baseUrl}".AppendPathSegment("/task").AppendPathSegment($"{id}").AppendPathSegment("/status").PutJsonAsync(status).ReceiveString();
        public static async void PutTaskComment(int id,string comment) => await $"{baseUrl}".AppendPathSegment("/task").AppendPathSegment($"{id}").AppendPathSegment("/comment").PutJsonAsync(comment).ReceiveString();
        public static async void DeleteTask(Models.Task task) => await $"{baseUrl}".AppendPathSegment("/task").AppendPathSegment($"{task.id}").DeleteAsync().ReceiveString();
        public static async void SaveRole(User user,Role role) => await $"{baseUrl}".AppendPathSegment("/user").AppendPathSegment($"{user.id}").PutJsonAsync(role).ReceiveString();
        public static async void SaveTemplate(Template template) => await $"{baseUrl}".AppendPathSegment("/template").PutJsonAsync(template).ReceiveString();
        public static async void AddDocument(Document document) => await $"{baseUrl}".AppendPathSegment("/document").PostJsonAsync(document).ReceiveString();
        public static async void AddDocumentToPerformer(Document document,Performer performer) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/perform").AppendPathSegment($"{performer.id}").PostJsonAsync(document).ReceiveString();
        public static async void SaveDocument(Document document) => await $"{baseUrl}".AppendPathSegment("/document").PutJsonAsync(document).ReceiveString();
        public static async void SaveFam(Familiarize familiarize) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/familiarize").AppendPathSegment($"{familiarize.id}").PutJsonAsync(familiarize).ReceiveString();
        public static async void SaveFamList(List<int> familiarize) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/familiarize").PutJsonAsync(familiarize).ReceiveString();
        public static async void SaveAgreementWithout(Agreement agreement) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/agreement").AppendPathSegment($"{agreement.id}").PutJsonAsync(agreement.status).ReceiveString();
        public static async void SaveAgreementWith(Agreement agreement) => await $"{baseUrl}".AppendPathSegment("/document").AppendPathSegment("/agreement").AppendPathSegment($"{agreement.id}").SetQueryParam($"comment={agreement.comment}").PutJsonAsync(agreement.status).ReceiveString();
    }
}
