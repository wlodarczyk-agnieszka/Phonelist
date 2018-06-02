using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Phonelist.Models;
using System.Data.SqlClient;

namespace Phonelist
{
    public class SourceManager
    {
        private string connString = "Initial Catalog=Phonelist;" +
                                    "Data Source=192.168.29.128;" +
                                    "User id=sa;" + "Password=Test2010;";


        public List<PersonModel> Get(int start, int take)
        {
            // metoda powinna zwracać listę obiektów z bazy zaczynającą się od podanego numeru wiersza. 
            // Długość listy określa drugi parametr

            List<PersonModel> Persons = new List<PersonModel>();

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "SELECT ID, FirstName, LastName, Phone, Email, Created, Updated FROM People WHERE ID >= @start AND ID <= @stop;";
                command.Parameters.Add(new SqlParameter("@start", SqlDbType.Int) { Value = start });
                command.Parameters.Add(new SqlParameter("@stop", SqlDbType.Int) { Value = start + take-1 });
                command.Connection = connection;
                connection.Open();

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime? updated = null; 
                        if (reader["Updated"].ToString() != "" )
                        {
                            updated = Convert.ToDateTime(reader["Updated"].ToString());
                        }

                        var person = new PersonModel(Convert.ToInt32(reader["ID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Phone"].ToString(), reader["Email"].ToString(), Convert.ToDateTime(reader["Created"]), updated);

                        Persons.Add(person);
                    }

                    connection.Close();
                    return Persons;
                    
                }

            }

        
            return null;
        }

        public PersonModel GetByID(int id)
        {
            // metoda powinna zwracać osobę o podanym ID

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "SELECT ID, FirstName, LastName, Phone, Email FROM People WHERE ID = @id;";
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = id});
                command.Connection = connection;
                connection.Open();

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    var person = new PersonModel(Convert.ToInt32(reader["ID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Phone"].ToString(), reader["Email"].ToString());

                    return person;
                }

            }

            return null;

        }

        public int Add(PersonModel personModel)
        {
            // metoda powinna zapisać dane obiektu do bazy danych, a zwrócić automatycznie nadany identyfikator osoby.

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "INSERT INTO People (FirstName, LastName, Phone, Email) VALUES (@first, @last, @phone, @email);";
                command.Parameters.Add(new SqlParameter("@first", SqlDbType.VarChar) { Value = personModel.FirstName });
                command.Parameters.Add(new SqlParameter("@last", SqlDbType.VarChar) { Value = personModel.LastName ?? "" });
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.VarChar) { Value = personModel.Phone });
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar) { Value = personModel.Email ?? "" });
                command.Connection = connection;
                connection.Open();

                var rowsInserted = command.ExecuteNonQuery();

                return rowsInserted;

            }

            return -1;

           
        }

        public void Update(PersonModel personModel)
        {
            // metoda powinna aktualizować podaną osobę w bazie danych
        }

        public void Remove(int id)
        {
            // metoda powinna usuwać podaną osobę z bazy danych

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "DELETE FROM People WHERE ID = @id;";
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });
                command.Connection = connection;
                connection.Open();

                command.ExecuteNonQuery();

            }

        }
    }
}
