// Gary.Zhang
// Gary@jzspace.com
// 2018/7/28
using System;
namespace Ksat.CommonModelLibrary.Station
{
    [Flags]
    public enum StationProperty : UInt64
    {
        None = 0,

        #region 软件自动计算，无需界面配置
        /// <summary>
        /// 工站头
        /// </summary>
        MachineHeader = (1 << 0),

        /// <summary>
        /// 工站尾
        /// </summary>
        MachineTail = (1 << 1),

        /// <summary>
        /// 工段头
        /// </summary>
        SectionHeader = (1 << 2),

        /// <summary>
        /// 工段尾
        /// </summary>
        SectionTail = (1 << 3),
        #endregion

        /// <summary>
        /// 禁止使用
        /// </summary>
        [Attr.Description("禁止使用")]
        Disable = (1 << 4),

        /// <summary>
        /// 容许修改加工结果，只能从OK到OK，或者OK到NG
        /// </summary>
        [Attr.Description("容许修改加工结果，只能从OK到OK，或者OK到NG")]
        AllowSetResult = (1 << 5),

        /// <summary>
        /// 容许重置结果，可能重置为任意值
        /// </summary>
        [Attr.Description("容许重置结果，可能重置为任意值")]
        AllowResetResult = (1 << 6),

        /// <summary>
        /// 容许重置结果为NG，不容许重置为OK，只能从OK到NG，NG1到NG2
        /// </summary>
        [Attr.Description("容许重置结果为NG，不容许重置为OK，只能从OK到NG，NG1到NG2")]
        AllowResetToNg = (1 << 7),

        /// <summary>
        /// 填充虚拟产品
        /// </summary>
        [Attr.Description("填充虚拟产品，产生虚拟产品码")]
        FixVisualProduct = (1 << 8),

        /// <summary>
        /// 允许上虚拟载具，自动创建虚拟载具码
        /// </summary>
        [Attr.Description("允许上载具，如果没有载具码，会自动创建虚拟载具码")]
        AllowFixFixture = (1 << 9),

        /// <summary>
        /// 允许投放载具，自动解绑
        /// </summary>
        [Attr.Description("允许自动解绑")]
        AutoUnBind = (1 << 10),

        /// <summary>
        /// 如果ok就自动解绑
        /// </summary>
        [Attr.Description("如果ok就自动解绑")]
        OkAutoUnBind = (1 << 11),

        /// <summary>
        /// 如果NG就自动解绑
        /// </summary>
        [Attr.Description("如果NG就自动解绑")]
        FailedAutoUnBind = (1 << 12),

        /// <summary>
        /// 如果是非废品NG就自动解绑。
        /// </summary>
        [Attr.Description("如果是非废品NG就自动解绑")]
        NotDumpAutoUnBind = (1 << 13),

        /// <summary>
        /// 校验结果的时候，产品结果优先，否则就是载具优先
        /// </summary>
        [Attr.Description("校验结果的时候，产品结果优先，否则就是载具优先")]
        ProductFirstForVerifyResult = (1 << 14),

        /// <summary>
        /// 该工位容许载具码为空
        /// </summary>
        [Attr.Description("该工位容许载具码为空")]
        IgnoreEmptyFixture = (1 << 15),

        /// <summary>
        /// 该工位容许产品码为空
        /// </summary>
        [Attr.Description("该工位容许产品码为空")]
        IgnoreEmptyProduct = (1 << 16),

        /// <summary>
        /// 忽略跳站检验，该工位容许跳过
        /// </summary>
        [Attr.Description("忽略跳站检验，该工位容许跳过")]
        IgnoreSkipVerify = (1 << 17),


        #region Mes Event

        [Attr.Description("需要呼叫Mes接口")]
        MesCheckIn = ((ulong)1 << 30),

        [Attr.Description("需要呼叫Mes接口")]
        MesCarrierBind = ((ulong)1 << 31),

        [Attr.Description("需要呼叫Mes接口")]
        MesCarrierCheck = ((ulong)1 << 32),

        [Attr.Description("需要呼叫Mes接口")]
        MesFeedingCheck = ((ulong)1 << 33),

        [Attr.Description("需要呼叫Mes接口")]
        MesDataCollection = ((ulong)1 << 34),

        [Attr.Description("需要呼叫Mes接口")]
        MesVerifyTray = ((ulong)1 << 35),

        [Attr.Description("需要呼叫Mes接口")]
        MesLinkComp = ((ulong)1 << 36),

        [Attr.Description("需要呼叫Mes接口")]
        MesTrayBind = ((ulong)1 << 37),

        [Attr.Description("需要呼叫Mes接口")]
        MesCheckOut = ((ulong)1 << 40),
        #endregion

    }

    public sealed class StationPropertyHelper
    {
        public static bool IsSupportProperty(StationProperty properties, StationProperty value)
        {
            //int v = (int)value;
            if ((properties & value) == value)
                return true;

            return false;
        }

        public static bool IsSupportAnyProperty(StationProperty properties, params StationProperty[] values)
        {
            StationProperty v = StationProperty.None;
            foreach (StationProperty value in values)
            {
                v |= value;
            }

            if ((properties & v) != 0)
                return true;

            return false;
        }

        public static string GetEnumDescription(StationProperty _enum)
        {

            Type type = _enum.GetType();
            System.Reflection.FieldInfo fd = type.GetField(_enum.ToString());
            if (fd == null) return String.Empty;

            object[] attrs = fd.GetCustomAttributes(typeof(Attr.DescriptionAttribute), false);
            string name = String.Empty;
            if (attrs != null)
            {
                foreach (Attr.DescriptionAttribute attr in attrs)
                {
                    name = attr.Value;
                }
            }

            return name;
        }
    }
}
