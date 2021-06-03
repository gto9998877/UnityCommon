using System;
using Sirenix.OdinInspector;
using Vee.Services.Times;

namespace Vee.Debugs.Log {
    
    [Serializable]
    public class HistoryLog {
        const int defaultMaxCount = 10;
        public HistoryLog() {
            _table = new HistoryTable<string>(defaultMaxCount);
        }
        public HistoryLog(int maxCount) {
            _table = new HistoryTable<string>(maxCount);
        }

        public void SetMaxCount(int count) {
            _table.Resize(count);
        }

        public bool TimeStampSuffix = true;
        [ShowInInspector] [HideLabel]
        HistoryTable<string> _table;

        public void Add(string mainName, string subName = "") {
            var timeString = TimeStampSuffix ? BuildTimeStamp() : string.Empty;
            var logString = string.Format($"[{mainName}]-[{subName}][{timeString}]");
            _table.Add(logString);
        }
        string BuildTimeStamp() {
            return TimeService.Instance.BuildTimeString(TimeService.Instance.GetNowDateTime(), TimeStringFormat.ColonHMS_MS);
        }
    }
}