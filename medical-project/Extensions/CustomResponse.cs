namespace medical_project.Extensions
{
    public static class CustomResponse
    {
        public static object CustResponse(string message, bool status, object? o = null)
        {
            if (status)
            {
                var result = new { Status = status, Message = message, Data = o };
                return result;
            }
            else
            {
                var result = new { Status = status, Message = message, Data = o };
                return result;
            }
        }

    }
}
