using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksat.AppPlugIn.Model.Settings;
using Ksat.CommonModelLibrary;
using Ksat.CommonModelLibrary.Station;

namespace MemoryEditor
{
    [Serializable]
    public class MemoryDesiginProfile : AbstractProfileSettings
    {
        public MemoryDesiginProfile() : base()
        {
            this.EventSections = new List<EventSection>();
        }

        public MemoryDesiginProfile(MemoryDesiginProfile other) : this()
        {
            this.CopyFrom(other);
        }

        public void CopyFrom(MemoryDesiginProfile other)
        {
            base.CopyFrom(other);

            this.EventSections.Clear();

            foreach (EventSection item in other.EventSections)
            {
                this.EventSections.Add((EventSection)item.Clone());
            }
        }

        public override AbstractProfileSettings Clone()
        {
            return new MemoryDesiginProfile(this);
        }

        protected override void onVersionUpgrade(int currentVersion)
        {

        }

        public override int GetLastVersionCode()
        {
            return 1;
        }

        public override void onFirstLoaded()
        {
            base.onFirstLoaded();
#if DEBUG
            initStationConfig();
#endif
        }

        /// <summary>
        /// 工段划分
        /// </summary>
        public List<EventSection> EventSections { get; set; }

        #region load and save
        public static void InitDataRootPath(string path = "")
        {
            ProfileHelper<MemoryDesiginProfile>.InitDataRootPath(path);
        }

        public static MemoryDesiginProfile ReLoad()
        {
            return ProfileHelper<MemoryDesiginProfile>.ReLoad();
        }

        public static MemoryDesiginProfile Load()
        {
            return ProfileHelper<MemoryDesiginProfile>.Load();
        }

        public static bool LoadFactory()
        {
            return ProfileHelper<MemoryDesiginProfile>.LoadFactory();
        }

        public static bool SaveFactory()
        {
            return ProfileHelper<MemoryDesiginProfile>.SaveFactory();
        }

        public bool Save()
        {
            return ProfileHelper<MemoryDesiginProfile>.Save(this);
        }

        public static bool Save(MemoryDesiginProfile obj)
        {
            return ProfileHelper<MemoryDesiginProfile>.Save(obj);
        }

        public static bool SaveAs(MemoryDesiginProfile obj, string path)
        {
            return ProfileHelper<MemoryDesiginProfile>.SaveAs(obj, path);
        }

        public static MemoryDesiginProfile LoadFrom(string path)
        {
            return ProfileHelper<MemoryDesiginProfile>.LoadFrom(path);
        }
        #endregion

        #region 初始化工站、工位配置信息
        private void initStationConfig()
        {
            if (this.EventSections.Count > 0)
                return;

            {
                Array items = Enum.GetValues(typeof(ProcedureSectionCode));
                foreach (ProcedureSectionCode item in items)
                {
                    if (item == ProcedureSectionCode.Unkown)
                        continue;

                    this.EventSections.Add(new EventSection()
                    {
                        EventName = item.ToString(),
                        MemoryBegin = 0,
                        MemoryEnd = 0,
                        DataSize = 0,
                        Description = String.Empty,
                    });
                }
            }
        }
        #endregion
    }
}
