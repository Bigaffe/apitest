using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace apitest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        [HttpGet]

        public string Index()
        {
            return "{{'name':'homeroute'}}";
        }
        [HttpGet("todo")]

        public TodoItem [] Todo()
        {
            TodoItem [] Arr = new TodoItem[3];

            TodoItem Todo = new TodoItem();
            Todo.Id = 12;
            Todo.Name = "Learn c#";
            Todo.IsComplete = false;

            Arr[0] = Todo;

            TodoItem Todo1 = new TodoItem();
            Todo1.Id = 13;
            Todo1.Name = "Learn .net Core";
            Todo1.IsComplete = false;

            Arr[1] = Todo1;

            TodoItem Todo2 = new TodoItem();
            Todo2.Id = 14;
            Todo2.Name = "Learn API in .net Core";
            Todo2.IsComplete = true;
            Arr[2] = Todo2;


            return Arr;
        }
    }

}
