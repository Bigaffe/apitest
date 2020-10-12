using System;
using System.Collections.Generic;
using System.Data;
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

        public List<TodoItem>Todo()
        {

            //Skapar fake data för nuet.
            if(TodoItem.TodoLista.Count == 0) {
                TodoItem Todo = new TodoItem();
                Todo.Id = 12;
                Todo.Name = "Learn c#";
                Todo.IsComplete = false;
                TodoItem.TodoLista.Add(Todo);

                TodoItem Todo1 = new TodoItem();
                Todo1.Id = 13;
                Todo1.Name = "Learn .net Core";
                Todo1.IsComplete = false;
                TodoItem.TodoLista.Add(Todo1);

                TodoItem Todo2 = new TodoItem();
                Todo2.Id = 14;
                Todo2.Name = "Learn API in .net Core";
                Todo2.IsComplete = true;
                TodoItem.TodoLista.Add(Todo2);

            }

                return TodoItem.TodoLista;
            
            
        }
        [HttpPost("todo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        ///<summary> Tar 3 parametrar från formet</summary>
        public ActionResult<TodoItem> Create([FromForm] int id, [FromForm] string name, [FromForm] bool complete)
        {
            //Tittar så allt är definerat annars returnrar den 404/Notfound();
            if (id == null || name == null || complete == null) return NotFound();
            //Allt var definerat och ett objekt skapas baserat på inputten
            TodoItem Todo = new TodoItem{Id = id,Name = name,IsComplete = complete};
            TodoItem.TodoLista.Add(Todo);
            
            return Todo;
        }
        
    }

}
