using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using apitest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
            var stm = "SELECT * FROM todotabell ORDER BY Id ASC";
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

        [HttpGet("{id}")]
        public List<TodoItem> Specifik(int Id)
        {
            string cs = @"server=localhost;userid=root;password='';database=todos;";

            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "SELECT * from todotabell where Id = @Id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            TodoItem.TodoLista.Clear();
            while (rdr.Read())
            {

                TodoItem Todo = new TodoItem { Id = rdr.GetInt32(0), Name = rdr.GetString(1), IsComplete = rdr.GetBoolean(2) };
                TodoItem.TodoLista.Add(Todo);
            }

            return TodoItem.TodoLista;
        }


        [HttpPost]
        public async Task<TodoItem> CreateJson()
        {
            using System.IO.StreamReader reader = new StreamReader(Request.Body);
            string body = await reader.ReadToEndAsync();
            TodoItem t = JsonConvert.DeserializeObject<TodoItem>(body);

            string cs = @"server=localhost;userid=root;password='';database=todos;";

            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "INSERT INTO todotabell (Name,IsComplete) values (@Name,@IsComplete)";
            using var cmd = new MySqlCommand(sql, con);
            //bool IsComplete = t.IsComplete;
            cmd.Parameters.AddWithValue("@Name", t.Name);
            cmd.Parameters.AddWithValue("@IsComplete", t.IsComplete);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            t.Id = id;


            return t;
        }

        //Post formulär 
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


        [HttpDelete("{id}")]
        public string Delete(int Id)
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


        [HttpPut]
        public async Task<TodoItem> Update()
        {

            using System.IO.StreamReader reader = new StreamReader(Request.Body);
            string body = await reader.ReadToEndAsync();
            TodoItem t = JsonConvert.DeserializeObject<TodoItem>(body);

            string cs = @"server=localhost;userid=root;password='';database=todos;";

            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "UPDATE todotabell SET Name=@name , Iscomplete=@complete where Id = @Id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", t.Id);
            cmd.Parameters.AddWithValue("@Name", t.Name);
            cmd.Parameters.AddWithValue("@complete", t.IsComplete);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            return t;
        }
        

    }

}

