using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace IDPA_Steganographie.HelperClasses.WaitDialogHelper
{
    /// <summary>
    /// Displays a progressring while the application is doing something in the background
    /// </summary>
    class WaitDialogHelper
    {
        private static BackgroundWorker _Worker;
        private static WaitDialog _ThreadDialog;
        private static Action _Action;
        private static Exception _Exception;
        private static Action<Exception> _FinishedAction;
        public static event EventHandler WaitDialogStarted;
        public static event EventHandler WaitDialogEnded;

        /// <summary>
        /// Shows a progressring while the application is doing something in the background
        /// </summary>
        /// <param name="infotext">Text that is shown while the progressring is visible</param>
        /// <param name="threadMethod">Action that can be done on an other thread (f.e. math-stuff)</param>
        /// <param name="finishedMethod">Action that has to be done on the GUI-Thread (GUI-stuff)</param>
        public static void ExecuteThread(string infotext, Action threadMethod, Action<Exception> finishedMethod)
        {
            _ThreadDialog = new WaitDialog();
            _ThreadDialog.Show(infotext + " ...");
            _Action = threadMethod;
            _FinishedAction = finishedMethod;
            _Worker = new BackgroundWorker();
            _Worker.DoWork += _Worker_DoWork;
            _Worker.RunWorkerCompleted += _Worker_RunWorkerCompleted;
            _Exception = null;
            _Worker.RunWorkerAsync();
        }

        /// <summary>
        /// Starts the finish action that will be done on the GUI-Thread and removes the progressring from the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _Exception = e.Error;
            if (_Worker != null)
            {
                _Worker.DoWork -= _Worker_DoWork;
                _Worker.RunWorkerCompleted -= _Worker_RunWorkerCompleted;
            }
            if (_ThreadDialog != null)
                _ThreadDialog.Close();
            _Action = null;
            _ThreadDialog = null;
            if (_Worker != null)
                _Worker.Dispose();
            _Worker = null;
            if (_FinishedAction != null)
                _FinishedAction(_Exception);
        }

        /// <summary>
        /// Execute the main action that will be done on an other thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_Action != null)
                _Action();
        }

        public static async Task ExecuteThreadAsync(string infotext, Action threadMethod, Action<Exception> finishAction)
        {
            await ExecuteThreadAsync(infotext, async () => await Task.Factory.StartNew(threadMethod), finishAction);
        }

        public static async Task ExecuteThreadAsync(string infotext, Action threadMethod)
        {
            await ExecuteThreadAsync(infotext, async () => await Task.Factory.StartNew(threadMethod), execption =>
                                                                                                      {
                                                                                                          if(execption != null)
                                                                                                            throw execption;
                                                                                                      });
        }

        public static async Task ExecuteThreadAsync(string infotext, Func<Task> taskMethod, Action<Exception> finishAction)
        {
            if(WaitDialogStarted != null)
            {
                WaitDialogStarted(null, null);
            }
            _ThreadDialog = new WaitDialog();
            _ThreadDialog.Show(infotext + " ...");
            try
            {
                await taskMethod();
            }
            catch(Exception ex)
            {
                _Exception = ex;
            }

            if(_ThreadDialog != null)
                _ThreadDialog.Close();
            if(WaitDialogEnded != null)
            {
                WaitDialogEnded(null, null);
            }
            _ThreadDialog = null;

            finishAction(_Exception);
            _Exception = null;
        }

        public static async Task ExecuteThreadAsyncNoDialog(Action threadMethod)
        {
            await _ExecuteThreadAsyncNoDialog(async () => await Task.Factory.StartNew(threadMethod), execption =>
            {
                if (execption != null)
                    throw execption;
            });
        }

        private static async Task _ExecuteThreadAsyncNoDialog(Func<Task> taskMethod, Action<Exception> finishAction)
        {
            if (WaitDialogStarted != null)
            {
                WaitDialogStarted(null, null);
            }
            try
            {
                await taskMethod();
            }
            catch (Exception ex)
            {
                _Exception = ex;
            }

            if (_ThreadDialog != null)
                _ThreadDialog.Close();
            if (WaitDialogEnded != null)
            {
                WaitDialogEnded(null, null);
            }
            _ThreadDialog = null;

            finishAction(_Exception);
            _Exception = null;
        }
    }
}
