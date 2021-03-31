
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System;

namespace ClienteMarWPFWin7.Domain.Helpers
{
    public static class CmdHelper
    {
        /// <span class="code-SummaryComment"><summary></span>
        /// Executes a shell command synchronously.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="command">string command</param></span>
        /// <span class="code-SummaryComment"><returns>string, as output of the command.</returns></span>
        public static void RunCmdCommand(object command)
        {
            try
            {

                var toExecute = command as string;

                if (toExecute != null && toExecute != string.Empty)
                {
                    toExecute = ReplaceCommandSection(toExecute, oldSection: "explorer|", newSection: "explorer "); //** explorer en caso


                    // create the ProcessStartInfo using "cmd" as the program to be run,
                    // and "/c " as the parameters.
                    // Incidentally, /c tells cmd that we want it to execute the command that follows,
                    // and then exit.
                    System.Diagnostics.ProcessStartInfo procStartInfo =
                        new System.Diagnostics.ProcessStartInfo("cmd", "/c " + toExecute);

                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.UseShellExecute = false;

                    // Do not create the black window.
                    procStartInfo.CreateNoWindow = true;

                    // Now we create a process, assign its ProcessStartInfo and start it
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStartInfo;
                    proc.Start();
                }
            }
            catch (Exception excepcion)
            {
                throw excepcion;
            }
        }


        /// <span class="code-SummaryComment"><summary></span>
        /// Execute the command Asynchronously.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="command">string command.</param></span>
        public static void RunCmdCommandAsync(string command)
        {
            try
            {
                //Asynchronously start the Thread to process the Execute command request.
                Thread objThread = new Thread(new ParameterizedThreadStart(RunCmdCommand));

                //Make the thread as background thread.
                objThread.IsBackground = true;

                //Set the Priority of the thread.
                objThread.Priority = ThreadPriority.AboveNormal;

                //Start the thread.
                objThread.Start(command);
            }
            catch (ThreadStartException objException)
            {
                throw objException;
            }
            catch (ThreadAbortException objException)
            {
                throw objException;
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }


        public static string ReplaceCommandSection(string command, string oldSection, string newSection)
        {
            foreach (var item in new string[] { command, oldSection, newSection })
            {
                if (item == null ||
                    item == string.Empty ||
                    string.IsNullOrEmpty(item) ||
                    string.IsNullOrWhiteSpace(item))
                {
                    return string.Empty;
                }
            }

            return command.Replace(oldSection, newSection);
        }




    }//fin de clase CmdCommandRunner
}//fin de namespace
