
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace RestFuncApp
{
    public static class TaskApi
    {   
        public static readonly List<Task> Items = new List<Task>();


        [FunctionName("CreateTask")]
        public static async Task<IActionResult> CreateTask(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "post", Route = "task")]
            HttpRequest req, TraceWriter log)
        {
            log.Info("Creating a new Task list item");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TaskCreateModel>(requestBody);

            var task = new Task() { TaskDescription = input.TaskDescription };
            Items.Add(task);
            return new OkObjectResult(task);
        }






        [FunctionName("GetAllTasks")]
        public static IActionResult GetAllTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "get", Route = "task")]
            HttpRequest req, TraceWriter log)
        {
            log.Info("Getting Task list items");
            return new OkObjectResult(Items);
        }










        [FunctionName("GetTaskById")]
        public static IActionResult GetTaskById(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "get", Route = "task/{id}")]
            HttpRequest req,
            TraceWriter log, string id)
        {
            var task = Items.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(task);
        }




        [FunctionName("UpdateTask")]
        public static async Task<IActionResult> UpdateTask(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "put", Route = "task/{id}")]
            HttpRequest req,
            TraceWriter log, string id)
        {
            var task = Items.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TaskUpdateModel>(requestBody);

            task.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrEmpty(updated.TaskDescription))
            {
                task.TaskDescription = updated.TaskDescription;
            }
            return new OkObjectResult(task);
        }




        [FunctionName("DeleteTask")]
        public static IActionResult DeleteTask(
            [HttpTrigger(AuthorizationLevel.Anonymous, 
                "delete", Route = "task/{id}")]
            HttpRequest req,
            TraceWriter log, string id)
        {
            var task = Items.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            Items.Remove(task);
            return new OkResult();
        }



    }
}
