using Ksat.AppPlugIn.UiCtrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksat.AppPlugIn.MultiLanguage
{
    public sealed class MultiLanguageManager
    {
        private static MultiLanguageManager sInstance;

        public static MultiLanguageManager Instance()
        {
            lock (typeof(MultiLanguageManager))
            {
                if (sInstance == null)
                    sInstance = new MultiLanguageManager();
            }

            return sInstance;
        }

        private LinkedList<AbstractFormBase> mObjectList = new LinkedList<AbstractFormBase>();

        private System.Globalization.CultureInfo mCultureInfo;
        
        private MultiLanguageManager()
        {
            mCultureInfo = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.InstalledUICulture.Name);
        }

        internal System.Globalization.CultureInfo getCultureInfo()
        {
            return mCultureInfo;
        }

        /// <summary>
        /// 注册FORM
        /// </summary>
        /// <param name="item"></param>
        public void RegObject(AbstractFormBase item)
        {
            if (mObjectList.Contains(item) != true)
            {
                mObjectList.AddLast(item);
            }
        }

        public void RemoveObject(AbstractFormBase item)
        {
            if(item != null)
                mObjectList.Remove(item);
        }

        public LanguageEnum getCurrentLanguage()
        {
            switch (mCultureInfo.Name)
            {
                case "zh-CN":
                    return LanguageEnum.CN;
                case "zh-HK":
                    return LanguageEnum.zh_HK;
                case "zh-TW":
                    return LanguageEnum.zh_TW;
                case "EN":
                    return LanguageEnum.EN;
                default:
                    return LanguageEnum.Default;
            }
        }

        public System.Globalization.CultureInfo getCultureInfo(LanguageEnum lg)
        {
            switch (lg)
            {
                case LanguageEnum.CN:
                    return new System.Globalization.CultureInfo("zh-CN");

                case LanguageEnum.EN:
                    return new System.Globalization.CultureInfo("EN");

                case LanguageEnum.zh_TW:
                    return new System.Globalization.CultureInfo("zh-TW");

                default:
#if DEBUG
                    Console.WriteLine(String.Format("Not support language:" + lg));
#endif
                    return new System.Globalization.CultureInfo(System.Globalization.CultureInfo.InstalledUICulture.Name);
            }
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="lg">语言种类</param>
        public void SetLanguage(LanguageEnum lg)
        {

            mCultureInfo = getCultureInfo(lg);

            System.Threading.Thread.CurrentThread.CurrentUICulture = mCultureInfo;
#if DEBUG
            Console.WriteLine(String.Format("SetLanguage:{0}", lg));
#endif

            //mResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(Properties.Resources));
            SwitchLanguage();
        }

        /// <summary>
        /// 遍历注册过的FORM，切换语言
        /// </summary>
        void SwitchLanguage()
        {
            //遍历所有Form，以切换其语言
            //foreach (FormBase form in objectList)
            LinkedListNode<AbstractFormBase> nodeNow = mObjectList.First;
            while (nodeNow != null)
            {
                nodeNow.Value.SwitchLanguage();
                nodeNow = nodeNow.Next;
            }
        }
    }
}
