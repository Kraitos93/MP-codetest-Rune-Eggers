using System.Linq;
using System.Threading.Tasks;

namespace LogTest
{
    public interface ILog
    {
        /// <summary>
        /// Stop the logging. All outstanding logs will be written to Log.
        /// </summary>
        void Stop_Without_Flush();

        /// <summary>
        /// Stop the logging. If there are any outstanding logs these will not be written to Log
        /// </summary>
        void Stop_With_Flush();

        /// <summary>
        /// Write a message to the Log.
        /// </summary>
        /// <param name="message">The message written to the log</param>
        /// <returns>Timestamp for logged message</returns>
        string WriteLog(string message);


    }
}
