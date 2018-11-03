using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.Utils
{
    static public class TaskHelper
    {
        /// <summary>
        /// delay
        /// </summary>
        /// <param name="dueTime"></param>
        static public Task Delay(int dueTime)
        {
            if (dueTime < -1) throw new ArgumentOutOfRangeException("dueTime");

            Timer timer = null;
            var source = new TaskCompletionSource<bool>();
            timer = new Timer(_ =>
            {
                using (timer) source.TrySetResult(true);
            }, null, dueTime, Timeout.Infinite);

            return source.Task;
        }
    }
}
