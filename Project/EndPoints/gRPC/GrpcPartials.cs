namespace Dariosoft.EmailSender.EndPoint.Abstraction.GrpcInterface
{
    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_Result
    {
        public static GrpcResult UnExcpectedError()
            => new GrpcResult
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }

    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_Result_Dariosoft_EmailSender_EndPoint_Abstraction_Models_Common_BaseModel_
    {
        public static GrpcResult_BaseModel UnExcpectedError()
            => new GrpcResult_BaseModel
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }

    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_Result_Dariosoft_EmailSender_EndPoint_Abstraction_Models_Host_HostModel_
    {
        public static GrpcResult_HostModel UnExcpectedError()
            => new GrpcResult_HostModel
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }

    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_ListResult_Dariosoft_EmailSender_EndPoint_Abstraction_Models_Host_HostModel_
    {
        public static GrpcListResult_HostModel UnExcpectedError()
            => new GrpcListResult_HostModel
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }

    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_Result_Dariosoft_EmailSender_EndPoint_Abstraction_Models_Account_AccountModel_
    {
        public static GrpcResult_AccountModel UnExcpectedError()
            => new GrpcResult_AccountModel
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }

    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_ListResult_Dariosoft_EmailSender_EndPoint_Abstraction_Models_Account_AccountModel_
    {
        public static GrpcListResult_AccountModel UnExcpectedError()
            => new GrpcListResult_AccountModel
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }

    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_Result_Dariosoft_EmailSender_EndPoint_Abstraction_Models_Message_MessageModel_
    {
        public static GrpcResult_MessageModel UnExcpectedError()
            => new GrpcResult_MessageModel
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }

    partial class GrpcDariosoft_EmailSender_EndPoint_Abstraction_Models_ListResult_Dariosoft_EmailSender_EndPoint_Abstraction_Models_Message_MessageModel_
    {
        public static GrpcListResult_MessageModel UnExcpectedError()
            => new GrpcListResult_MessageModel
            {
                IsSuccessful = false,
                Errors = { new GrpcReason[] { new GrpcReason { Text = "Unexpected error.", Code = "" } } }
            };
    }
}
