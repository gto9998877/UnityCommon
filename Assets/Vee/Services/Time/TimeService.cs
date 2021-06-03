using System;
using System.Collections;
using System.Collections.Generic;
using Vee.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Vee.Consts;
using Vee.Services.Localize;
using Vee.Services.Time;

namespace Vee.Services.Times
{
    [Serializable]
    public class ServerTimeString
    {
        public string OutStr;
    }

    /// <summary>
    /// 时间字符串格式
    /// </summary>
    public enum TimeStringFormat {
        Default = 0,
        _h_m_s = 1,       // XhXmXs 
        ColonHMS = 2,    // 冒号分隔，XX:XX:XX， 不足10补0
        ColonHMS_MS = 3, // 冒号分隔, XX:XX:XX.XX
        _m_d = 4,        // X月X日
        YYYY_MM_DD_HH_MM_SS_MS,
        
        _day_h_m,    // X天X小时X分钟
    }

    public class TimeService : ServiceBase {
        protected override void OnInitialize() {
            base.OnInitialize();
            requestingServerTime = false;
            IsHoliday = false;
            
            standardTime = GetStandardTime();
            InitStringBuilders();

            if (! localMode)
            {
                SyncWithServer();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"[TimeService] use local time");
#endif
            }
        }

        public void SyncWithServer()
        {
            StartCoroutine(new InterceptableEnumerator(RequestServer(), Debug.LogWarning));
        }
        
        
        public static TimeService Instance => ServiceManager.Get<TimeService>();

        public void LogTime(string prefix) {
            Debug.Log($"[{prefix}] time : {GetTimeNow()}");
        }

        /// <summary>
        /// 服务器是否可用
        /// </summary>
        /// <value><c>true</c> if server enabled; otherwise, <c>false</c>.</value>
        public bool ServerEnabled
        {
            get { return serverEnable || localMode; }
        }
        
        public bool IsHoliday = false; // 是否法定节假日
        
        /// <summary>
        /// 当前时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public long GetTimeNow()
        {
            return GetServerTimeStamp();
        }

        /// <summary>
        /// 当前时间戳(秒)
        /// </summary>
        /// <returns></returns>
        public int GetTimeNowInSecond () {
            return ToSecondInt(GetTimeNow());
        }

        /// <summary>
        /// 获取当前服务器时间的时间戳(毫秒)
        /// </summary>
        /// <returns>The time now.</returns>
        public long GetServerTimeStamp()
        {
            var off = localMode ? 0 : serverTimeOff;
            return GetLocalTimeStamp() + off;
        }
        /// <summary>
        /// 本地时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public long GetLocalTimeStamp()
        {
            return DateTimeToMilliSeconds(DateTime.UtcNow);
        }
        
        /// <summary>
        /// Utc时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public long GetLocalTimeStampUtc()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() * 1000;
        }
        
        /// <summary>
        /// C#当前时间
        /// </summary>
        /// <returns></returns>
        public System.DateTime GetNowDateTime() {
            return TimeStampToDataTime(GetTimeNow());
        }
        
        /// <summary>
        /// C#今天0点时间
        /// </summary>
        /// <returns></returns>
        public System.DateTime GetTodayDateTime()
        {
            var date = GetNowDateTime();
            return date.Date;
        }
        
        /// <summary>
        /// 北京时间
        /// </summary>
        /// <returns></returns>
        public System.DateTime  GetNowDateTimeBeijing()
        {
//            return TimeService.Instance.GetNowDateTime().ToLocalTime();
            return TimeService.Instance.GetNowDateTime().AddHours(8);
        }
        
        /// <summary>
        /// 计算到某个时间的剩余时间，可以为负值（表示当前时间已经过了完成时间）
        /// </summary>
        /// <param name="finishTime">完成时间</param>
        /// <returns></returns>
        public long CalcRestTimeTo (long finishTime)
        {
            return finishTime - GetTimeNow();
        }

        /// <summary>
        /// 计算从某个时间开始到现在的经过时间，可以为负值（表示当前时间还没有到开始时间）
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public long CalcPastTimeSince(long startTime)
        {
            return GetTimeNow() - startTime;
        }

        /// <summary>
        /// 计算时间进度，返回0-1的值
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="needTime">所需时间</param>
        /// <returns></returns>
        public float CalcProgress (long startTime, long needTime) {
            var past = CalcPastTimeSince(startTime);
            if (past <= 0) return 0f;
            else {
                float rate = (float)past / (float)needTime;
                return Mathf.Clamp01(rate);
            }
        }

        /// <summary>
        /// 比较a，b的天数，返回b-a天（自燃天）
        /// </summary>
        /// <param name="a">秒</param>
        /// <param name="b">秒</param>
        /// <returns>b-a的天数（自燃天）</returns>
        public int CompareDay(long a, long b)
        {
            var daySecs = 60 * 60 * 24;
            var aDay = (int)(a / daySecs);
            var bDay = (int)(b / daySecs);

            return bDay - aDay;
        }

        /// <summary>
        /// 是否是今天
        /// </summary>
        /// <param name="sec">Timestamp秒数</param>
        /// <returns></returns>
        public bool IsToday(long sec)
        {
            return CompareDay(GetTimeNowInSecond(), sec) == 0;
        }

        public bool BeforeToday(long sec)
        {
            return CompareDay(GetTimeNowInSecond(), sec) < 0;
        }

        public bool AfterToday(long sec)
        {
            return CompareDay(GetTimeNowInSecond(), sec) > 0;
        }

        /// <summary>
        /// 某个时间戳(秒)是不是星期几
        /// </summary>
        /// <param name="sec"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public bool IsDayOfWeek(long sec, DayOfWeek dayOfWeek) {
            var checkDay = GetDayOfWeek(sec);
            return checkDay.Equals(dayOfWeek);
        }

        /// <summary>
        /// 某个时间戳(秒)是星期几
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public DayOfWeek GetDayOfWeek(long sec) {
            var dt = TimestampSecondToDateTime(sec);
            return dt.DayOfWeek;
        }

        /// <summary>
        /// 今天是星期几
        /// </summary>
        /// <returns></returns>
        public DayOfWeek GetDayOfWeek() {
            return GetDayOfWeek(GetTimeNowInSecond());
        }

        /// <summary>
        /// 星期的名称
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string GetDayOfWeekName(DayOfWeek day) {
            string key;
            switch (day) {
                case DayOfWeek.Sunday:
                    key = "周日";
                    break;
                case DayOfWeek.Monday:
                    key = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    key = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    key = "周三";
                    break;
                case DayOfWeek.Thursday:
                    key = "周四";
                    break;
                case DayOfWeek.Friday:
                    key = "周五";
                    break;
                case DayOfWeek.Saturday:
                    key = "周六";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(day), day, null);
            }

            return LocalizeService.Instance.GetLocalizedString(key);
        }
        /// <summary>
        /// 判断某个时间戳(秒)是不是本周
        /// </summary>
        /// <param name="sec1"></param>
        /// <returns></returns>
        public bool IsThisWeek(long sec1) {
            return AreInSameWeek(sec1, GetTimeNowInSecond());
        }
        
        /// <summary>
        /// 比较两个时间戳(秒)是否是同一周
        /// </summary>
        /// <param name="sec1"></param>
        /// <param name="sec2"></param>
        /// <returns></returns>
        public bool AreInSameWeek(long sec1, long sec2)
        {
            var date1 = TimestampSecondToDateTime(sec1);
            var date2 = TimestampSecondToDateTime(sec2);

            return AreInSameWeek(date1, date2);
        }
        
        public bool AreInSameWeek(DateTime date1, DateTime date2)
        {
            date1 = new DateTime(date1.Year, date1.Month, date1.Day);
            date2 = new DateTime(date2.Year, date2.Month, date2.Day);
            return date1.AddDays(-(int)date1.DayOfWeek) == date2.AddDays(-(int)date2.DayOfWeek);
        }
        
        public bool AreInSameMonth(long sec1, long sec2)
        {
            var date1 = TimestampSecondToDateTime(sec1);
            var date2 = TimestampSecondToDateTime(sec2);

            return AreInSameMonth(date1, date2);
        }
        
        public bool AreInSameMonth(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month;
        }

        /// <summary>
        /// 比较两个时间戳是否是同一天
        /// </summary>
        /// <param name="sec1">秒单位</param>
        /// <param name="sec2">秒单位</param>
        /// <returns></returns>
        public bool AreInSameDay(long sec1, long sec2)
        {
            var date1 = TimestampSecondToDateTime(sec1);
            var date2 = TimestampSecondToDateTime(sec2);

            return AreInSameDay(date1, date2);
        }
        
        public bool AreInSameDay_ms(long msec1, long msec2)    // 毫秒，时间戳
        {
            var date1 = TimestampSecondToDateTime(msec1 / 1000);
            var date2 = TimestampSecondToDateTime(msec2 / 1000);

            return AreInSameDay(date1, date2);
        }
        
        public bool AreInSameDay(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }
        
        #region 时间转换
        /// <summary>
        /// 秒转换成毫秒
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static long ToMilliSecond (float second){
            return (long)(second * 1000);
        }

        /// <summary>
        /// 毫秒转换为秒(整数)
        /// </summary>
        /// <param name="milliSec"></param>
        /// <returns></returns>
        public static int ToSecondInt(long milliSec) {
            return (int)(milliSec / 1000);
        }
        
        /// <summary>
        /// 毫秒转换为秒(浮点数)
        /// </summary>
        /// <param name="milliSec"></param>
        /// <returns></returns>
        public static float ToSecondFloat (long milliSec) {
            return (float) milliSec / 1000f;
        }

        /// <summary>
        /// DateTime时间格式转换为13位的Unix时间戳（毫秒）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DateTimeToMilliSeconds(System.DateTime time)
        {
            return (long)((time - standardTime).TotalMilliseconds);
        }

        /// <summary>
        /// DateTime时间格式转换为10位的Unix时间戳（秒单位）
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int DateTimeToSeconds(System.DateTime time)
        {
            return (int)((time - standardTime).TotalSeconds);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public System.DateTime TimeStampToDataTime(long timeStamp) {
            var dtStart = GetStandardTime();
            var toNow = TimeSpan.FromMilliseconds(timeStamp);
            return dtStart.Add(toNow);
        }
        
        public DateTime TimestampSecondToDateTime(long timestamp)
        {
            var start = GetStandardTime();
            var ts = TimeSpan.FromSeconds(timestamp);
            return start.Add(ts);
        }
        
        /// <summary>
        /// 构建时间长度的字符串
        /// </summary>
        /// <param name="timeSpan">时间跨度，毫秒</param>
        /// <param name="format">字符串格式</param>
        /// <returns></returns>
        public string BuildTimeString(long timeSpanMs, TimeStringFormat format = TimeStringFormat._h_m_s) {
            var builder = GetStringBuilder(format);
            return builder.BuildSpan(timeSpanMs);
        }

        public string BuildTimeString(System.TimeSpan tSpan, TimeStringFormat format = TimeStringFormat._h_m_s) {
            var builder = GetStringBuilder(format);
            return builder.BuildSpan(tSpan);
        }
        
        public string BuildTimeString(System.DateTime cst, TimeStringFormat format = TimeStringFormat._h_m_s) {
            var builder = GetStringBuilder(format);
            return builder.Build(cst);
        }
        #endregion // 时间转换

        

        

        static DateTime GetStandardTime() {
            return TimeZoneInfo.ConvertTimeFromUtc(new System.DateTime(1970, 1, 1), TimeZoneInfo.Utc);
        }
        
        static DateTime standardTime;
        // 本地模式，为true时server默认可用，server时间等同于本机时间
        public static bool localMode = false;

        [BoxGroup("Server Status"), ShowInInspector, ReadOnly] 
        bool serverEnable = false;
        [BoxGroup("Server Status"), ShowInInspector, ReadOnly] 
        long serverTimeOff = 0;
        [BoxGroup("Server Status"), ShowInInspector, ReadOnly] 
        long lastRequestTime;
        [BoxGroup("Server Status")] 
        public int maxRetryCount = 5;

        Dictionary<TimeStringFormat, TimeStringBuilderBase> _stringBuilders;

        void InitStringBuilders() {
            _stringBuilders = new Dictionary<TimeStringFormat, TimeStringBuilderBase> {
                {TimeStringFormat.Default, new TimeStringBuilderBase()},
                // {TimeStringFormat._h_m_s, new TSB_HMS()},
                {TimeStringFormat.ColonHMS, new TSB_ColonHMS()},
                {TimeStringFormat.ColonHMS_MS, new TSB_ColonHMS_MS()},
                // {TimeStringFormat._m_d, new TSB_MonthDay()},
                {TimeStringFormat.YYYY_MM_DD_HH_MM_SS_MS, new TSB_YYYY_MM_DD_HH_MM_SS_MS()},
                // {TimeStringFormat._day_h_m, new TSB_DayMS()},
            };
        }

        TimeStringBuilderBase GetStringBuilder(TimeStringFormat format) {
            if (_stringBuilders.ContainsKey(format)) {
                return _stringBuilders[format];
            }
            else {
                Debug.LogWarning("Can't Find TimeStringBuilder For Format {format}");
                return _stringBuilders[TimeStringFormat.Default];;
            }
        }

        bool requestingServerTime = false;
        private IEnumerator RequestServer()
        {
            if (requestingServerTime) 
                yield break;
            
            requestingServerTime = true;
            for (var i=0; i< maxRetryCount; ++i)
            {
                var www = UnityWebRequest.Get(Urls.ServerTime);
                lastRequestTime = GetLocalTimeStampUtc();
                yield return www.SendWebRequest();

                if (! string.IsNullOrEmpty(www.error))
                {
                    Debug.LogWarning(www.error);
                    SetServerState(false);
                } 
                else
                {
                    string rawText = www.downloadHandler.text;
                    long ticks;
                    if (long.TryParse(rawText, out ticks))
                    {
                        SetServerState(true, ticks);
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Server Time Format Error : " + rawText);
                        SetServerState(false);
                    }
                }
            }

            if (serverEnable)
            {
                Debug.Log($"Server is enable, Server Time Offset : {serverTimeOff} MilliSeconds");

                // RequestServerCheckHoliday();
            }
            else
            {
                Debug.LogWarning("Server is not enable !");
            }
            
            requestingServerTime = false;
        }

        [Serializable]
        class RespIsHoliday
        {
            public bool isHoliday;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="serverTimeStamp">單位秒</param>
        void SetServerState(bool enable, long serverTimeStamp = 0)
        {
            //Debug.Log("Server Enabled : " + enable);
            serverEnable = enable;
            if (enable)
            {
                var off = serverTimeStamp * 1000 - lastRequestTime;
                SetUpServerTime(off);
            }
            else
            {
                SetUpServerTime(0);
            }
        }

        void SetUpServerTime(long timeOff)
        {
            //Debug.Log("Server Time Offset : " + timeOff.ToString());
            serverTimeOff = timeOff;
        }
    }
}