using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Common;
using MySql.Data.MySqlClient;
using System;

namespace DeliveryRoomWatcher.Repositories
{

    public class CompanyRepository
    {
        DatabaseConfig dbConfig = new DatabaseConfig();
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
                    try { 
                    byte[] bytes = con.QuerySingle<byte[]>(
                        $@"SELECT hosplogo AS 'logo' FROM hospitallogo WHERE hospcode = GetDefaultValues('hospinitial')",
                                null, transaction: tran);
                    return new ResponseModel
                    {
                        success = true,
                        data = "data:image/jpg;base64," + Convert.ToBase64String(bytes)
                    };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }
                }
            }
        }

        public ResponseModel CompanyName()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try {
                    string hospitalName = con.QuerySingle<string>(
                        $@"SELECT `GetDefaultValues`('HOSPNAME') AS 'hosp_name'",
                                null, transaction: tran);
                    return new ResponseModel
                    {
                        success = true,
                        data = hospitalName
                    };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }
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
                    try
                    {

                    
                    string tagLine = con.QuerySingle<string>(
                        $@"SELECT `GetDefaultValue`('HOSPITALTAGLINE') AS 'hosp_tagline'",
                                null, transaction: tran);
                    return new ResponseModel
                    {
                        success = true,
                        data = tagLine
                    };
                    }
                    catch (Exception e)
                    {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }
                }
            }
        }
    }
}
