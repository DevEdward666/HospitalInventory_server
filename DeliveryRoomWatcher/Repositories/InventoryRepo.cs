using Dapper;
using DeliveryRoomWatcher.Config;
using DeliveryRoomWatcher.Hooks;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Repositories
{
    public class InventoryRepo
    {
        CompanyRepository _company = new CompanyRepository();
        public ResponseModel getlistofrequest(InventoryModel.listofrequest listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"select * from requestdashboardbydept where todept=@id",
                          listofrequest, transaction: tran
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
        public ResponseModel getlistofrequestsearched(InventoryModel.listofrequestseareched listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string request = "";
                       if (listofrequest.status=="For Approval" && listofrequest.date!="")
                        {
                            request = $@"SELECT * FROM requestdashboardbydept WHERE todept=@dept  AND STATUS=@status AND reqdate=DATE_FORMAT(@date,'%Y-%m-%d')";

                        }
                        else if (listofrequest.status == "Approved" && listofrequest.date != "")
                        {
                            request = $@"SELECT * FROM requestdashboardbydept WHERE todept=@dept AND STATUS=@status  AND apprdate=DATE_FORMAT(@date,'%Y-%m-%d')";
                        }
                        else if (listofrequest.status == "Cancelled" && listofrequest.date != "")
                        {
                            request = $@"SELECT * FROM requestdashboardbydept WHERE todept=@dept  AND STATUS=@status AND cancelleddate=DATE_FORMAT(@date,'%Y-%m-%d')" ;
                        }
                        else if(listofrequest.date == "" && listofrequest.status !="")
                        {
                            request = $@"SELECT * FROM requestdashboardbydept WHERE todept=@dept  AND STATUS=@status";
                        }
                        else
                        {
                            request = $@"SELECT * FROM requestdashboardbydept WHERE todept=@dept  AND dateencoded=DATE_FORMAT(@date,'%Y-%m-%d')";
                        }

                        var data = con.Query(request,listofrequest, transaction: tran
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
        public ResponseModel getlistofrequestbystatus(InventoryModel.listofrequestbydept listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"select * from requestdashboardbydept where todept=@id and STATUS=@status",
                          listofrequest, transaction: tran
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
        public ResponseModel getlistofreqyestApproved(InventoryModel.listofrequest listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT rd.reqno,rd.reqby,rs.`dateencoded`,rs.`apprbyname`,rs.`apprdate`,rd.`STATUS` FROM requestdashboard rd JOIN requestsum rs ON rd.`reqno`=rs.`reqno` JOIN department dp ON rs.`deptcode`=dp.`deptcode`  WHERE rs.`deptcode`='@id' AND rd.`STATUS`='Approved'",
                          listofrequest, transaction: tran
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
        public ResponseModel getlistofreqyestCancelled(InventoryModel.listofrequest listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT rd.reqno,rd.reqby,rs.`dateencoded`,rs.`apprbyname`,rs.`apprdate`,rd.`STATUS` FROM requestdashboard rd JOIN requestsum rs ON rd.`reqno`=rs.`reqno` JOIN department dp ON rs.`deptcode`=dp.`deptcode`   WHERE rs.`deptcode`='@id' AND rd.`STATUS`='Cancelled'",
                          listofrequest, transaction: tran
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
        public ResponseModel getlistofreqyestForApproval(InventoryModel.listofrequest listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT rd.reqno,rd.reqby,rs.`dateencoded`,rs.`apprbyname`,rs.`apprdate`,rd.`STATUS` FROM requestdashboard rd JOIN requestsum rs ON rd.`reqno`=rs.`reqno` JOIN department dp ON rs.`deptcode`=dp.`deptcode`   WHERE rs.`deptcode`='@id' AND rd.`STATUS`='For Approval'",
                          listofrequest, transaction: tran
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
        public ResponseModel getlistofreqyestIssued(InventoryModel.listofrequest listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT rd.reqno,rd.reqby,rs.`dateencoded`,rs.`apprbyname`,rs.`apprdate`,rd.`STATUS` FROM requestdashboard rd JOIN requestsum rs ON rd.`reqno`=rs.`reqno` JOIN department dp ON rs.`deptcode`=dp.`deptcode`  WHERE rs.`deptcode`='@id' AND rd.`STATUS`='Issued'",
                          listofrequest, transaction: tran
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
        public ResponseModel getnoninventoryitem(string classcode)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT inv.stockcode,inv.stockdesc,inv.packdesc,inv.unitdesc,  inv.averagecost  FROM invmaster inv JOIN stockclass stc ON inv.stocktag=stc.classcode WHERE stc.classcode=@classcode"
                          ,new { classcode } ,transaction: tran
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
        public ResponseModel getstockclass()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT * FROM stockclass",
                           transaction: tran
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
        public ResponseModel getinventoryitem(InventoryModel.searchitems search)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT inv.stockcode,inv.stockdesc,inv.packdesc,inv.unitdesc,  inv.averagecost  FROM invmaster inv JOIN stockclass stc ON inv.stocktag=stc.classcode WHERE  stc.classcode=@classcode AND inv.stockdesc LIKE CONCAT('%',@search,'%')",
                           search,transaction: tran
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
        public ResponseModel getdepartmentList()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT dp.deptcode,dp.`deptname`FROM department dp WHERE deptactive='Y'",
                           transaction: tran
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
        public ResponseModel InsertNewRequest(mdlRequestHeader requests)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string reqno = con.QuerySingle<string>($@"SELECT CASE WHEN MAX(reqno)+1  IS NULL THEN 1 ELSE  MAX(reqno)+1  end reqno FROM requestsum", null, transaction: tran);
                        requests.reqno = reqno;
                        string sql_insert_request_header = $@"INSERT INTO requestsum SET reqno=@reqno ,deptcode=@deptcode,sectioncode='',reqdate=NOW(),reqby=@reqby,apprbycode=NULL,
                        apprbyname=NULL,apprdate=NULL,todept=@todept,tosection='',issueno=NULL,reqtype='O',reqstatus='O',trantype='T',
                        headapprovebycode=NULL,headapprovebyname=NULL,headdateapprove=NULL,cancelledbycode=NULL,cancelledbyname=NULL,
                        datecancelled=NULL,reqremarks=@reqremarks,encodedby='pgh',dateencoded=NOW(),tsreference=NOW()";
                        int insert_user_information = con.Execute(sql_insert_request_header, requests, transaction: tran);
                        
                    
                        if (insert_user_information > 0)
                            {
                            int i = requests.lisrequesttdtls.Count;

                            foreach (var rdl in requests.lisrequesttdtls)
                                 {
                                  rdl.reqno = reqno;
                                  string sql_add_dtls = $@"INSERT INTO requestdtls SET reqno = @reqno,lineno = @lineno,linestatus = 'O',deptcode = @deptcode ,sectioncode = ''
                                                                  ,todept = @todept,tosection = '',stockcode = @stockcode,stockdesc = @stockdesc,reqqty = @reqqty,averagecost=@averagecost,costamount=@costamount,unitdesc = @unitdesc,packed = 'N',
                                                                  issueqty = NULL,itemtrantype = 'I',itemremarks = @itemremarks,docdate = NOW()";

                                        int insert_prdetails_result = con.Execute(sql_add_dtls, rdl, transaction: tran);
                                        if (insert_prdetails_result <= 0)
                                        {
                                    tran.Rollback();
                                    return new ResponseModel
                                            {
                                                   
                                    success = false,
                                             message = "Database error has occured. No affected rows"
                                            };
                                        }
                                 }
                            if (i > 0)
                            {

                                tran.Commit();
                                return new ResponseModel
                                {
                                    success = true,
                                    message = $@"Your Request has been Added sucessfully, Request No.{requests.reqno}"
                                };
                            }
                            else
                            {
                                tran.Rollback();
                                return new ResponseModel
                                {

                                    success = false,
                                    message = "Database error has occured. No affected rows"
                                };
                            }
                        }
                        
                        else
                        {
                            tran.Rollback();
                            return new ResponseModel
                            {
                                    
                            success = false,
                                message = "Database error has occured. No affected rows"
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
        public ResponseModel updaterequestApproved(mdlSingleRequest.SingleRequestApprove requests)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string sql_insert_request_header = $@"UPDATE requestsum SET apprbycode=@apprbycode,apprbyname=@apprbyname,apprdate=NOW() WHERE reqno=@reqno";
                        int insert_user_information = con.Execute(sql_insert_request_header, requests, transaction: tran);
                        if (insert_user_information > 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = $@"Request No.{requests.reqno} has been Approved sucessfully"
                            };
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "User Already Exist"
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

        public ResponseModel updaterequestCancelled(mdlSingleRequest.SingleRequestCancelled requests)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string sql_insert_request_header = $@"UPDATE requestsum SET cancelledbycode=@cancelledbycode,cancelledbyname=@cancelledbyname,datecancelled=NOW() WHERE reqno=@reqno";
                        int insert_user_information = con.Execute(sql_insert_request_header, requests, transaction: tran);
                        if (insert_user_information > 0)
                        {
                            tran.Commit();
                            return new ResponseModel
                            {
                                success = true,
                                message = $@"Request No.{requests.reqno} has been Cancelled"
                            };
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                success = false,
                                message = "User Already Exist"
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
            
            
       public ResponseModel getmop()
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT mopcode,mopdesc FROM mopref ORDER BY mopkey",
                           transaction: tran
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
        public ResponseModel getsinglerequestheader (mdlSingleRequest singleRequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT reqno,todept,reqby,CASE WHEN issueno IS NOT NULL THEN 'Issued'  WHEN apprbycode IS NOT NULL AND cancelledbycode IS NULL  THEN 'Approved' WHEN cancelledbycode IS NOT NULL THEN 'Cancelled' ELSE 'For Approval' END reqstatus,reqremarks from requestsum WHERE reqno=@reqno",
                           singleRequest,transaction: tran
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

        } public ResponseModel dashboardnumbers (InventoryModel.listofrequest listofrequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT COUNT(CASE WHEN issueno IS NOT NULL THEN 'Issued' END) issued, COUNT(CASE  WHEN apprbycode IS NOT NULL AND cancelledbycode IS NULL  THEN 'Approved' END) approved,COUNT(CASE WHEN cancelledbycode IS NOT NULL THEN 'Cancelled' END) cancelled,COUNT(CASE WHEN apprbycode IS NULL THEN 'For Approval' END) forapproval FROM requestsum WHERE todept=@id",
                            listofrequest,transaction: tran
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
        public ResponseModel getPRPdf(int reqno)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
                {
                    con.Open();
                    using (var tran = con.BeginTransaction())
                    {
                        string brand_logo = _company.hospitalLogo().data.ToString();
                        string brand_name = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospname' limit 1;", null, transaction: tran);
                        string brand_phone = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'SMSNUMBER' limit 1;", null, transaction: tran);
                        string brand_address = con.QuerySingleOrDefault<string>("SELECT datval FROM defvalues WHERE remarks = 'hospadd' limit 1;", null, transaction: tran);
                        string brand_email = "-";



                        ListOfItems item_request = con.QuerySingle<ListOfItems>($@"
                                        SELECT rs.*,(SELECT deptname FROM department WHERE deptcode=rs.deptcode) requested_from,(SELECT deptname FROM department WHERE deptcode=rs.todept) requested_to FROM `requestsum` rs  WHERE reqno=@reqno LIMIT 1; 
                            ", new { reqno }, transaction: tran);


                        //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        //QRCodeData qrCodeData = qrGenerator.CreateQrCode(hash_req_pk, QRCodeGenerator.ECCLevel.Q);
                        //QRCode qrCode = new QRCode(qrCodeData);


                        //var brand_logo_bitmap = UseFileParser.Base64StringToBitmap(brand_logo);
                        //Bitmap qrCodeImage = qrCode.GetGraphic(35, Color.Black, Color.White, brand_logo_bitmap, 25);
                        //string qr_with_brand_logo = UseFileParser.BitmapToBase64(qrCodeImage);


                        item_request.requestitems = con.Query<ListofItemDetails>("select * from requestdtls where reqno=@reqno;",
                             new { reqno }, transaction: tran).ToList();


                        var pr_pdf = HtmltoPdf.PRHtmlPdf.geratePRPdf(brand_name, brand_logo, brand_address, brand_phone, brand_email, item_request);



                        return new ResponseModel
                        {
                            success = true,
                            data = Convert.ToBase64String(pr_pdf)
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
        public ResponseModel getsinglerequestdtls(mdlSingleRequest singleRequest)
        {
            using (var con = new MySqlConnection(DatabaseConfig.GetConnection()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        var data = con.Query($@"SELECT stockcode,stockdesc,unitdesc,reqqty,averagecost,(reqqty*averagecost) costamount,issueqty,itemremarks  FROM requestdtls where reqno=@reqno",
                           singleRequest, transaction: tran);


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
    }
}
