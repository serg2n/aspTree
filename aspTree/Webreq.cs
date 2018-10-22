using System;
using System.Net;
using System.IO;

namespace aspTree
{
    public class Webreq
    {
        [STAThread]
        public string GetReq()
        {
            string webString = SetReqString(DateTime.Now);
            WebRequest MyRequest = WebRequest.Create(webString);
            WebResponse MyResponse = MyRequest.GetResponse();
            Stream MyStream = MyResponse.GetResponseStream();
            StreamReader MyReader = new StreamReader(MyStream);
            string MyWebLine;
            string reqString="";

            while ((MyWebLine = MyReader.ReadLine()) != null)
            {
                reqString += MyWebLine;
            }

            MyStream.Close();
            return reqString;
        }
        private string SetReqString(DateTime dat1)
        {
            string szHeader = "http://mdr.stim.by/records3.php?start_d=";
            string szData = String.Format("{0:yyyy.dd.MM}", dat1);
            szHeader += szData;
            szHeader += "&stop_d=";
            DateTime dat2 = dat1.AddDays(1);
            szData = String.Format("{0:yyyy.dd.MM}", dat2);
            szHeader += szData;
            return szHeader;
        }
    }
    public class TimeService
    {
        public string GetTime() => System.DateTime.Now.ToString("hh:mm:ss");
    }
    public class DateService
    {
        public string GetDate() => System.DateTime.Now.ToString("dd.MM.yyyy");
    }
}
