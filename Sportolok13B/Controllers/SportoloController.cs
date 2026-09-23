using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolok13B.Models.DTOs;
using System.Xml.Linq;
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
                Name = datareader.GetString(0),
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

        // 6. Feladat

        [HttpGet("NameandEmail")]
        public List<object> nameAndEmail(int id)
        {
            List<object> bynameandemail = new List<object>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT Name, Email FROM `sportolo` WHERE sportolo.`id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var datareader = cmd.ExecuteReader();
            while (datareader.Read())
            {
                var sportoloNamesAndEmail = new
                {
                    Name = datareader.GetString(0),
                    Email = datareader.GetString(1)
                };

                bynameandemail.Add(sportoloNamesAndEmail);

            }


            connector.Close();
            return bynameandemail;
        }

        // 7. Feladat
        [HttpGet("byName")]
        public List<object> GetSportoloByName(int id)
        {
            List<object> byname= new List<object>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT sportolo.Name, eredmeny.Competition, eredmeny.Description FROM `sportolo` INNER JOIN eredmeny ON name.id = eredmeny.SportoloId WHERE sportolo.`id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var datareader = cmd.ExecuteReader();
            while (datareader.Read())
            {
                var sportoloNames = new
                {
                    Name = datareader.GetString(0),
                    Competition= datareader.GetString(1),
                    Description = datareader.GetString(2)
                };

                byname.Add(sportoloNames);

            }

            connector.Close();
            return byname;
        }

        // 8. Feladat

        [HttpGet("eredmenydb")]
        public List<Eredmeny> GetEredmenyDb()
        {
            List<Eredmeny> eredmenyekdb = new List<Eredmeny>();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT COUNT(*) FROM eredmeny;";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var eredmenydb = new Eredmeny
                {
                    Id = dataReader.GetInt32(0),
                    Competition = dataReader.GetString(1),
                    Description = dataReader.GetString(2),
                    ResultTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4),
                    SportoloId = dataReader.GetInt32(5),
                };
                eredmenyekdb.Add(eredmenydb);
            }


            connector.Close();
            return eredmenyekdb;
        }

        // 9. Feladat

        // Nincs olyan tábla hogy hány eredmény van




    }
}
