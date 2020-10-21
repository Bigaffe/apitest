using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Renci.SshNet.Security.Cryptography;

namespace apitest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    { 

        [HttpGet]

        public List<TodoItem> Index()
        {
            
            string cs = @"server=localhost;userid=root;password='';database=todos;";

            using var con = new MySqlConnection(cs);
            con.Open();
            var stm = "SELECT * FROM todotabell";
            var cmd = new MySqlCommand(stm, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();
            TodoItem.TodoLista.Clear();
            while (rdr.Read())
            {
               
                TodoItem Todo = new TodoItem { Id = rdr.GetInt32(0), Name = rdr.GetString(1), IsComplete = rdr.GetBoolean(2)};
                TodoItem.TodoLista.Add(Todo);
            }

            return TodoItem.TodoLista;
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
        public string Create( [FromForm] string name, [FromForm] string complete)
        {
            //Tittar så allt är definerat annars returnrar den 404/Notfound();
            //if (name == null) return NotFound();
            //Allt var definerat och ett objekt skapas baserat på inputten

            //TodoItem Todo = new TodoItem{Id = id,Name = name,IsComplete = complete};
            //TodoItem.TodoLista.Add(Todo);

            string cs = @"server=localhost;userid=root;password='';database=todos;";

            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "INSERT INTO todotabell (Name,IsComplete) values (@Name,@IsComplete)";
            using var cmd = new MySqlCommand(sql, con);
            bool IsComplete = Boolean.Parse(complete);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@IsComplete", IsComplete);
            cmd.Prepare();

            cmd.ExecuteNonQuery();


            return "{'message': '"+IsComplete+"'}";
        }
        
        [HttpGet("todos")]
        public List<TodoItem> Todos()
        {
            if (TodoItem.TodoLista.Count == 0)
            {
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




        [HttpPost("todos")]
        public string Delete([FromForm] int Id)
        {
            string cs = @"server=localhost;userid=root;password='';database=todos;";

            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "DELETE from todotabell where Id = @Id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            return "{'message': '" + Id + "'}";
        }

        [HttpGet("uptodos")]
        public List<TodoItem> UpdateTodos()
        {
            if (TodoItem.TodoLista.Count == 0)
            {
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




        [HttpPost("uptodos")]
        public string Update([FromForm] int Id,  [FromForm] string name, [FromForm] string complete)
        {
            string cs = @"server=localhost;userid=root;password='';database=todos;";

            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "UPDATE todotabell SET Name=@name , Iscomplete=@complete where Id = @Id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@complete", complete);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            return "{'message': '" + Id + "'}";
        }

    }

}

