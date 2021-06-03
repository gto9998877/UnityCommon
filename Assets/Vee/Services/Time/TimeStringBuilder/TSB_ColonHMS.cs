using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vee.Services.Time {
    public class TSB_ColonHMS : TimeStringBuilderBase {
        public override string BuildSpan(long RawSpan) {   
            RawSpan /= 1000; //转换为秒

            var hour = RawSpan / 3600;  RawSpan -= hour * 3600;
            var minite = RawSpan / 60;
            var second = RawSpan - minite * 60;

            var hourString = hour < 10 ? $"0{hour}" : $"{hour}";
            var minString = minite < 10 ? $"0{minite}" : $"{minite}";
            var secString = second < 10 ? $"0{second}" : $"{second}";

            if (hour > 0) {
                return $"{hourString}:{minString}:{secString}";
            }

            return minite > 0 ? $"{minString}:{secString}" : $"{secString}";
        }
        
        public override string Build(System.DateTime cst) {
            var hourString = cst.Hour < 10 ? $"0{cst.Hour}" : $"{cst.Hour}";
            var minString = cst.Minute < 10 ? $"0{cst.Minute}" : $"{cst.Minute}";
            var secString = cst.Second < 10 ? $"0{cst.Second}" : $"{cst.Second}";
            
            var str = string.Format($"{hourString}:{minString}:{secString}");
            return str;
        }
    }

    
    public class TSB_ColonHMS_MS : TimeStringBuilderBase {
        public override string BuildSpan(long RawSpan) {   
            var hour = RawSpan / 3600000;  RawSpan -= hour * 3600000;
            var minite = RawSpan / 60000;  RawSpan -= minite * 60000;
            var second = RawSpan / 1000; RawSpan -= second * 1000;
            var millisecond = RawSpan;

            string hourStr = string.Format($"{hour}:{minite}:{second}.{millisecond}");
            string miniteStr = string.Format($"{minite}:{second}.{millisecond}");
            string secondStr = string.Format($"{second}.{millisecond}");

            if (hour > 0) {
                return hourStr;
            }

            return minite > 0 ? miniteStr : secondStr;
        }
        
        public override string Build(System.DateTime cst) {
            var str = string.Format($"{cst.Hour}:{cst.Minute}:{cst.Second}.{cst.Millisecond}");
            return str;
        }
    }
}