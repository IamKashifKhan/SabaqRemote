using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SabaqRemote
{

    public class SabaqRemoteDB
    {
        private SQLiteAsyncConnection database;

        public SabaqRemoteDB(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<BluetoothData>().Wait();

            database.CreateTableAsync<GData>();
        }

        //public IEnumerable<SQL_SelectedUserLan> GetSelectedLanID()
        //{
        //    return (from t in database.Table<SQL_SelectedUserLan>()
        //            select t).ToListAsync();
        //}


        public Task<List<BluetoothData>> GetBthDeviceList()
        {
            return database.Table<BluetoothData>().ToListAsync();
        }

        public Task<List<GData>> GetGData()
        {
            return database.Table<GData>().ToListAsync();

        }
        public void DeleteAllDevices()
        {
            database.DeleteAllAsync<BluetoothData>();
        }
        public void AddBLDevice(string bthid, string BLDName)
        {
            var newLoggedinUser = new BluetoothData
            {
                BthID = bthid,
                BthName = BLDName,
            };

            database.InsertAsync(newLoggedinUser);
        }








        public void DeleteSensorData()
        {
            database.DeleteAllAsync<GData>();
        }


        public void AddData(string GValue)
        {
            var NewGData = new GData
            {
                GValue = GValue,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),


            };

            database.InsertAsync(NewGData);
        }



    }

    [Preserve(AllMembers = true)]
    public class GData
    {
        public string GValue { get; set; }
        // public string Password { get; set; }
        public string Timestamp { get; set; }

        public GData()
        {
        }
    }

    [Preserve(AllMembers = true)]

    public class BluetoothData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string BthName { get; set; }
        public string BthID { get; set; }

        public BluetoothData()
        {
        }
    }
}



