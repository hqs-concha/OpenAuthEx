
using Snowflake.Core;

namespace Tools.Utils
{
    /// <summary>
    /// 全局唯一Id生成辅助类
    /// </summary>
    public class KeyHelper
    {
        #region GUID

        /// <summary>
        /// 获取GUID(长度32)
        /// </summary>
        /// <returns></returns>
        public static string Guid()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 获取GUID(长度36)
        /// </summary>
        /// <returns></returns>
        public static string LongGuid()
        {
            return System.Guid.NewGuid().ToString();
        }

        #endregion

        #region snowflake 雪花算法

        private static IdWorker _idWorker;

        private static void Init(long wordId, long dataCenterId)
        {
            _idWorker = new IdWorker(wordId, dataCenterId);
        }

        /// <summary>
        /// 雪花算法生成的Long型ID，号称每秒可生成26万不重复的有序ID
        /// </summary>
        /// <param name="wordId"></param>
        /// <param name="dataCenterId"></param>
        /// <returns></returns>
        public static long NextId(long wordId = 1, long dataCenterId = 1)
        {
            if (_idWorker == null)
            {
                Init(wordId, dataCenterId);
            }

            lock (_idWorker)
            {
                var id = _idWorker.NextId();
                return id;
            }
        }

        #endregion
    }
}
