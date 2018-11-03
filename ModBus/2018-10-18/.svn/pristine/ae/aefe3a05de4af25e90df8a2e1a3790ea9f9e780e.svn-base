using Ksat.AppPlugIn.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugInUiLibrary.Login
{
    public class LoginFormManager : ILoginResultListener
    {
        private static LoginFormManager gInstance;
        public static LoginFormManager Instance()
        {
            lock (typeof(LoginFormManager))
            {
                if (gInstance == null)
                {
                    gInstance = new LoginFormManager();
                }
            }

            return gInstance;
        }

        public static void upgrade_profile_settings(string old_file)
        {
            string dest_path = Path.Combine(StorePassword.getLocalDataFile(), StorePassword.FILE_NAME);
            if (File.Exists(dest_path))
            {
                return;
            }

            if (File.Exists(old_file))
            {
                File.Copy(old_file, dest_path);
            }
        }


        private LoginFormManager()
        {
            mCurrentAccount = AccountType.Guest;
            mListenerList = new List<ILoginResultListener>();
        }

        public bool SupportAccountId { get; set; }

        public bool LockAccountId { get; set; }

        private List<ILoginResultListener> mListenerList;
        private AccountType mCurrentAccount;

        public AccountType getCurrentLoginAccount()
        {
            return mCurrentAccount;
        }

        public void registerListener(ILoginResultListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("registerListener() listener can't be null...");
            }

            lock (mListenerList)
            {
                if (mListenerList.Contains(listener))
                {
                    return;
                }
                mListenerList.Add(listener);
            }
        }

        public void unregisterListener(ILoginResultListener listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("unregisterListener() listener can't be null...");
            }
            lock (mListenerList)
            {
                mListenerList.Remove(listener);
            }
        }

        public string getLastLoginId()
        {
            return StorePassword.load().LastLoginId;
        }

        public void SetLoginId(string id)
        {
            StorePassword.load().LastLoginId = id;
        }

        public void LockLoginId(string id, bool lockid)
        {
            StorePassword cfg = StorePassword.load();
            cfg.LastLoginId = id;
            this.LockAccountId = lockid;
            cfg.Store();
        }

        /// <summary>
        /// 显示用户登录界面
        /// </summary>
        public Form ShowForm(Form owner)
        {
            Form form = LoginForm.show(this);
            form.Owner = owner;

            return form;
        }

        public DialogResult ShowFormDialog()
        {
            return LoginForm.showDialog(this);
        }

        public void onLoginResult(string loginid, AccountType t)
        {
            //throw new NotImplementedException();
            Logger.Info("onLoginResult(), id:"+loginid+ ", AccountType:" + t);
            mCurrentAccount = t;
            foreach (ILoginResultListener l in mListenerList)
            {
                l.onLoginResult(loginid, t);
            }
        }
    }
}
