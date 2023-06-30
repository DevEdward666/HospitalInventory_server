﻿using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.User;
using DeliveryRoomWatcher.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using QueueCore.Models;
using QueueCore.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DeliveryRoomWatcher.Repositories
{
    public class UserRepository
    {
        DatabaseConfig dbConfig = new DatabaseConfig();

        public List<UserModel> authenticateUser(PAuthUser cred)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                        var user_credentials = con.Query<UserModel>($@"SELECT u.`username`,TRIM(p.`modid`) AS 'modid' FROM `userpermission` p
                                JOIN `usermaster` u ON u.`username` = p.`username`  
                                WHERE p.`modid` = 'cash' AND p.logid = 'login'  OR p.`modid` = 'admin'  AND p.logid = 'login' AND AES_ENCRYPT(@password,@username) = u.`password`
                                GROUP BY p.modid LIMIT 1", cred, transaction: tran).ToList();

                        return user_credentials;
                      
                    
                   
                    
                }
            }
        }
        public List<UserModel> authenticateAdmin(PAuthUser cred)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    var user_credentials = con.Query<UserModel>($@"SELECT u.`username`,TRIM(p.`modid`) AS 'modid' FROM `userpermission` p
                                JOIN `usermaster` u ON u.`username` = p.`username`  
                                WHERE p.`modid` = 'user' AND p.logid = 'login'  OR p.`modid` = 'admin'  AND p.logid = 'login' AND AES_ENCRYPT(@password,@username) = u.`password`
                                GROUP BY p.modid", cred, transaction: tran).ToList();
                    return user_credentials;




                }
            }
        } 
 
        public ResponseModel InserNewUser(PAddNewUsers user_info)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try {
                        string query = $@"SELECT * FROM prem_usermaster WHERE username=@username";
                        var data = con.Query<string>(query, user_info, transaction: tran);
                        if (data.Count() == 0)
                        {
                            int insert_user_information = con.Execute($@"INSERT INTO prem_usersinfo (idno,img,docs,username,firstname,middlename,lastname,gender,
                                                                    birthdate,mobileno,email,region_code,city_code,province_code,barangay_code,zipcode,nationality_code,address)
                                                                    VALUES(null,@url,@url_docs,@username,@firstname,@middlename,@lastname,@gender,
                                                                    date_Format(@birthdate,'%Y-%m-%d'),@mobileno,@email,@region_code,@city_code,
                                                                    @province_code,@barangay_code,@zipcode,@nationality_code,@fulladdress)",
                                                                    user_info, transaction: tran);

                            if (insert_user_information >= 0)
                            {
                                int insert_user_cred = con.Execute($@"INSERT INTO prem_usermaster(fullname,email,username,PASSWORD,pin,activated,active,creaateddate)
                                                                    VALUES(CONCAT(@lastname,',',@firstname,' ',@middlename),@email,@username,
                                                                    AES_ENCRYPT(@password,@username),@pin,'N','false',NOW())",
                                                                      user_info, transaction: tran);

                                if (insert_user_cred >= 0)
                                {
                                    int insert_user_otp = con.Execute($@"INSERT INTO prem_otp  (id,otp,toUserName,date_expire,createdAt) VALUES(NULL,LPAD(FLOOR(RAND() * 999999.99), 6, '0'),@username,NOW(),NOW())",
                                                                     user_info, transaction: tran);
                                    if (insert_user_otp >= 0)
                                    {
                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            success = true,
                                            message = "The User credentials has been Added sucessfully."
                                        };

                                    }
                                    else
                                    {
                                        return new ResponseModel
                                        {
                                            success = false,
                                            message = "Error! Insert usermaster Failed."
                                        };
                                    }

                                }
                                else
                                {
                                    return new ResponseModel
                                    {
                                        success = false,
                                        message = "Error! Insert OTP Failed."
                                    };
                                }

                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! Insert Failed."
                                };

                            }
                        }
                        else{
                            return new ResponseModel
                            {
                                success = false,
                                message = "User Already Exist"
                            };
                        }
                    }
                    catch (Exception e) {
                        return new ResponseModel
                        {
                            success = false,
                            message = $@"External server error. {e.Message.ToString()}",
                        };
                    }
                  
                }
            }
        }
     
        //public ResponseModel getUserInfo(PGetUsername username)
        //{
        //    using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
        //    {
        //        con.Open();
        //        using (var tran = con.BeginTransaction())
        //        {
        //            try
        //            {
    
        //                var data = con.QuerySingle(
        //                                $@"SELECT u.username,CONCAT(emp.`lastname`,',',emp.firstname) AS empname,emp.deptcode,dp.deptname FROM usermaster u  JOIN userpermission up ON up.`username`=u.`username` JOIN usermodule um ON um.`modid`=up.`modid` JOIN empmast emp ON emp.`idno`=u.`empidno` JOIN department dp ON dp.deptcode = emp.deptcode  where u.username = @username limit 1
        //                                ",
        //                  username, transaction: tran
        //                    );

        //                return new ResponseModel
        //                {
        //                    success = true,
        //                    data = data
        //                };
        //            }
        //            catch (Exception e)
        //            {
        //                return new ResponseModel
        //                {
        //                    success = false,
        //                    message = $@"External server error. {e.Message.ToString()}",
        //                };
        //            }

        //        }
        //    }

        //}
        public ResponseModel getUserPerrmission(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.QuerySingle(
                                        $@"SELECT DISTINCT up.logid,up.modid FROM usermaster u  JOIN userpermission up ON up.`username`=u.`username` JOIN usermodule um ON um.`modid`=up.`modid` JOIN empmast emp ON emp.`idno`=u.`empidno` JOIN department dp ON dp.deptcode = emp.deptcode  WHERE u.username = @username AND up.modid='invservices' AND up.logid='approve'
                                        ",
                          username, transaction: tran
                            );

                        return new ResponseModel
                        {
                            success = true,
                            data = data
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
        public ResponseModel getUserPerrmissionCancel(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.QuerySingle(
                                        $@"SELECT DISTINCT up.logid,up.modid FROM usermaster u  JOIN userpermission up ON up.`username`=u.`username` JOIN usermodule um ON um.`modid`=up.`modid` JOIN empmast emp ON emp.`idno`=u.`empidno` JOIN department dp ON dp.deptcode = emp.deptcode  WHERE u.username = @username AND up.modid='invservices' AND up.logid='cancel'
                                        ",
                          username, transaction: tran
                            );

                        return new ResponseModel
                        {
                            success = true,
                            data = data
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
        public ResponseModel getUsernameExist(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.QuerySingle($@"select username from usermaster where username=@username",
                            username, transaction: tran
                            );

                        return new ResponseModel
                        {
                            success = true,
                            data = data
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
        public ResponseModel getUserInfo(PGetUsername username)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        //string username = ((ClaimsIdentity)HttpContext.Current.User.Identity).Name;

                        var data = con.QuerySingle("SELECT um.username,um.`empname`,u.accnt_type,u.redirectto,u.type FROM usermaster um JOIN queue_usermaster u ON u.username=um.username  WHERE um.`username`=@username",
                            username, transaction: tran
                            );

                        return new ResponseModel
                        {
                            success = true,
                            data = data
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
      
        public ResponseModel getselectusers()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var data = con.Query($@"SELECT username,empname FROM usermaster WHERE username NOT IN (SELECT username FROM queue_usermaster)",
                           null, transaction: tran
                            );

                        return new ResponseModel
                        {
                            success = true,
                            data = data
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
        public ResponseModel gettableusers()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {


                    try
                    {
                        var data = con.Query($@"SELECT fullname,username,accnt_type,redirectto,type FROM queue_usermaster",
                           null, transaction: tran
                            );

                        return new ResponseModel
                        {
                            success = true,
                            data = data
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
        public ResponseModel insertqueueuser(Users.gettableusers insertusers)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    string query = $@"SELECT empname FROM usermaster WHERE username=@username";
                    try
                    {
                        string empname = con.QuerySingle<string>(query, insertusers, transaction: tran);
                        if (empname != null)
                        {

                            int isQueue_UserInserted = con.Execute($@"insert into queue_usermaster (fullname,username,accnt_type,redirectto,type) values('{empname}',@username,@accnt_type,@redirectto,@type)",
                                                  insertusers, transaction: tran);
                            if (isQueue_UserInserted == 1)
                            {

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Success! The new user has been added successfully"
                                };
                            }
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! No rows affected while inserting the new record."
                                };
                            }
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! No rows affected while inserting the new record."
                            };
                        }

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
        public ResponseModel updateuser(Users.gettableusers updateusers)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int isQueue_UserInserted = con.Execute($@"update queue_usermaster set fullname=@fullname,accnt_type=@accnt_type,redirectto=@redirectto,type=@type  where username=@username",
                                              updateusers, transaction: tran);
                        if (isQueue_UserInserted == 1)
                        {

                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Success! The new user has been updated successfully"
                            };
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! No rows affected while inserting the new record."
                            };
                        }


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
