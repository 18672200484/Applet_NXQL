using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CMCS.Common
{
	/// <summary>
	/// 打印配置
	/// </summary>
	public class PrintAppConfig
	{
		private static string ConfigXmlPath = "Print.AppConfig.xml";

		private static PrintAppConfig instance;

		public static PrintAppConfig GetInstance()
		{
			return instance;
		}

		static PrintAppConfig()
		{
			instance = CMCS.Common.Utilities.XOConverter.LoadConfig<PrintAppConfig>(ConfigXmlPath);
		}

		/// <summary>
		/// 保存配置
		/// </summary>
		public void Save()
		{
			CMCS.Common.Utilities.XOConverter.SaveConfig(instance, ConfigXmlPath);
		}

		private int _TitleFontSize = 26;
		/// <summary>
		/// 标题字体大小
		/// </summary>
		public int TitleFontSize
		{
			get { return _TitleFontSize; }
			set { _TitleFontSize = value; }
		}

		private string _TitleFont = "宋体";
		/// <summary>
		/// 标题字体
		/// </summary>
		public string TitleFont
		{
			get { return _TitleFont; }
			set { _TitleFont = value; }
		}

		private string _TitleContent = "国电投青铝发电有限公司过磅单";
		/// <summary>
		/// 标题内容
		/// </summary>
		public string TitleContent
		{
			get { return _TitleContent; }
			set { _TitleContent = value; }
		}

		private int _ContentFontSize = 20;
		/// <summary>
		/// 内容字体大小
		/// </summary>
		public int ContentFontSize
		{
			get { return _ContentFontSize; }
			set { _ContentFontSize = value; }
		}

		private string _ContentFont = "宋体";
		/// <summary>
		/// 内容字体
		/// </summary>
		public string ContentFont
		{
			get { return _ContentFont; }
			set { _ContentFont = value; }
		}

	}
}
