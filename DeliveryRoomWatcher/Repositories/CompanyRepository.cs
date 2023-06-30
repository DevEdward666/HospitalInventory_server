using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Common;
using MySqlConnector;
using System;

namespace DeliveryRoomWatcher.Repositories
{

    public class CompanyRepository
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
        public ResponseModel CompanyName()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    string hospitalName = con.QuerySingleOrDefault<string>(
                        $@"SELECT `GetDefaultValue`('HOSPNAME') AS 'hosp_name'",
                                null, transaction: tran);
                    return new ResponseModel
                    {
                        success = true,
                        data = hospitalName
                    };
                }
            }

        }

        public ResponseModel hospitalLogo()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        string logo = Convert.ToBase64String(con.QuerySingle<byte[]>($@"
                                        SELECT hosplogo  FROM hospitallogo WHERE hospcode = GetDefaultValue('hospinitial')
                                        "
                                         , null, transaction: tran));
                        return new ResponseModel
                        {
                            success = true,
                            data = logo
                        };
                    }
                }

            }
            catch (Exception err)
            {

                return new ResponseModel
                {
                    success = false,
                    message = err.Message
                };
            }
        }
        public ResponseModel CompanyLogo()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    byte[] bytes = con.QuerySingleOrDefault<byte[]>(
                        $@"SELECT hosplogo AS 'logo' FROM hospitallogo WHERE hospcode = GetDefaultValue('hospinitial')",
                                null, transaction: tran);
                    return new ResponseModel
                    {
                        success = true,
                        data = "data:image/jpg;base64," + Convert.ToBase64String(bytes)
                    };
                }
            }
        }

        public ResponseModel CompanyTagLine()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    string tagLine = con.QuerySingleOrDefault<string>(
                        $@"SELECT `GetDefaultValue`('HOSPITALTAGLINE') AS 'hosp_tagline'",
                                null, transaction: tran);
                    return new ResponseModel
                    {
                        success = true,
                        data = tagLine
                    };
                }
            }

        }
    }
}
