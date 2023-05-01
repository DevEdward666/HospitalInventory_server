using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Repositories;
using MySql.Data.MySqlClient;
using QueueCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueCore.Repositories
{
    public class QueueRepo
    {
        CompanyRepository _company = new CompanyRepository();
        public ResponseModel waiting(Queue.waiting waitings)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT * FROM queue WHERE countername=@countername AND STATUS='Queue' and countertype=@countertype AND DATE_FORMAT(docdate,'%Y-%m-%d')=DATE_FORMAT(CURDATE(),'%Y-%m-%d')",
                                                 waitings, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel Reception_Waiting()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT * FROM queue WHERE STATUS='Queue' AND countername='RECEPTION'  AND DATE_FORMAT(docdate, '%Y-%m-%d %H:%m:%s') >= DATE_FORMAT(CURDATE(), '%Y-%m-%d %H:%m:%s') ORDER BY DATE_FORMAT(docdate, '%Y-%m-%d %H:%m:%s') ASC",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getqueuemaintable(Queue.queues queue)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT  * from queue",
                                                 queue, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel nextclienttotext(Queue.nextclienttotext next)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT NextQueueNumberToText(@counter) AS phonenumber",
                                                 next, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel message()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT message from message_template",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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

        public ResponseModel getcounters_table()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT  counter_name,displayedto,type from counters",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getcounterlist(Queue.getlobbynos counterlist)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        if (counterlist.lobbyName == "ALL")
                        {
                            var data = con.Query($@"SELECT DISTINCT MAX(ql.queueno) queueno,q.countername,ql.`countername` AS counter,ql.`docdate` FROM queue q LEFT JOIN queue_log ql  ON q.countername = ql.`counter`  WHERE q.queueno IN (SELECT q.queueno FROM queue q WHERE DATE_FORMAT(ql.docdate, '%Y-%m-%d') = DATE_FORMAT(CURDATE(), '%Y-%m-%d') AND ql.`countername` IS NOT NULL GROUP BY q.countername) GROUP BY q.countername,ql.`countername`  ORDER BY ql.docdate DESC ",
                                                counterlist, transaction: tran);
                            return new ResponseModel
                            {
                                success = true,
                                data = data
                            };
                        }
                        else
                        {
                            var data = con.Query($@"SELECT distinct MAX(q.queueno) queueno,q.countername,ql.`countername` AS counter,ql.`docdate` FROM queue q LEFT JOIN queue_log ql ON q.queueno = ql.`queueno` WHERE q.queueno IN (SELECT q.queueno FROM queue q WHERE DATE_FORMAT(q.docdate, '%Y-%m-%d') = DATE_FORMAT(CURDATE(), '%Y-%m-%d') AND ql.counter IN (SELECT ld.`countername` FROM lobbyheader lh JOIN lobbydtls ld ON lh.`lobbyno`=ld.`lobbyno`WHERE lh.`lobbyno` = @lobbyno) GROUP BY q.countername) GROUP BY q.countername,ql.`countername`  ",
                                                                            counterlist, transaction: tran);
                            return new ResponseModel
                            {
                                success = true,
                                data = data
                            };
                        }
                    }
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
        public ResponseModel lastqueueno(Queue.counterlist counterlist)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT  ql.counter,ql.queueno,q.countertype  FROM queue_log ql JOIN queue q ON ql.queueno=q.`queueno`   WHERE ql.countername=@countername AND ql.counter=@counter  ORDER BY ql.docdate DESC LIMIT 1 ",
                                                 counterlist, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel Reception_lastqueueno()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT  ql.counter,ql.queueno,q.countertype  FROM queue_log ql JOIN queue q ON ql.queueno=q.`queueno` WHERE ql.counter='RECEPTION' ORDER BY ql.docdate DESC LIMIT 1 ",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getcountertype()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT typename from queue_type ",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getmaxnumber(Queue.getMaxNUmber getMaxNUmber)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.QuerySingle($@"SELECT MaxNumber FROM queue_setnumber WHERE Counter=@counter",
                                                 getMaxNUmber, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel generatequeuenumber(Queue.generatenumber generatenumber)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.QuerySingle($@"SELECT NextQueueNo(@counter,@maxnumber) queueno",
                                                 generatenumber, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel lobbytable()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT lobbyno,location from lobbyheader",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getqueuemain()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT (SELECT q.queueno FROM  queue q WHERE q.countername=qm.`countername`AND DATE(q.docdate) = CURDATE() ORDER BY q.docdate DESC LIMIT 1) AS queueno, countername,countertype FROM queue_main qm WHERE countertype='Regular' AND active = '1' ORDER BY qm.`countername` ASC",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getcounternumber(Queue.getcounterno counterno)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT counter_name from counters where displayedto=@displayedto",
                                                 counterno, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getqueuemainsenior()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT countername,countertype from queue_main where countertype='Senior'",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel reception_getqueuno()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT queueno, countername FROM queue WHERE  STATUS = 'Queue' AND countername='RECEPTION' AND DATE_FORMAT(docdate, '%Y-%m-%d') = DATE_FORMAT(CURDATE(), '%Y-%m-%d') LIMIT 1",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel getqueuno(Queue.getqueues getqueuno)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT queueno, countername FROM queue WHERE countername = @countername  AND STATUS = 'Queue'  AND countertype=@countertype AND DATE_FORMAT(docdate,'%Y-%m-%d')=DATE_FORMAT(CURDATE(),'%Y-%m-%d') LIMIT 1",
                                                 getqueuno, transaction: tran);
                        return new ResponseModel
                        {
                            success = true, 
                            data = data
                        };
                    }
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
        public ResponseModel getcountermaintable()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.Query($@"SELECT  id as counterid,countername,countertype,active from queue_main group by countername",
                                                 null, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel generatecounterno(Queue.generatecounterno generateno)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        var data = con.QuerySingle($@"SELECT CASE WHEN MAX(c.counter_name) +1 IS NULL THEN +1 ELSE MAX(c.counter_name) +1  END counterno  FROM counters c JOIN queue_main q ON c.displayedto=q.countername WHERE c.displayedto=@displayedto AND c.type=@type",
                                                 generateno, transaction: tran);
                        return new ResponseModel
                        {
                            success = true,
                            data = data
                        };
                    }
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
        public ResponseModel generatenumberkiosk(Queue.generatecounternumber cntr)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {


                    con.Open();


                    using (var tran = con.BeginTransaction())
                    {
                        int x = 0;
                        List<String> queuenos = new List<String>();
                        DateTime now = DateTime.Now;
                        int hour = now.Hour;
                        int minute = now.Minute;
                        DateTime date = now.Date;
                        string brand_logo = _company.hospitalLogo().data.ToString();
                        string brand_name = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospname' limit 1;", null, transaction: tran);
                        string brand_phone = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'SMSNUMBER' limit 1;", null, transaction: tran);
                        string brand_address = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospadd' limit 1;", null, transaction: tran);

                        string querygetmaxnumber = $@"SELECT MaxNumber FROM queue_setnumber WHERE Counter =@generated_counter";



                        string getmaxnumber = con.QuerySingle<string>(querygetmaxnumber, cntr, transaction: tran);
                        if (getmaxnumber != null)
                        {
                            string query = $@"SELECT NextQueueNo(@generated_counter,'{getmaxnumber}') queueno";

                            string nextQueueNo = con.QuerySingleOrDefault<string>(query, cntr, transaction: tran);
                            string Queueno = nextQueueNo.Substring(nextQueueNo.Length - 4);
                            string formatQueueno = "";
                            if (Int32.Parse(Queueno) <= 0009)
                            {
                                formatQueueno = Queueno.Substring(3);
                            }
                            else if (Int32.Parse(Queueno) <= 0099)
                            {
                                formatQueueno = Queueno.Substring(2);
                            } else if(Int32.Parse(Queueno) <= 0999)
                            {
                                formatQueueno = Queueno.Substring(1);
                            }
                            int isQueuegeneratednumberInserted = con.Execute($@"insert into queue set queueno='{nextQueueNo}',countername=@generated_counter,countertype=@generated_countertype,phonenumber='+63',docdate=NOW(),status='Queue'",
                                                     cntr, transaction: tran);
                            var pr_pdf = HtmltoPdf.PRHtmlPdf.geratePRPdf(brand_name, brand_logo, brand_address, brand_phone, formatQueueno, cntr.generated_counter, cntr.generated_countertype, now);



                            if (isQueuegeneratednumberInserted > 0)
                            {

                                tran.Commit();
                                return new ResponseModel
                                {
                                    data = Convert.ToBase64String(pr_pdf),
                                    success = true,
                                    message = "Success! The new generated number has been added successfully"
                                };
                            }
                            return new ResponseModel
                            {
                                data = Convert.ToBase64String(pr_pdf),
                                success = true,
                                message = "Success! The new generated number has been added successfully"
                            };
                        }
                        else
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {

                                success = false,
                                message = "Error! No rows affected while inserting the new record."
                            };
                        }

                    }
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
        public ResponseModel generatenumber(Queue.generatecounternumber cntr)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {


                    con.Open();


                    using (var tran = con.BeginTransaction())
                    {
                        int i = 0;
                        int x = 0;
                        List<String> queuenos = new List<String>();
                        DateTime now = DateTime.Now;
                        int hour = now.Hour;
                        int minute = now.Minute;
                        DateTime date = now.Date;
                        string brand_logo = _company.hospitalLogo().data.ToString();
                        string brand_name = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospname' limit 1;", null, transaction: tran);
                        string brand_phone = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'SMSNUMBER' limit 1;", null, transaction: tran);
                        string brand_address = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospadd' limit 1;", null, transaction: tran);
                        int counter = int.Parse(cntr.counter);
                        for (i = 0; i < counter; i++)
                        {
                            string query = $@"SELECT NextQueueNo(@generated_counter,@maxnumber) queueno";

                            string nextQueueNo = con.QuerySingle<string>(query, cntr, transaction: tran);

                            int isQueuegeneratednumberInserted = con.Execute($@"insert into queue set queueno='{nextQueueNo}',countername=@generated_counter,countertype=@generated_countertype,phonenumber='+63',docdate=NOW(),status='Queue'",
                                                     cntr, transaction: tran);
                            queuenos.Add(nextQueueNo);

                            if (isQueuegeneratednumberInserted <= 0)
                            {

                                tran.Rollback();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Error! No rows affected while inserting the new record.",

                                };
                            }



                        }
                        if (i > 0)
                        {

                            String[] queue = queuenos.ToArray();
                            List<String> base64 = new List<String>();
                            for (x = 0; x < queuenos.Count(); x++)
                            {
                                var pr_pdf = HtmltoPdf.PRHtmlPdf.geratePRPdf(brand_name, brand_logo, brand_address, brand_phone, queue[x], cntr.generated_counter, cntr.generated_countertype, date);


                                base64.Add(Convert.ToBase64String(pr_pdf));

                            }
                            String[] base64results = base64.ToArray();
                            if (x >= queuenos.Count())
                            {
                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Success ",
                                    data = base64results

                                };

                            }

                            return new ResponseModel
                            {
                                success = true,
                                message = "Error! No rows affected while inserting the new record.",
                                data = base64results

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
        public ResponseModel generatenumberwithoutpdf(Queue.generatecounternumber cntr)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {


                    con.Open();


                    using (var tran = con.BeginTransaction())
                    {
                        int i = 0;
                        int x = 0;
                        int isQueuegeneratednumberInserted = 0;
                        List<String> queuenos = new List<String>();
                        DateTime now = DateTime.Now;
                        int hour = now.Hour;
                        int minute = now.Minute;
                        DateTime date = now.Date;
                        string brand_logo = _company.hospitalLogo().data.ToString();
                        string brand_name = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospname' limit 1;", null, transaction: tran);
                        string brand_phone = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'SMSNUMBER' limit 1;", null, transaction: tran);
                        string brand_address = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospadd' limit 1;", null, transaction: tran);
                        int counter = int.Parse(cntr.counter);
                        for (i = 0; i < counter; i++)
                        {
                            string query = $@"SELECT NextQueueNo(@generated_counter,@maxnumber) queueno";
                            string nextQueueNo = con.QuerySingle<string>(query, cntr, transaction: tran);
                            isQueuegeneratednumberInserted = con.Execute($@"insert into queue set queueno='{nextQueueNo}',countername=@generated_counter,countertype=@generated_countertype,phonenumber='+63',docdate=NOW(),status='Queue'",
                                                     cntr, transaction: tran);
                            queuenos.Add(nextQueueNo);
                        }
                        if (isQueuegeneratednumberInserted > 0)
                        {

                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Successfully generated",

                            };
                        }
                        else
                        {

                            tran.Rollback();
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! No rows affected while inserting the new record."
                            };
                        }
                    }
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
        public ResponseModel addnewcounternumber(Queue.addnewcounternumber counter)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {


                    con.Open();


                    using (var tran = con.BeginTransaction())
                    {
                        string query = $@"SELECT MaxNumber FROM queue_setnumber WHERE counter=@displayedto";
                        var getmaxnumber = con.Query<string>(query, counter, transaction: tran);
                        if (getmaxnumber.Count() == 0)
                        {
                            int insertqueuesetnumber = con.Execute($@"insert into queue_setnumber set Counter=@displayedto,Maxnumber=999",
                                                       counter, transaction: tran);
                            if (insertqueuesetnumber >= 1)
                            {
                                if (!getmaxnumber.Equals("@counter_name"))
                                {


                                    string query2 = $@"SELECT counter_name from counters where counter_name=@counter_name and displayedto=@displayedto and type=@type";
                                    var counterexist = con.Query<string>(query2, counter, transaction: tran);
                                    if (counterexist.Count() == 0)
                                    {
                                        int iscounternumberInserted = con.Execute($@"insert into counters set counter_name = @counter_name,displayedto = @displayedto,type=@type,docdate = CURDATE()",
                                                                 counter, transaction: tran);
                                        if (iscounternumberInserted == 1)
                                        {

                                            tran.Commit();
                                            return new ResponseModel
                                            {
                                                success = true,
                                                message = "Success! The new counter number has been added successfully"
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
                        else
                        {
                            if (!getmaxnumber.Equals("@counter_name"))
                            {


                                string query2 = $@"SELECT counter_name from counters where counter_name=@counter_name and displayedto=@displayedto and type=@type";
                                var counterexist = con.Query<string>(query2, counter, transaction: tran);
                                if (counterexist.Count() == 0)
                                {
                                    int iscounternumberInserted = con.Execute($@"insert into counters values(@counter_name,@displayedto,@type,CURDATE())",
                                                             counter, transaction: tran);
                                    if (iscounternumberInserted == 1)
                                    {

                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            success = true,
                                            message = "Success! The new counter number has been added successfully"
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
                            else
                            {
                                return new ResponseModel
                                {
                                    success = false,
                                    message = "Error! No rows affected while inserting the new record."
                                };
                            }
                        }


                    }
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
        public ResponseModel addlobby(Queue.addlobby add)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {


                    con.Open();


                    using (var tran = con.BeginTransaction())
                    {
                        string query = $@"SELECT location from lobbyheader where location=@location group by location Limit 1";
                        string locationexist = "";
                        try
                        {
                            locationexist = con.QuerySingle<string>(query, add, transaction: tran);
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! No rows affected while inserting the new record."
                            };

                        }
                        catch (Exception e)
                        {
                            if (!locationexist.Equals("@location") && locationexist.Equals(""))
                            {


                                int islobbyheaderInserted = con.Execute($@"insert into lobbyheader (location) values(@location)",
                                                         add, transaction: tran);
                                if (islobbyheaderInserted == 1)
                                {

                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Success! The new location has been added successfully"
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




                    }
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
        public ResponseModel addlobbydtls(Queue.addlobbydtls adddtls)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {


                    con.Open();



                    using (var tran = con.BeginTransaction())
                    {
                        string query = $@"SELECT countername,counterno FROM lobbydtls WHERE lobbyno =@lobbyno AND countername=@countername AND counterno=@counterno";
                        var lobby = con.Query<string>(query, adddtls, transaction: tran);
                        if (lobby.Count() == 0)
                        {

                            int islobbyheaderInserted = con.Execute($@"insert into lobbydtls (lobbyno,countername,counterno) values(@lobbyno,@countername,@counterno)",
                                                     adddtls, transaction: tran);
                            if (islobbyheaderInserted == 1)
                            {

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Success! The new generatednumber has been added successfully"
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

        public ResponseModel getcounterexist(Queue.getqueues getqueuno)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        string query = $@"SELECT countername,countertype from queue_main where countername=@countername and countertype=@countertype";
                        var data = con.Query<string>(query, getqueuno, transaction: tran);
                        if (data.Count() == 0)
                        {
                            int isCounterInserted = con.Execute($@"insert into queue_main set countername = Trim(@countername),countertype=@countertype,createddate=NOW(), active = @active",
                                                    getqueuno, transaction: tran);
                            if (isCounterInserted == 1)
                            {
                                int addSetNumber = con.Execute($@"insert into queue_setnumber set Counter = Trim(@countername),MaxNumber='999'",
                                                   getqueuno, transaction: tran);
                                if (addSetNumber == 1)
                                {
                                    tran.Commit();
                                    return new ResponseModel
                                    {
                                        success = true,
                                        message = "Success! The new counter has been added successfully"
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
                        return new ResponseModel
                        {
                            success = false,
                            message = "Error! No rows affected while inserting the new record."
                        };
                    }
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
        public ResponseModel getcounterexistandupdate(Queue.updatequeues updatequeues)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        string query = $@"SELECT countername,countertype from queue_main where id=@counterid";
                        var data = con.Query<string>(query, updatequeues, transaction: tran);
                        if (data.Count() > 0)
                        {
                            int isCounterInserted = con.Execute($@"update queue_main set countername = Trim(@countername),countertype = @countertype,createddate = NOW(), active = @active where id = @counterid",
                                                    updatequeues, transaction: tran);
                            if (isCounterInserted > 0)
                            { 
                                int updatecounter = con.Execute($@"update counters set displayedto=@countername  where displayedto=@prev_counter_name",
                                                   updatequeues, transaction: tran);
                                if (updatecounter >= 0)
                                {
                                    int updatemaxnumber = con.Execute($@"update queue_setnumber set MaxNumber='999' ,Counter=@countername  where Counter=@prev_counter_name",
                                                  updatequeues, transaction: tran);
                                    if (updatemaxnumber > 0)
                                    {
                                        tran.Commit();
                                        return new ResponseModel
                                        {
                                            success = true,
                                            message = "Success! The new counter has been added successfully"
                                        };
                                    }
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
                        return new ResponseModel
                        {
                            success = false,
                            message = "Error! No rows affected while inserting the new record."
                        };
                    }
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
        public ResponseModel updatequeue(Queue.UpdateQueue UpdateQueue)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int isQueueUpdated = con.Execute($@"update queue set status= 'Keep' where queueno=@queueno and countername=@countername  AND DATE_FORMAT(docdate,'%Y-%m-%d')=DATE_FORMAT(CURDATE(),'%Y-%m-%d')",
                                                UpdateQueue, transaction: tran);

                        if (isQueueUpdated == 1)
                        {
                            int isQueueLogInserted = con.Execute($@"insert into queue_log values(@queueno,@counter ,@countername ,NOW(),'KEEP')",
                                              UpdateQueue, transaction: tran);
                            if (isQueueLogInserted == 1)
                            {

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Success! The new spot has been added successfully"
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
                                message = "Error! No rows affected while inserting the new housekeeper."
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
        public ResponseModel updatequeuephone(Queue.updatephonenumber updatephone)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int isQueueUpdated = con.Execute($@"update queue set phonenumber= @phonenumber where queueno=@queueno and countername=@countername",
                                                updatephone, transaction: tran);

                        if (isQueueUpdated == 1)
                        {

                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Success! The new spot has been added successfully"
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! No rows affected while inserting the new housekeeper."
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
        public ResponseModel insertmessage(Queue.insertmessage next)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int isQueueUpdated = con.Execute($@"INSERT INTO messageout(MessageTo, MessageText) VALUES(@phonenumber,@msg)",
                                                next, transaction: tran);

                        if (isQueueUpdated == 1)
                        {

                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = "Success! The message sent successfully"
                            };

                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "Error! No rows affected while inserting the new housekeeper."
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
        public ResponseModel updateservedqueue(Queue.UpdateQueue UpdateQueue)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        int isQueueUpdated = con.Execute($@"update queue set status= 'Served' where queueno=@queueno and countername=@countername  AND DATE_FORMAT(docdate,'%Y-%m-%d')=DATE_FORMAT(CURDATE(),'%Y-%m-%d')",
                                                UpdateQueue, transaction: tran);

                        if (isQueueUpdated >= 1)
                        {
                            int isQueueLogInserted = con.Execute($@"insert into queue_log values(@queueno,@counter ,@countername ,NOW(),'SERVED')",
                                              UpdateQueue, transaction: tran);
                            if (isQueueLogInserted == 1)
                            {

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = "Success! The new spot has been added successfully"
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
                                message = "Error! No rows affected while inserting the new housekeeper."
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
