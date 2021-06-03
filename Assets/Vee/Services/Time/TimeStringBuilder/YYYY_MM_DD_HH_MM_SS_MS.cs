

namespace Vee.Services.Time {
    public class TSB_YYYY_MM_DD_HH_MM_SS_MS : TimeStringBuilderBase {

        public override string Build(System.DateTime cst) {
            var str = string.Format($"{cst.Year}_{cst.Month}_{cst.Day}_{cst.Hour}_{cst.Minute}_{cst.Second}_{cst.Millisecond}");
            return str;
        }
    }

}