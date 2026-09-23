using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolok13B.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sportolok13B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportoloController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=sportolo13b;uid=root;password=";

        [HttpGet]
        public List<Sportolo> GetAllSportolo()
        {
            List<Sportolo> sportolok = new List<Sportolo>();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT * FROM sportolo;";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var sportolo = new Sportolo
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Age = dataReader.GetInt32(3),
                    Password = dataReader.GetString(4),
                    RegTime = dataReader.GetDateTime(5),
                };
                sportolok.Add(sportolo);
            }


            connector.Close();
            return sportolok;
        }

        [HttpGet("byId")]
        public object GetSportolokById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT `Name`, `Email` FROM `sportolo` WHERE `Id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var datareader = cmd.ExecuteReader();
            datareader.Read();

            var eredmeny = new
            {
                Competition = datareader.GetString(0)
            };

            connector.Close();
            return eredmeny;
        }

        [HttpPost]
        public object AddNewSportolo(Sportolo sportolo)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sport = new Sportolo
            {
                Name = sportolo.Name,
                Email = sportolo.Email,
                Age = sportolo.Age,
                Password = sportolo.Password,
                RegTime = DateTime.Now
            };

            var sql = $"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegTime`) VALUES (@name,@email,@age,@password,@regtime)";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", sport.Name);
            cmd.Parameters.AddWithValue("@email", sport.Email);
            cmd.Parameters.AddWithValue("@age", sport.Age);
            cmd.Parameters.AddWithValue("@password", sport.Password);
            cmd.Parameters.AddWithValue("@regtime", sport.RegTime);

            cmd.ExecuteNonQuery();
            connector.Close();
            return sport;
        }

        [HttpPut]
        public object UpdateSportolo([FromQuery] int id, [FromBody] Sportolo updateSportolo)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"UPDATE `sportolo` SET `name`=@name,`email`=@email,`age`=@age,`password`=@password 
                WHERE `id`= @id;";


            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", updateSportolo.Name);
            cmd.Parameters.AddWithValue("@email", updateSportolo.Email);
            cmd.Parameters.AddWithValue("@age", updateSportolo.Age);
            cmd.Parameters.AddWithValue("@password", updateSportolo.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updatedSportolo = new Sportolo
            {
                Name = updateSportolo.Name,
                Email = updateSportolo.Email,
                Age = updateSportolo.Age,
                Password = updateSportolo.Password
            };

            connector.Close();

            return new { message = "Sikeres frissítés.", result = updatedSportolo };
        }

        [HttpDelete]
        public object DeleteSportolo(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"DELETE FROM `sportolo` WHERE id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();

            connector.Close();


            return new { message = "Sikeres törlés" };
        }




    }
}
