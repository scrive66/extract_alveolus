using System;
using System.Runtime.InteropServices;

using Pluto;

namespace Plugin
{
	[PluginType( PluginCategory.Filter )]
	public class FilterPlugin : ICommonPlugin
	{
		#region フィールド
		DataManager _dataManager;
		IPluto _iPluto;
		#endregion

		#region IPlugin メンバ
		string IPlugin.Author { get { return ( "PLUTO Development Team" ); } }

		string IPlugin.Text { get { return ( "Filter Plugin (#3)" ); } }

		string IPlugin.Comment { get { return ( "Filter Plugin (Tutorial 3)" ); } }

		bool IPlugin.Initialize( DataManager data, IPluto pluto )
		{
			// DataManager および IPluto を取得する．
			_dataManager = data;
			_iPluto = pluto;

			return ( true );
		}
		#endregion

		#region ICommonPlugin メンバ
		object ICommonPlugin.Run( params object[] args )
		{
			// DataManager および IPluto の取得に失敗している．または，画像が選択されていない．
			if( _dataManager == null || _iPluto == null || _dataManager.Active == null )
			{
				return ( null );
			}

			// 選択された画像を入力画像とする．
			var ct = _dataManager.Active;
			// 出力画像を入力画像と同じサイズで作成する．
			var mk = new Mist.MistArray( ct.Size1, ct.Size2, ct.Size3, ct.Reso1, ct.Reso2, ct.Reso3 );

			// 処理を行う．
			if (extract_alveolus(ct.ImagePointer, mk.ImagePointer, ct.Size1, ct.Size2, ct.Size3, 
                ct.Reso1, ct.Reso2, ct.Reso3) == false)
			{
				return ( null );
			}

			// 出力画像を DataManager へ追加する．
			if( _dataManager.Add( mk, false, true ) < 0 )
			{
				// 追加に失敗したら，出力画像のリソースを開放する．
				mk.Dispose( );
				return ( null );
			}
			else
			{
				return ( mk );
			}
		}  
		#endregion

		[DllImport("CppDLL_3.dll", EntryPoint = "extract_alveolus")]
		internal static extern bool extract_alveolus(IntPtr ctIntPtr, IntPtr markIntPtr,
            int size1, int size2, int size3, double reso1, double reso2, double rezo3);
	}
}
