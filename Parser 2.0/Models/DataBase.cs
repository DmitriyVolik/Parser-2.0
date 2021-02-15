using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Doctors.Models
{
    public class DataBase
    {
        private string _connectionString;


        public DataBase(string connectionString)
        {
            _connectionString = connectionString;


        }

        public List<Service> Find(Doctor doctor)
        {
            List<Service> result = new List<Service>();
            string sqlExpression = "select Id, Title from DoctorServices JOIN Services S on S.Id = DoctorServices.ServiceId where DoctorId=@DoctorId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@DoctorId", doctor.Id));
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        Service temp = new Service();
                        temp.Id = Convert.ToInt32(reader.GetValue(0));
                        temp.Title = reader.GetValue(1).ToString();
                        result.Add(temp);
                    }
                }
                reader.Close();
            }
            return result;
        }

        public List<Doctor> FindAll()
        {
            List<Doctor> result = new List<Doctor>();

            string sqlExpression = "select Id, Name, PostalCode, City, Address, Phone, Description, Url from Doctors";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        Doctor temp = new Doctor();

                        temp.Id = Convert.ToInt32(reader.GetValue(0));

                        temp.Name = reader.GetValue(1).ToString();

                        temp.PostalCode = reader.GetValue(2).ToString();

                        temp.City = reader.GetValue(3).ToString();

                        temp.Address = reader.GetValue(4).ToString();

                        temp.Phone = reader.GetValue(5).ToString();

                        temp.Description = reader.GetValue(6).ToString();

                        temp.Url = reader.GetValue(7).ToString();

                        result.Add(temp);
                    }
                }

                reader.Close();
            }

            return result;
        }

        public List<Doctor> FindAllByService(Service service)
        {
            List<Doctor> result = new List<Doctor>();

            string sqlExpression = "select Id, Name, PostalCode, City, Address, Phone, Description, Url from Doctors join DoctorServices DS on Doctors.Id = DS.DoctorId where ServiceId = @ServiceId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@ServiceId", service.Id));


                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        Doctor temp = new Doctor();

                        temp.Id = Convert.ToInt32(reader.GetValue(0));

                        temp.Name = reader.GetValue(1).ToString();

                        temp.PostalCode = reader.GetValue(2).ToString();

                        temp.City = reader.GetValue(3).ToString();

                        temp.Address = reader.GetValue(4).ToString();

                        temp.Phone = reader.GetValue(5).ToString();

                        temp.Description = reader.GetValue(6).ToString();

                        temp.Url = reader.GetValue(7).ToString();

                        result.Add(temp);
                    }
                }

                reader.Close();
            }

            return result;
        }


        public List<Doctor> getNotCollectedDoctors(int count = 10)
        {
            List<Doctor> result = new List<Doctor>();

            string sqlExpression;

            sqlExpression = "select TOP (@Count) Id, Name, PostalCode, City, Address, Phone, Description, Url from Doctors where Collected = 0";




            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@Count", count));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        Doctor temp = new Doctor();

                        temp.Id = Convert.ToInt32(reader.GetValue(0));

                        temp.Name = reader.GetValue(1).ToString();

                        temp.PostalCode = reader.GetValue(2).ToString();

                        temp.City = reader.GetValue(3).ToString();

                        temp.Address = reader.GetValue(4).ToString();

                        temp.Phone = reader.GetValue(5).ToString();

                        temp.Description = reader.GetValue(6).ToString();

                        temp.Url = reader.GetValue(7).ToString();

                        result.Add(temp);
                    }
                }

                reader.Close();
            }

            return result;
        }

        private void BindParams(SqlCommand command, Doctor doctor)
        {
            command.Parameters.Add(new SqlParameter("@Uid", Helper.MD5(doctor.Url)));
            command.Parameters.Add(new SqlParameter("@Url", doctor.Url));
            command.Parameters.Add(new SqlParameter("@Name",
                doctor.Name != null ?
                (object)doctor.Name : DBNull.Value));
            command.Parameters.Add(new SqlParameter("@PostalCode",
                doctor.PostalCode != null ?
                (object)doctor.PostalCode : DBNull.Value));
            command.Parameters.Add(new SqlParameter("@City",
                doctor.City != null ?
                (object)doctor.City : DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Address",
                doctor.Address != null ?
                (object)doctor.Address : DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Phone",
                doctor.Phone != null ?
                (object)doctor.Phone : DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Description",
                doctor.Description != null ?
                (object)doctor.Description : DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Collected",
                doctor.Phone != null));
        }

        private void Insert(Doctor doctor)
        {
            string sqlExpression = "insert into Doctors (Uid, Name, PostalCode, City, Address, Phone, Description, Url, Collected)" +
                " VALUES (@Uid, @Name, @PostalCode, @City, @Address, @Phone, @Description, @Url, @Collected); SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                BindParams(command, doctor);
                int insertedID = Convert.ToInt32(command.ExecuteScalar());
                doctor.Id = insertedID;
            }
        }

        private void Update(Doctor doctor)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sqlExpression = "update Doctors SET Uid=@Uid, Name=@Name, PostalCode=@PostalCode, City=@City, Address=@Address, Phone=@Phone, Description=@Description, Url=@Url, Collected=@Collected WHERE Id=@Id";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                BindParams(command, doctor);
                command.Parameters.Add(new SqlParameter("@Id", doctor.Id));
                command.ExecuteNonQuery();

                foreach (var service in doctor.Services)
                {
                    try
                    {
                        Save(doctor, service);
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
        }

        public Service FindServiceByTitle(string title)
        {
            Service temp = null;


            string sqlExpression = "SELECT Id, Title FROM Services WHERE Title = @Title";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@Title", title));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    temp = new Service();
                    while (reader.Read()) // построчно считываем данные
                    {

                        temp.Id = Convert.ToInt32(reader.GetValue(0));

                        temp.Title = reader.GetValue(1).ToString();




                    }
                }

                reader.Close();
            }

            return temp;
        }



        public List<Service> FindAllServices()
        {
            var result = new List<Service>();
            string sqlExpression = "SELECT Id, Title FROM Services ORDER BY Title";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                Service temp;
                if (reader.HasRows) // если есть данные
                {
                    
                    while (reader.Read()) // построчно считываем данные
                    {
                        temp = new Service();
                        temp.Id = Convert.ToInt32(reader.GetValue(0));
                        temp.Title = reader.GetValue(1).ToString();
                        result.Add(temp);
                    }
                }

                reader.Close();
            }

            return result;
        }

        public void Save(Service service)
        {
            string sqlExpression = "insert into Services (Title)" +
                " VALUES (@Title); SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@Title", service.Title));

                int insertedID = Convert.ToInt32(command.ExecuteScalar());
                service.Id = insertedID;
            }
        }

        public void Link(Doctor doctor, Service service)
        {
            string sqlExpression = "insert into DoctorServices (DoctorId, ServiceId)" +
                " VALUES (@DoctorId, @ServiceId)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("@DoctorId", doctor.Id));
                command.Parameters.Add(new SqlParameter("@ServiceId", service.Id));

                command.ExecuteNonQuery();
            }
        }

        public void Save(Doctor doctor, Service service)
        {
            var dbService = FindServiceByTitle(service.Title);
            if (dbService == null)
            {
                Save(service);
                Link(doctor, service);
            }
            else
            {
                Link(doctor, dbService);
            }
        }

        public void Save(Doctor doctor)
        {
            try
            {
                if (doctor.Id == null)
                {
                    Insert(doctor);
                }
                else
                {
                    Update(doctor);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //public void Save(Service service)
        //{

        //}

        //public List<Doctor> Find()
        //{
        //    return new List<Doctor>();
        //}

        //public bool Contain(Doctor doctor)
        //{
        //    return false;
        //}

    }
}
