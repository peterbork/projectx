﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace projectx {
    class Controller {
        public void RegisterNewPerson(string CprNr, string Name) {
            Person person = new Person(CprNr, Name);

            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SetPerson", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@CPRNR", person.CprNr));
                cmd.Parameters.Add(new SqlParameter("@Name", person.Name));
                cmd.ExecuteScalar();

            }
            catch (Exception e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
        }
        public void RegisterNewSensor(string model, string cprnr, string sensorid, DateTime batterylastchanged) {
            Sensor sensor = new Sensor(model, cprnr, sensorid, batterylastchanged);

            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SetSensor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@Model", sensor.Model));
                cmd.Parameters.Add(new SqlParameter("@CPRNR", sensor.CprNr));
                cmd.Parameters.Add(new SqlParameter("@SensorID", sensor.Id));
                cmd.Parameters.Add(new SqlParameter("@BatteryLastChanged", sensor.BatteryLastChanged));
                cmd.ExecuteScalar();

            }
            catch (Exception e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
        }
        public List<Sensor> GetSensors() {
            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);
            List<Sensor> sensors = new List<Sensor>();

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetSensorFromModel", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@Model", ""));
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read()) {
                    sensors.Add(new Sensor(reader["S_Model"].ToString(), reader["S_P_CPRNR"].ToString(), reader["S_SensorID"].ToString(), Convert.ToDateTime(reader["S_BatteryLastChanged"].ToString())));
                }
                reader.Close();
            }
            catch (SqlException e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            return sensors;
        }
        public Sensor GetSensorFromCPRNR(string cprnr) {
            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);
            Sensor s = new Sensor();

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetSensorFromCPRNR", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@CPRNR", cprnr));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    s = new Sensor(reader["S_Model"].ToString(), reader["S_P_CPRNR"].ToString(), reader["S_SensorID"].ToString(), Convert.ToDateTime(reader["S_BatteryLastChanged"].ToString()));
                }
                reader.Close();
            }
            catch (SqlException e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            return s;
        }
        public void UpdateBatteryStatusOnSensorFromSensorID(string id){
            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);

            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateBatteryStatusOnSensorFromSensorID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@SensorID", id));
                cmd.ExecuteScalar();
            }
            catch (Exception e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
        }
        public List<Location> GetIntervalLocationFromSensorID(string sensorId, DateTime startDate, DateTime endDate) {
            List<Location> locations = new List<Location>();
            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetIntervalLocationFromSensorID", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@SensorID", sensorId));
                cmd.Parameters.Add(new SqlParameter("@FromDateTime", startDate));
                cmd.Parameters.Add(new SqlParameter("@ToDateTime", endDate));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    locations.Add(new Location(reader["L_S_SensorID"].ToString(), reader["L_Latitude"].ToString(), reader["L_Longitude"].ToString(), Convert.ToDateTime(reader["L_DateTime"].ToString())));
                }
                reader.Close();
            }
            catch (SqlException e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            return locations;
        }
        public List<Location> GetNewestLocationFromSensorID(string sensorId, int count) {
            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);
            List<Location> _l = new List<Location>();

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetNewestLocationFromSensorID", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@SensorID", sensorId));
                cmd.Parameters.Add(new SqlParameter("@Count", count));
                SqlDataReader reader = cmd.ExecuteReader();
                
                
                if (reader.HasRows){
                    while (reader.Read()) {
                        _l.Add(new Location(reader["L_S_SensorID"].ToString(), reader["L_Latitude"].ToString(), reader["L_Longitude"].ToString(), Convert.ToDateTime(reader["L_DateTime"].ToString())));
                    }
                }
                else { 
                    _l = new List<Location>();
                }
            reader.Close();
            }
            catch (SqlException e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            return _l;
        }
        public Person getName(string cprnr) {
            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);
            Person p = new Person(" ", " ");

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetPersonFromCPRNR", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@CPRNR", cprnr));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                   p = new Person(reader["P_CPRNR"].ToString(), reader["P_Name"].ToString());
                }
                reader.Close();

            }
            catch (SqlException e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            return p;
        }

        public List<String> GetNames(){
            SqlConnection conn = new SqlConnection(DBConnectionString.Conn);
            List<String> names = new List<String>();

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("Select P_Name FROM [EJL15_DB].[dbo].[P_Person]", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    names.Add(reader["P_Name"].ToString());
                }
                reader.Close();

            }
            catch (SqlException e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return names;
        }

        public Temperatur GetNewestTemperatureFromSensorID(string sensorId, int count){
              SqlConnection conn = new SqlConnection(DBConnectionString.Conn);
            Temperatur _t = new Temperatur();

            try {
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetNewestTemperatureFromSensorID", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                cmd.Parameters.Add(new SqlParameter("@SensorID", sensorId));
                cmd.Parameters.Add(new SqlParameter("@Count", count));
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _t = new Temperatur(reader["T_S_SensorID"].ToString(), int.Parse(reader["T_SensorValueInOhm"].ToString()), Convert.ToDateTime(reader["T_MeasuredAtDateTime"].ToString()));
                    }
                }
                else
                    _t = new Temperatur();
                reader.Close();

            }
            catch (SqlException e) {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            return _t;
        } 


    }
}
