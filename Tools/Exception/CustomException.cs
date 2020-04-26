
namespace Tools.Exception
{
    /// <summary>
    /// 自定义系统异常
    /// </summary>
    public class CustomException : System.Exception
    {
        public CustomException()
        {

        }

        public CustomException(string errMsg) : base(errMsg)
        {

        }
    }
}
