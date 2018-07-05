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


        public List<PersonModel> Get(int skip, int take)
        {
            // metoda powinna zwracać listę obiektów z bazy zaczynającą się od podanego numeru wiersza. 
            // Długość listy określa drugi parametr

            List<PersonModel> persons = new List<PersonModel>();

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                //command.CommandText = "SELECT ID, FirstName, LastName, Phone, Email, Created, Updated FROM People WHERE ID >= @start AND ID <= @stop;";
                command.CommandText = "SELECT ID, FirstName, LastName, Phone, Email, Created, Updated FROM People ORDER BY 1 OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY; ";
                command.Parameters.Add(new SqlParameter("@skip", SqlDbType.Int) { Value = skip });
                command.Parameters.Add(new SqlParameter("@take", SqlDbType.Int) { Value = take });
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

                        persons.Add(person);
                    }

                    connection.Close();
                    return persons;
                    
                }

            }

        
            //return null;
            // w przypadku braku rekordów zwraca pusta liste
            return persons;
        }

        public PersonModel GetByID(int id)
        {
            // metoda powinna zwracać osobę o podanym ID

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "SELECT ID, FirstName, LastName, Phone, Email, Created, Updated FROM People WHERE ID = @id;";
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) {Value = id});
                command.Connection = connection;
                connection.Open();

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    DateTime? updated = null;
                    if (reader["Updated"].ToString() != "")
                    {
                        updated = Convert.ToDateTime(reader["Updated"].ToString());
                    }

                    var person = new PersonModel(Convert.ToInt32(reader["ID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Phone"].ToString(), reader["Email"].ToString(), Convert.ToDateTime(reader["Created"]), updated);

                    return person;
                }
                else
                {
                    return null;
                }

            }
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

        }

        public void Update(PersonModel personModel)
        {
            // metoda powinna aktualizować podaną osobę w bazie danych

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "UPDATE People SET FirstName = @first, LastName = @last, Phone = @phone, Email = @email, Updated = @updated WHERE ID = @id;";
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = personModel.ID });
                command.Parameters.Add(new SqlParameter("@first", SqlDbType.VarChar) { Value = personModel.FirstName });
                command.Parameters.Add(new SqlParameter("@last", SqlDbType.VarChar) { Value = personModel.LastName ?? "" });
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.VarChar) { Value = personModel.Phone });
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar) { Value = personModel.Email ?? "" });
                command.Parameters.Add(new SqlParameter("@updated", SqlDbType.DateTime) { Value = DateTime.Now });
                command.Connection = connection;
                connection.Open();

                command.ExecuteNonQuery();

            }
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

        public List<PersonModel> Search(string searchText)
        {
            List<PersonModel> persons = new List<PersonModel>();

            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "SELECT ID, FirstName, LastName, Phone, Email, Created, Updated FROM People WHERE FirstName like @txt OR LastName like @txt OR Phone like @txt OR Email like @txt;"; //OR LastName like @txt OR Phone like @txt OR Email like @txt;
                command.Parameters.Add(new SqlParameter("@txt", SqlDbType.VarChar) { Value = $"%{searchText}%" });
                command.Connection = connection;
                connection.Open();

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime? updated = null;
                        if (reader["Updated"].ToString() != "")
                        {
                            updated = Convert.ToDateTime(reader["Updated"].ToString());
                        }

                        var person = new PersonModel(Convert.ToInt32(reader["ID"]), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["Phone"].ToString(), reader["Email"].ToString(), Convert.ToDateTime(reader["Created"]), updated);

                        persons.Add(person);
                    }

                    connection.Close();
                    return persons;

                }
                else
                {
                    connection.Close();
                    // brak rekordow -> zwracamy pusta kolekcje
                    return persons;
                }

            }
        }

        public int NumberOfRecords()
        {
            using (var connection = new SqlConnection(connString))
            {
                var command = new SqlCommand();

                command.Parameters.Clear();
                command.CommandText = "SELECT count(*) FROM People;";
                command.Connection = connection;
                connection.Open();
                
                return Convert.ToInt32(command.ExecuteScalar()); 
            }
        }
    }
}
