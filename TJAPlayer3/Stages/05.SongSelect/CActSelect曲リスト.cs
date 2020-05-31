using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Drawing.Text;
using CSharpTest.Net.Collections;
using SlimDX;
using FDK;

namespace TJAPlayer3
{
	internal class CActSelect曲リスト : CActivity
	{
		// プロパティ

		public bool bIsEnumeratingSongs
		{
			get;
			set;
		}
		public bool bスクロール中
		{
			get
			{
				if( this.n目標のスクロールカウンタ == 0 )
				{
					return ( this.n現在のスクロールカウンタ != 0 );
				}
				return true;
			}
		}
		public int n現在のアンカ難易度レベル 
		{
			get;
			private set;
		}
		public int n現在選択中の曲の現在の難易度レベル
		{
			get
			{
				return this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r現在選択中の曲 );
			}
		}
		public Cスコア r現在選択中のスコア
		{
			get
			{
				if( this.r現在選択中の曲 != null )
				{
					return this.r現在選択中の曲.arスコア[ this.n現在選択中の曲の現在の難易度レベル ];
				}
				return null;
			}
		}
		public C曲リストノード r現在選択中の曲 
		{
			get;
			private set;
		}

		public int nスクロールバー相対y座標
		{
			get;
			private set;
		}
        public int xDansAnime { get; private set; }

        // t選択曲が変更された()内で使う、直前の選曲の保持
        // (前と同じ曲なら選択曲変更に掛かる再計算を省略して高速化するため)
        private C曲リストノード song_last = null;

		
		// コンストラクタ

		public CActSelect曲リスト()
        {
            #region[ レベル数字 ]
            STレベル数字[] stレベル数字Ar = new STレベル数字[ 10 ];
            STレベル数字 st数字0 = new STレベル数字();
            STレベル数字 st数字1 = new STレベル数字();
            STレベル数字 st数字2 = new STレベル数字();
            STレベル数字 st数字3 = new STレベル数字();
            STレベル数字 st数字4 = new STレベル数字();
            STレベル数字 st数字5 = new STレベル数字();
            STレベル数字 st数字6 = new STレベル数字();
            STレベル数字 st数字7 = new STレベル数字();
            STレベル数字 st数字8 = new STレベル数字();
            STレベル数字 st数字9 = new STレベル数字();

            st数字0.ch = '0';
            st数字1.ch = '1';
            st数字2.ch = '2';
            st数字3.ch = '3';
            st数字4.ch = '4';
            st数字5.ch = '5';
            st数字6.ch = '6';
            st数字7.ch = '7';
            st数字8.ch = '8';
            st数字9.ch = '9';
            st数字0.ptX = 0;
            st数字1.ptX = 22;
            st数字2.ptX = 44;
            st数字3.ptX = 66;
            st数字4.ptX = 88;
            st数字5.ptX = 110;
            st数字6.ptX = 132;
            st数字7.ptX = 154;
            st数字8.ptX = 176;
            st数字9.ptX = 198;

            stレベル数字Ar[0] = st数字0;
            stレベル数字Ar[1] = st数字1;
            stレベル数字Ar[2] = st数字2;
            stレベル数字Ar[3] = st数字3;
            stレベル数字Ar[4] = st数字4;
            stレベル数字Ar[5] = st数字5;
            stレベル数字Ar[6] = st数字6;
            stレベル数字Ar[7] = st数字7;
            stレベル数字Ar[8] = st数字8;
            stレベル数字Ar[9] = st数字9;
            this.st小文字位置 = stレベル数字Ar;
            #endregion


            this.r現在選択中の曲 = null;
            this.n現在のアンカ難易度レベル = TJAPlayer3.ConfigIni.nDefaultCourse;
			base.b活性化してない = true;
			this.bIsEnumeratingSongs = false;
		}


		// メソッド

		public int n現在のアンカ難易度レベルに最も近い難易度レベルを返す( C曲リストノード song )
		{
			// 事前チェック。

			if( song == null )
				return this.n現在のアンカ難易度レベル;	// 曲がまったくないよ

			if( song.arスコア[ this.n現在のアンカ難易度レベル ] != null )
				return this.n現在のアンカ難易度レベル;	// 難易度ぴったりの曲があったよ

			if( ( song.eノード種別 == C曲リストノード.Eノード種別.BOX ) || ( song.eノード種別 == C曲リストノード.Eノード種別.BACKBOX ) )
				return 0;								// BOX と BACKBOX は関係無いよ


			// 現在のアンカレベルから、難易度上向きに検索開始。

			int n最も近いレベル = this.n現在のアンカ難易度レベル;

			for( int i = 0; i < (int)Difficulty.Total; i++ )
			{
				if( song.arスコア[ n最も近いレベル ] != null )
					break;	// 曲があった。

				n最も近いレベル = ( n最も近いレベル + 1 ) % (int)Difficulty.Total;	// 曲がなかったので次の難易度レベルへGo。（5以上になったら0に戻る。）
			}


			// 見つかった曲がアンカより下のレベルだった場合……
			// アンカから下向きに検索すれば、もっとアンカに近い曲があるんじゃね？

			if( n最も近いレベル < this.n現在のアンカ難易度レベル )
			{
				// 現在のアンカレベルから、難易度下向きに検索開始。

				n最も近いレベル = this.n現在のアンカ難易度レベル;

				for( int i = 0; i < (int)Difficulty.Total; i++ )
				{
					if( song.arスコア[ n最も近いレベル ] != null )
						break;	// 曲があった。

					n最も近いレベル = ( ( n最も近いレベル - 1 ) + (int)Difficulty.Total) % (int)Difficulty.Total;	// 曲がなかったので次の難易度レベルへGo。（0未満になったら4に戻る。）
				}
			}

			return n最も近いレベル;
		}
		public C曲リストノード r指定された曲が存在するリストの先頭の曲( C曲リストノード song )
		{
			List<C曲リストノード> songList = GetSongListWithinMe( song );
			return ( songList == null ) ? null : songList[ 0 ];
		}
		public C曲リストノード r指定された曲が存在するリストの末尾の曲( C曲リストノード song )
		{
			List<C曲リストノード> songList = GetSongListWithinMe( song );
			return ( songList == null ) ? null : songList[ songList.Count - 1 ];
		}

		private List<C曲リストノード> GetSongListWithinMe( C曲リストノード song )
		{
			if ( song.r親ノード == null )					// root階層のノートだったら
			{
				return TJAPlayer3.Songs管理.list曲ルート;	// rootのリストを返す
			}
			else
			{
				if ( ( song.r親ノード.list子リスト != null ) && ( song.r親ノード.list子リスト.Count > 0 ) )
				{
					return song.r親ノード.list子リスト;
				}
				else
				{
					return null;
				}
			}
		}


		public delegate void DGSortFunc( List<C曲リストノード> songList, E楽器パート eInst, int order, params object[] p);
		/// <summary>
		/// 主にCSong管理.cs内にあるソート機能を、delegateで呼び出す。
		/// </summary>
		/// <param name="sf">ソート用に呼び出すメソッド</param>
		/// <param name="eInst">ソート基準とする楽器</param>
		/// <param name="order">-1=降順, 1=昇順</param>
		public void t曲リストのソート( DGSortFunc sf, E楽器パート eInst, int order, params object[] p )
		{
			List<C曲リストノード> songList = GetSongListWithinMe( this.r現在選択中の曲 );
			if ( songList == null )
			{
				// 何もしない;
			}
			else
			{
//				CDTXMania.Songs管理.t曲リストのソート3_演奏回数の多い順( songList, eInst, order );
				sf( songList, eInst, order, p );
//				this.r現在選択中の曲 = CDTXMania
				this.t現在選択中の曲を元に曲バーを再構成する();
			}
		}

		public bool tBOXに入る()
		{
            Bar_Center_Animation.n現在の値 = 180;
            this.ctバーフェード用.t開始(0, 400, 1, TJAPlayer3.Timer);
            bool ret = false;
			if ( CSkin.GetSkinName( TJAPlayer3.Skin.GetCurrentSkinSubfolderFullName( false ) ) != CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath )
				&& CSkin.bUseBoxDefSkin )
			{
				ret = true;
				// BOXに入るときは、スキン変更発生時のみboxdefスキン設定の更新を行う
				TJAPlayer3.Skin.SetCurrentSkinSubfolderFullName(
					TJAPlayer3.Skin.GetSkinSubfolderFullNameFromSkinName( CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) ), false );
			}

			if( ( this.r現在選択中の曲.list子リスト != null ) && ( this.r現在選択中の曲.list子リスト.Count > 0 ) )
			{
				this.r現在選択中の曲 = this.r現在選択中の曲.list子リスト[ 0 ];
				this.t現在選択中の曲を元に曲バーを再構成する();
				this.t選択曲が変更された(false);									// #27648 項目数変更を反映させる
				this.b選択曲が変更された = true;
			}
			return ret;
		}
		public bool tBOXを出る()
		{
			bool ret = false;
			if ( CSkin.GetSkinName( TJAPlayer3.Skin.GetCurrentSkinSubfolderFullName( false ) ) != CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath )
				&& CSkin.bUseBoxDefSkin )
			{
				ret = true;
			}
			// スキン変更が発生しなくても、boxdef圏外に出る場合は、boxdefスキン設定の更新が必要
			// (ユーザーがboxdefスキンをConfig指定している場合への対応のために必要)
			// tBoxに入る()とは処理が微妙に異なるので注意
			TJAPlayer3.Skin.SetCurrentSkinSubfolderFullName(
				( this.r現在選択中の曲.strSkinPath == "" ) ? "" : TJAPlayer3.Skin.GetSkinSubfolderFullNameFromSkinName( CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) ), false );
			if ( this.r現在選択中の曲.r親ノード != null )
			{
				this.r現在選択中の曲 = this.r現在選択中の曲.r親ノード;
				this.t現在選択中の曲を元に曲バーを再構成する();
				this.t選択曲が変更された(false);									// #27648 項目数変更を反映させる
				this.b選択曲が変更された = true;
			}
			return ret;
		}
		public void t現在選択中の曲を元に曲バーを再構成する()
		{
			this.tバーの初期化();
		}
		public void t次に移動()
		{
			if( this.r現在選択中の曲 != null )
			{
				this.n目標のスクロールカウンタ += 100;              
            }
            ジャンル音声のリセット();
            Bar_Center_Animation.t開始(0, 180, 1, TJAPlayer3.Timer);
            this.b選択曲が変更された = true;
            
		}
		public void t前に移動()
		{
			if( this.r現在選択中の曲 != null )
			{              
                this.n目標のスクロールカウンタ -= 100;
            }
            ジャンル音声のリセット();
            Bar_Center_Animation.t開始(0, 180, 1, TJAPlayer3.Timer);
            this.b選択曲が変更された = true;
            
        }
		public void t難易度レベルをひとつ進める()
		{
			if( ( this.r現在選択中の曲 == null ) || ( this.r現在選択中の曲.nスコア数 <= 1 ) )
				return;		// 曲にスコアが０～１個しかないなら進める意味なし。
			

			// 難易度レベルを＋１し、現在選曲中のスコアを変更する。

			this.n現在のアンカ難易度レベル = this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r現在選択中の曲 );

			for( int i = 0; i < (int)Difficulty.Total; i++ )
			{
				this.n現在のアンカ難易度レベル = ( this.n現在のアンカ難易度レベル + 1 ) % (int)Difficulty.Total;	// ５以上になったら０に戻る。
				if( this.r現在選択中の曲.arスコア[ this.n現在のアンカ難易度レベル ] != null )	// 曲が存在してるならここで終了。存在してないなら次のレベルへGo。
					break;
			}


			// 曲毎に表示しているスキル値を、新しい難易度レベルに合わせて取得し直す。（表示されている13曲全部。）

			C曲リストノード song = this.r現在選択中の曲;
			for( int i = 0; i < 5; i++ )
				song = this.r前の曲( song );

			for( int i = this.n現在の選択行 - 5; i < ( ( this.n現在の選択行 - 5 ) + 13 ); i++ )
			{
				int index = ( i + 13 ) % 13;
				for( int m = 0; m < 3; m++ )
				{
					this.stバー情報[ index ].nスキル値[ m ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ m ];
				}
				song = this.r次の曲( song );
			}


			// 選曲ステージに変更通知を発出し、関係Activityの対応を行ってもらう。

			TJAPlayer3.stage選曲.t選択曲変更通知();
		}
        /// <summary>
        /// 不便だったから作った。
        /// </summary>
		public void t難易度レベルをひとつ戻す()
		{
			if( ( this.r現在選択中の曲 == null ) || ( this.r現在選択中の曲.nスコア数 <= 1 ) )
				return;		// 曲にスコアが０～１個しかないなら進める意味なし。
			

			// 難易度レベルを＋１し、現在選曲中のスコアを変更する。

			this.n現在のアンカ難易度レベル = this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r現在選択中の曲 );

            this.n現在のアンカ難易度レベル--;
            if( this.n現在のアンカ難易度レベル < 0 ) // 0より下になったら4に戻す。
            {
                this.n現在のアンカ難易度レベル = 4;
            }

            //2016.08.13 kairera0467 かんたん譜面が無い譜面(ふつう、むずかしいのみ)で、難易度を最上位に戻せない不具合の修正。
            bool bLabel0NotFound = true;
            for( int i = this.n現在のアンカ難易度レベル; i >= 0; i-- )
            {
                if( this.r現在選択中の曲.arスコア[ i ] != null )
                {
                    this.n現在のアンカ難易度レベル = i;
                    bLabel0NotFound = false;
                    break;
                }
            }
            if( bLabel0NotFound )
            {
                for( int i = 4; i >= 0; i-- )
                {
                    if( this.r現在選択中の曲.arスコア[ i ] != null )
                    {
                        this.n現在のアンカ難易度レベル = i;
                        break;
                    }
                }
            }

			// 曲毎に表示しているスキル値を、新しい難易度レベルに合わせて取得し直す。（表示されている13曲全部。）

			C曲リストノード song = this.r現在選択中の曲;
			for( int i = 0; i < 5; i++ )
				song = this.r前の曲( song );

			for( int i = this.n現在の選択行 - 5; i < ( ( this.n現在の選択行 - 5 ) + 13 ); i++ )
			{
				int index = ( i + 13 ) % 13;
				for( int m = 0; m < 3; m++ )
				{
					this.stバー情報[ index ].nスキル値[ m ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ m ];
				}
				song = this.r次の曲( song );
			}


			// 選曲ステージに変更通知を発出し、関係Activityの対応を行ってもらう。

			TJAPlayer3.stage選曲.t選択曲変更通知();
		}


		/// <summary>
		/// 曲リストをリセットする
		/// </summary>
		/// <param name="cs"></param>
		public void Refresh(CSongs管理 cs, bool bRemakeSongTitleBar )		// #26070 2012.2.28 yyagi
		{
//			this.On非活性化();

			if ( cs != null && cs.list曲ルート.Count > 0 )	// 新しい曲リストを検索して、1曲以上あった
			{
				TJAPlayer3.Songs管理 = cs;

				if ( this.r現在選択中の曲 != null )			// r現在選択中の曲==null とは、「最初songlist.dbが無かった or 検索したが1曲もない」
				{
					this.r現在選択中の曲 = searchCurrentBreadcrumbsPosition( TJAPlayer3.Songs管理.list曲ルート, this.r現在選択中の曲.strBreadcrumbs );
					if ( bRemakeSongTitleBar )					// 選曲画面以外に居るときには再構成しない (非活性化しているときに実行すると例外となる)
					{
						this.t現在選択中の曲を元に曲バーを再構成する();
					}
#if false			// list子リストの中まではmatchしてくれないので、検索ロジックは手書きで実装 (searchCurrentBreadcrumbs())
					string bc = this.r現在選択中の曲.strBreadcrumbs;
					Predicate<C曲リストノード> match = delegate( C曲リストノード c )
					{
						return ( c.strBreadcrumbs.Equals( bc ) );
					};
					int nMatched = CDTXMania.Songs管理.list曲ルート.FindIndex( match );

					this.r現在選択中の曲 = ( nMatched == -1 ) ? null : CDTXMania.Songs管理.list曲ルート[ nMatched ];
					this.t現在選択中の曲を元に曲バーを再構成する();
#endif
					return;
				}
			}
			this.On非活性化();
			this.r現在選択中の曲 = null;
			this.On活性化();
		}


		/// <summary>
		/// 現在選曲している位置を検索する
		/// (曲一覧クラスを新しいものに入れ替える際に用いる)
		/// </summary>
		/// <param name="ln">検索対象のList</param>
		/// <param name="bc">検索するパンくずリスト(文字列)</param>
		/// <returns></returns>
		private C曲リストノード searchCurrentBreadcrumbsPosition( List<C曲リストノード> ln, string bc )
		{
			foreach (C曲リストノード n in ln)
			{
				if ( n.strBreadcrumbs == bc )
				{
					return n;
				}
				else if ( n.list子リスト != null && n.list子リスト.Count > 0 )	// 子リストが存在するなら、再帰で探す
				{
					C曲リストノード r = searchCurrentBreadcrumbsPosition( n.list子リスト, bc );
					if ( r != null ) return r;
				}
			}
			return null;
		}

		/// <summary>
		/// BOXのアイテム数と、今何番目を選択しているかをセットする
		/// </summary>
		public void t選択曲が変更された( bool bForce )	// #27648
		{
			C曲リストノード song = TJAPlayer3.stage選曲.r現在選択中の曲;
			if ( song == null )
				return;
			if ( song == song_last && bForce == false )
				return;
				
			song_last = song;
			List<C曲リストノード> list = ( song.r親ノード != null ) ? song.r親ノード.list子リスト : TJAPlayer3.Songs管理.list曲ルート;
			int index = list.IndexOf( song ) + 1;
			if ( index <= 0 )
			{
				nCurrentPosition = nNumOfItems = 0;
			}
			else
			{
				nCurrentPosition = index;
				nNumOfItems = list.Count;
			}
            TJAPlayer3.stage選曲.act演奏履歴パネル.tSongChange();
		}

		// CActivity 実装

		public override void On活性化()
		{
			if( this.b活性化してる )
				return;

            // Reset to not performing calibration each time we
            // enter or return to the song select screen.
            TJAPlayer3.IsPerformingCalibration = false;

            new CPrivateFastFont.Replace("ー", "庭");
            if (!string.IsNullOrEmpty(TJAPlayer3.ConfigIni.FontName))
            {
                this.pfMusicName = new CPrivateFastFont(new FontFamily(TJAPlayer3.ConfigIni.FontName), 30);
                this.pfSubtitle = new CPrivateFastFont(new FontFamily(TJAPlayer3.ConfigIni.FontName), 23);
            }
            else
            {
                this.pfMusicName = new CPrivateFastFont(new FontFamily("MS UI Gothic"), 30);
                this.pfSubtitle = new CPrivateFastFont(new FontFamily("MS UI Gothic"), 23);
            }

		    _titleTextures.ItemRemoved += OnTitleTexturesOnItemRemoved;
		    _titleTextures.ItemUpdated += OnTitleTexturesOnItemUpdated;

            this.e楽器パート = E楽器パート.DRUMS;
			this.n目標のスクロールカウンタ = 0;
			this.n現在のスクロールカウンタ = 0;
			this.nスクロールタイマ = -1;

			// フォント作成。
			// 曲リスト文字は２倍（面積４倍）でテクスチャに描画してから縮小表示するので、フォントサイズは２倍とする。

			FontStyle regular = FontStyle.Regular;
			this.ft曲リスト用フォント = new Font( TJAPlayer3.ConfigIni.FontName, 40f, regular, GraphicsUnit.Pixel );
			

			// 現在選択中の曲がない（＝はじめての活性化）なら、現在選択中の曲をルートの先頭ノードに設定する。

			if( ( this.r現在選択中の曲 == null ) && ( TJAPlayer3.Songs管理.list曲ルート.Count > 0 ) )
				this.r現在選択中の曲 = TJAPlayer3.Songs管理.list曲ルート[ 0 ];

			// バー情報を初期化する。

			this.tバーの初期化();

            this.ct三角矢印アニメ = new CCounter();

			base.On活性化();

			this.t選択曲が変更された(true);		// #27648 2012.3.31 yyagi 選曲画面に入った直後の 現在位置/全アイテム数 の表示を正しく行うため
		}
		public override void On非活性化()
		{
			if( this.b活性化してない )
				return;

		    _titleTextures.ItemRemoved -= OnTitleTexturesOnItemRemoved;
		    _titleTextures.ItemUpdated -= OnTitleTexturesOnItemUpdated;

		    TJAPlayer3.t安全にDisposeする(ref pfMusicName);
		    TJAPlayer3.t安全にDisposeする(ref pfSubtitle);

			TJAPlayer3.t安全にDisposeする( ref this.ft曲リスト用フォント );

            this.ct三角矢印アニメ = null;
            this.ctバーフェード用 = null;

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( this.b活性化してない )
				return;

            this.Bar_Center_Animation = new CCounter();

            this.ctバーフェード用 = new CCounter();

            for ( int i = 0; i < 13; i++ )
            {
                this.stバー情報[ i ].ttkタイトル = this.ttk曲名テクスチャを生成する( this.stバー情報[ i ].strタイトル文字列, this.stバー情報[i].ForeColor, this.stバー情報[i].BackColor);
            }

            int c = ( CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja" ) ? 0 : 1;
            #region[ジャンル音声]
            this.soundJPOP = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\J-POP.ogg"), ESoundGroup.Voice);
            this.soundアニメ = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Anime.ogg"), ESoundGroup.Voice);
            this.soundゲームミュージック = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\GameMusic.ogg"), ESoundGroup.Voice);
            this.soundナムコオリジナル = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\NamcoOriginal.ogg"), ESoundGroup.Voice);
            this.soundクラシック = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Classic.ogg"), ESoundGroup.Voice);
            this.soundバラエティ = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Variety.ogg"), ESoundGroup.Voice);
            this.soundどうよう = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Child.ogg"), ESoundGroup.Voice);
            this.soundボーカロイド = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Vocaloid.ogg"), ESoundGroup.Voice);
            this.sound段位道場 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Dan_Select.ogg"), ESoundGroup.Voice);
            #endregion
            #region [ Songs not found画像 ]
            try
            {
				using( Bitmap image = new Bitmap( 640, 128 ) )
				using( Graphics graphics = Graphics.FromImage( image ) )
				{
					string[] s1 = { "曲データが見つかりません。", "Songs not found." };
					string[] s2 = { "曲データをDTXManiaGR.exe以下の", "You need to install songs." };
					string[] s3 = { "フォルダにインストールして下さい。", "" };
					graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 2f );
					graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 0f );
					graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 44f );
					graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 42f );
					graphics.DrawString( s3[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 86f );
					graphics.DrawString( s3[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 84f );

					this.txSongNotFound = new CTexture( TJAPlayer3.app.Device, image, TJAPlayer3.TextureFormat );

					this.txSongNotFound.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );	// 半分のサイズで表示する。
				}
			}
			catch( CTextureCreateFailedException e )
			{
				Trace.TraceError( e.ToString() );
				Trace.TraceError( "SoungNotFoundテクスチャの作成に失敗しました。" );
				this.txSongNotFound = null;
			}
			#endregion
			#region [ "曲データを検索しています"画像 ]
			try
			{
				using ( Bitmap image = new Bitmap( 640, 96 ) )
				using ( Graphics graphics = Graphics.FromImage( image ) )
				{
					string[] s1 = { "曲データを検索しています。", "Now enumerating songs." };
					string[] s2 = { "そのまましばらくお待ち下さい。", "Please wait..." };
					graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 2f );
					graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 0f );
					graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 44f );
					graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 42f );

					this.txEnumeratingSongs = new CTexture( TJAPlayer3.app.Device, image, TJAPlayer3.TextureFormat );

					this.txEnumeratingSongs.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );	// 半分のサイズで表示する。
				}
			}
			catch ( CTextureCreateFailedException e )
			{
				Trace.TraceError( e.ToString() );
				Trace.TraceError( "txEnumeratingSongsテクスチャの作成に失敗しました。" );
				this.txEnumeratingSongs = null;
			}
            #endregion
            base.OnManagedリソースの作成();
		}
		public override void OnManagedリソースの解放()
		{
			if( this.b活性化してない )
				return;

            for ( int i = 0; i < 13; i++ )
            {
                TJAPlayer3.tテクスチャの解放( ref this.stバー情報[ i ].txタイトル名 );
                this.stバー情報[ i ].ttkタイトル = null;
            }

		    ClearTitleTextureCache();

            TJAPlayer3.tテクスチャの解放( ref this.txEnumeratingSongs );
            TJAPlayer3.tテクスチャの解放( ref this.txSongNotFound );
            TJAPlayer3.tテクスチャの解放(ref Dan_Plate);
            base.OnManagedリソースの解放();
		}
		public override int On進行描画()
		{
			if( this.b活性化してない )
				return 0;

			#region [ 初めての進行描画 ]
			//-----------------
			if( this.b初めての進行描画 )
			{
                Bar_Center_Animation.n現在の値 = 180;

				this.nスクロールタイマ = CSound管理.rc演奏用タイマ.n現在時刻;
				TJAPlayer3.stage選曲.t選択曲変更通知();

                this.n矢印スクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
				this.ct三角矢印アニメ.t開始( 0, 1000, 1, TJAPlayer3.Timer );
                    base.b初めての進行描画 = false;
			}
            //-----------------
            #endregion

            Bar_Center_Animation.t進行();

            this.ctバーフェード用.t進行();

			// まだ選択中の曲が決まってなければ、曲ツリールートの最初の曲にセットする。

			if( ( this.r現在選択中の曲 == null ) && ( TJAPlayer3.Songs管理.list曲ルート.Count > 0 ) )
				this.r現在選択中の曲 = TJAPlayer3.Songs管理.list曲ルート[ 0 ];


            // 本ステージは、(1)登場アニメフェーズ → (2)通常フェーズ　と二段階にわけて進む。

            // 進行。
            if (n現在のスクロールカウンタ == 0) ct三角矢印アニメ.t進行Loop();
            else ct三角矢印アニメ.n現在の値 = 0;

            #region [ (2) 通常フェーズの進行。]
            //-----------------
            long n現在時刻 = CSound管理.rc演奏用タイマ.n現在時刻;

            if (n現在時刻 < this.nスクロールタイマ) // 念のため
                this.nスクロールタイマ = n現在時刻;

            const int nアニメ間隔 = 2;
            while ((n現在時刻 - this.nスクロールタイマ) >= nアニメ間隔)
            {
                int n加速度 = 1;
                int n残距離 = Math.Abs((int)(this.n目標のスクロールカウンタ - this.n現在のスクロールカウンタ));

                #region [ 残距離が遠いほどスクロールを速くする（＝n加速度を多くする）。]
                //-----------------
                if (n残距離 <= 40)
                {
                    n加速度 = 1;
                }
                else if (n残距離 <= 100)
                {
                    n加速度 = 1;
                }

                else if (n残距離 <= 170)
                {
                    n加速度 = 2;
                }

                else if (n残距離 <= 240)
                {
                    n加速度 = 3;
                }
                else if (n残距離 <= 300)
                {
                    n加速度 = 3;
                }
                else if (n残距離 <= 360)
                {
                    n加速度 = 4;
                }
                else if (n残距離 <= 390)
                {
                    n加速度 = 4;
                }
                else if (n残距離 <= 420)
                {
                    n加速度 = 5;
                }
                else if (n残距離 <= 430)
                {
                    n加速度 = 6;
                }

                else if (n残距離 <= 440)
                {
                    n加速度 = 7;
                }
                else
                {
                    n加速度 = 8;
                }
                //-----------------
                #endregion

                #region [ 加速度を加算し、現在のスクロールカウンタを目標のスクロールカウンタまで近づける。 ]
                //-----------------
                if (this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ)        // (A) 正の方向に未達の場合：
                {
                    this.n現在のスクロールカウンタ += n加速度;                             // カウンタを正方向に移動する。

                    if (this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ)
                        this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;    // 到着！スクロール停止！
                }

                else if (this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ)   // (B) 負の方向に未達の場合：
                {
                    this.n現在のスクロールカウンタ -= n加速度;                             // カウンタを負方向に移動する。

                    if (this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ)    // 到着！スクロール停止！
                        this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
                }
                //-----------------
                #endregion

                if (this.n現在のスクロールカウンタ >= 100)      // １行＝100カウント。
                {
                    #region [ パネルを１行上にシフトする。]
                    //-----------------

                    // 選択曲と選択行を１つ下の行に移動。

                    this.r現在選択中の曲 = this.r次の曲(this.r現在選択中の曲);
                    this.n現在の選択行 = (this.n現在の選択行 + 1) % 13;


                    // 選択曲から７つ下のパネル（＝新しく最下部に表示されるパネル。消えてしまう一番上のパネルを再利用する）に、新しい曲の情報を記載する。

                    C曲リストノード song = this.r現在選択中の曲;
                    for (int i = 0; i < 7; i++)
                        song = this.r次の曲(song);

                    int index = (this.n現在の選択行 + 7) % 13;    // 新しく最下部に表示されるパネルのインデックス（0～12）。
                    this.stバー情報[index].strタイトル文字列 = song.strタイトル;
                    this.stバー情報[index].ForeColor = song.ForeColor;
                    this.stバー情報[index].BackColor = song.BackColor;
                    this.stバー情報[index].strジャンル = song.strジャンル;
                    this.stバー情報[index].strサブタイトル = song.strサブタイトル;
                    this.stバー情報[index].ar難易度 = song.nLevel;
                    for (int f = 0; f < (int)Difficulty.Total; f++)
                    {
                        if (song.arスコア[f] != null)
                            this.stバー情報[index].b分岐 = song.arスコア[f].譜面情報.b譜面分岐;
                    }


                    // stバー情報[] の内容を1行ずつずらす。

                    C曲リストノード song2 = this.r現在選択中の曲;
                    for (int i = 0; i < 5; i++)
                        song2 = this.r前の曲(song2);

                    for (int i = 0; i < 13; i++)
                    {
                        int n = (((this.n現在の選択行 - 5) + i) + 13) % 13;
                        this.stバー情報[n].eバー種別 = this.e曲のバー種別を返す(song2);
                        song2 = this.r次の曲(song2);
                        this.stバー情報[i].ttkタイトル = this.ttk曲名テクスチャを生成する(this.stバー情報[i].strタイトル文字列, this.stバー情報[i].ForeColor, this.stバー情報[i].BackColor);

                    }


                    // 新しく最下部に表示されるパネル用のスキル値を取得。

                    for (int i = 0; i < 3; i++)
                        this.stバー情報[index].nスキル値[i] = (int)song.arスコア[this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(song)].譜面情報.最大スキル[i];


                    // 1行(100カウント)移動完了。

                    this.n現在のスクロールカウンタ -= 100;
                    this.n目標のスクロールカウンタ -= 100;

                    this.t選択曲が変更された(false);             // スクロールバー用に今何番目を選択しているかを更新



                    if (this.n目標のスクロールカウンタ == 0)
                        Bar_Center_Animation.t開始(0, 180, 1, TJAPlayer3.Timer);
                    TJAPlayer3.stage選曲.t選択曲変更通知();      // スクロール完了＝選択曲変更！

                    //-----------------
                    #endregion
                }
                else if (this.n現在のスクロールカウンタ <= -100)
                {
                    #region [ パネルを１行下にシフトする。]
                    //-----------------

                    // 選択曲と選択行を１つ上の行に移動。

                    this.r現在選択中の曲 = this.r前の曲(this.r現在選択中の曲);
                    this.n現在の選択行 = ((this.n現在の選択行 - 1) + 13) % 13;


                    // 選択曲から５つ上のパネル（＝新しく最上部に表示されるパネル。消えてしまう一番下のパネルを再利用する）に、新しい曲の情報を記載する。

                    C曲リストノード song = this.r現在選択中の曲;
                    for (int i = 0; i < 5; i++)
                        song = this.r前の曲(song);

                    int index = ((this.n現在の選択行 - 5) + 13) % 13; // 新しく最上部に表示されるパネルのインデックス（0～12）。
                    this.stバー情報[index].strタイトル文字列 = song.strタイトル;
                    this.stバー情報[index].ForeColor = song.ForeColor;
                    this.stバー情報[index].BackColor = song.BackColor;
                    this.stバー情報[index].strサブタイトル = song.strサブタイトル;
                    this.stバー情報[index].strジャンル = song.strジャンル;
                    this.stバー情報[index].ar難易度 = song.nLevel;
                    for (int f = 0; f < (int)Difficulty.Total; f++)
                    {
                        if (song.arスコア[f] != null)
                            this.stバー情報[index].b分岐 = song.arスコア[f].譜面情報.b譜面分岐;
                    }

                    // stバー情報[] の内容を1行ずつずらす。

                    C曲リストノード song2 = this.r現在選択中の曲;
                    for (int i = 0; i < 5; i++)
                        song2 = this.r前の曲(song2);

                    for (int i = 0; i < 13; i++)
                    {
                        int n = (((this.n現在の選択行 - 5) + i) + 13) % 13;
                        this.stバー情報[n].eバー種別 = this.e曲のバー種別を返す(song2);
                        song2 = this.r次の曲(song2);
                        this.stバー情報[i].ttkタイトル = this.ttk曲名テクスチャを生成する(this.stバー情報[i].strタイトル文字列, this.stバー情報[i].ForeColor, this.stバー情報[i].BackColor);
                    }


                    // 新しく最上部に表示されるパネル用のスキル値を取得。

                    for (int i = 0; i < 3; i++)
                        this.stバー情報[index].nスキル値[i] = (int)song.arスコア[this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(song)].譜面情報.最大スキル[i];


                    // 1行(100カウント)移動完了。

                    this.n現在のスクロールカウンタ += 100;
                    this.n目標のスクロールカウンタ += 100;

                    this.t選択曲が変更された(false);             // スクロールバー用に今何番目を選択しているかを更新

                    this.ttk選択している曲の曲名 = null;
                    this.ttk選択している曲のサブタイトル = null;

                    if (this.n目標のスクロールカウンタ == 0)
                        TJAPlayer3.stage選曲.t選択曲変更通知();      // スクロール完了＝選択曲変更！
                                                            //-----------------
                    #endregion
                }

                if (this.b選択曲が変更された && n現在のスクロールカウンタ == 0)
                {
                    if (this.ttk選択している曲の曲名 != null)
                    {
                        this.ttk選択している曲の曲名 = null;
                        this.b選択曲が変更された = false;
                    }
                    if (this.ttk選択している曲のサブタイトル != null)
                    {
                        this.ttk選択している曲のサブタイトル = null;
                        this.b選択曲が変更された = false;
                    }
                }
                this.nスクロールタイマ += nアニメ間隔;
            }
            //-----------------
            #endregion


            // 描画。

            if ( this.r現在選択中の曲 == null )
			{
				#region [ 曲が１つもないなら「Songs not found.」を表示してここで帰れ。]
				//-----------------
				if ( bIsEnumeratingSongs )
				{
                    this.txEnumeratingSongs?.t2D描画(TJAPlayer3.app.Device, 320, 160);
                }
				else
				{
                    this.txSongNotFound?.t2D描画(TJAPlayer3.app.Device, 320, 160);
                }
				//-----------------
				#endregion

				return 0;
			}

            int i選曲バーX座標 = 673; //選曲バーの座標用
            int i選択曲バーX座標 = 665; //選択曲バーの座標用

            #region [ (2) 通常フェーズの描画。]
            //-----------------
            for (int i = 0; i < 13; i++)    // パネルは全13枚。
            {
                if ((i == 0 && this.n現在のスクロールカウンタ > 0) ||       // 最上行は、上に移動中なら表示しない。
                    (i == 12 && this.n現在のスクロールカウンタ < 0))        // 最下行は、下に移動中なら表示しない。
                    continue;

                int nパネル番号 = (((this.n現在の選択行 - 5) + i) + 13) % 13;
                int n見た目の行番号 = i;
                int n次のパネル番号 = (this.n現在のスクロールカウンタ <= 0) ? ((i + 1) % 13) : (((i - 1) + 13) % 13);
                int x = i選曲バーX座標;
                int xAnime = this.ptバーの座標[n見た目の行番号].X + ((int)((this.ptバーの座標[n次のパネル番号].X - this.ptバーの座標[n見た目の行番号].X) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int y = this.ptバーの基本座標[n見た目の行番号].Y + ((int)((this.ptバーの基本座標[n次のパネル番号].Y - this.ptバーの基本座標[n見た目の行番号].Y) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));

                // 難易度がTower、Danではない
                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                {
                    if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち?.n現在の値 > 0 && !TJAPlayer3.stage選曲.ctDiffSelect移動待ち.b終了値に達した)
                    {
                        //難易度選択画面を開くアニメーション
                        if (i < 5)
                            xAnime -= TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 < 480 ? (int)(500 * (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 / 480.0f)) : 500;
                        else if (i > 5)
                            xAnime += TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 < 480 ? (int)(500 * (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 / 480.0f)) : 500;
                    }
                    else if (TJAPlayer3.stage選曲.act難易度選択画面.bIsDifficltSelect && TJAPlayer3.stage選曲.ctDiffSelect移動待ち.b終了値に達した)
                    {
                        xAnime = 500;
                    }
                    else if (TJAPlayer3.stage選曲.ctDiffSelect戻り待ち?.n現在の値 > 0 && !TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.b終了値に達した)
                    {
                        //難易度選択画面を閉じるアニメーション
                        if (i < 5)
                            xAnime -= TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.n現在の値 > 582 ? 500 - (int)(500 * ((TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.n現在の値 - 582) / 480.0f)) : 500;
                        else if (i > 5)
                            xAnime += TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.n現在の値 > 582 ? 500 - (int)(500 * ((TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.n現在の値 - 582) / 480.0f)) : 500;
                    }
                }

                {
                    // (B) スクロール中の選択曲バー、またはその他のバーの描画。

                    #region [ バーテクスチャを描画。]
                    //-----------------
                    if (n現在のスクロールカウンタ != 0)
                        this.tジャンル別選択されていない曲バーの描画(xAnime, TJAPlayer3.Skin.SongSelect_Overall_Y, this.stバー情報[nパネル番号].strジャンル, this.stバー情報[nパネル番号].eバー種別);
                    else if (n見た目の行番号 != 5)
                        this.tジャンル別選択されていない曲バーの描画(xAnime, TJAPlayer3.Skin.SongSelect_Overall_Y, this.stバー情報[nパネル番号].strジャンル, this.stバー情報[nパネル番号].eバー種別);
                    if (this.stバー情報[nパネル番号].b分岐[TJAPlayer3.stage選曲.n現在選択中の曲の難易度] == true && n見た目の行番号 != 5)
                        TJAPlayer3.Tx.SongSelect_Branch?.t2D描画(TJAPlayer3.app.Device, xAnime + 66, TJAPlayer3.Skin.SongSelect_Overall_Y - 5);
                    //-----------------
                    #endregion

                    #region [ タイトル名テクスチャを描画。]


                    if (n現在のスクロールカウンタ != 0)
                    {
                        ResolveTitleTexture(this.stバー情報[nパネル番号].ttkタイトル).n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                        ResolveTitleTexture(this.stバー情報[nパネル番号].ttkタイトル)?.t2D描画(TJAPlayer3.app.Device, xAnime + 28, TJAPlayer3.Skin.SongSelect_Overall_Y + 23);
                    }
                    else if (n見た目の行番号 != 5)
                    {
                        ResolveTitleTexture(this.stバー情報[nパネル番号].ttkタイトル).n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                        ResolveTitleTexture(this.stバー情報[nパネル番号].ttkタイトル)?.t2D描画(TJAPlayer3.app.Device, xAnime + 28, TJAPlayer3.Skin.SongSelect_Overall_Y + 23);
                    }
                    #endregion

                    if (this.stバー情報[nパネル番号].ar難易度 != null)
                    {
                        int nX補正 = 0;
                        if (this.stバー情報[nパネル番号].ar難易度[TJAPlayer3.stage選曲.n現在選択中の曲の難易度].ToString().Length == 2)
                            nX補正 = -6;
                        this.t小文字表示(xAnime + 65 + nX補正, 559, this.stバー情報[nパネル番号].ar難易度[TJAPlayer3.stage選曲.n現在選択中の曲の難易度].ToString());
                    }
                    //-----------------						
                }
                #endregion
            }
            if (this.n現在のスクロールカウンタ < 0 || this.n現在のスクロールカウンタ > 0)
            {
                this.Bar_Center_Animation.n現在の値 = 0;
            }
            if (this.n現在のスクロールカウンタ == 0)
            {
                // 難易度がTower、Danではない
                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                {
                    if (TJAPlayer3.stage選曲.act難易度選択画面.bIsDifficltSelect)
                    {
                        if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.b進行中)
                        {
                            if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち?.n現在の値 < 480)
                            {
                                if (TJAPlayer3.Tx.SongSelect_Bar_Center[0] != null)
                                    TJAPlayer3.Tx.SongSelect_Bar_Center[0].t2D描画(TJAPlayer3.app.Device, 448, TJAPlayer3.Skin.SongSelect_Overall_Y);
                            }
                            else
                            {
                                int count = TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値;
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;

                                if (count <= 780)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 435 - (int)(195.0f * ((count - 480.0f) / 300.0f)), 94, new Rectangle(2, 2, 30, 480));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + (390.0f * ((count - 480.0f) / 300.0f)); //349 -> 739 (390)
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 465 - (int)(195.0f * ((count - 480.0f) / 300.0f)), 94, new Rectangle(75, 2, 1, 480));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 814 + (int)(195.0f * ((count - 480.0f) / 300.0f)), 94, new Rectangle(38, 2, 30, 480));
                                }
                                else if (count <= 1030)
                                {
                                    //左上
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 103 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(2, 10, 30, 30));
                                    //右上
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 103 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(38, 10, 30, 30));

                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + 390.0f;
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 131, new Rectangle(75, 38, 1, 442)); //中央
                                                                                                                                                    //上縁
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 103 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(75, 10, 1, 30));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 60.0f * ((count - 780.0f) / 250.0f);
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 133 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(75, 26, 1, 1));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 133 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(2, 26, 30, 1));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 133 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(38, 26, 30, 1));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 131, new Rectangle(2, 38, 30, 442));
                                    //右
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 131, new Rectangle(38, 38, 30, 442));
                                }
                                else
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 131, new Rectangle(2, 38, 30, 442)); //左
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + 390.0f; // 349 -> 739 (390)
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 131, new Rectangle(75, 38, 1, 442)); // 中央
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 43, new Rectangle(75, 10, 1, 30));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 72.0f;
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 59, new Rectangle(75, 26, 1, 1));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f; //両端中
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 59, new Rectangle(2, 26, 30, 1));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 59, new Rectangle(38, 26, 30, 1));

                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;


                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 43, new Rectangle(2, 10, 30, 30));

                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 43, new Rectangle(38, 10, 30, 30));
                                    TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 131, new Rectangle(38, 38, 30, 442)); //右
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.b進行中 && TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.b終了値に達してない)
                        {
                            int count = TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.n現在の値;
                            //count = 260;
                            TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                            if (count < 250)
                            {
                                //左上
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 103 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(2, 10, 30, 30));
                                //右上
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 103 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(38, 10, 30, 30));

                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + 390.0f;
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 131, new Rectangle(75, 38, 1, 442)); //中央
                                                                                                                                                //上縁
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 103 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(75, 10, 1, 30));
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 60.0f - (60.0f * ((count - 282.0f) / 250.0f));
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 270, 133 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(75, 26, 1, 1));
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 133 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(2, 26, 30, 1));
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 133 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(38, 26, 30, 1));

                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;

                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240, 131, new Rectangle(2, 38, 30, 442));
                                //右
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009, 131, new Rectangle(38, 38, 30, 442));
                            }
                            else if (count >= 250 && count < 500)
                            {
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 240 + (int)(210.0f * ((count - 250.0f) / 250.0f)), 103, new Rectangle(2, 10, 30, 460)); //左
                                //左半分
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = (211.0f - (211.0f * ((count - 250.0f) / 250.0f)));
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 480 - (int)(211.0f - (211.0f * ((count - 250.0f) / 250.0f))), 103, new Rectangle(75, 10, 1, 460));

                                //右半分
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 798, 103, new Rectangle(75, 10, 1, 460));

                                //最低限用意する領域 318px
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 318.0f;
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 480, 103, new Rectangle(75, 10, 1, 460));

                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;

                                TJAPlayer3.Tx.SongSelect_Difficulty_BOX?.t2D描画(TJAPlayer3.app.Device, 1009 - (int)(210.0f * ((count - 250.0f) / 250.0f)), 103, new Rectangle(38, 10, 30, 442));
                            }
                            else
                            {
                                if (TJAPlayer3.Tx.SongSelect_Bar_Center[0] != null)
                                    TJAPlayer3.Tx.SongSelect_Bar_Center[0]?.t2D描画(TJAPlayer3.app.Device, 448, TJAPlayer3.Skin.SongSelect_Overall_Y);
                            }
                        }
                        else
                        {
                            switch (r現在選択中の曲.eノード種別)
                            {
                                case C曲リストノード.Eノード種別.SCORE:
                                case C曲リストノード.Eノード種別.BACKBOX:
                                    {
                                        if (Bar_Center_Animation.n現在の値 >= 30)
                                        {
                                            TJAPlayer3.Tx.SongSelect_Bar_Center[0].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                            TJAPlayer3.Tx.SongSelect_Bar_Center[0]?.t2D描画(TJAPlayer3.app.Device, 628 - Bar_Center_Animation.n現在の値, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(0, 0, Bar_Center_Animation.n現在の値 + 16, 472));
                                            TJAPlayer3.Tx.SongSelect_Bar_Center[0]?.t2D描画(TJAPlayer3.app.Device, 644, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(374 - Bar_Center_Animation.n現在の値 + 2, 0, 0 + (Bar_Center_Animation.n現在の値) + 15, 472));
                                        }
                                        else
                                        {
                                            TJAPlayer3.Tx.SongSelect_Bar_Center_NOT?.t2D描画(TJAPlayer3.app.Device, 448, TJAPlayer3.Skin.SongSelect_Overall_Y);
                                        }
                                        break;
                                    }
                                case C曲リストノード.Eノード種別.BOX:
                                    {
                                        if (Bar_Center_Animation.n現在の値 >= 60)
                                        {
                                            TJAPlayer3.Tx.SongSelect_Bar_Center_Text[this.nStr(this.r現在選択中の曲.strジャンル)].n透明度 = 0 + (Bar_Center_Animation.n現在の値 / 1.5f) * (255 / 120);
                                        }
                                        else
                                        {
                                            TJAPlayer3.Tx.SongSelect_Bar_Center_Text[this.nStr(this.r現在選択中の曲.strジャンル)].n透明度 = 0;
                                        }
                                        if (Bar_Center_Animation.n現在の値 < 100)
                                        {
                                            縦拡大 = (Bar_Center_Animation.n現在の値) / 100f;
                                        }
                                        else
                                        {
                                            縦拡大 = 1f;
                                        }
                                        float 余白Y = 56f;
                                        float 拡大だよお = (148f / 148) - 縦拡大;
                                        for (int i = 0; i < 10; i++)
                                        {
                                            TJAPlayer3.Tx.SongSelect_Bar_Center_Header[i].vc拡大縮小倍率.Y = 0 + 縦拡大;
                                        }

                                        TJAPlayer3.Tx.SongSelect_Bar_Center_BOX[this.nStBOXSS(this.r現在選択中の曲.strジャンル)]?.t2D描画(TJAPlayer3.app.Device, 628 - Bar_Center_Animation.n現在の値, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(0, 0, Bar_Center_Animation.n現在の値 + 16, 472));
                                        TJAPlayer3.Tx.SongSelect_Bar_Center_BOX[this.nStBOXSS(this.r現在選択中の曲.strジャンル)]?.t2D描画(TJAPlayer3.app.Device, 644, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(374 - Bar_Center_Animation.n現在の値 + 2, 0, 0 + (Bar_Center_Animation.n現在の値) + 15, 472));

                                        TJAPlayer3.Tx.SongSelect_Bar_Center_Header[this.nStBOXSS(this.r現在選択中の曲.strジャンル)]?.t2D拡大率考慮下基準描画(TJAPlayer3.app.Device, 448, 48 + 余白Y);//448

                                        if (Bar_Center_Animation.n現在の値 >= 70)
                                        {
                                            TJAPlayer3.Tx.SongSelect_Bar_Center_Text[this.nStr(this.r現在選択中の曲.strジャンル)]?.t2D描画(TJAPlayer3.app.Device, 448, 48 - 15);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            {
                                if (Bar_Center_Animation.n現在の値 >= 30)
                                {
                                    TJAPlayer3.Tx.SongSelect_Bar_Center[0].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                    TJAPlayer3.Tx.SongSelect_Bar_Center[0]?.t2D描画(TJAPlayer3.app.Device, 628 - Bar_Center_Animation.n現在の値, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(0, 0, Bar_Center_Animation.n現在の値 + 16, 472));
                                    TJAPlayer3.Tx.SongSelect_Bar_Center[0]?.t2D描画(TJAPlayer3.app.Device, 644, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(374 - Bar_Center_Animation.n現在の値 + 2, 0, 0 + (Bar_Center_Animation.n現在の値) + 15, 472));
                                }
                                else
                                {
                                    TJAPlayer3.Tx.SongSelect_Bar_Center_NOT?.t2D描画(TJAPlayer3.app.Device, 448, TJAPlayer3.Skin.SongSelect_Overall_Y);
                                }
                                break;
                            }
                        case C曲リストノード.Eノード種別.BOX:
                            {
                                if (Bar_Center_Animation.n現在の値 >= 60)
                                {
                                    TJAPlayer3.Tx.SongSelect_Bar_Center_Text[this.nStr(this.r現在選択中の曲.strジャンル)].n透明度 = 0 + (Bar_Center_Animation.n現在の値 / 1.5f) * (255 / 120);
                                }
                                else
                                {
                                    TJAPlayer3.Tx.SongSelect_Bar_Center_Text[this.nStr(this.r現在選択中の曲.strジャンル)].n透明度 = 0;
                                }
                                if (Bar_Center_Animation.n現在の値 < 100)
                                {
                                    縦拡大 = (Bar_Center_Animation.n現在の値) / 100f;
                                }
                                else
                                {
                                    縦拡大 = 1f;
                                }
                                float 余白Y = 56f;
                                float 拡大だよお = (148f / 148) - 縦拡大;
                                for (int i = 0; i < 10; i++)
                                {
                                    TJAPlayer3.Tx.SongSelect_Bar_Center_Header[i].vc拡大縮小倍率.Y = 0 + 縦拡大;
                                }

                                TJAPlayer3.Tx.SongSelect_Bar_Center_BOX[this.nStBOXSS(this.r現在選択中の曲.strジャンル)]?.t2D描画(TJAPlayer3.app.Device, 628 - Bar_Center_Animation.n現在の値, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(0, 0, Bar_Center_Animation.n現在の値 + 16, 472));
                                TJAPlayer3.Tx.SongSelect_Bar_Center_BOX[this.nStBOXSS(this.r現在選択中の曲.strジャンル)]?.t2D描画(TJAPlayer3.app.Device, 644, TJAPlayer3.Skin.SongSelect_Overall_Y, new Rectangle(374 - Bar_Center_Animation.n現在の値 + 2, 0, 0 + (Bar_Center_Animation.n現在の値) + 15, 472));

                                TJAPlayer3.Tx.SongSelect_Bar_Center_Header[this.nStBOXSS(this.r現在選択中の曲.strジャンル)]?.t2D拡大率考慮下基準描画(TJAPlayer3.app.Device, 448, 48 + 余白Y);//448

                                if (Bar_Center_Animation.n現在の値 >= 70)
                                {
                                    TJAPlayer3.Tx.SongSelect_Bar_Center_Text[this.nStr(this.r現在選択中の曲.strジャンル)]?.t2D描画(TJAPlayer3.app.Device, 448, 48 - 15);
                                }
                                break;
                            }
                    }
                }
                switch (r現在選択中の曲.eノード種別)
                {
                    case C曲リストノード.Eノード種別.SCORE:
                        {

                            if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Dan)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Center_Dan?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                            }
                            if (TJAPlayer3.Tx.SongSelect_Frame_Score != null)
                            {
                                // 難易度がTower、Danではない
                                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                                {
                                    for (int i = 0; i < (int)Difficulty.Edit + 1; i++)
                                    {
                                        //透明度操作
                                        if (TJAPlayer3.stage選曲.act難易度選択画面.bIsDifficltSelect)
                                        {
                                            if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 110)
                                            {
                                                TJAPlayer3.Tx.SongSelect_Level.n透明度 = 255 - (int)(((TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                                TJAPlayer3.Tx.SongSelect_Branch_Text.n透明度 = 255 - (int)(((TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                            }
                                            else if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110)
                                            {
                                                TJAPlayer3.Tx.SongSelect_Level.n透明度 = 0;
                                                TJAPlayer3.Tx.SongSelect_Branch_Text.n透明度 = 0;
                                            }
                                        }
                                        else
                                        {
                                            if (this.Bar_Center_Animation.n現在の値 >= 0 && this.Bar_Center_Animation.n現在の値 < 140)
                                            {
                                                TJAPlayer3.Tx.SongSelect_Level.n透明度 = 0 + (this.Bar_Center_Animation.n現在の値 * (255 / 140));
                                                TJAPlayer3.Tx.SongSelect_Frame_Score.n透明度 = 0 + (this.Bar_Center_Animation.n現在の値 * (255 / 140));
                                            }
                                            else
                                            {
                                                TJAPlayer3.Tx.SongSelect_Level.n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                                TJAPlayer3.Tx.SongSelect_Frame_Score.n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                            }
                                            TJAPlayer3.Tx.SongSelect_Branch_Text.n透明度 = 255;
                                            if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i] >= 0)
                                            {
                                                // レベルが0以上
                                                TJAPlayer3.Tx.SongSelect_Frame_Score.color4 = new Color4(1f, 1f, 1f);
                                                if (i == 4 && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == 4)
                                                {
                                                    // エディット
                                                    TJAPlayer3.Tx.SongSelect_Frame_Score?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + (3 * 60), TJAPlayer3.Skin.SongSelect_Overall_Y + 463, new Rectangle(60 * i, 0, 60, 360));
                                                }
                                                else if (i != 4)
                                                {
                                                    TJAPlayer3.Tx.SongSelect_Frame_Score?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + (i * 60), TJAPlayer3.Skin.SongSelect_Overall_Y + 463, new Rectangle(60 * i, 0, 60, 360));
                                                }
                                            }
                                            else
                                            {
                                                // レベルが0未満 = 譜面がないとみなす
                                                TJAPlayer3.Tx.SongSelect_Frame_Score.color4 = new Color4(0.5f, 0.5f, 0.5f);
                                                if (i == 4 && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == 4)
                                                {
                                                    // エディット
                                                    TJAPlayer3.Tx.SongSelect_Frame_Score?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + (3 * 60), TJAPlayer3.Skin.SongSelect_Overall_Y + 463, new Rectangle(60 * i, 0, 60, 360));
                                                }
                                                else if (i != 4)
                                                {
                                                    TJAPlayer3.Tx.SongSelect_Frame_Score?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + (i * 60), TJAPlayer3.Skin.SongSelect_Overall_Y + 463, new Rectangle(60 * i, 0, 60, 360));
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Dan)
                                    {
                                        if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[TJAPlayer3.stage選曲.n現在選択中の曲の難易度] >= 0)
                                        {
                                            // 譜面がありますね
                                            TJAPlayer3.Tx.SongSelect_Frame_Score.color4 = new Color4(1f, 1f, 1f);
                                            TJAPlayer3.Tx.SongSelect_Frame_Score?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + 120, TJAPlayer3.Skin.SongSelect_Overall_Y + 463, new Rectangle(0, 360 + (360 * (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 - (int)Difficulty.Tower)), TJAPlayer3.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Width, 360));
                                        }
                                        else
                                        {
                                            // ないですね
                                            TJAPlayer3.Tx.SongSelect_Frame_Score.color4 = new Color4(0.5f, 0.5f, 0.5f);
                                            TJAPlayer3.Tx.SongSelect_Frame_Score?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + 120, TJAPlayer3.Skin.SongSelect_Overall_Y + 463, new Rectangle(0, 360 + (360 * (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 - (int)Difficulty.Tower)), TJAPlayer3.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Width, 360));
                                        }
                                    }
                                }
                                #region[ 星 ]
                                if (TJAPlayer3.Tx.SongSelect_Level != null)
                                {
                                    // 全難易度表示
                                    // 難易度がTower、Danではない
                                    if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                                    {
                                        for (int i = 0; i < (int)Difficulty.Edit + 1; i++)
                                        {
                                            for (int n = 0; n < TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i]; n++)
                                            {
                                                // 星11以上はループ終了
                                                // 裏なら鬼と同じ場所に
                                                if (i == 3 && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == 4) break;
                                                if (i == 4 && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == 4)
                                                {
                                                    TJAPlayer3.Tx.SongSelect_Level?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + (3 * 60), TJAPlayer3.Skin.SongSelect_Overall_Y + 413 - (n * 17), new Rectangle(32 * i, 0, 32, 32));
                                                }
                                                if (i != 4)
                                                {
                                                    TJAPlayer3.Tx.SongSelect_Level?.t2D下中央基準描画(TJAPlayer3.app.Device, 494 + (i * 60), TJAPlayer3.Skin.SongSelect_Overall_Y + 413 - (n * 17), new Rectangle(32 * i, 0, 32, 32));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Dan)
                                        {
                                            for (int i = 0; i < TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[TJAPlayer3.stage選曲.n現在選択中の曲の難易度]; i++)
                                            {
                                                TJAPlayer3.Tx.SongSelect_Level?.t2D下中央基準描画(TJAPlayer3.app.Device, 494, TJAPlayer3.Skin.SongSelect_Overall_Y + 413 - (i * 17), new Rectangle(32 * TJAPlayer3.stage選曲.n現在選択中の曲の難易度, 0, 32, 32));
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        break;

                    case C曲リストノード.Eノード種別.BOX:
                        TJAPlayer3.Tx.SongSelect_Frame_Box?.t2D描画(TJAPlayer3.app.Device, 450, TJAPlayer3.Skin.SongSelect_Overall_Y);

                        switch (this.r現在選択中の曲.strジャンル)
                        {
                            case "J-POP":
                                #region [ J-POP ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundJPOP.t再生を停止する();
                                }
                                else
                                {
                                    soundJPOP.tサウンドを再生する();
                                }
                                    //-----------------
                                #endregion
                                break;
                            case "アニメ":
                                #region [ アニメ ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundアニメ.t再生を停止する();
                                }
                                else
                                {
                                    soundアニメ.tサウンドを再生する();
                                }
                                //-----------------
                                #endregion
                                break;
                            case "ゲームミュージック":
                                #region [ ゲーム ]
                                //-----------------if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundゲームミュージック.t再生を停止する();
                                }
                                else
                                {
                                    soundゲームミュージック.tサウンドを再生する();
                                }
                               
                                //-----------------
                                #endregion
                                break;
                            case "ナムコオリジナル":
                                #region [ ナムコオリジナル ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundナムコオリジナル.t再生を停止する();
                                }
                                else
                                {
                                    soundナムコオリジナル.tサウンドを再生する();
                                }
                                //-----------------
                                #endregion
                                break;
                            case "クラシック":
                                #region [ クラシック ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundクラシック.t再生を停止する();
                                }
                                else
                                {
                                    soundクラシック.tサウンドを再生する();
                                }
                                //-----------------
                                #endregion
                                break;
                            case "バラエティ":
                                #region [ バラエティ ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundバラエティ.t再生を停止する();
                                }
                                else
                                {
                                    soundバラエティ.tサウンドを再生する();
                                }
                                //-----------------
                                #endregion
                                break;
                            case "どうよう":
                                #region [ どうよう ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundどうよう.t再生を停止する();
                                }
                                else
                                {
                                    soundどうよう.tサウンドを再生する();
                                }
                                //-----------------
                                #endregion
                                break;
                            case "ボーカロイド":
                            case "VOCALOID":
                                #region [ ボカロ ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    soundボーカロイド.t再生を停止する();
                                }
                                else
                                {
                                    soundボーカロイド.tサウンドを再生する();
                                }
                                //-----------------
                                #endregion
                                break;
                            case "段位道場":
                                #region [ 段位道場 ]
                                //-----------------
                                if (TJAPlayer3.Skin.soundゲーム開始音.b再生中)
                                {
                                    sound段位道場.t再生を停止する();
                                }
                                else
                                {
                                    sound段位道場.tサウンドを再生する();
                                }
                                //-----------------
                                #endregion
                                break;
                            default:
                                break;
                        }

                        break;

                    case C曲リストノード.Eノード種別.BACKBOX:
                        TJAPlayer3.Tx.SongSelect_Bar_Center_BackBox?.t2D描画(TJAPlayer3.app.Device, 448, TJAPlayer3.Skin.SongSelect_Overall_Y);
                        break;

                    case C曲リストノード.Eノード種別.RANDOM:
                        TJAPlayer3.Tx.SongSelect_Frame_Random?.t2D描画(TJAPlayer3.app.Device, 450, TJAPlayer3.Skin.SongSelect_Overall_Y);
                        break;
                }
                if (TJAPlayer3.Tx.SongSelect_Branch_Text != null && TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.b譜面分岐[TJAPlayer3.stage選曲.n現在選択中の曲の難易度])
                    TJAPlayer3.Tx.SongSelect_Branch_Text?.t2D描画(TJAPlayer3.app.Device, 483, TJAPlayer3.Skin.SongSelect_Overall_Y + 21);

            }

            #region [ 項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。]
            //-----------------
            if ((this.n目標のスクロールカウンタ == 0))
            {
                int Cursor_L = 372 - this.ct三角矢印アニメ.n現在の値 / 50;
                int Cursor_R = 819 + this.ct三角矢印アニメ.n現在の値 / 50;
                int y = 289;

                // 描画。

                // 難易度がTower、Danではない
                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                {
                    if (!TJAPlayer3.stage選曲.act難易度選択画面.bIsDifficltSelect)
                    {
                        if (TJAPlayer3.Tx.SongSelect_Cursor_Left != null)
                        {
                            TJAPlayer3.Tx.SongSelect_Cursor_Left.n透明度 = 255 - (ct三角矢印アニメ.n現在の値 * 255 / ct三角矢印アニメ.n終了値);
                            TJAPlayer3.Tx.SongSelect_Cursor_Left?.t2D描画(TJAPlayer3.app.Device, Cursor_L, y);
                        }
                        if (TJAPlayer3.Tx.SongSelect_Cursor_Right != null)
                        {
                            TJAPlayer3.Tx.SongSelect_Cursor_Right.n透明度 = 255 - (ct三角矢印アニメ.n現在の値 * 255 / ct三角矢印アニメ.n終了値);
                            TJAPlayer3.Tx.SongSelect_Cursor_Right?.t2D描画(TJAPlayer3.app.Device, Cursor_R, y);
                        }
                    }
                }
            }
            //-----------------
            #endregion


            for (int i = 0; i < 13; i++)    // パネルは全13枚。
            {
                if ((i == 0 && this.n現在のスクロールカウンタ > 0) ||       // 最上行は、上に移動中なら表示しない。
                    (i == 12 && this.n現在のスクロールカウンタ < 0))        // 最下行は、下に移動中なら表示しない。
                    continue;

                int nパネル番号 = (((this.n現在の選択行 - 5) + i) + 13) % 13;
                int n見た目の行番号 = i;
                int n次のパネル番号 = (this.n現在のスクロールカウンタ <= 0) ? ((i + 1) % 13) : (((i - 1) + 13) % 13);
                int x = i選曲バーX座標;
                int xAnime = this.ptバーの座標[n見た目の行番号].X + ((int)((this.ptバーの座標[n次のパネル番号].X - this.ptバーの座標[n見た目の行番号].X) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int y = this.ptバーの基本座標[n見た目の行番号].Y + ((int)((this.ptバーの基本座標[n次のパネル番号].Y - this.ptバーの基本座標[n見た目の行番号].Y) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int xSelectAnime = 0;
                int ySelectAnime = 0;

                if ((i == 5) && (this.n現在のスクロールカウンタ == 0))
                {
                    // (A) スクロールが停止しているときの選択曲バーの描画。

                    #region [ タイトル名テクスチャを描画。]
                    //-----------------
                    if (this.stバー情報[nパネル番号].strタイトル文字列 != "" && this.ttk選択している曲の曲名 == null)
                        this.ttk選択している曲の曲名 = this.ttk曲名テクスチャを生成する(this.stバー情報[nパネル番号].strタイトル文字列, Color.White, Color.Black);
                    if (this.stバー情報[nパネル番号].strサブタイトル != "" && this.ttk選択している曲のサブタイトル == null)
                        this.ttk選択している曲のサブタイトル = this.ttkサブタイトルテクスチャを生成する(this.stバー情報[nパネル番号].strサブタイトル);

                    if (TJAPlayer3.stage選曲.act難易度選択画面.bIsDifficltSelect)
                    {
                        if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                        {
                            int count = TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値;
                            if (count >= 480 && count <= 780)
                            {
                                xSelectAnime = (int)(175f * ((count - 480.0f) / 300.0f));
                            }
                            else if (count >= 780 && count <= 1030)
                            {
                                xSelectAnime = 175;
                                ySelectAnime = -(int)(38f * ((count - 780.0f) / 250.0f));
                            }
                            else if (count > 1030)
                            {
                                xSelectAnime = 175;
                                ySelectAnime = -38;
                            }


                        }
                    }
                    xDansAnime = +(int)(220f * ((Bar_Center_Animation.n現在の値) / 250f));
                    //サブタイトルがあったら700

                    //if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                    //{
                    if (this.ttk選択している曲のサブタイトル != null)
                    {
                        var tx選択している曲のサブタイトル = ResolveTitleTexture(ttk選択している曲のサブタイトル);
                        int nサブタイY = (int)(TJAPlayer3.Skin.SongSelect_Overall_Y + 440 - (tx選択している曲のサブタイトル.sz画像サイズ.Height * tx選択している曲のサブタイトル.vc拡大縮小倍率.Y));


                        tx選択している曲のサブタイトル.n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                        tx選択している曲のサブタイトル?.t2D描画(TJAPlayer3.app.Device, (704 - 228) + 72 + xDansAnime + xSelectAnime, nサブタイY + ySelectAnime);

                        if (this.ttk選択している曲の曲名 != null)
                        {


                            ResolveTitleTexture(this.ttk選択している曲の曲名).n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                            ResolveTitleTexture(this.ttk選択している曲の曲名)?.t2D描画(TJAPlayer3.app.Device, (723 - 200) + 71 + xDansAnime + xSelectAnime, TJAPlayer3.Skin.SongSelect_Overall_Y + 23 + ySelectAnime);

                        }
                    }
                    else
                    {
                        if (this.ttk選択している曲の曲名 != null)
                        {


                            ResolveTitleTexture(this.ttk選択している曲の曲名).n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                            ResolveTitleTexture(this.ttk選択している曲の曲名)?.t2D描画(TJAPlayer3.app.Device, (723 - 200) + 71 + xDansAnime + xSelectAnime, TJAPlayer3.Skin.SongSelect_Overall_Y + 23 + ySelectAnime);

                        }
                    }
                    //-----------------
                    #endregion
                }

            }
            //-----------------
            if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
            {
                TJAPlayer3.Tx.SongSelect_Bar_Center_Dan?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                TJAPlayer3.stage結果.actParameterPanel.Dan_Plate?.t2D描画(TJAPlayer3.app.Device, 0, 0);
            }
                #region [ スクロール地点の計算(描画はCActSelectShowCurrentPositionにて行う) #27648 ]
                int py;
			double d = 0;
			if ( nNumOfItems > 1 )
			{
				d = ( 336 - 6 - 8 ) / (double) ( nNumOfItems - 1 );
				py = (int) ( d * ( nCurrentPosition - 1 ) );
			}
			else
			{
				d = 0;
				py = 0;
			}
			int delta = (int) ( d * this.n現在のスクロールカウンタ / 100 );
			if ( py + delta <= 336 - 6 - 8 )
			{
				this.nスクロールバー相対y座標 = py + delta;
			}
			#endregion

			#region [ アイテム数の描画 #27648 ]
			tアイテム数の描画();
            #endregion

            if ((this.e曲のバー種別を返す(this.r現在選択中の曲)) == Eバー種別.Score && this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル) != 8)
            {
                // 透明度操作
                if (TJAPlayer3.stage選曲.act難易度選択画面.bIsDifficltSelect)
                {
                    if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                    {
                        if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 110)
                        {
                            TJAPlayer3.Tx.SongSelect_GenreText.n透明度 = 255 - (int)(((TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                        }
                        else if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110)
                        {
                            TJAPlayer3.Tx.SongSelect_GenreText.n透明度 = 0;
                        }
                    }
                }
                else
                {
                    TJAPlayer3.Tx.SongSelect_GenreText.n透明度 = 255;
                }
                TJAPlayer3.Tx.SongSelect_GenreText?.t2D描画(TJAPlayer3.app.Device, 496, TJAPlayer3.Skin.SongSelect_Overall_Y - 64, new Rectangle(0, 60 * this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル), 288, 60));
            }

            for (int i = 0; i < 9; i++)
            {
                if (TJAPlayer3.Tx.SongSelect_BoxBack[i] != null)
                {
                    // 透明度操作
                    if (TJAPlayer3.stage選曲.act難易度選択画面.bIsDifficltSelect)
                    {
                        if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                        {
                            if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 360)
                            {
                                TJAPlayer3.Tx.SongSelect_BoxBack[i].n透明度 = 255 - (int)(((TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値) / 360.0f) * 255);
                            }
                            else if (TJAPlayer3.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110)
                            {
                                TJAPlayer3.Tx.SongSelect_BoxBack[i].n透明度 = 0;
                            }
                        }
                    }
                    else
                    {
                        TJAPlayer3.Tx.SongSelect_BoxBack[i].n透明度 = 255;
                    }
                }
            }

            TJAPlayer3.Tx.SongSelect_Footer?.t2D描画(TJAPlayer3.app.Device, 0, 720 - TJAPlayer3.Tx.SongSelect_Footer.sz画像サイズ.Height);

            #region ネームプレート
            for (int i = 0; i < TJAPlayer3.ConfigIni.nPlayerCount; i++)
            {
                TJAPlayer3.Tx.NamePlate[i]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.SongSelect_NamePlate_X[i], TJAPlayer3.Skin.SongSelect_NamePlate_Y[i]);

                if (TJAPlayer3.Tx.PlateEffect != null)
                {
                    TJAPlayer3.stage選曲.ctプレートエフェクト.t進行Loop();
                    TJAPlayer3.Tx.PlateEffect[TJAPlayer3.stage選曲.ctプレートエフェクト.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.SongSelect_PlateEffect_X[i], TJAPlayer3.Skin.SongSelect_PlateEffect_Y[i] - 2);
                }
                TJAPlayer3.Tx.nP[i]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.SongSelect_NamePlate_X[i] - 7, TJAPlayer3.Skin.SongSelect_NamePlate_Y[i] + 2);
                TJAPlayer3.Tx.Taiko_PlateText?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.SongSelect_NamePlate_X[i] + 83, TJAPlayer3.Skin.SongSelect_NamePlate_Y[i]);
            }
            #endregion

            TJAPlayer3.stage選曲.ctどんちゃん入場モーション.t進行();
            TJAPlayer3.Tx.SongSelect_Inchara[TJAPlayer3.stage選曲.ctどんちゃん入場モーション.n現在の値]?.t2D描画(TJAPlayer3.app.Device, 0, 295);

            if (TJAPlayer3.stage選曲.ctどんちゃん入場モーション.b終了値に達してない)
            {
                TJAPlayer3.Tx.SongSelect_Inchara[TJAPlayer3.stage選曲.ctどんちゃん入場モーション.n現在の値]?.t2D描画(TJAPlayer3.app.Device, 0, 295);
            }
            if (TJAPlayer3.stage選曲.ctどんちゃん入場モーション.b終了値に達した && TJAPlayer3.stage選曲.どんちゃんアクション中 == false)
            {
                TJAPlayer3.stage選曲.ctどんちゃんモーションノーマル.t進行Loop();
                TJAPlayer3.Tx.SongSelect_Chara[TJAPlayer3.stage選曲.ctどんちゃんモーションノーマル.n現在の値]?.t2D描画(TJAPlayer3.app.Device, -45, 415);
            }
            return 0;
		}


        // その他

        #region [ private ]
        //-----------------
        private enum Eバー種別 { Score, Box, BackBox, Random, Other }

        private CCounter Bar_Center_Animation;

		private struct STバー情報
		{
			public CActSelect曲リスト.Eバー種別 eバー種別;
			public string strタイトル文字列;
			public CTexture txタイトル名;
			public STDGBVALUE<int> nスキル値;
			public Color col文字色;
            public Color ForeColor;
            public Color BackColor;
            public int[] ar難易度;
            public bool[] b分岐;
            public string strジャンル;
            public string strサブタイトル;
            public TitleTextureKey ttkタイトル;
		}

        public bool b選択曲が変更された = true;
		private Color color文字影 = Color.FromArgb( 0x40, 10, 10, 10 );
        private CCounter ct三角矢印アニメ;
        public CCounter ctバーフェード用;
        private CCounter counter;
        private EFIFOモード mode;
        private CPrivateFastFont pfMusicName;
        private CPrivateFastFont pfSubtitle;

	    // 2018-09-17 twopointzero: I can scroll through 2300 songs consuming approx. 200MB of memory.
	    //                          I have set the title texture cache size to a nearby round number (2500.)
        //                          If we'd like title textures to take up no more than 100MB, for example,
        //                          then a cache size of 1000 would be roughly correct.
	    private readonly LurchTable<TitleTextureKey, CTexture> _titleTextures =
	        new LurchTable<TitleTextureKey, CTexture>(LurchTableOrder.Access, 2500);

		private E楽器パート e楽器パート;
		private Font ft曲リスト用フォント;
		private long nスクロールタイマ;
		private int n現在のスクロールカウンタ;
		private int n現在の選択行;
		private int n目標のスクロールカウンタ;
        private readonly Point[] ptバーの基本座標 = new Point[] { new Point( 0x2c4, 5 ), new Point( 0x272, 56 ), new Point( 0x242, 107 ), new Point( 0x222, 158 ), new Point( 0x210, 209 ), new Point( 0x1d0, 270 ), new Point( 0x224, 362 ), new Point( 0x242, 413 ), new Point( 0x270, 464 ), new Point( 0x2ae, 515 ), new Point( 0x314, 566 ), new Point( 0x3e4, 617 ), new Point( 0x500, 668 ) };
        private Point[] ptバーの座標 = new Point[]
        { new Point( -60, 180 ), new Point( 40, 180 ), new Point( 140, 180 ), new Point( 241, 180 ), new Point( 341, 180 ),
          new Point( 590, 180 ),
          new Point( 840, 180 ), new Point( 941, 180 ), new Point( 1041, 190 ), new Point( 1142, 180 ), new Point( 1242, 180 ), new Point( 1343, 180 ), new Point( 1443, 180 ) };

        private STバー情報[] stバー情報 = new STバー情報[ 13 ];
        private CTexture Dan_Plate;
        private CTexture txSongNotFound, txEnumeratingSongs;
        private TitleTextureKey ttk選択している曲の曲名;
        private TitleTextureKey ttk選択している曲のサブタイトル;

        private CTexture[] tx曲バー_難易度 = new CTexture[ 5 ];

        private long n矢印スクロール用タイマ値;

		private int nCurrentPosition = 0;
		private int nNumOfItems = 0;

		private Eバー種別 e曲のバー種別を返す( C曲リストノード song )
		{
			if( song != null )
			{
				switch( song.eノード種別 )
				{
                    case C曲リストノード.Eノード種別.SCORE:
                    case C曲リストノード.Eノード種別.SCORE_MIDI:
                        return Eバー種別.Score;

                    case C曲リストノード.Eノード種別.BOX:
                        return Eバー種別.Box;

                    case C曲リストノード.Eノード種別.BACKBOX:
                        return Eバー種別.BackBox;

                    case C曲リストノード.Eノード種別.RANDOM:
                        return Eバー種別.Random;
                }
			}
			return Eバー種別.Other;
		}
		private C曲リストノード r次の曲( C曲リストノード song )
		{
			if( song == null )
				return null;

			List<C曲リストノード> list = ( song.r親ノード != null ) ? song.r親ノード.list子リスト : TJAPlayer3.Songs管理.list曲ルート;
	
			int index = list.IndexOf( song );

			if( index < 0 )
				return null;

			if( index == ( list.Count - 1 ) )
				return list[ 0 ];

			return list[ index + 1 ];
		}
		private C曲リストノード r前の曲( C曲リストノード song )
		{
			if( song == null )
				return null;

			List<C曲リストノード> list = ( song.r親ノード != null ) ? song.r親ノード.list子リスト : TJAPlayer3.Songs管理.list曲ルート;

			int index = list.IndexOf( song );
	
			if( index < 0 )
				return null;

			if( index == 0 )
				return list[ list.Count - 1 ];

			return list[ index - 1 ];
		}
		private void tバーの初期化()
		{
			C曲リストノード song = this.r現在選択中の曲;
			
			if( song == null )
				return;

			for( int i = 0; i < 5; i++ )
				song = this.r前の曲( song );

			for( int i = 0; i < 13; i++ )
			{
				this.stバー情報[ i ].strタイトル文字列 = song.strタイトル;
                this.stバー情報[ i ].strジャンル = song.strジャンル;
				this.stバー情報[ i ].col文字色 = song.col文字色;
                this.stバー情報[i].ForeColor = song.ForeColor;
                this.stバー情報[i].BackColor = song.BackColor;
				this.stバー情報[ i ].eバー種別 = this.e曲のバー種別を返す( song );
                this.stバー情報[ i ].strサブタイトル = song.strサブタイトル;
                this.stバー情報[ i ].ar難易度 = song.nLevel;

			    for( int f = 0; f < (int)Difficulty.Total; f++ )
                {
                    if( song.arスコア[ f ] != null )
                        this.stバー情報[ i ].b分岐 = song.arスコア[ f ].譜面情報.b譜面分岐;
                }
				
				for( int j = 0; j < 3; j++ )
					this.stバー情報[ i ].nスキル値[ j ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ j ];

                this.stバー情報[ i ].ttkタイトル = this.ttk曲名テクスチャを生成する( this.stバー情報[ i ].strタイトル文字列, this.stバー情報[i].ForeColor, this.stバー情報[i].BackColor);


                song = this.r次の曲( song );
			}

			this.n現在の選択行 = 5;
		}
		private void tジャンル別選択されていない曲バーの描画( int x, int y, string strジャンル, Eバー種別 eバー種別)
		{
			if( x >= SampleFramework.GameWindowSize.Width || y >= SampleFramework.GameWindowSize.Height )
				return;

            var rc = new Rectangle( 0, 48, 128, 48 );

            switch (strジャンル)
            {
                case "J-POP":
                    #region [ J-POP ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[1] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[1].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[1]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[1]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "アニメ":
                    #region [ アニメ ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[2] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[2].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[2]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[2]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "ゲームミュージック":
                    #region [ ゲーム ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[3] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[3].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[3]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[3]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "ナムコオリジナル":
                    #region [ ナムコ ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[4] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[4].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[4]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[4]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "クラシック":
                    #region [ クラシック ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[5] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[5].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[5]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[5]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "バラエティ":
                    #region [ バラエティ ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[6] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[6].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[6]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[6]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "どうよう":
                    #region [ どうよう ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[7] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[7].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[7]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[7]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "ボーカロイド":
                    #region [ ボカロ ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (TJAPlayer3.Tx.SongSelect_Bar_Genre[8] != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[8].n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                                TJAPlayer3.Tx.SongSelect_Bar_Genre[8]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            }
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[8]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "段位道場":
                    #region [ 段位道場 ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            TJAPlayer3.Tx.SongSelect_Bar_Genre[10]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            break;
                        default:
                            TJAPlayer3.Tx.SongSelect_Bar_Box[10]?.t2D描画(TJAPlayer3.app.Device, x, 88);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "段位-黒":
                    #region [ 段位-黒 ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            TJAPlayer3.Tx.SongSelect_Bar_Genre_Dan[0]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "段位-赤":
                    #region [ 段位-赤 ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            TJAPlayer3.Tx.SongSelect_Bar_Genre_Dan[1]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "段位-銀":
                    #region [ 段位-銀 ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            TJAPlayer3.Tx.SongSelect_Bar_Genre_Dan[2]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            break;
                    }
                    //-----------------
                    #endregion
                    break;
                case "段位-金":
                    #region [ 段位-金 ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            TJAPlayer3.Tx.SongSelect_Bar_Genre_Dan[3]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            break;
                    }

                    //-----------------
                    #endregion
                    break;
                case "外伝":
                    #region [ 段位-外伝 ]
                    //-----------------
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                        case C曲リストノード.Eノード種別.BACKBOX:
                            TJAPlayer3.Tx.SongSelect_Bar_Genre_Dan[4]?.t2D描画(TJAPlayer3.app.Device, x, y);
                            break;
                    }

                    //-----------------
                    #endregion
                    break;
                case "J-POP.":
                case "アニメ.":
                case "ゲームミュージック.":
                case "ナムコオリジナル.":
                case "クラシック.":
                case "バラエティ.":
                case "どうよう.":
                case "ボーカロイド.":
                    #region [ とじる ]
                    //-----------------
                    if (TJAPlayer3.Tx.SongSelect_Bar_BackBox != null)
                    {
                        TJAPlayer3.Tx.SongSelect_Bar_BackBox.n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                        TJAPlayer3.Tx.SongSelect_Bar_BackBox?.t2D描画(TJAPlayer3.app.Device, x, y);
                    }
                    //-----------------
                    #endregion
                    break;
                case "難易度ソート":
                    #region [ 難易度ソート ]
                    //-----------------
                    this.tx曲バー_難易度[this.n現在選択中の曲の現在の難易度レベル]?.t2D描画(TJAPlayer3.app.Device, x, y);
                    //-----------------
                    #endregion
                    break;
                default:
                    #region [ その他の場合 ]
                    //-----------------
                    if (TJAPlayer3.Tx.SongSelect_Bar_BackBox != null && eバー種別 == Eバー種別.BackBox)
                    {
                        TJAPlayer3.Tx.SongSelect_Bar_BackBox.n透明度 = this.ctバーフェード用.n現在の値 - (this.ctバーフェード用.n終了値 - 255);
                        TJAPlayer3.Tx.SongSelect_Bar_BackBox?.t2D描画(TJAPlayer3.app.Device, x, y);
                    }
                    if (TJAPlayer3.Tx.SongSelect_Bar_Genre[0] != null && eバー種別 == Eバー種別.Random)
                        TJAPlayer3.Tx.SongSelect_Bar_Genre[0]?.t2D描画(TJAPlayer3.app.Device, x, y);
                    if (TJAPlayer3.Tx.SongSelect_Bar_Genre[0] != null && eバー種別 != Eバー種別.BackBox && eバー種別 != Eバー種別.Random)
                        TJAPlayer3.Tx.SongSelect_Bar_Genre[0]?.t2D描画(TJAPlayer3.app.Device, x, y);
                    //-----------------
                    #endregion
                    break;
            }
		}
        public void ジャンル音声のリセット()
        {
            soundJPOP.tサウンドを停止する();
            soundどうよう.tサウンドを停止する();
            soundアニメ.tサウンドを停止する();
            soundクラシック.tサウンドを停止する();
            soundゲームミュージック.tサウンドを停止する();
            soundナムコオリジナル.tサウンドを停止する();
            soundバラエティ.tサウンドを停止する();
            soundボーカロイド.tサウンドを停止する();
            sound段位道場.tサウンドを停止する();

            soundJPOP.t再生位置を先頭に戻す();
            soundどうよう.t再生位置を先頭に戻す();
            soundアニメ.t再生位置を先頭に戻す();
            soundクラシック.t再生位置を先頭に戻す();
            soundゲームミュージック.t再生位置を先頭に戻す();
            soundナムコオリジナル.t再生位置を先頭に戻す();
            soundバラエティ.t再生位置を先頭に戻す();
            soundボーカロイド.t再生位置を先頭に戻す();
            sound段位道場.t再生位置を先頭に戻す();
        }
        private int nStr(string strジャンル)
        {
            int nGenre = 8;
            switch (strジャンル)
            {
                case "アニメ":
                    nGenre = 0;
                    break;
                case "J-POP":
                    nGenre = 1;
                    break;
                case "ゲームミュージック":
                    nGenre = 2;
                    break;
                case "ナムコオリジナル":
                    nGenre = 3;
                    break;
                case "クラシック":
                    nGenre = 4;
                    break;
                case "どうよう":
                    nGenre = 5;
                    break;
                case "バラエティ":
                    nGenre = 6;
                    break;
                case "ボーカロイド":
                case "VOCALOID":
                    nGenre = 7;
                    break;
                case "段位道場":
                    nGenre = 8;
                    break;
                case "段位-黒":
                case "段位-赤":
                case "段位-銀":
                case "段位-金":
                case "段位-外伝":
                    nGenre = 9;
                    break;
                default:
                    nGenre = 0;
                    break;

            }

            return nGenre;
        }
        private int nStBOXSS(string strジャンル)
        {
            int nGenre = 8;
            switch (strジャンル)
            {
                case "アニメ":
                    nGenre = 2;
                    break;
                case "J-POP":
                    nGenre = 1;
                    break;
                case "ゲームミュージック":
                    nGenre = 3;
                    break;
                case "ナムコオリジナル":
                    nGenre = 4;
                    break;
                case "クラシック":
                    nGenre = 5;
                    break;
                case "どうよう":
                    nGenre = 7;
                    break;
                case "バラエティ":
                    nGenre =6;
                    break;
                case "ボーカロイド":
                case "VOCALOID":
                    nGenre = 8;
                    break;
                case "段位道場":
                    nGenre = 0;
                    break;
                case "段位-黒":
                case "段位-赤":
                case "段位-銀":
                case "段位-金":
                case "段位-外伝":
                    nGenre = 9;
                    break;
                default:
                    nGenre = 8;
                    break;

            }

            return nGenre;
        }
        private int nStrジャンルtoNum( string strジャンル )
        {
            int nGenre = 8;
            switch( strジャンル )
            {
                case "アニメ":
                    nGenre = 1;
                    break;
                case "J-POP":
                    nGenre = 0;
                    break;
                case "ゲームミュージック":
                    nGenre = 3;
                    break;
                case "ナムコオリジナル":
                    nGenre = 4;
                    break;
                case "クラシック":
                    nGenre = 5;
                    break;
                case "どうよう":
                    nGenre = 6;
                    break;
                case "バラエティ":
                    nGenre = 7;
                    break;
                case "ボーカロイド":
                case "VOCALOID":
                    nGenre = 8;
                    break;
                case "段位道場":
                    nGenre = 0;
                    break;
                case "段位-黒":
                case "段位-赤":
                case "段位-銀":
                case "段位-金":
                case "段位-外伝":
                    nGenre = 9;
                    break;
                default:
                    nGenre = 8;
                    break;

            }

            return nGenre;
        }

        private TitleTextureKey ttk曲名テクスチャを生成する( string str文字, Color forecolor, Color backcolor)
        {
            return new TitleTextureKey(str文字, pfMusicName, forecolor, backcolor, 440);
        }

	    private TitleTextureKey ttkサブタイトルテクスチャを生成する( string str文字 )
        {
            return new TitleTextureKey(str文字, pfSubtitle, Color.White, Color.Black, 370);
        }

	    private CTexture ResolveTitleTexture(TitleTextureKey titleTextureKey)
	    {
	        if (!_titleTextures.TryGetValue(titleTextureKey, out var texture))
	        {
	            texture = GenerateTitleTexture(titleTextureKey);
                _titleTextures.Add(titleTextureKey, texture);
	        }

	        return texture;
	    }

	    private static CTexture GenerateTitleTexture(TitleTextureKey titleTextureKey)
	    {
	        using (var bmp = new Bitmap(titleTextureKey.cPrivateFastFont.DrawPrivateFont(
	            titleTextureKey.str文字, titleTextureKey.forecolor, titleTextureKey.backcolor, true)))
	        {
	            CTexture tx文字テクスチャ = TJAPlayer3.tテクスチャの生成(bmp, false);
	            if (tx文字テクスチャ.szテクスチャサイズ.Height > titleTextureKey.maxHeight)
	            {
	                tx文字テクスチャ.vc拡大縮小倍率.Y = (float) (((double) titleTextureKey.maxHeight) / tx文字テクスチャ.szテクスチャサイズ.Height);
	            }

	            return tx文字テクスチャ;
	        }
	    }

	    private static void OnTitleTexturesOnItemUpdated(
	        KeyValuePair<TitleTextureKey, CTexture> previous, KeyValuePair<TitleTextureKey, CTexture> next)
	    {
            previous.Value.Dispose();
	    }

	    private static void OnTitleTexturesOnItemRemoved(
	        KeyValuePair<TitleTextureKey, CTexture> kvp)
	    {
	        kvp.Value.Dispose();
	    }

	    private void ClearTitleTextureCache()
	    {
	        foreach (var titleTexture in _titleTextures.Values)
	        {
	            titleTexture.Dispose();
	        }

            _titleTextures.Clear();
	    }

        public CSound soundJPOP;
        public CSound soundアニメ;
        public CSound soundゲームミュージック;
        public CSound soundナムコオリジナル;
        public CSound soundクラシック;
        public CSound soundバラエティ;
        public CSound soundどうよう;
        public CSound soundボーカロイド;
        public CSound sound段位道場;
        private sealed class TitleTextureKey
	    {
	        public readonly string str文字;
	        public readonly CPrivateFastFont cPrivateFastFont;
	        public readonly Color forecolor;
	        public readonly Color backcolor;
	        public readonly int maxHeight;

	        public TitleTextureKey(string str文字, CPrivateFastFont cPrivateFastFont, Color forecolor, Color backcolor, int maxHeight)
	        {
	            this.str文字 = str文字;
	            this.cPrivateFastFont = cPrivateFastFont;
	            this.forecolor = forecolor;
	            this.backcolor = backcolor;
	            this.maxHeight = maxHeight;
	        }

	        private bool Equals(TitleTextureKey other)
	        {
	            return string.Equals(str文字, other.str文字) &&
	                   cPrivateFastFont.Equals(other.cPrivateFastFont) &&
	                   forecolor.Equals(other.forecolor) &&
	                   backcolor.Equals(other.backcolor) &&
	                   maxHeight == other.maxHeight;
	        }

	        public override bool Equals(object obj)
	        {
	            if (ReferenceEquals(null, obj)) return false;
	            if (ReferenceEquals(this, obj)) return true;
	            return obj is TitleTextureKey other && Equals(other);
	        }

	        public override int GetHashCode()
	        {
	            unchecked
	            {
	                var hashCode = str文字.GetHashCode();
	                hashCode = (hashCode * 397) ^ cPrivateFastFont.GetHashCode();
	                hashCode = (hashCode * 397) ^ forecolor.GetHashCode();
	                hashCode = (hashCode * 397) ^ backcolor.GetHashCode();
	                hashCode = (hashCode * 397) ^ maxHeight;
	                return hashCode;
	            }
	        }

	        public static bool operator ==(TitleTextureKey left, TitleTextureKey right)
	        {
	            return Equals(left, right);
	        }

	        public static bool operator !=(TitleTextureKey left, TitleTextureKey right)
	        {
	            return !Equals(left, right);
	        }
	    }

		private void tアイテム数の描画()
		{
			string s = nCurrentPosition.ToString() + "/" + nNumOfItems.ToString();
			int x = 639 - 8 - 12;
			int y = 362;

			for ( int p = s.Length - 1; p >= 0; p-- )
			{
				tアイテム数の描画_１桁描画( x, y, s[ p ] );
				x -= 8;
			}
		}
		private void tアイテム数の描画_１桁描画( int x, int y, char s数値 )
		{
			int dx, dy;
			if ( s数値 == '/' )
			{
				dx = 48;
				dy = 0;
			}
			else
			{
				int n = (int) s数値 - (int) '0';
				dx = ( n % 6 ) * 8;
				dy = ( n / 6 ) * 12;
			}
		}


        //数字フォント
        private CTexture txレベル数字フォント;
        [StructLayout( LayoutKind.Sequential )]
        private struct STレベル数字
        {
            public char ch;
            public int ptX;
        }
        private STレベル数字[] st小文字位置 = new STレベル数字[ 10 ];
        private float 縦拡大;

        private void t小文字表示(int x, int y, string str)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.st小文字位置.Length; i++)
                {
                    if( this.st小文字位置[i].ch == ch )
                    {
                        Rectangle rectangle = new Rectangle( this.st小文字位置[i].ptX, 0, 22, 28 );
                        if (this.txレベル数字フォント != null)
                        {
                            this.txレベル数字フォント.t2D描画(TJAPlayer3.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 16;
            }
        }
		//-----------------
		#endregion
	}
}
