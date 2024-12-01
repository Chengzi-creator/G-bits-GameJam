//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024/12/1 10:49:01
//------------------------------------------------------------
using UnityGameFramework.Runtime;
using System.IO;
using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Party;
using UnityEngine;

namespace GameMain
{
	/// <summary>
	/// 题库表
	/// </summary>
	public class DRTestHub : DataRowBase
	{
		 private int m_Id = 0;

		/// <summary>
		/// 获取编号
		/// </summary>
		public override int Id
		{
			get
			{
				return m_Id;
			}
		}
		/// <summary>
		/// 获取题目描述
		/// </summary>
		public string TestName
		{
			get;
			private set;
		}
		/// <summary>
		/// 获取回答1
		/// </summary>
		public string Answer1
		{
			get;
			private set;
		}
		/// <summary>
		/// 获取回答2
		/// </summary>
		public string Answer2
		{
			get;
			private set;
		}
		/// <summary>
		/// 获取回答3
		/// </summary>
		public string Answer3
		{
			get;
			private set;
		}
		/// <summary>
		/// 获取正确答案
		/// </summary>
		public int Correct
		{
			get;
			private set;
		}
		public override bool ParseDataRow(string dataRowString, object userData)
		{
			string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
			for (int i = 0; i < columnStrings.Length; i++)
			{
				columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
			}

			int index = 0;
			index++;
			m_Id = int.Parse(columnStrings[index++]);
			TestName = columnStrings[index++];
			Answer1 = columnStrings[index++];
			Answer2 = columnStrings[index++];
			Answer3 = columnStrings[index++];
			Correct = int.Parse(columnStrings[index++]);

			GeneratePropertyArray();
			return true;
		}

		private void GeneratePropertyArray()
		{

		}
	}
}
