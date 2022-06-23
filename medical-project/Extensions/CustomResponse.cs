namespace medical_project.Extensions
{
    public static class CustomResponse
    {
        public static object CustResponse(object o, bool status)
        {
            if (status)
            {
                var result = new { Status = true, Message = o };
                return result;
            }
            else
            {
                var Message = new { Status = false, Message = o };
                return Message;
            }
        }

    }
}
