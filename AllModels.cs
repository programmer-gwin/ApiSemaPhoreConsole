using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class AllModels
    {
    }

    public class UserInfo
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class UpdatedUssdRequest
    {
        public UserInfo userInfo { get; set; }
        public PosRequest posRequest { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string CustomerEmail { get; set; }
        public string client_id { get; set; }
        public string client_trans_id { get; set; }
        public string username { get; set; }
        public List<ItemList> subscription_list { get; set; }
    }

    public class PosRequest
    {
        public decimal? amount { get; set; }
        public string reference { get; set; }
        public string tnx { get; set; }
    }

    public class ItemList
    {
        public decimal amount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class GenRefReceiveModel
    {
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string amount { get; set; }
        public string responsemessage { get; set; }
        public string tnx { get; set; }
    }

    public class ResponseReceiveModel
    {
        public string responseCode { get; set; }
        public string reference { get; set; }
        public string amount { get; set; }
        public string terminalId { get; set; }
        public string merchantId { get; set; }
        public string merchantname { get; set; }
        public string responsemessage { get; set; }
        public string retrievalReference { get; set; }
        public string shortName { get; set; }
        public string shortcode { get; set; }
        public string customer_mobile { get; set; }
        public string tnx { get; set; }
        public decimal feeAmount { get; set; }

        //my custom model 
        public string TransType { get; set; }
        public string TransTime { get; set; }
    }

    public class PaymentItem
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal ItemAmount { get; set; }
    }

    public class BankModel
    {
        public string BankName { get; set; }
        public string BankUSSDCode { get; set; }
    }

}
