namespace eWAY.Rapid.Internals.Response
{
    internal class CaptureAuthBaseResponse: BaseResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public int TransactionID { get; set; }
        public bool TransactionStatus { get; set; }
    }
}
