using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace HairSalon.Models
{
    public class Stylist
    {
        private int _id;
        private string _name;

        public Stylist(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public override bool Equals(System.Object otherStylist)
        {
            if (!(otherStylist is Stylist))
            {
                return false;
            }
            else
            {
                Stylist newStylist = (Stylist) otherStylist;
                bool idEquality = (this.GetId()) == newStylist.GetId();
                bool nameEquality = (this.GetName()) == newStylist.GetName();

                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public static List<Stylist> GetAll()
        {
            List<Stylist> allStylists = new List<Stylist>();

            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM stylists ORDER BY name;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int stylistId = rdr.GetInt32(0);
                string stylistName = rdr.GetString(1);
                Stylist newStylist = new Stylist(stylistName, stylistId);
                allStylists.Add(newStylist);
            }
            conn.Close();
            return allStylists;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO stylists (name) VALUES (@name);";
            
            MySqlParameter stylistName = new MySqlParameter();
            stylistName.ParameterName = "@name";
            stylistName.Value = this._name;
            cmd.Parameters.Add(stylistName);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
        }

        public static Stylist Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM stylists WHERE id = (@searchId);";

            MySqlParameter stylistId = new MySqlParameter();
            stylistId.ParameterName = "@searchId";
            stylistId.Value = id;
            cmd.Parameters.Add(stylistId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int foundId = 0;
            string foundName = "";

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
            }

            Stylist foundStylist = new Stylist(foundName, foundId);
            conn.Close();
            return foundStylist;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM stylists;";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteThis()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM stylists WHERE id = @thisId;";
            
            MySqlParameter stylistId = new MySqlParameter();
            stylistId.ParameterName = "@thisId";
            stylistId.Value = this._id;
            cmd.Parameters.Add(stylistId);

        cmd.ExecuteNonQuery();
        conn.Close();
        }

        public List<Client> GetAllClients()
        {
            List<Client> allClients = new List<Client>();

            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM clients WHERE stylist_id = @thisId;";

            MySqlParameter stylistId = new MySqlParameter();
            stylistId.ParameterName = "@thisId";
            stylistId.Value = this._id;
            cmd.Parameters.Add(stylistId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int clientId = rdr.GetInt32(0);
                string clientName = rdr.GetString(1);
                int clientStylist = rdr.GetInt32(2);
                Client newClient = new Client(clientName, clientStylist, clientId);
                allClients.Add(newClient);
            }
            conn.Close();
            return allClients;
        }

        public void UpdateName(string newNameInput)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE stylists SET name = @newName WHERE id = @thisId;";

            MySqlParameter stylistId = new MySqlParameter();
            stylistId.ParameterName = "@thisId";
            stylistId.Value = this._id;
            cmd.Parameters.Add(stylistId);

            MySqlParameter newName = new MySqlParameter();
            newName.ParameterName = "@newName";
            newName.Value = newNameInput;
            cmd.Parameters.Add(newName);

            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}