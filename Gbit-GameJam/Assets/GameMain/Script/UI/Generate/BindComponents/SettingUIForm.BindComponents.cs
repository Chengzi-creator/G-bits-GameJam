using Autobind;
using UnityEngine;
using UnityEngine.UI;

//自动生成于：2024/11/30 10:14:14
namespace GameMain
{

	public partial class SettingUIForm
	{

		private Button m_btn_Setting;
		private Button m_btn_Retry;
		private Button m_btn_Home;
		private Button m_btn_Volume;
		private Button m_btn_Back;
		private Toggle m_tog_Ison;
		private Slider m_slider_Volume;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_btn_Setting = autoBindTool.GetBindComponent<Button>(0);
			m_btn_Retry = autoBindTool.GetBindComponent<Button>(1);
			m_btn_Home = autoBindTool.GetBindComponent<Button>(2);
			m_btn_Volume = autoBindTool.GetBindComponent<Button>(3);
			m_btn_Back = autoBindTool.GetBindComponent<Button>(4);
			m_tog_Ison = autoBindTool.GetBindComponent<Toggle>(5);
			m_slider_Volume = autoBindTool.GetBindComponent<Slider>(6);
		}

		private void ReleaseBindComponents()
		{
			//可以根据需要在这里添加代码，位置UIFormCodeGenerator.cs GenAutoBindCode()函数
		}

	}
}
