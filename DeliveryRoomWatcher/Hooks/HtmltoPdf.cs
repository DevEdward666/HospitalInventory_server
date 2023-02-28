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

            public static Byte[] geratePRPdf(string brand_name, string brand_logo, string brand_address, string brand_phone, string queueno, string counter, string countertype, DateTime date)
            {

                Byte[] res = null;
                MemoryStream ms = null;

                using (ms = new MemoryStream())
                {

                    HtmlToPdf converter = new HtmlToPdf();
                    converter.Options.PdfPageSize = PdfPageSize.A6;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                    string css = @"<style>
     
        body {
            display: flex;
            align-content: flex-center;
            flex-direction: column;
            font-family: Arial;
            padding: 10px;
            font-size: 14px;
            width:100%;
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
            height: 165px;
            width: 165px;
            padding: 5px;
        }

        .header .brand-main-info .brand-name {
            font-weight: 900;
            font-size: 36px;
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
            font-size: 24px;
            padding: 5px;
            max-width: 100%;
            width:100%;

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
            width:100%
        }

        .req-info {
            flex-grow: 1;
            margin-right: 20px;
            width:100%;
        }

        .pay-info {
            flex-grow: 1;
            margin-left: 20px;
        }

        .counter-name{
            margin-top: 10px;
            margin-bottom: 5px;
            font-weight: 900;
            font-size: 72px;
            align-text:center;
            align-items: center;
            align-content: center;
            justify-content: center;
        }
        .info-title {
            font-weight: 900;
            font-size: 72px;
            opacity: .6;
            margin-top: 10px;
            margin-bottom: 5px;
            align-text:center;
            align-items: center;
            align-content: center;
            justify-content: center;
        }
        .datetime {
            font-weight: 900;
            font-size: 36px;
            opacity: .6;
            margin-top: 10px;
            margin-bottom: 5px;
            align-text:center;
            align-items: center;
            align-content: center;
            justify-content: center;
        }
        .info-subtitle {
            font-weight: 900;
            font-size: 36px;
            opacity: .6;
            margin-top: 10px;
            margin-bottom: 5px;
            justify-self:center;
            align-text:center;
            align-items: center;
            align-content: center;
            justify-content: center;
        }

        .info-group {
            display: flex;
            padding: 7px 0;
            align-items: center;
            align-content: center;
            justify-content: center;
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
    </div>
   </div>



            <div class='info-container'>
            <div class='req-info'>
            <div class='info-item  info-group'>
          
                <div class='counter-name'>
                   {counter}
                </div>
            </div>
            <div class='info-item  info-group'>
          
                <div class='info-title'>
                   {queueno}
                </div>
            </div>
        <div class='info-item  info-group'>
                <div class='datetime'>
                     {String.Format("{0:MMM-dd-yyyy hh:mm }", date)}
                </div>
         </div>
        </div> 

    
  



    </div>

    
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
