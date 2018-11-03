using Ksat.AppPlugIn.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Logging.log
{
    internal class FileLogger : ILogger
    {
        //log file stream writer
        private StreamWriter _writer;

        //public FileLogger()
        //{
            
        //}

        public void Dispose()
        {
            //throw new NotImplementedException();
            _writer.Flush();
            _writer.Close();
            _writer.Dispose();
            _writer = null;
        }

        public LoggerType GetLoggerType()
        {
            return LoggerType.File;
        }

        private string mLastFileName = String.Empty;
        private long mTimeSpan;
        private long mNextCheckTimeTicks = 0L;
        private const long MAX_FILE_LENGTH = (100 * 1024 * 1024);
        private void openFile()
        {
            if(_writer != null)
            {
                DateTime curnow = DateTime.Now;

                if (curnow.Ticks <= mNextCheckTimeTicks)
                    return;

                DateTime _timeSign = new DateTime(curnow.Year, curnow.Month, curnow.Day, curnow.Hour, curnow.Minute, curnow.Second);
                mNextCheckTimeTicks = _timeSign.AddMinutes(3).Ticks;

                if (curnow.Ticks >= mTimeSpan)
                {
                    _writer.Flush();
                    _writer.Close();
                }
                else if (!String.IsNullOrEmpty(mLastFileName))
                {
                    try
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(mLastFileName);
                        if (fileInfo == null || !fileInfo.Exists)
                        {
                            return;
                        }

                        if(fileInfo.Length <= MAX_FILE_LENGTH)
                        {
                            return;
                        }

                        _writer.Flush();
                        _writer.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("get file:"+mLastFileName+ " info Exception:" + e.Message);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            {
                string logPath = Path.Combine(Logger.mLogPath, "Log");
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                DateTime now = DateTime.Now;
                DateTime _timeSign = new DateTime(now.Year, now.Month, now.Day);
                mTimeSpan = _timeSign.AddDays(1).Ticks;

                _timeSign = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                mNextCheckTimeTicks = _timeSign.AddMinutes(3).Ticks;

                string file_name = Path.Combine(logPath,
                    String.Format("{0}_Log_{1}.txt", AppHelper.GetApplicationName(), now.ToString("yyyyMMdd_HHmm")));

                _writer = new StreamWriter(file_name, true, Encoding.UTF8);
                mLastFileName = file_name;
            }
        }

        public void Remove_old_file()
        {
            string root_dir = Path.Combine(Logger.mLogPath, "Log");
            if (!Directory.Exists(root_dir))
            {
                Console.WriteLine("Remove_old_file(), root dir not existing...");
                return;
            }

            string last_day = DateTime.Now.AddDays(-LoggerImpl.Instance().Holding_Log_Day).ToString("yyyyMMdd");
            int last_day_int = Convert.ToInt32(last_day);

            foreach(string file in Directory.EnumerateFiles(root_dir))
            {
                string[] item = Path.GetFileNameWithoutExtension(file).Split('_');
                if (item == null || item.Length == 0)
                    continue;

                int cur_log_day = 0;
                int index = 0;
                if(item.Length < 3)
                {
                    index = 0;
                    
                }
                else if(item.Length == 3)
                {
                    index = 2;
                }
                else
                {
                    index = item.Length - 2;
                }

                if (!int.TryParse(item[index], out cur_log_day) || cur_log_day >= last_day_int)
                {
                    continue;
                }

                Console.WriteLine("start remove file:" + file + ", day:" + cur_log_day+", "+ item[index]+", last day:"+ last_day_int);

                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("remove file:"+file+", exception:"+ex.ToString());
                }
            }
        }

        public void Write(Model.Args.LogMessageEventArgs args)
        {
            try
            {
                openFile();

                _writer.WriteLine(string.Concat(args.TimeTicks, " ",
                                                    args.Tag, " ",
                                                    args.LogType.ToString().Substring(0, 1), " ",
                                                    args.Value));
                _writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write file error:" + ex.ToString());
            }
        }

        public void Write(string msg, string tag, LogMessageType msgtype)
        {
            try
            {
                openFile();

                _writer.WriteLine(msg);
                _writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write file error:"+ex.ToString());
            }
            
        }
    }
}
