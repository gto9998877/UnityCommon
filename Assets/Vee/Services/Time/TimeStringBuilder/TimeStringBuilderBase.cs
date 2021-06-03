using System;
namespace Vee.Services.Time {
    public class TimeStringBuilderBase {
        /// <summary>
        /// 转换时间span为string
        /// </summary>
        /// <param name="RawSpan">毫秒</param>
        /// <returns></returns>
        public virtual string BuildSpan(long RawSpan) {
            return RawSpan.ToString();
        }
        
        public virtual string BuildSpan(TimeSpan RawSpan) {
            return RawSpan.Ticks.ToString();
        }
        
        /// <summary>
        /// 转换C#时间为string
        /// </summary>
        /// <param name="cst"></param>
        /// <returns></returns>
        public virtual string Build(System.DateTime cst) {
            return cst.ToString();
        }
    }

}
