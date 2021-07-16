using DeliveryRoomWatcher.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Hooks
{
    public class HtmltoPdf
    {
        public static class PRHtmlPdf
        {

            public static Byte[] geratePRPdf(string brand_name, string brand_logo, string brand_address, string brand_phone, string brand_email, ListOfItems pr_reqest)
            {

                Byte[] res = null;
                MemoryStream ms = null;

                using (ms = new MemoryStream())
                {

                   SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                    string css = @"<style>
        * {
            letter-spacing: .3pt;
            word-spacing: .3pt; 
        }
        body {
            display: flex;
            align-content: flex-start;
            flex-direction: column;
            font-family: Arial;
            padding: 10px;
            font-size: 14px;
        }

        .header-container {
            justify-self: center;
            display: flex;
            margin-bottom: 20px;
        }

        .header-container img {
            height: 90px;
            width: 90px;
        }

        .header-container .header-text {
            margin-left: 10px;
            font-weight: 500;
        }

        .header-container .header-text .doc-name {
            font-size: 22px;
            font-weight: 900;
        }

        .header {
            display: flex;
            align-items: center;
            align-content: center;
   width: 100%;

    border-bottom: 1px dotted rgba(0, 0, 0, 1);
        }

        .header .brand-main-info {
           width: 100%;
            justify-self: flex-start;
            align-self: flex-start;
            flex-grow: 1;
            display: flex;
            align-items: center;
            align-content: center;
            font-size: 24px;
            font-weight: 500;
        }

        .header .brand-main-info .brand-logo {
            height: 65px;
            width: 65px;
            padding: 5px;
        }

        .header .brand-main-info .brand-name {
            font-weight: 900;
            font-size: 20px;
            padding: 5px;
            max-width: 100%;
        }

        .header .brand-sub-info {
            justify-self: flex-end;
            align-self: flex-end;
            flex-grow: 1;
            text-align: end;
            display: flex;
            flex-direction: column;
            justify-content: right;
            justify-items: right;
            font-size: 24px;
            max-width: 100%;

        }

        .header .brand-sub-info>div {
            padding: 3px;
        }

        .document-title {
    font-weight: 500;
            font-size: 16px;
            padding: 5px;
            max-width: 100%;

        }

        .request-info-ctnr {
            display: flex;
            align-items: center;
            align-content: center;
            justify-items: flex-end;
            padding: 10px 0;
        }

        .request-info-ctnr .details {
            flex-grow: 1;
            text-align: start;
            max-width: 350px;
            flex-shrink: 0;
        }

        .request-info-ctnr .details>div {
            padding: 3px 0;
        }

        .request-info-ctnr .qr {

            display: flex;
            flex-grow: 1;
            align-items: center;
            align-content: center;

            justify-items: flex-end;
            justify-content: flex-end;

            /* max-width: 300px; */
        }

        .request-info-ctnr .qr img {
            height: 200px;
            width: 200px;
        }


        .info-container {
            display: flex;
            flex-direction: row;
        }

        .req-info {
            flex-grow: 1;
            margin-right: 20px;
        }

        .pay-info {
            flex-grow: 1;
            margin-left: 20px;
        }


        .info-title {
            font-weight: 900;
            font-size: 18px;
            opacity: .6;
            margin-top: 10px;
            margin-bottom: 5px;
        }

        .info-group {
            display: flex;
            border-bottom: 1px dotted rgba(0, 0, 0, 1);
            padding: 7px 0;
            align-items: center;
            align-content: center;
            justify-content: flex-start;
        }

        .info-group .label {
            margin-right: 30px;
            flex-grow: auto;
            white-space: nowrap;
            opacity: .55;
            font-weight: 600;
        }

        .info-group .value {
            flex-grow: 1;
            font-weight: 400;
            font-size: 16px;
        }

        table {
            border-collapse: collapse;
            font-size: 14px;
            width: 100%;
        }


        table thead tr {
            text-align: left;
            font-weight: 600;
            background-color: #f5f5f5;
        }

        table th,
        table td {
            padding: 12px 15px;
        }

        tbody tr {
            border-bottom: 1px solid #dddddd;
        }


        .footer {
            padding: 10px;
            justify-self: flex-end;
            align-self: flex-end;
        }

        .footer-info-group {
            display: flex;
            font-weight: 400;
            font-size: 14px;
        }
     .footer-info-group .label2 {
            width:100%;
        }
        .footer-info-group .label {
            margin-right: 20px;
        }

        .footer-info-group .value {
            white-space: nowrap;
        }
    </style>";

                    string table_procedure = "";

                    float total_price = 0;
                    float unit_price = 0;

                    foreach (var x in pr_reqest.requestitems)
                    {

                        unit_price = x.reqqty * x.averagecost;
                        total_price += unit_price;
                        table_procedure += $@"
                                        <tr>
                                            <td>
                                                {x.lineno}.
                                            </td>
   <td>
                                                {x.stockcode}
                                            </td>

<td>
                                                {x.stockdesc}
                                            </td>
<td>
                                                {x.unitdesc}
                                            </td>
<td>
                                                {x.reqqty}
                                            </td>
<td>
                                                {x.averagecost}
                                            </td>
                                            <td align='right'>{unit_price.ToString("0.00")}
                                            </td>
<td>
                                             {x.itemremarks}
                                            </td>
                                        </tr>"
                                            ;
                    }

                    pr_reqest.total_price = total_price;


                    PdfDocument doc = converter.ConvertHtmlString($@"
<html>
<head>
    {css}
</head>

<body>
    <div class='header'>
         <div class='brand-main-info'>
  <div class='brand-logo'>
               <img src='data:image/png;base64,{brand_logo}' class='brand-logo' />
 </div>
  <div class='brand-name'>
{brand_name}
<br>
  <div class='document-title'>
{brand_address}

 </div>

 </div>
<br>
<br>
<br>
  <div class='brand-sub-info'>
   Stock Requisition Slip
 </div>
            
    </div>

   </div>

  

    <div class='request-info-ctnr'>
        <div class='details'>
        </div>
     
    </div>



    <div class='info-container'>
    <div class='req-info'>
            <div class='info-item  info-group'>
                <div class='label'>
                   Requesting To
                </div>
                <div class='value'>
                {pr_reqest.requested_to}
                
                </div>
            </div>
            <div class=' info-item  info-group'>
                <div class='label'>
                    Requesting Dept.
                </div>
                <div class='value'>
                    {pr_reqest.requested_from}
                </div>
            </div>
       
        </div> 

        <div class='req-info'>
            <div class='info-item  info-group'>
                <div class='label'>
                    Request No. :
                </div>
                <div class='value'>
                   {pr_reqest.reqno}
                </div>
            </div>
            <div class=' info-item  info-group'>
                <div class='label'>
                    Request Date
                </div>
                <div class='value'>
                   {String.Format("{0:MMMM/dd/yyyy}", pr_reqest.reqdate)}
                </div>
            </div>
       
        </div>
  



    </div>

   <br>
             <br>

    <table>
        <thead>
            <tr>
                <td>
                    #
                </td>
  <td>
                    Code 
                </td>

  <td>
                    Description
                </td>
  <td>
                    Unit 
                </td>
  <td>
                    Qty
                </td>
                <td>
                    Cost
                </td>
  <td>
                    Amount
                </td>
  <td>
                    Item Remarks
                </td>
            </tr>
        </thead>
        <tbody>
            {table_procedure}
        </tbody>
    </table>

    <div class='footer'>
 <div class='footer-info-group'>

      <div class='label2'>Total Cost: &#8369;</div>
             {total_price.ToString("0.00")}
        </div>
  </div>
        <div class='footer-info-group'>
    <div class='label'>Remarks </div>
          {pr_reqest.reqremarks}
        </div>
        <br>
             <br>

    </div>
</body>

</html>

");

                    doc.Save(ms);
                    res = ms.ToArray();
                }

                return res;
                //return ms;
            }
        }
    }

}
