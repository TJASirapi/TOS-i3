using System;
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

namespace DTXMania
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
                if (this.n目標のスクロールカウンタ == 0)
                {
                    return (this.n現在のスクロールカウンタ != 0);
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
                return this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(this.r現在選択中の曲);
            }
        }
        public Cスコア r現在選択中のスコア
        {
            get
            {
                if (this.r現在選択中の曲 != null)
                {
                    return this.r現在選択中の曲.arスコア[this.n現在選択中の曲の現在の難易度レベル];
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

        // t選択曲が変更された()内で使う、直前の選曲の保持
        // (前と同じ曲なら選択曲変更に掛かる再計算を省略して高速化するため)
        private C曲リストノード song_last = null;


        // コンストラクタ

        public CActSelect曲リスト()
        {
            #region[ レベル数字 ]
            STレベル数字[] stレベル数字Ar = new STレベル数字[10];
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
            this.n現在のアンカ難易度レベル = CDTXMania.ConfigIni.nDefaultCourse;
            base.b活性化してない = true;
            this.bIsEnumeratingSongs = false;
        }


        // メソッド

        public int n現在のアンカ難易度レベルに最も近い難易度レベルを返す(C曲リストノード song)
        {
            // 事前チェック。

            if (song == null)
                return this.n現在のアンカ難易度レベル;  // 曲がまったくないよ

            if (song.arスコア[this.n現在のアンカ難易度レベル] != null)
                return this.n現在のアンカ難易度レベル;  // 難易度ぴったりの曲があったよ

            if ((song.eノード種別 == C曲リストノード.Eノード種別.BOX) || (song.eノード種別 == C曲リストノード.Eノード種別.BACKBOX))
                return 0;                               // BOX と BACKBOX は関係無いよ


            // 現在のアンカレベルから、難易度上向きに検索開始。

            int n最も近いレベル = this.n現在のアンカ難易度レベル;

            for (int i = 0; i < 5; i++)
            {
                if (song.arスコア[n最も近いレベル] != null)
                    break;  // 曲があった。

                n最も近いレベル = (n最も近いレベル + 1) % 5;  // 曲がなかったので次の難易度レベルへGo。（5以上になったら0に戻る。）
            }


            // 見つかった曲がアンカより下のレベルだった場合……
            // アンカから下向きに検索すれば、もっとアンカに近い曲があるんじゃね？

            if (n最も近いレベル < this.n現在のアンカ難易度レベル)
            {
                // 現在のアンカレベルから、難易度下向きに検索開始。

                n最も近いレベル = this.n現在のアンカ難易度レベル;

                for (int i = 0; i < 5; i++)
                {
                    if (song.arスコア[n最も近いレベル] != null)
                        break;  // 曲があった。

                    n最も近いレベル = ((n最も近いレベル - 1) + 5) % 5;    // 曲がなかったので次の難易度レベルへGo。（0未満になったら4に戻る。）
                }
            }

            return n最も近いレベル;
        }
        public C曲リストノード r指定された曲が存在するリストの先頭の曲(C曲リストノード song)
        {
            List<C曲リストノード> songList = GetSongListWithinMe(song);
            return (songList == null) ? null : songList[0];
        }
        public C曲リストノード r指定された曲が存在するリストの末尾の曲(C曲リストノード song)
        {
            List<C曲リストノード> songList = GetSongListWithinMe(song);
            return (songList == null) ? null : songList[songList.Count - 1];
        }

        private List<C曲リストノード> GetSongListWithinMe(C曲リストノード song)
        {
            if (song.r親ノード == null)                 // root階層のノートだったら
            {
                return CDTXMania.Songs管理.list曲ルート;  // rootのリストを返す
            }
            else
            {
                if ((song.r親ノード.list子リスト != null) && (song.r親ノード.list子リスト.Count > 0))
                {
                    return song.r親ノード.list子リスト;
                }
                else
                {
                    return null;
                }
            }
        }


        public delegate void DGSortFunc(List<C曲リストノード> songList, E楽器パート eInst, int order, params object[] p);
        /// <summary>
        /// 主にCSong管理.cs内にあるソート機能を、delegateで呼び出す。
        /// </summary>
        /// <param name="sf">ソート用に呼び出すメソッド</param>
        /// <param name="eInst">ソート基準とする楽器</param>
        /// <param name="order">-1=降順, 1=昇順</param>
        public void t曲リストのソート(DGSortFunc sf, E楽器パート eInst, int order, params object[] p)
        {
            List<C曲リストノード> songList = GetSongListWithinMe(this.r現在選択中の曲);
            if (songList == null)
            {
                // 何もしない;
            }
            else
            {
                //				CDTXMania.Songs管理.t曲リストのソート3_演奏回数の多い順( songList, eInst, order );
                sf(songList, eInst, order, p);
                //				this.r現在選択中の曲 = CDTXMania
                this.t現在選択中の曲を元に曲バーを再構成する();
            }
        }

        public bool tBOXに入る()
        {
            //Trace.TraceInformation( "box enter" );
            //Trace.TraceInformation( "Skin現在Current : " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
            //Trace.TraceInformation( "Skin現在System  : " + CSkin.strSystemSkinSubfolderFullName );
            //Trace.TraceInformation( "Skin現在BoxDef  : " + CSkin.strBoxDefSkinSubfolderFullName );
            //Trace.TraceInformation( "Skin現在: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
            //Trace.TraceInformation( "Skin現pt: " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
            //Trace.TraceInformation( "Skin指定: " + CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) );
            //Trace.TraceInformation( "Skinpath: " + this.r現在選択中の曲.strSkinPath );
            bool ret = false;
            if (CSkin.GetSkinName(CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false)) != CSkin.GetSkinName(this.r現在選択中の曲.strSkinPath)
                && CSkin.bUseBoxDefSkin)
            {
                ret = true;
                // BOXに入るときは、スキン変更発生時のみboxdefスキン設定の更新を行う
                CDTXMania.Skin.SetCurrentSkinSubfolderFullName(
                    CDTXMania.Skin.GetSkinSubfolderFullNameFromSkinName(CSkin.GetSkinName(this.r現在選択中の曲.strSkinPath)), false);
            }

            //Trace.TraceInformation( "Skin変更: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
            //Trace.TraceInformation( "Skin変更Current : "+  CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
            //Trace.TraceInformation( "Skin変更System  : "+  CSkin.strSystemSkinSubfolderFullName );
            //Trace.TraceInformation( "Skin変更BoxDef  : "+  CSkin.strBoxDefSkinSubfolderFullName );

            if ((this.r現在選択中の曲.list子リスト != null) && (this.r現在選択中の曲.list子リスト.Count > 0))
            {
                this.r現在選択中の曲 = this.r現在選択中の曲.list子リスト[0];
                this.t現在選択中の曲を元に曲バーを再構成する();
                this.t選択曲が変更された(false);                                 // #27648 項目数変更を反映させる
                this.b選択曲が変更された = true;
            }
            return ret;
        }
        public bool tBOXを出る()
        {
            //Trace.TraceInformation( "box exit" );
            //Trace.TraceInformation( "Skin現在Current : " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
            //Trace.TraceInformation( "Skin現在System  : " + CSkin.strSystemSkinSubfolderFullName );
            //Trace.TraceInformation( "Skin現在BoxDef  : " + CSkin.strBoxDefSkinSubfolderFullName );
            //Trace.TraceInformation( "Skin現在: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
            //Trace.TraceInformation( "Skin現pt: " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
            //Trace.TraceInformation( "Skin指定: " + CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) );
            //Trace.TraceInformation( "Skinpath: " + this.r現在選択中の曲.strSkinPath );
            bool ret = false;
            if (CSkin.GetSkinName(CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false)) != CSkin.GetSkinName(this.r現在選択中の曲.strSkinPath)
                && CSkin.bUseBoxDefSkin)
            {
                ret = true;
            }
            // スキン変更が発生しなくても、boxdef圏外に出る場合は、boxdefスキン設定の更新が必要
            // (ユーザーがboxdefスキンをConfig指定している場合への対応のために必要)
            // tBoxに入る()とは処理が微妙に異なるので注意
            CDTXMania.Skin.SetCurrentSkinSubfolderFullName(
                (this.r現在選択中の曲.strSkinPath == "") ? "" : CDTXMania.Skin.GetSkinSubfolderFullNameFromSkinName(CSkin.GetSkinName(this.r現在選択中の曲.strSkinPath)), false);
            //Trace.TraceInformation( "SKIN変更: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
            //Trace.TraceInformation( "SKIN変更Current : "+  CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
            //Trace.TraceInformation( "SKIN変更System  : "+  CSkin.strSystemSkinSubfolderFullName );
            //Trace.TraceInformation( "SKIN変更BoxDef  : "+  CSkin.strBoxDefSkinSubfolderFullName );
            if (this.r現在選択中の曲.r親ノード != null)
            {
                this.r現在選択中の曲 = this.r現在選択中の曲.r親ノード;
                this.t現在選択中の曲を元に曲バーを再構成する();
                this.t選択曲が変更された(false);                                 // #27648 項目数変更を反映させる
                this.b選択曲が変更された = true;
            }
            return ret;
        }
        public void t現在選択中の曲を元に曲バーを再構成する()
        {
            this.tバーの初期化();
            for (int i = 0; i < 13; i++)
            {
                //this.t曲名バーの生成( i, this.stバー情報[ i ].strタイトル文字列, this.stバー情報[ i ].ForeColor, this.stバー情報[i].BackColor);
            }
        }
        public void t次に移動()
        {
            if (this.r現在選択中の曲 != null)
            {
                this.n目標のスクロールカウンタ += 100;
            }
            ジャンル音声のリセット();
            this.b選択曲が変更された = true;
        }
        public void t前に移動()
        {
            if (this.r現在選択中の曲 != null)
            {
                this.n目標のスクロールカウンタ -= 100;
            }
            ジャンル音声のリセット();
            this.b選択曲が変更された = true;
        }
        public void t難易度レベルをひとつ進める()
        {
            if ((this.r現在選択中の曲 == null) || (this.r現在選択中の曲.nスコア数 <= 1))
                return;     // 曲にスコアが０～１個しかないなら進める意味なし。


            // 難易度レベルを＋１し、現在選曲中のスコアを変更する。

            this.n現在のアンカ難易度レベル = this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(this.r現在選択中の曲);

            for (int i = 0; i < 5; i++)
            {
                this.n現在のアンカ難易度レベル = (this.n現在のアンカ難易度レベル + 1) % 5;  // ５以上になったら０に戻る。
                if (this.r現在選択中の曲.arスコア[this.n現在のアンカ難易度レベル] != null)    // 曲が存在してるならここで終了。存在してないなら次のレベルへGo。
                    break;
            }


            // 曲毎に表示しているスキル値を、新しい難易度レベルに合わせて取得し直す。（表示されている13曲全部。）

            C曲リストノード song = this.r現在選択中の曲;
            for (int i = 0; i < 5; i++)
                song = this.r前の曲(song);

            for (int i = this.n現在の選択行 - 5; i < ((this.n現在の選択行 - 5) + 13); i++)
            {
                int index = (i + 13) % 13;
                for (int m = 0; m < 3; m++)
                {
                    this.stバー情報[index].nスキル値[m] = (int)song.arスコア[this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(song)].譜面情報.最大スキル[m];
                }
                song = this.r次の曲(song);
            }


            // 選曲ステージに変更通知を発出し、関係Activityの対応を行ってもらう。

            CDTXMania.stage選曲.t選択曲変更通知();
        }
        /// <summary>
        /// 不便だったから作った。
        /// </summary>
		public void t難易度レベルをひとつ戻す()
        {
            if ((this.r現在選択中の曲 == null) || (this.r現在選択中の曲.nスコア数 <= 1))
                return;     // 曲にスコアが０～１個しかないなら進める意味なし。


            // 難易度レベルを＋１し、現在選曲中のスコアを変更する。

            this.n現在のアンカ難易度レベル = this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(this.r現在選択中の曲);

            this.n現在のアンカ難易度レベル--;
            if (this.n現在のアンカ難易度レベル < 0) // 0より下になったら4に戻す。
            {
                this.n現在のアンカ難易度レベル = 4;
            }

            //2016.08.13 kairera0467 かんたん譜面が無い譜面(ふつう、むずかしいのみ)で、難易度を最上位に戻せない不具合の修正。
            bool bLabel0NotFound = true;
            for (int i = this.n現在のアンカ難易度レベル; i >= 0; i--)
            {
                if (this.r現在選択中の曲.arスコア[i] != null)
                {
                    this.n現在のアンカ難易度レベル = i;
                    bLabel0NotFound = false;
                    break;
                }
            }
            if (bLabel0NotFound)
            {
                for (int i = 4; i >= 0; i--)
                {
                    if (this.r現在選択中の曲.arスコア[i] != null)
                    {
                        this.n現在のアンカ難易度レベル = i;
                        break;
                    }
                }
            }

            // 曲毎に表示しているスキル値を、新しい難易度レベルに合わせて取得し直す。（表示されている13曲全部。）

            C曲リストノード song = this.r現在選択中の曲;
            for (int i = 0; i < 5; i++)
                song = this.r前の曲(song);

            for (int i = this.n現在の選択行 - 5; i < ((this.n現在の選択行 - 5) + 13); i++)
            {
                int index = (i + 13) % 13;
                for (int m = 0; m < 3; m++)
                {
                    this.stバー情報[index].nスキル値[m] = (int)song.arスコア[this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(song)].譜面情報.最大スキル[m];
                }
                song = this.r次の曲(song);
            }


            // 選曲ステージに変更通知を発出し、関係Activityの対応を行ってもらう。

            CDTXMania.stage選曲.t選択曲変更通知();
        }


        /// <summary>
        /// 曲リストをリセットする
        /// </summary>
        /// <param name="cs"></param>
        public void Refresh(CSongs管理 cs, bool bRemakeSongTitleBar)      // #26070 2012.2.28 yyagi
        {
            //			this.On非活性化();

            if (cs != null && cs.list曲ルート.Count > 0)    // 新しい曲リストを検索して、1曲以上あった
            {
                CDTXMania.Songs管理 = cs;

                if (this.r現在選択中の曲 != null)          // r現在選択中の曲==null とは、「最初songlist.dbが無かった or 検索したが1曲もない」
                {
                    this.r現在選択中の曲 = searchCurrentBreadcrumbsPosition(CDTXMania.Songs管理.list曲ルート, this.r現在選択中の曲.strBreadcrumbs);
                    if (bRemakeSongTitleBar)                    // 選曲画面以外に居るときには再構成しない (非活性化しているときに実行すると例外となる)
                    {
                        this.t現在選択中の曲を元に曲バーを再構成する();
                    }
#if false          // list子リストの中まではmatchしてくれないので、検索ロジックは手書きで実装 (searchCurrentBreadcrumbs())
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
        private C曲リストノード searchCurrentBreadcrumbsPosition(List<C曲リストノード> ln, string bc)
        {
            foreach (C曲リストノード n in ln)
            {
                if (n.strBreadcrumbs == bc)
                {
                    return n;
                }
                else if (n.list子リスト != null && n.list子リスト.Count > 0)    // 子リストが存在するなら、再帰で探す
                {
                    C曲リストノード r = searchCurrentBreadcrumbsPosition(n.list子リスト, bc);
                    if (r != null) return r;
                }
            }
            return null;
        }

        /// <summary>
        /// BOXのアイテム数と、今何番目を選択しているかをセットする
        /// </summary>
        public void t選択曲が変更された(bool bForce) // #27648
        {
            C曲リストノード song = CDTXMania.stage選曲.r現在選択中の曲;
            if (song == null)
                return;
            if (song == song_last && bForce == false)
                return;

            song_last = song;
            List<C曲リストノード> list = (song.r親ノード != null) ? song.r親ノード.list子リスト : CDTXMania.Songs管理.list曲ルート;
            int index = list.IndexOf(song) + 1;
            if (index <= 0)
            {
                nCurrentPosition = nNumOfItems = 0;
            }
            else
            {
                nCurrentPosition = index;
                nNumOfItems = list.Count;
            }
            CDTXMania.stage選曲.act演奏履歴パネル.tSongChange();
        }

        // CActivity 実装

        public override void On活性化()
        {
            if (this.b活性化してる)
                return;

            if (!string.IsNullOrEmpty(CDTXMania.ConfigIni.FontName))
            {
                this.pfMusicName = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.FontName), 28);
                this.pfSubtitle = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.FontName), 20);
            }
            else
            {
                this.pfMusicName = new CPrivateFastFont(new FontFamily("MS UI Gothic"), 28);
                this.pfSubtitle = new CPrivateFastFont(new FontFamily("MS UI Gothic"), 20);
            }

            _titleTextures.ItemRemoved += OnTitleTexturesOnItemRemoved;
            _titleTextures.ItemUpdated += OnTitleTexturesOnItemUpdated;

            this.e楽器パート = E楽器パート.DRUMS;
            this.b登場アニメ全部完了 = false;
            this.n目標のスクロールカウンタ = 0;
            this.n現在のスクロールカウンタ = 0;
            this.nスクロールタイマ = -1;

            // フォント作成。
            // 曲リスト文字は２倍（面積４倍）でテクスチャに描画してから縮小表示するので、フォントサイズは２倍とする。

            FontStyle regular = FontStyle.Regular;
            this.ft曲リスト用フォント = new Font(CDTXMania.ConfigIni.FontName, 40f, regular, GraphicsUnit.Pixel);


            // 現在選択中の曲がない（＝はじめての活性化）なら、現在選択中の曲をルートの先頭ノードに設定する。

            if ((this.r現在選択中の曲 == null) && (CDTXMania.Songs管理.list曲ルート.Count > 0))
                this.r現在選択中の曲 = CDTXMania.Songs管理.list曲ルート[0];




            // バー情報を初期化する。

            this.tバーの初期化();

            this.ct三角矢印アニメ = new CCounter();

            base.On活性化();

            this.t選択曲が変更された(true);      // #27648 2012.3.31 yyagi 選曲画面に入った直後の 現在位置/全アイテム数 の表示を正しく行うため
        }
        public override void On非活性化()
        {
            if (this.b活性化してない)
                return;

            _titleTextures.ItemRemoved -= OnTitleTexturesOnItemRemoved;
            _titleTextures.ItemUpdated -= OnTitleTexturesOnItemUpdated;

            CDTXMania.t安全にDisposeする(ref this.ft曲リスト用フォント);

            for (int i = 0; i < 13; i++)
                this.ct登場アニメ用[i] = null;

            this.ct三角矢印アニメ = null;

            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (this.b活性化してない)
                return;

            //this.ctジャンル音声用タイマー = new CCounter(1, 50, 10, CDTXMania.Timer);
            //this.tx曲名バー.Score = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_bar score.png" ), false );
            //this.tx曲名バー.Box = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_bar box.png" ), false );
            //this.tx曲名バー.Other = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_bar other.png" ), false );
            //this.tx選曲バー.Score = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_bar score selected.png" ), false );
            //this.tx選曲バー.Box = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_bar box selected.png" ), false );
            //this.tx選曲バー.Other = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_bar other selected.png" ), false );
            //this.txスキル数字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_skill number on list.png" ), false );

            //this.tx曲バー_JPOP = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_JPOP.png" ), false );
            //this.tx曲バー_アニメ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_anime.png" ), false );
            //this.tx曲バー_ゲーム = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_game.png" ), false );
            //this.tx曲バー_ナムコ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_namco.png" ), false );
            //this.tx曲バー_クラシック = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_classic.png" ), false );
            //this.tx曲バー_バラエティ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_variety.png" ), false );
            //this.tx曲バー_どうよう = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_child.png" ), false );
            //this.tx曲バー_ボカロ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_vocaloid.png" ), false );
            //this.tx曲バー = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard.png" ), false );

            //this.tx曲バー_難易度[0] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Easy.png" ) );
            //this.tx曲バー_難易度[1] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Normal.png" ) );
            //this.tx曲バー_難易度[2] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Hard.png" ) );
            //this.tx曲バー_難易度[3] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Master.png" ) );
            //this.tx曲バー_難易度[4] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Edit.png" ) );

            //this.tx難易度星 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_levelstar.png" ), false );
            //this.tx難易度パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_level_panel.png" ), false );
            //this.tx譜面分岐曲バー用 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_branch.png" ) );
            //this.tx譜面分岐中央パネル用 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_center panel_branch.png" ) );
            //this.txバー中央 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_center panel.png" ) );
            //this.tx上部ジャンル名 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_genrename.png" ) );
            //this.txレベル数字フォント = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_levelfont.png" ) );

            //this.txカーソル左 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_cursor left.png" ) );
            //this.txカーソル右 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_cursor right.png" ) );

            for (int i = 0; i < 13; i++)
            {
                //this.t曲名バーの生成(i, this.stバー情報[i].strタイトル文字列, this.stバー情報[i].ForeColor, this.stバー情報[i].BackColor);
                this.stバー情報[i].ttkタイトル = this.ttk曲名テクスチャを生成する(this.stバー情報[i].strタイトル文字列, this.stバー情報[i].ForeColor, this.stバー情報[i].BackColor);
            }

            #region[ジャンル音声]
            this.soundJPOP = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\J-POP.ogg"), ESoundGroup.Voice);
            this.soundアニメ = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Anime.ogg"), ESoundGroup.Voice);
            this.soundゲームミュージック = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\GameMusic.ogg"), ESoundGroup.Voice);
            this.soundナムコオリジナル = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\NamcoOriginal.ogg"), ESoundGroup.Voice);
            this.soundクラシック = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Classic.ogg"), ESoundGroup.Voice);
            this.soundバラエティ = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Variety.ogg"), ESoundGroup.Voice);
            this.soundどうよう = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Child.ogg"), ESoundGroup.Voice);
            this.soundボーカロイド = CDTXMania.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Vocaloid.ogg"), ESoundGroup.Voice);
            #endregion

            int c = (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja") ? 0 : 1;
            #region [ Songs not found画像 ]
            try
            {
                using (Bitmap image = new Bitmap(640, 128))
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    string[] s1 = { "曲データが見つかりません。", "Songs not found." };
                    string[] s2 = { "曲データをDTXManiaGR.exe以下の", "You need to install songs." };
                    string[] s3 = { "フォルダにインストールして下さい。", "" };
                    graphics.DrawString(s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float)2f, (float)2f);
                    graphics.DrawString(s1[c], this.ft曲リスト用フォント, Brushes.White, (float)0f, (float)0f);
                    graphics.DrawString(s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float)2f, (float)44f);
                    graphics.DrawString(s2[c], this.ft曲リスト用フォント, Brushes.White, (float)0f, (float)42f);
                    graphics.DrawString(s3[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float)2f, (float)86f);
                    graphics.DrawString(s3[c], this.ft曲リスト用フォント, Brushes.White, (float)0f, (float)84f);

                    this.txSongNotFound = new CTexture(CDTXMania.app.Device, image, CDTXMania.TextureFormat);

                    this.txSongNotFound.vc拡大縮小倍率 = new Vector3(0.5f, 0.5f, 1f); // 半分のサイズで表示する。
                }
            }
            catch (CTextureCreateFailedException e)
            {
                Trace.TraceError(e.ToString());
                Trace.TraceError("SoungNotFoundテクスチャの作成に失敗しました。");
                this.txSongNotFound = null;
            }
            #endregion
            #region [ "曲データを検索しています"画像 ]
            try
            {
                using (Bitmap image = new Bitmap(640, 96))
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    string[] s1 = { "曲データを検索しています。", "Now enumerating songs." };
                    string[] s2 = { "そのまましばらくお待ち下さい。", "Please wait..." };
                    graphics.DrawString(s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float)2f, (float)2f);
                    graphics.DrawString(s1[c], this.ft曲リスト用フォント, Brushes.White, (float)0f, (float)0f);
                    graphics.DrawString(s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float)2f, (float)44f);
                    graphics.DrawString(s2[c], this.ft曲リスト用フォント, Brushes.White, (float)0f, (float)42f);

                    this.txEnumeratingSongs = new CTexture(CDTXMania.app.Device, image, CDTXMania.TextureFormat);

                    this.txEnumeratingSongs.vc拡大縮小倍率 = new Vector3(0.5f, 0.5f, 1f); // 半分のサイズで表示する。
                }
            }
            catch (CTextureCreateFailedException e)
            {
                Trace.TraceError(e.ToString());
                Trace.TraceError("txEnumeratingSongsテクスチャの作成に失敗しました。");
                this.txEnumeratingSongs = null;
            }
            #endregion
            #region [ 曲数表示 ]
            //this.txアイテム数数字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenSelect skill number on gauge etc.png" ), false );
            #endregion
            base.OnManagedリソースの作成();
        }
        public override void OnManagedリソースの解放()
        {
            if (this.b活性化してない)
                return;

            //CDTXMania.t安全にDisposeする( ref this.txアイテム数数字 );

            for (int i = 0; i < 13; i++)
            {
                CDTXMania.tテクスチャの解放(ref this.stバー情報[i].txタイトル名);
                this.stバー情報[i].ttkタイトル = null;
            }

            ClearTitleTextureCache();

            //CDTXMania.t安全にDisposeする( ref this.txスキル数字 );
            CDTXMania.tテクスチャの解放(ref this.txEnumeratingSongs);
            CDTXMania.tテクスチャの解放(ref this.txSongNotFound);
            //CDTXMania.t安全にDisposeする( ref this.tx曲名バー.Score );
            //CDTXMania.t安全にDisposeする( ref this.tx曲名バー.Box );
            //CDTXMania.t安全にDisposeする( ref this.tx曲名バー.Other );
            //CDTXMania.t安全にDisposeする( ref this.tx選曲バー.Score );
            //CDTXMania.t安全にDisposeする( ref this.tx選曲バー.Box );
            //CDTXMania.t安全にDisposeする( ref this.tx選曲バー.Other );

            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_JPOP );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_アニメ );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_ゲーム );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_ナムコ );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_クラシック );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_どうよう );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_バラエティ );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー_ボカロ );
            //CDTXMania.tテクスチャの解放( ref this.tx曲バー );
            //CDTXMania.tテクスチャの解放( ref this.tx譜面分岐曲バー用 );

            //for( int i = 0; i < 5; i++ )
            //   {
            //       CDTXMania.tテクスチャの解放( ref this.tx曲バー_難易度[ i ] );
            //   }

            //   CDTXMania.tテクスチャの解放( ref this.tx難易度パネル );
            //   CDTXMania.tテクスチャの解放( ref this.txバー中央 );
            //   CDTXMania.tテクスチャの解放( ref this.tx難易度星 );
            //   CDTXMania.tテクスチャの解放( ref this.tx譜面分岐中央パネル用 );
            //   CDTXMania.tテクスチャの解放( ref this.tx上部ジャンル名 );
            //   CDTXMania.tテクスチャの解放( ref this.txレベル数字フォント );

            //   CDTXMania.tテクスチャの解放( ref this.txカーソル左 );
            //   CDTXMania.tテクスチャの解放( ref this.txカーソル右 );

            CDTXMania.t安全にDisposeする(ref pfMusicName);
            CDTXMania.t安全にDisposeする(ref pfSubtitle);

            base.OnManagedリソースの解放();
        }
        public override int On進行描画()
        {
            if (this.b活性化してない)
                return 0;
            #region [ 初めての進行描画 ]
            //-----------------
            if (this.b初めての進行描画)
            {
                for (int i = 0; i < 13; i++)
                    this.ct登場アニメ用[i] = new CCounter(-i * 10, 100, 3, CDTXMania.Timer);

                this.nスクロールタイマ = CSound管理.rc演奏用タイマ.n現在時刻;
                CDTXMania.stage選曲.t選択曲変更通知();

                this.n矢印スクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
                this.ct三角矢印アニメ.t開始(0, 1000, 1, CDTXMania.Timer);
                base.b初めての進行描画 = false;
            }
            //-----------------
            #endregion


            // まだ選択中の曲が決まってなければ、曲ツリールートの最初の曲にセットする。

            if ((this.r現在選択中の曲 == null) && (CDTXMania.Songs管理.list曲ルート.Count > 0))
                this.r現在選択中の曲 = CDTXMania.Songs管理.list曲ルート[0];


            // 本ステージは、(1)登場アニメフェーズ → (2)通常フェーズ　と二段階にわけて進む。

            // 進行。
            if (n現在のスクロールカウンタ == 0) ct三角矢印アニメ.t進行Loop();
            else ct三角矢印アニメ.n現在の値 = 0;

            if (!this.b登場アニメ全部完了)
            {
                #region [ (1) 登場アニメフェーズの進行。]
                //-----------------
                for (int i = 0; i < 13; i++)    // パネルは全13枚。
                {
                    this.ct登場アニメ用[i].t進行();

                    if (this.ct登場アニメ用[i].b終了値に達した)
                        this.ct登場アニメ用[i].t停止();
                }

                // 全部の進行が終わったら、this.b登場アニメ全部完了 を true にする。

                this.b登場アニメ全部完了 = true;
                for (int i = 0; i < 13; i++)    // パネルは全13枚。
                {
                    if (this.ct登場アニメ用[i].b進行中)
                    {
                        this.b登場アニメ全部完了 = false;    // まだ進行中のアニメがあるなら false のまま。
                        break;
                    }
                }
                //-----------------
                #endregion
            }
            else
            {
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
                        this.t曲名バーの生成(index, this.stバー情報[index].strタイトル文字列, this.stバー情報[index].ForeColor, this.stバー情報[index].BackColor);
                        for (int f = 0; f < 5; f++)
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
                            CDTXMania.stage選曲.t選択曲変更通知();       // スクロール完了＝選択曲変更！

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
                        this.t曲名バーの生成(index, this.stバー情報[index].strタイトル文字列, this.stバー情報[index].ForeColor, this.stバー情報[index].BackColor);
                        for (int f = 0; f < 5; f++)
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
                            CDTXMania.stage選曲.t選択曲変更通知();       // スクロール完了＝選択曲変更！
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
            }


            // 描画。

            if (this.r現在選択中の曲 == null)
            {
                #region [ 曲が１つもないなら「Songs not found.」を表示してここで帰れ。]
                //-----------------
                if (bIsEnumeratingSongs)
                {
                    if (this.txEnumeratingSongs != null)
                    {
                        this.txEnumeratingSongs.t2D描画(CDTXMania.app.Device, 320, 160);
                    }
                }
                else
                {
                    if (this.txSongNotFound != null)
                        this.txSongNotFound.t2D描画(CDTXMania.app.Device, 320, 160);
                }
                //-----------------
                #endregion

                return 0;
            }

            int i選曲バーX座標 = 673; //選曲バーの座標用
            int i選択曲バーX座標 = 665; //選択曲バーの座標用

            if (!this.b登場アニメ全部完了)
            {
                if (this.n現在のスクロールカウンタ == 0)
                {
                    if (CDTXMania.Tx.SongSelect_Bar_Center[0] != null)
                        CDTXMania.Tx.SongSelect_Bar_Center[0].t2D描画(CDTXMania.app.Device, 448, CDTXMania.Skin.SongSelect_Overall_Y);
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                            {
                                if (CDTXMania.Tx.SongSelect_Frame_Score != null)
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i] >= 0)
                                        {
                                            // レベルが0以上
                                            CDTXMania.Tx.SongSelect_Frame_Score.color4 = new Color4(1f, 1f, 1f);
                                            if (i == 3 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4) ;
                                            else if (i == 4 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4)
                                            {
                                                // エディット
                                                CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (3 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                            }
                                            else if (i != 4)
                                            {
                                                CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (i * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                            }
                                        }
                                        else
                                        {
                                            // レベルが0未満 = 譜面がないとみなす
                                            CDTXMania.Tx.SongSelect_Frame_Score.color4 = new Color4(0.5f, 0.5f, 0.5f);
                                            if (i == 3 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4) ;
                                            else if (i == 4 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4)
                                            {
                                                // エディット
                                                CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (3 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                            }
                                            else if (i != 4)
                                            {
                                                CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (i * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                            }

                                        }
                                    }
                                }
                                #region[ 星 ]
                                if (CDTXMania.Tx.SongSelect_Level != null)
                                {
                                    // 全難易度表示
                                    for (int i = 0; i < 5; i++)
                                    {
                                        for (int n = 0; n < CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i]; n++)
                                        {
                                            // 星11以上はループ終了
                                            //if (n > 9) break;
                                            // 裏なら鬼と同じ場所に
                                            if (i == 3 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4) break;
                                            if (i == 4 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4)
                                            {
                                                CDTXMania.Tx.SongSelect_Level.t2D下中央基準描画(CDTXMania.app.Device, 494 + (3 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 413 - (n * 17), new Rectangle(32 * i, 0, 32, 32));
                                            }
                                            if (i != 4)
                                            {
                                                CDTXMania.Tx.SongSelect_Level.t2D下中央基準描画(CDTXMania.app.Device, 494 + (i * 60), CDTXMania.Skin.SongSelect_Overall_Y + 413 - (n * 17), new Rectangle(32 * i, 0, 32, 32));
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            break;

                        case C曲リストノード.Eノード種別.BOX:
                            if (CDTXMania.Tx.SongSelect_Frame_Box != null)
                                CDTXMania.Tx.SongSelect_Frame_Box.t2D描画(CDTXMania.app.Device, 450, CDTXMania.Skin.SongSelect_Overall_Y);
                            switch (this.r現在選択中の曲.strジャンル)
                            {
                                case "J-POP":
                                    #region [ J-POP ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[1] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[1].t2D描画(CDTXMania.app.Device, 448, 38);//448
                                    //-----------------
                                    #endregion
                                    break;
                                case "アニメ":
                                    #region [ アニメ ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[2] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[2].t2D描画(CDTXMania.app.Device, 448, 38);
                                    //-----------------
                                    #endregion
                                    break;
                                case "ゲームミュージック":
                                    #region [ ゲーム ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[3] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[3].t2D描画(CDTXMania.app.Device, 448, 38);
                                    //-----------------
                                    #endregion
                                    break;
                                case "ナムコオリジナル":
                                    #region [ ナムコオリジナル ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[4] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[4].t2D描画(CDTXMania.app.Device, 448, 38);
                                    //-----------------
                                    #endregion
                                    break;
                                case "クラシック":
                                    #region [ クラシック ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[5] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[5].t2D描画(CDTXMania.app.Device, 448, 38);
                                    //-----------------
                                    #endregion
                                    break;
                                case "バラエティ":
                                    #region [ バラエティ ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[6] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[6].t2D描画(CDTXMania.app.Device, 448, 38);
                                    //-----------------
                                    #endregion
                                    break;
                                case "どうよう":
                                    #region [ どうよう ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[7] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[7].t2D描画(CDTXMania.app.Device, 448, 38);
                                    //-----------------
                                    #endregion
                                    break;
                                case "ボーカロイド":
                                case "VOCALOID":
                                    #region [ ボカロ ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[8] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[8].t2D描画(CDTXMania.app.Device, 448, 38);
                                    //-----------------
                                    #endregion
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (CDTXMania.Tx.SongSelect_Bar_Center[9] != null)
                                CDTXMania.Tx.SongSelect_Bar_Center[9].t2D描画(CDTXMania.app.Device, 448, CDTXMania.Skin.SongSelect_Overall_Y);

                            break;

                        case C曲リストノード.Eノード種別.RANDOM:
                            if (CDTXMania.Tx.SongSelect_Frame_Random != null)
                                CDTXMania.Tx.SongSelect_Frame_Random.t2D描画(CDTXMania.app.Device, 450, CDTXMania.Skin.SongSelect_Overall_Y);
                            break;
                            //case C曲リストノード.Eノード種別.DANI:
                            //    if (CDTXMania.Tx.SongSelect_Frame_Dani != null)
                            //        CDTXMania.Tx.SongSelect_Frame_Dani.t2D描画(CDTXMania.app.Device, 450, nバーの高さ);
                            //    break;
                    }
                    if (CDTXMania.Tx.SongSelect_Branch_Text != null && CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.b譜面分岐[CDTXMania.stage選曲.n現在選択中の曲の難易度])
                        CDTXMania.Tx.SongSelect_Branch_Text.t2D描画(CDTXMania.app.Device, 483, CDTXMania.Skin.SongSelect_Overall_Y + 21);
                }
                #region [ (1) 登場アニメフェーズの描画。]
                //-----------------
                for (int i = 0; i < 13; i++)    // パネルは全13枚。
                {
                    if (this.ct登場アニメ用[i].n現在の値 >= 0)
                    {
                        double db割合0to1 = ((double)this.ct登場アニメ用[i].n現在の値) / 100.0;
                        double db回転率 = Math.Sin(Math.PI * 3 / 5 * db割合0to1);
                        int nパネル番号 = (((this.n現在の選択行 - 5) + i) + 13) % 13;

                        if (i == 5)
                        {
                            // (A) 選択曲パネルを描画。

                            #region [ タイトル名テクスチャを描画。]
                            //-----------------
                            if (this.stバー情報[nパネル番号].strタイトル文字列 != "" && this.stバー情報[nパネル番号].strタイトル文字列 != null && this.ttk選択している曲の曲名 == null)
                                this.ttk選択している曲の曲名 = this.ttk曲名テクスチャを生成する(this.stバー情報[nパネル番号].strタイトル文字列, Color.White, Color.Black);
                            if (this.stバー情報[nパネル番号].strサブタイトル != "" && this.stバー情報[nパネル番号].strサブタイトル != null && this.ttk選択している曲のサブタイトル == null)
                                this.ttk選択している曲のサブタイトル = this.ttkサブタイトルテクスチャを生成する(this.stバー情報[nパネル番号].strサブタイトル);


                            if (this.ttk選択している曲のサブタイトル != null)
                            {
                                var tx選択している曲のサブタイトル = ResolveTitleTexture(ttk選択している曲のサブタイトル);
                                int nサブタイY = (int)(CDTXMania.Skin.SongSelect_Overall_Y + 440 - (tx選択している曲のサブタイトル.sz画像サイズ.Height * tx選択している曲のサブタイトル.vc拡大縮小倍率.Y));
                                tx選択している曲のサブタイトル.t2D描画(CDTXMania.app.Device, 707, nサブタイY);
                                if (this.ttk選択している曲の曲名 != null)
                                {
                                    ResolveTitleTexture(this.ttk選択している曲の曲名).t2D描画(CDTXMania.app.Device, 750, CDTXMania.Skin.SongSelect_Overall_Y + 23);
                                }
                            }
                            else
                            {
                                if (this.ttk選択している曲の曲名 != null)
                                {
                                    ResolveTitleTexture(this.ttk選択している曲の曲名).t2D描画(CDTXMania.app.Device, 750, CDTXMania.Skin.SongSelect_Overall_Y + 23);
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            // (B) その他のパネルの描画。

                            #region [ バーテクスチャの描画。]
                            //-----------------
                            int width = (int)(((double)((640 - this.ptバーの基本座標[i].X) + 1)) / Math.Sin(Math.PI * 3 / 5));
                            int x = i選曲バーX座標 + 500 - ((int)(db割合0to1 * 500));
                            int y = this.ptバーの基本座標[i].Y;
                            this.tジャンル別選択されていない曲バーの描画(this.ptバーの座標[nパネル番号].X, CDTXMania.Skin.SongSelect_Overall_Y, this.stバー情報[nパネル番号].strジャンル);
                            if (this.stバー情報[nパネル番号].b分岐[CDTXMania.stage選曲.n現在選択中の曲の難易度] == true && i != 5)
                                CDTXMania.Tx.SongSelect_Branch.t2D描画(CDTXMania.app.Device, this.ptバーの座標[nパネル番号].X + 66, CDTXMania.Skin.SongSelect_Overall_Y - 5);
                            if (this.stバー情報[nパネル番号].ar難易度 != null)
                            {
                                int nX補正 = 0;
                                if (this.stバー情報[nパネル番号].ar難易度[CDTXMania.stage選曲.n現在選択中の曲の難易度].ToString().Length == 2)
                                    nX補正 = -6;
                                this.t小文字表示(this.ptバーの座標[nパネル番号].X + 65 + nX補正, 559, this.stバー情報[nパネル番号].ar難易度[CDTXMania.stage選曲.n現在選択中の曲の難易度].ToString());
                            }
                            //-----------------
                            #endregion
                            #region [ タイトル名テクスチャを描画。]
                            if (this.stバー情報[nパネル番号].ttkタイトル != null)
                                ResolveTitleTexture(this.stバー情報[nパネル番号].ttkタイトル).t2D描画(CDTXMania.app.Device, this.ptバーの座標[i].X + 28, CDTXMania.Skin.SongSelect_Overall_Y + 23);
                            #endregion
                        }
                    }
                }
                //-----------------
                #endregion
            }
            else
            {
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

                    if (CDTXMania.stage選曲.ctboxopen.n現在の値 == 11)
                    {
                        CDTXMania.stage選曲.ct登場時.t開始(0, 100, 5, CDTXMania.Timer);

                    }
                    if (CDTXMania.stage選曲.ct登場時.b終了値に達した)
                    {
                        CDTXMania.stage選曲.ctboxopenBer.t開始(0, 1200, 1, CDTXMania.Timer);
                        //     CDTXMania.stage選曲.ctboxopen.t開始(0, 1200, 1, CDTXMania.Timer);
                    }

                    if (CDTXMania.stage選曲.ctboxopen.n現在の値 > 11 && !CDTXMania.stage選曲.ctboxopen.b終了値に達した)

                    {


                        // 難易度選択画面を開くアニメーション
                        int あまりX = 30;
                        if (i < 5)




                            xAnime -= CDTXMania.stage選曲.ctboxopenBer.n現在の値 < 480 ? (int)(500 * (CDTXMania.stage選曲.ctboxopenBer.n現在の値 / 480.0f)) : 500;
                        else if (i > 5)
                            xAnime += CDTXMania.stage選曲.ctboxopenBer.n現在の値 < 480 ? (int)(500 * (CDTXMania.stage選曲.ctboxopenBer.n現在の値 / 480.0f)) : 500;
                    }

                    if (CDTXMania.stage選曲.ctDiffSelect移動待ち?.n現在の値 > 0 && !CDTXMania.stage選曲.ctDiffSelect移動待ち.b終了値に達した)
                    {
                        //難易度選択画面を開くアニメーション
                        if (i < 5)
                            xAnime -= CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 < 480 ? (int)(500 * (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 / 480.0f)) : 500;
                        else if (i > 5)
                            xAnime += CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 < 480 ? (int)(500 * (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 / 480.0f)) : 500;
                    }
                    else if (CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect && CDTXMania.stage選曲.ctDiffSelect移動待ち.b終了値に達した)
                    {
                        xAnime = 500;
                    }
                    else if (CDTXMania.stage選曲.ctDiffSelect戻り待ち?.n現在の値 > 0 && !CDTXMania.stage選曲.ctDiffSelect戻り待ち.b終了値に達した)
                    {
                        //難易度選択画面を閉じるアニメーション
                        if (i < 5)
                            xAnime -= CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 > 582 ? 500 - (int)(500 * ((CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 - 582) / 480.0f)) : 500;
                        else if (i > 5)
                            xAnime += CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 > 582 ? 500 - (int)(500 * ((CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 - 582) / 480.0f)) : 500;
                    }

                    {
                        // (B) スクロール中の選択曲バー、またはその他のバーの描画。

                        #region [ バーテクスチャを描画。]
                        //-----------------
                        if (n現在のスクロールカウンタ != 0)
                            this.tジャンル別選択されていない曲バーの描画(xAnime, CDTXMania.Skin.SongSelect_Overall_Y, this.stバー情報[nパネル番号].strジャンル);
                        else if (n見た目の行番号 != 5)
                            this.tジャンル別選択されていない曲バーの描画(xAnime, CDTXMania.Skin.SongSelect_Overall_Y, this.stバー情報[nパネル番号].strジャンル);
                        if (this.stバー情報[nパネル番号].b分岐[CDTXMania.stage選曲.n現在選択中の曲の難易度] == true && n見た目の行番号 != 5)
                            CDTXMania.Tx.SongSelect_Branch.t2D描画(CDTXMania.app.Device, xAnime + 66, CDTXMania.Skin.SongSelect_Overall_Y - 5);
                        //-----------------
                        #endregion

                        #region [ タイトル名テクスチャを描画。]

                        if (n現在のスクロールカウンタ != 0)
                            ResolveTitleTexture(this.stバー情報[nパネル番号].ttkタイトル).t2D描画(CDTXMania.app.Device, xAnime + 28, CDTXMania.Skin.SongSelect_Overall_Y + 23);
                        else if (n見た目の行番号 != 5)
                            ResolveTitleTexture(this.stバー情報[nパネル番号].ttkタイトル).t2D描画(CDTXMania.app.Device, xAnime + 28, CDTXMania.Skin.SongSelect_Overall_Y + 23);

                        #endregion

                        if (this.stバー情報[nパネル番号].ar難易度 != null)
                        {
                            int nX補正 = 0;
                            if (this.stバー情報[nパネル番号].ar難易度[CDTXMania.stage選曲.n現在選択中の曲の難易度].ToString().Length == 2)
                                nX補正 = -6;
                            this.t小文字表示(xAnime + 65 + nX補正, 559, this.stバー情報[nパネル番号].ar難易度[CDTXMania.stage選曲.n現在選択中の曲の難易度].ToString());
                        }
                        //-----------------						
                    }
                    #endregion
                }

                if (this.n現在のスクロールカウンタ == 0)
                {
                    if (CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect)
                    {
                        if (CDTXMania.stage選曲.ctDiffSelect移動待ち.b進行中)
                        {
                            if (CDTXMania.stage選曲.ctDiffSelect移動待ち?.n現在の値 < 480)
                            {
                                if (CDTXMania.Tx.SongSelect_Bar_Center[0] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Center[0].t2D描画(CDTXMania.app.Device, 448, CDTXMania.Skin.SongSelect_Overall_Y);
                            }
                            else
                            {
                                int count = CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値;
                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;

                                if (count <= 780)
                                {
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 435 - (int)(195.0f * ((count - 480.0f) / 300.0f)), 94, new Rectangle(2, 2, 30, 480));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + (390.0f * ((count - 480.0f) / 300.0f)); //349 -> 739 (390)
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 465 - (int)(195.0f * ((count - 480.0f) / 300.0f)), 94, new Rectangle(75, 2, 1, 480));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 814 + (int)(195.0f * ((count - 480.0f) / 300.0f)), 94, new Rectangle(38, 2, 30, 480));
                                }
                                else if (count <= 1030)
                                {
                                    //左上
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 103 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(2, 10, 30, 30));
                                    //右上
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 103 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(38, 10, 30, 30));

                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + 390.0f;
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 131, new Rectangle(75, 38, 1, 442)); //中央
                                                                                                                                                  //上縁
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 103 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(75, 10, 1, 30));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 60.0f * ((count - 780.0f) / 250.0f);
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 133 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(75, 26, 1, 1));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 133 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(2, 26, 30, 1));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 133 - (int)(60f * ((count - 780.0f) / 250.0f)), new Rectangle(38, 26, 30, 1));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 131, new Rectangle(2, 38, 30, 442));
                                    //右
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 131, new Rectangle(38, 38, 30, 442));
                                }
                                else
                                {
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 131, new Rectangle(2, 38, 30, 442)); //左
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + 390.0f; // 349 -> 739 (390)
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 131, new Rectangle(75, 38, 1, 442)); // 中央
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 43, new Rectangle(75, 10, 1, 30));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 72.0f;
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 59, new Rectangle(75, 26, 1, 1));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f; //両端中
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 59, new Rectangle(2, 26, 30, 1));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 59, new Rectangle(38, 26, 30, 1));

                                    CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;


                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 43, new Rectangle(2, 10, 30, 30));

                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 43, new Rectangle(38, 10, 30, 30));
                                    CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 131, new Rectangle(38, 38, 30, 442)); //右
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CDTXMania.stage選曲.ctDiffSelect戻り待ち.b進行中 && CDTXMania.stage選曲.ctDiffSelect戻り待ち.b終了値に達してない)
                        {
                            int count = CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値;
                            //count = 260;
                            CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                            if (count < 250)
                            {
                                //左上
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 103 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(2, 10, 30, 30));
                                //右上
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 103 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(38, 10, 30, 30));

                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 349.0f + 390.0f;
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 131, new Rectangle(75, 38, 1, 442)); //中央
                                //上縁
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 103 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(75, 10, 1, 30));
                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 60.0f - (60.0f * ((count - 282.0f) / 250.0f));
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 270, 133 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(75, 26, 1, 1));
                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 133 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(2, 26, 30, 1));
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 133 + (int)(60f * ((count - 250.0f) / 250.0f)), new Rectangle(38, 26, 30, 1));

                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;

                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240, 131, new Rectangle(2, 38, 30, 442));
                                //右
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009, 131, new Rectangle(38, 38, 30, 442));
                            }
                            else if (count >= 250 && count < 500)
                            {
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 240 + (int)(210.0f * ((count - 250.0f) / 250.0f)), 103, new Rectangle(2, 10, 30, 460)); //左

                                //this.txバー中央_アニメ中.vc拡大縮小倍率.X = 349.0f + ( 390.0f - ( 390.0f * (( count - 250.0f ) / 250.0f ) ) ); // 349 -> 739 (390)
                                //this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270 + (int)(60f * (( count - 250.0f ) / 250.0f)), 103, new Rectangle( 75, 10, 1, 460 ) ); // 中央
                                //this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270 + (int)(60f * (( count - 250.0f ) / 250.0f)), 103, new Rectangle( 75, 10, 1, 30 ) );

                                //左半分
                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = (211.0f - (211.0f * ((count - 250.0f) / 250.0f)));
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 480 - (int)(211.0f - (211.0f * ((count - 250.0f) / 250.0f))), 103, new Rectangle(75, 10, 1, 460));

                                //右半分
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 798, 103, new Rectangle(75, 10, 1, 460));

                                //最低限用意する領域 318px
                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 318.0f;
                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 480, 103, new Rectangle(75, 10, 1, 460));

                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.X = 1.0f;
                                CDTXMania.Tx.SongSelect_Difficulty_BOX.vc拡大縮小倍率.Y = 1.0f;

                                CDTXMania.Tx.SongSelect_Difficulty_BOX?.t2D描画(CDTXMania.app.Device, 1009 - (int)(210.0f * ((count - 250.0f) / 250.0f)), 103, new Rectangle(38, 10, 30, 442));
                                //this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009 - (int)(210.0f * (( count - 250.0f ) / 250.0f)), 131, new Rectangle( 38, 38, 30, 442 ) ); //右
                            }
                            else
                            {
                                if (CDTXMania.Tx.SongSelect_Bar_Center[0] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Center[0]?.t2D描画(CDTXMania.app.Device, 448, CDTXMania.Skin.SongSelect_Overall_Y);
                            }
                        }
                        else
                        {
                            if (CDTXMania.Tx.SongSelect_Bar_Center[0] != null)
                                CDTXMania.Tx.SongSelect_Bar_Center[0]?.t2D描画(CDTXMania.app.Device, 448, CDTXMania.Skin.SongSelect_Overall_Y);
                        }
                    }
                    switch (r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                            {
                                if (CDTXMania.Tx.SongSelect_Frame_Score != null)
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        //透明度操作
                                        if (CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect)
                                        {
                                            if (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 110)
                                            {
                                                CDTXMania.Tx.SongSelect_Score_Select.n透明度 = 255 - (int)(((CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                                CDTXMania.Tx.SongSelect_Level.n透明度 = 255 - (int)(((CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                                CDTXMania.Tx.SongSelect_Branch_Text.n透明度 = 255 - (int)(((CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                            }
                                            else if (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110)
                                            {
                                                CDTXMania.Tx.SongSelect_Score_Select.n透明度 = 0;
                                                CDTXMania.Tx.SongSelect_Level.n透明度 = 0;
                                                CDTXMania.Tx.SongSelect_Branch_Text.n透明度 = 0;
                                            }
                                        }
                                        else
                                        {
                                            CDTXMania.Tx.SongSelect_Score_Select.n透明度 = 255;
                                            CDTXMania.Tx.SongSelect_Level.n透明度 = 255;
                                            CDTXMania.Tx.SongSelect_Branch_Text.n透明度 = 255;

                                            if (CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i] >= 0)
                                            {
                                                // レベルが0以上
                                                CDTXMania.Tx.SongSelect_Frame_Score.color4 = new Color4(1f, 1f, 1f);
                                                if (i == 3 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4) ;
                                                else if (i == 4 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4)
                                                {
                                                    // エディット
                                                    CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (3 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                                }
                                                else if (i != 4)
                                                {
                                                    CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (i * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                                }
                                            }
                                            else
                                            {
                                                // レベルが0未満 = 譜面がないとみなす
                                                CDTXMania.Tx.SongSelect_Frame_Score.color4 = new Color4(0.5f, 0.5f, 0.5f);
                                                if (i == 3 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4) ;
                                                else if (i == 4 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4)
                                                {
                                                    // エディット
                                                    CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (3 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                                }
                                                else if (i != 4)
                                                {
                                                    CDTXMania.Tx.SongSelect_Frame_Score.t2D下中央基準描画(CDTXMania.app.Device, 494 + (i * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443, new Rectangle(60 * i, 0, 60, CDTXMania.Tx.SongSelect_Frame_Score.szテクスチャサイズ.Height));
                                                }

                                            }
                                        }
                                    }
                                    #region[ 星 ]
                                    if (CDTXMania.Tx.SongSelect_Level != null)
                                    {
                                        // 全難易度表示
                                        for (int i = 0; i < 5; i++)
                                        {
                                            for (int n = 0; n < CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i]; n++)
                                            {
                                                // 星11以上はループ終了
                                                //if (n > 9) break;
                                                // 裏なら鬼と同じ場所に
                                                if (i == 3 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4) break;
                                                if (i == 4 && CDTXMania.stage選曲.n現在選択中の曲の難易度 == 4)
                                                {
                                                    CDTXMania.Tx.SongSelect_Level.t2D下中央基準描画(CDTXMania.app.Device, 494 + (3 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 413 - (n * 17), new Rectangle(32 * i, 0, 32, 32));
                                                }
                                                if (i != 4)
                                                {
                                                    CDTXMania.Tx.SongSelect_Level.t2D下中央基準描画(CDTXMania.app.Device, 494 + (i * 60), CDTXMania.Skin.SongSelect_Overall_Y + 413 - (n * 17), new Rectangle(32 * i, 0, 32, 32));
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 選択カーソル
                                    if (CDTXMania.stage選曲.n現在選択中の曲の難易度 != 4)
                                    {
                                        //CDTXMania.Tx.SongSelect_Score_Select?.t2D下中央基準描画(CDTXMania.app.Device, 494 + (CDTXMania.stage選曲.n現在選択中の曲の難易度 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443);
                                    }
                                    else
                                    {
                                        //CDTXMania.Tx.SongSelect_Score_Select?.t2D下中央基準描画(CDTXMania.app.Device, 494 + (3 * 60), CDTXMania.Skin.SongSelect_Overall_Y + 443);
                                    }
                                    #endregion
                                }
                            }
                            break;

                        case C曲リストノード.Eノード種別.BOX:
                            if (CDTXMania.Tx.SongSelect_Frame_Box != null)
                                CDTXMania.Tx.SongSelect_Frame_Box.t2D描画(CDTXMania.app.Device, 450, CDTXMania.Skin.SongSelect_Overall_Y);
                            switch (this.r現在選択中の曲.strジャンル)
                            {
                                case "J-POP":
                                    #region [ J-POP ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[1] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[1].t2D描画(CDTXMania.app.Device, 448, 38);//448
                                    
                                    soundJPOP.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                case "アニメ":
                                    #region [ アニメ ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[2] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[2].t2D描画(CDTXMania.app.Device, 448, 38);

                                    soundアニメ.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                case "ゲームミュージック":
                                    #region [ ゲーム ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[3] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[3].t2D描画(CDTXMania.app.Device, 448, 38);

                                    soundゲームミュージック.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                case "ナムコオリジナル":
                                    #region [ ナムコオリジナル ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[4] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[4].t2D描画(CDTXMania.app.Device, 448, 38);

                                    soundナムコオリジナル.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                case "クラシック":
                                    #region [ クラシック ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[5] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[5].t2D描画(CDTXMania.app.Device, 448, 38);

                                    soundクラシック.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                case "バラエティ":
                                    #region [ バラエティ ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[6] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[6].t2D描画(CDTXMania.app.Device, 448, 38);

                                    soundバラエティ.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                case "どうよう":
                                    #region [ どうよう ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[7] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[7].t2D描画(CDTXMania.app.Device, 448, 38);

                                    soundどうよう.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                case "ボーカロイド":
                                case "VOCALOID":
                                    #region [ ボカロ ]
                                    //-----------------
                                    if (CDTXMania.Tx.SongSelect_Bar_Center[8] != null)
                                        CDTXMania.Tx.SongSelect_Bar_Center[8].t2D描画(CDTXMania.app.Device, 448, 38);

                                    soundボーカロイド.tサウンドを再生する();
                                    //-----------------
                                    #endregion
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case C曲リストノード.Eノード種別.BACKBOX:
                            if (CDTXMania.Tx.SongSelect_Bar_Center[9] != null)
                                CDTXMania.Tx.SongSelect_Bar_Center[9].t2D描画(CDTXMania.app.Device, 448, CDTXMania.Skin.SongSelect_Overall_Y);
                            break;

                        case C曲リストノード.Eノード種別.RANDOM:
                            if (CDTXMania.Tx.SongSelect_Frame_Random != null)
                                CDTXMania.Tx.SongSelect_Frame_Random.t2D描画(CDTXMania.app.Device, 450, CDTXMania.Skin.SongSelect_Overall_Y);
                            break;
                            //case C曲リストノード.Eノード種別.DANI:
                            //    if (CDTXMania.Tx.SongSelect_Frame_Dani != null)
                            //        CDTXMania.Tx.SongSelect_Frame_Dani.t2D描画(CDTXMania.app.Device, 450, nバーの高さ);
                            //    break;
                    }
                    //if( CDTXMania.Tx.SongSelect_Level != null )
                    //    CDTXMania.Tx.SongSelect_Level.t2D描画( CDTXMania.app.Device, 518, 169 );
                    if (CDTXMania.Tx.SongSelect_Branch_Text != null && CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.b譜面分岐[CDTXMania.stage選曲.n現在選択中の曲の難易度])
                        CDTXMania.Tx.SongSelect_Branch_Text.t2D描画(CDTXMania.app.Device, 483, CDTXMania.Skin.SongSelect_Overall_Y + 21);
                }

                #region [ 項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。]
                //-----------------
                if ((this.n目標のスクロールカウンタ == 0))
                {
                    int Cursor_L = 372 - this.ct三角矢印アニメ.n現在の値 / 50;
                    int Cursor_R = 819 + this.ct三角矢印アニメ.n現在の値 / 50;
                    int y = 289;

                    // 描画。

                    if (!CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect)
                    {
                        if (CDTXMania.Tx.SongSelect_Cursor_Left != null)
                        {
                            CDTXMania.Tx.SongSelect_Cursor_Left.n透明度 = 255 - (ct三角矢印アニメ.n現在の値 * 255 / ct三角矢印アニメ.n終了値);
                            CDTXMania.Tx.SongSelect_Cursor_Left.t2D描画(CDTXMania.app.Device, Cursor_L, y);
                        }
                        if (CDTXMania.Tx.SongSelect_Cursor_Right != null)
                        {
                            CDTXMania.Tx.SongSelect_Cursor_Right.n透明度 = 255 - (ct三角矢印アニメ.n現在の値 * 255 / ct三角矢印アニメ.n終了値);
                            CDTXMania.Tx.SongSelect_Cursor_Right.t2D描画(CDTXMania.app.Device, Cursor_R, y);
                        }
                    }
                }
                //-----------------
                #endregion

                switch (r現在選択中の曲.eノード種別)
                {

                    case C曲リストノード.Eノード種別.BOX:

                        if (CDTXMania.stage選曲.OKかどうか == true)
                        {


                            if (true)//r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.BACKBOX | r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.SCORE)
                            {

                                int あまりX = 5;
                                int あまりX2 = 439;
                                int 倍率を増やすX2 = 800;
                                int あまりX22 = 838;
                                int あまりY = 108;
                                int あまりY上に行く = 5;
                                int 倍率を増やすX = 454;
                                int さっき余った分だよバカ = 453;
                                switch (CDTXMania.stage選曲.ctboxopen.n現在の値)
                                {




                                    case 0:
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].n透明度 = 41;

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 5, new Rectangle(0, 0, 1280, 100));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 5, new Rectangle(0, 90, 1280, 50));

                                        /*
                                        //
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 14 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));

                                        //
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 14 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                                        //
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 14 - 2, 0 + 96 - 2, new Rectangle(439, 83, 16, 50));

                                        */

                                        break;
                                    case 1:
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].n透明度 = 120;

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 10, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 10, new Rectangle(0, 90, 1280, 53));
                                        /*
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 12 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));

                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 12 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 12 - 2, 0 + 96 - 4, new Rectangle(439, 83, 16, 50));
                            */

                                        break;
                                    case 2:
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].n透明度 = 200;

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 15, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 15, new Rectangle(0, 90, 1280, 56));
                                        /*
                               //
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 10 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));

                               //
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 10 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                               //
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 10 - 2, 0 + 96 - 7, new Rectangle(439, 83, 16, 50));
                               */
                                        break;
                                    case 3:
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].n透明度 = 255;

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 20, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 20, new Rectangle(0, 90, 1280, 59));
                                        /*
                             //
                             CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                             CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 8 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));
                             //
                             CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                             CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 8 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                             //
                             CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                             CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 8 - 2, 0 + 96 - 10, new Rectangle(439, 83, 16, 50));
                             */
                                        break;
                                    case 4:
                                        //    CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].n透明度 = 165;

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 25, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 25, new Rectangle(0, 90, 1280, 62));
                                        /*
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 6 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 6 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 6 - 2, 0 + 96 - 10, new Rectangle(439, 83, 16, 50));
                            */
                                        break;
                                    case 5:
                                        //      CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].n透明度 = 190;

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 30, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 30, new Rectangle(0, 90, 1280, 65));
                                        /*
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 4 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 4 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 4 - 2, 0 + 96, new Rectangle(439, 83, 16, 50));
                            */
                                        break;
                                    case 6:

                                        //         CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].n透明度 = 255;

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 32, new Rectangle(0, 0, 1280, 100));


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 32, new Rectangle(0, 90, 1280, 68));
                                        /*
                              //
                              CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                              CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 2 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));
                              //
                              CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                              CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 2 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                              //
                              CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                              CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 2 - 2, 0 + 96, new Rectangle(439, 83, 16, 50));
                              */
                                        break;



                                    case 7:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 30, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 30, new Rectangle(0, 90, 1280, 72));
                                        /*
                               //
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));


                               //
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 + 0 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                               //
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                               CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 83, 16, 50));
                               */
                                        break;
                                    case 8:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 28, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 28, new Rectangle(0, 90, 1280, 75));
                                        /*
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 + 0 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 83, 16, 50));
                            */
                                        break;
                                    case 9:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 26, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 26, new Rectangle(0, 90, 1280, 72));
                                        /*
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 + 0 + 2 + 3 + 3, 0 + 96, new Rectangle(822, 96, 16, 624));

                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 83, 16, 50));
                            */
                                        break;
                                    case 10:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 3 - あまりY上に行く, new Rectangle(0, 100, 1280, 630));


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 20 - 23, new Rectangle(0, 0, 1280, 100));

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, あまりY - 23, new Rectangle(0, 90, 1280, 68));
                                        /*
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 96, 16, 624));
                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 822 - 0 + 2 + 3 + 3 , 0 + 96, new Rectangle(822, 96, 16, 624));

                            //
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 3 + 439 + 0 - 2, 0 + 96, new Rectangle(439, 83, 16, 50));
                            */
                                        break;
                                    case 11:


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 0);
                                        break;

                                    case 12:


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 2.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 25 + 倍率を増やすX - 4, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -25 + あまりX2 - 4, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 2.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 20 + 20 - 4, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +25 + あまりX22 - 15 - 10 - 4, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 13:


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 4f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 50 + 倍率を増やすX - 3, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -50 + あまりX2 - 3, 0, new Rectangle(439, 0, 35, 720));
                                        #region R

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 4f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 20 - 3, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +50 + あまりX22 - 15 - 20 - 3, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 14:


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 5.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 75 + 倍率を増やすX - 2, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -75 + あまりX2 - 2, 0, new Rectangle(439, 0, 35, 720));
                                        #region R


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 5.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 20 - 5 - 2, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +75 + あまりX22 - 15 - 20 - 2, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 15:


                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 7f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 100 + 倍率を増やすX - 1, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -90 + あまりX2 - 6 - 1, 0, new Rectangle(439, 0, 35, 720));
                                        #region R

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 7f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 7 - 15 - 20 + 20 - 10 - 1, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +90 + あまりX22 - 15 - 20 - 1, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 16:
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 8.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 125 + 倍率を増やすX + 1, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -125 + あまりX2 + 1, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 8.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 20 - 11 + 1, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +125 + あまりX22 - 15 - 20 + 1, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 17:
                                        //パス
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 10f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 150 + 倍率を増やすX + 2, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -150 + あまりX2 + 2, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 10f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 2, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +150 + あまりX22 - 15 - 20 + 2, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 18:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 11.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 175 + 倍率を増やすX + 3, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -175 + あまりX2 + 3 + 3, 0, new Rectangle(439, 0, 35, 720));
                                        #region R

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 11.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 5 + 3, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +175 + あまりX22 - 15 - 20 + 3, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 19:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 13f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 200 + 倍率を増やすX + 4, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -200 + あまりX2 + 4, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 13f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 4, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +200 + あまりX22 + 7 - 15 - 20 + 4, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 20:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 14.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 225 + 倍率を増やすX + 5, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -225 + あまりX2 + 5, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 14.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 5, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +225 + あまりX22 + 12 - 15 - 20 + 5, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 21:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 16f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 250 + 倍率を増やすX + 6, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -250 + あまりX2 + 6, 0, new Rectangle(439, 0, 35, 720));
                                        #region R

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 16f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 6, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +250 + あまりX22 + 16 - 15 - 20 + 6, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 22:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 17.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 275 + 倍率を増やすX + 7, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -275 + あまりX2 + 7, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 17.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 7, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +275 + あまりX22 + 20 - 15 - 20 + 7, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 23:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 19f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 300 + 倍率を増やすX + 8, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -300 + あまりX2 + 8, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 19f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 + 8, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +300 + あまりX22 + 24 - 15 - 20 + 8, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 24:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 20.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 325 + 倍率を増やすX + 9, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -325 + あまりX2 + 9, 0, new Rectangle(439, 0, 35, 720));
                                        #region R

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 20.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 5 + 9, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +325 + あまりX22 + 30 - 15 - 20 - 5 + 9, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 25:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 22f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 350 + 倍率を増やすX + 10, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -350 + あまりX2 + 10, 0, new Rectangle(439, 0, 35, 720));
                                        #region R

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 22f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 5 + 10, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +350 + あまりX22 + 35 - 15 - 20 - 5 + 10, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 26:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 23.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 375 + 倍率を増やすX + 11, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -375 + あまりX2 + 11, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 23.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 5 + 11, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +375 + あまりX22 + 40 - 15 - 20 - 5 + 11, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 27:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 25f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 400 + 倍率を増やすX + 12, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -400 + あまりX2 + 12, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 25f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 5 - 12 + 12, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +400 + あまりX22 + 30 - 15 - 20 - 5 + 12, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 28:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 26.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 425 + 倍率を増やすX + 13, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -425 + あまりX2 + 13, 0, new Rectangle(439, 0, 35, 720));
                                        #region R

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 26.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 7 - 12 + 13, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +425 + あまりX22 + 34 - 15 - 20 - 5 + 13, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 29:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 28f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 450 + 倍率を増やすX + 14, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -450 + あまりX2 + 14, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 28f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 13 - 12 + 14, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +450 + あまりX22 + 40 - 15 - 20 - 5 + 14, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 30:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 29.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 475 + 倍率を増やすX + 15, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -475 + あまりX2 + 15, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 29.5f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 13 - 12 + 15, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +475 + あまりX22 + 45 - 15 - 20 - 5 + 15, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 31:

                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 31f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX - 500 + 倍率を増やすX + 16, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, -500 + あまりX2 + 16, 0, new Rectangle(439, 0, 35, 720));
                                        #region R
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 31f;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + 倍率を増やすX2 - 15 - 20 - 13 - 12 + 16, 0, new Rectangle(453, 0, 20, 720));
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, +500 + あまりX22 + 50 - 15 - 20 - 5 + 16, 0, new Rectangle(821, 0, 35, 720));
                                        #endregion
                                        if (CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)

                                            CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].vc拡大縮小倍率.X = 1;
                                        CDTXMania.Tx.SongSelect_BoxBackC[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX + さっき余った分だよバカ, 0, new Rectangle(453, 0, 370, 720));
                                        break;
                                    case 32:
                                        if (CDTXMania.Tx.SongSelect_BoxBack[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)] != null)
                                            CDTXMania.Tx.SongSelect_BoxBack[this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル)].t2D描画(CDTXMania.app.Device, 0 - あまりX, 0);

                                        break;
                                }
                                break;
                            }


                            break;


                            break;
                        }
                        break;
                }

                for (int i = 0; i < 13; i++)    // パネルは全13枚。
                {
                    if ((i == 0 && this.n現在のスクロールカウンタ > 0) ||       // 最上行は、上に移動中なら表示しない。
                        (i == 12 && this.n現在のスクロールカウンタ < 0))        // 最下行は、下に移動中なら表示しない。
                        continue;

                    int nパネル番号 = (((this.n現在の選択行 - 5) + i) + 13) % 13;
                    int n見た目の行番号 = i;
                    int n次のパネル番号 = (this.n現在のスクロールカウンタ <= 0) ? ((i + 1) % 13) : (((i - 1) + 13) % 13);
                    //int x = this.ptバーの基本座標[ n見た目の行番号 ].X + ( (int) ( ( this.ptバーの基本座標[ n次のパネル番号 ].X - this.ptバーの基本座標[ n見た目の行番号 ].X ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );
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

                        if (CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect)
                        {
                            int count = CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値;
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

                        //サブタイトルがあったら700

                        if (this.ttk選択している曲のサブタイトル != null)
                        {
                            var tx選択している曲のサブタイトル = ResolveTitleTexture(ttk選択している曲のサブタイトル);
                            int nサブタイY = (int)(CDTXMania.Skin.SongSelect_Overall_Y + 440 - (tx選択している曲のサブタイトル.sz画像サイズ.Height * tx選択している曲のサブタイトル.vc拡大縮小倍率.Y));
                            tx選択している曲のサブタイトル.t2D描画(CDTXMania.app.Device, 707 + xSelectAnime, nサブタイY + ySelectAnime);
                            if (this.ttk選択している曲の曲名 != null)
                            {
                                ResolveTitleTexture(this.ttk選択している曲の曲名).t2D描画(CDTXMania.app.Device, 750 + xSelectAnime, CDTXMania.Skin.SongSelect_Overall_Y + 23 + ySelectAnime);
                            }
                        }
                        else
                        {
                            if (this.ttk選択している曲の曲名 != null)
                            {
                                ResolveTitleTexture(this.ttk選択している曲の曲名).t2D描画(CDTXMania.app.Device, 750 + xSelectAnime, CDTXMania.Skin.SongSelect_Overall_Y + 23 + ySelectAnime);
                            }
                        }

                        //if( this.stバー情報[ nパネル番号 ].txタイトル名 != null )
                        //	this.stバー情報[ nパネル番号 ].txタイトル名.t2D描画( CDTXMania.app.Device, i選択曲バーX座標 + 65, y選曲 + 6 );

                        //CDTXMania.act文字コンソール.tPrint( i選曲バーX座標 - 20, y選曲 + 6, C文字コンソール.Eフォント種別.白, this.r現在選択中のスコア.譜面情報.b譜面分岐[3].ToString() );
                        //-----------------
                        #endregion
                    }

                }
                //-----------------
            }
            #region [ スクロール地点の計算(描画はCActSelectShowCurrentPositionにて行う) #27648 ]
            int py;
            double d = 0;
            if (nNumOfItems > 1)
            {
                d = (336 - 6 - 8) / (double)(nNumOfItems - 1);
                py = (int)(d * (nCurrentPosition - 1));
            }
            else
            {
                d = 0;
                py = 0;
            }
            int delta = (int)(d * this.n現在のスクロールカウンタ / 100);
            if (py + delta <= 336 - 6 - 8)
            {
                this.nスクロールバー相対y座標 = py + delta;
            }
            #endregion

            #region [ アイテム数の描画 #27648 ]
            tアイテム数の描画();
            #endregion

            if (((this.e曲のバー種別を返す(this.r現在選択中の曲)) == Eバー種別.Score && CDTXMania.stage選曲.actSortSongs.e現在のソート != CActSortSongs.EOrder.Title) && this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル) != 8)
            {
                // 透明度操作
                if (CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect)
                {
                    if (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 110)
                    {
                        CDTXMania.Tx.SongSelect_GenreText.n透明度 = 255 - (int)(((CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                    }
                    else if (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110)
                    {
                        CDTXMania.Tx.SongSelect_GenreText.n透明度 = 0;
                    }
                }
                else
                {
                    CDTXMania.Tx.SongSelect_GenreText.n透明度 = 255;
                }
                if (CDTXMania.Tx.SongSelect_GenreText != null)
                    CDTXMania.Tx.SongSelect_GenreText.t2D描画(CDTXMania.app.Device, 496, CDTXMania.Skin.SongSelect_Overall_Y - 64, new Rectangle(0, 60 * this.nStrジャンルtoNum(this.r現在選択中の曲.strジャンル), 288, 60));
            }

            for (int i = 0; i < 9; i++)
            {
                if (CDTXMania.Tx.SongSelect_BoxBack[i] != null)
                {
                    // 透明度操作
                    if (CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect)
                    {
                        if (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 360)
                        {
                            CDTXMania.Tx.SongSelect_BoxBack[i].n透明度 = 255 - (int)(((CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 360.0f) * 255);
                        }
                        else if (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110)
                        {
                            CDTXMania.Tx.SongSelect_BoxBack[i].n透明度 = 0;
                        }
                    }
                    else
                    {
                        CDTXMania.Tx.SongSelect_BoxBack[i].n透明度 = 255;
                    }
                }
            }


            return 0;
        }


        // その他

        #region [ private ]
        //-----------------
        private enum Eバー種別 { Score, Box, Other }

        private struct STバー
        {
            public CTexture Score;
            public CTexture Box;
            public CTexture Other;
            public CTexture this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.Score;

                        case 1:
                            return this.Box;

                        case 2:
                            return this.Other;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.Score = value;
                            return;

                        case 1:
                            this.Box = value;
                            return;

                        case 2:
                            this.Other = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }

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

        private struct ST選曲バー
        {
            public CTexture Score;
            public CTexture Box;
            public CTexture Other;
            public CTexture this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.Score;

                        case 1:
                            return this.Box;

                        case 2:
                            return this.Other;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.Score = value;
                            return;

                        case 1:
                            this.Box = value;
                            return;

                        case 2:
                            this.Other = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public bool b選択曲が変更された = true;
        private bool b登場アニメ全部完了;
        private Color color文字影 = Color.FromArgb(0x40, 10, 10, 10);
        private CCounter[] ct登場アニメ用 = new CCounter[13];
        private CCounter ct三角矢印アニメ;
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
        private readonly Point[] ptバーの基本座標 = new Point[] { new Point(0x2c4, 5), new Point(0x272, 56), new Point(0x242, 107), new Point(0x222, 158), new Point(0x210, 209), new Point(0x1d0, 270), new Point(0x224, 362), new Point(0x242, 413), new Point(0x270, 464), new Point(0x2ae, 515), new Point(0x314, 566), new Point(0x3e4, 617), new Point(0x500, 668) };
        private Point[] ptバーの座標 = new Point[]
        { new Point( -60, 180 ), new Point( 40, 180 ), new Point( 140, 180 ), new Point( 241, 180 ), new Point( 341, 180 ),
          new Point( 590, 180 ),
          new Point( 840, 180 ), new Point( 941, 180 ), new Point( 1041, 180 ), new Point( 1142, 180 ), new Point( 1242, 180 ), new Point( 1343, 180 ), new Point( 1443, 180 ) };

        private STバー情報[] stバー情報 = new STバー情報[13];
        private CTexture txSongNotFound, txEnumeratingSongs;
        //private CTexture txスキル数字;
        //private CTexture txアイテム数数字;
        //private STバー tx曲名バー;
        //private ST選曲バー tx選曲バー;
        //      private CTexture txバー中央;
        private TitleTextureKey ttk選択している曲の曲名;
        private TitleTextureKey ttk選択している曲のサブタイトル;

        //private CTexture tx曲バー_アニメ;
        //private CTexture tx曲バー_JPOP;
        //private CTexture tx曲バー_クラシック;
        //private CTexture tx曲バー_ゲーム;
        //private CTexture tx曲バー_ナムコ;
        //private CTexture tx曲バー_バラエティ;
        //private CTexture tx曲バー_どうよう;
        //private CTexture tx曲バー_ボカロ;
        //private CTexture tx曲バー;

        private CTexture[] tx曲バー_難易度 = new CTexture[5];
        
        public CSound soundJPOP;
        public CSound soundアニメ;
        public CSound soundゲームミュージック;
        public CSound soundナムコオリジナル;
        public CSound soundクラシック;
        public CSound soundバラエティ;
        public CSound soundどうよう;
        public CSound soundボーカロイド;

        //private CTexture tx譜面分岐曲バー用;
        //private CTexture tx難易度パネル;
        //private CTexture tx上部ジャンル名;


        //private CTexture txカーソル左;
        //private CTexture txカーソル右;

        //private CTexture tx難易度星;
        //private CTexture tx譜面分岐中央パネル用;

        private long n矢印スクロール用タイマ値;

        private int nCurrentPosition = 0;
        private int nNumOfItems = 0;

        //private string strBoxDefSkinPath = "";
        private Eバー種別 e曲のバー種別を返す(C曲リストノード song)
        {
            if (song != null)
            {
                switch (song.eノード種別)
                {
                    case C曲リストノード.Eノード種別.SCORE:
                    case C曲リストノード.Eノード種別.SCORE_MIDI:
                        return Eバー種別.Score;

                    case C曲リストノード.Eノード種別.BOX:
                    case C曲リストノード.Eノード種別.BACKBOX:
                        return Eバー種別.Box;
                }
            }
            return Eバー種別.Other;
        }
        private C曲リストノード r次の曲(C曲リストノード song)
        {
            if (song == null)
                return null;

            List<C曲リストノード> list = (song.r親ノード != null) ? song.r親ノード.list子リスト : CDTXMania.Songs管理.list曲ルート;

            int index = list.IndexOf(song);

            if (index < 0)
                return null;

            if (index == (list.Count - 1))
                return list[0];

            return list[index + 1];
        }
        private C曲リストノード r前の曲(C曲リストノード song)
        {
            if (song == null)
                return null;

            List<C曲リストノード> list = (song.r親ノード != null) ? song.r親ノード.list子リスト : CDTXMania.Songs管理.list曲ルート;

            int index = list.IndexOf(song);

            if (index < 0)
                return null;

            if (index == 0)
                return list[list.Count - 1];

            return list[index - 1];
        }
        private void tスキル値の描画(int x, int y, int nスキル値)
        {
            if (nスキル値 <= 0 || nスキル値 > 100)      // スキル値 0 ＝ 未プレイ なので表示しない。
                return;

            int color = (nスキル値 == 100) ? 3 : (nスキル値 / 25);

            int n百の位 = nスキル値 / 100;
            int n十の位 = (nスキル値 % 100) / 10;
            int n一の位 = (nスキル値 % 100) % 10;


            // 百の位の描画。

            if (n百の位 > 0)
                this.tスキル値の描画_１桁描画(x, y, n百の位, color);


            // 十の位の描画。

            if (n百の位 != 0 || n十の位 != 0)
                this.tスキル値の描画_１桁描画(x + 7, y, n十の位, color);


            // 一の位の描画。

            this.tスキル値の描画_１桁描画(x + 14, y, n一の位, color);
        }
        private void tスキル値の描画_１桁描画(int x, int y, int n数値, int color)
        {
            //int dx = ( n数値 % 5 ) * 9;
            //int dy = ( n数値 / 5 ) * 12;

            //switch( color )
            //{
            //	case 0:
            //		if( this.txスキル数字 != null )
            //			this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 45 + dx, 24 + dy, 9, 12 ) );
            //		break;

            //	case 1:
            //		if( this.txスキル数字 != null )
            //			this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 45 + dx, dy, 9, 12 ) );
            //		break;

            //	case 2:
            //		if( this.txスキル数字 != null )
            //			this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( dx, 24 + dy, 9, 12 ) );
            //		break;

            //	case 3:
            //		if( this.txスキル数字 != null )
            //			this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( dx, dy, 9, 12 ) );
            //		break;
            //}
        }
        private void tバーの初期化()
        {
            C曲リストノード song = this.r現在選択中の曲;

            if (song == null)
                return;

            for (int i = 0; i < 5; i++)
                song = this.r前の曲(song);

            for (int i = 0; i < 13; i++)
            {
                this.stバー情報[i].strタイトル文字列 = song.strタイトル;
                this.stバー情報[i].strジャンル = song.strジャンル;
                this.stバー情報[i].col文字色 = song.col文字色;
                this.stバー情報[i].ForeColor = song.ForeColor;
                this.stバー情報[i].BackColor = song.BackColor;
                this.stバー情報[i].eバー種別 = this.e曲のバー種別を返す(song);
                this.stバー情報[i].strサブタイトル = song.strサブタイトル;
                this.stバー情報[i].ar難易度 = song.nLevel;

                for (int f = 0; f < 5; f++)
                {
                    if (song.arスコア[f] != null)
                        this.stバー情報[i].b分岐 = song.arスコア[f].譜面情報.b譜面分岐;
                }

                for (int j = 0; j < 3; j++)
                    this.stバー情報[i].nスキル値[j] = (int)song.arスコア[this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(song)].譜面情報.最大スキル[j];

                this.stバー情報[i].ttkタイトル = this.ttk曲名テクスチャを生成する(this.stバー情報[i].strタイトル文字列, this.stバー情報[i].ForeColor, this.stバー情報[i].BackColor);

                song = this.r次の曲(song);
            }

            this.n現在の選択行 = 5;
        }
        private void tバーの描画(int x, int y, Eバー種別 type, bool b選択曲)
        {
            //if( x >= SampleFramework.GameWindowSize.Width || y >= SampleFramework.GameWindowSize.Height )
            //	return;

            //if( b選択曲 )
            //{
            //	#region [ (A) 選択曲の場合 ]
            //	//-----------------
            //	if( this.tx選曲バー[ (int) type ] != null )
            //		this.tx選曲バー[ (int) type ].t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 0, 0, 128, 96 ) );	// ヘサキ
            //	x += 128;

            //	var rc = new Rectangle( 128, 0, 128, 96 );
            //	while( x < 1280 )
            //	{
            //		if( this.tx選曲バー[ (int) type ] != null )
            //			this.tx選曲バー[ (int) type ].t2D描画( CDTXMania.app.Device, x, y, rc );	// 胴体；64pxずつ横につなげていく。
            //		x += 128;
            //	}
            //	//-----------------
            //	#endregion
            //}
            //else
            //{
            //	#region [ (B) その他の場合 ]
            //	//-----------------
            //	if( this.tx曲名バー[ (int) type ] != null )
            //		this.tx曲名バー[ (int) type ].t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 0, 0, 128, 48 ) );		// ヘサキ
            //	x += 128;

            //	var rc = new Rectangle( 0, 48, 128, 48 );
            //	while( x < 1280 )
            //	{
            //		if( this.tx曲名バー[ (int) type ] != null )
            //			this.tx曲名バー[ (int) type ].t2D描画( CDTXMania.app.Device, x, y, rc );	// 胴体；64pxずつ横につなげていく。
            //		x += 128;
            //	}
            //	//-----------------
            //	#endregion
            //}
        }
        private void tジャンル別選択されていない曲バーの描画(int x, int y, string strジャンル)
        {
            if (x >= SampleFramework.GameWindowSize.Width || y >= SampleFramework.GameWindowSize.Height)
                return;

            var rc = new Rectangle(0, 48, 128, 48);
            {
                switch (strジャンル)
                {
                    case "J-POP":
                        #region [ J-POP ]
                        //-----------------
                        switch (r現在選択中の曲.eノード種別)
                        {
                            case C曲リストノード.Eノード種別.SCORE:
                            case C曲リストノード.Eノード種別.BACKBOX:
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[1] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[1].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[1] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[1].t2D描画(CDTXMania.app.Device, x, 88);
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
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[2] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[2].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[2] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[2].t2D描画(CDTXMania.app.Device, x, 88);
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
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[3] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[3].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[3] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[3].t2D描画(CDTXMania.app.Device, x, 88);
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
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[4] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[4].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[4] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[4].t2D描画(CDTXMania.app.Device, x, 88);
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
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[5] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[5].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[5] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[5].t2D描画(CDTXMania.app.Device, x, 88);
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
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[6] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[6].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[6] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[6].t2D描画(CDTXMania.app.Device, x, 88);
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
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[7] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[7].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[7] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[7].t2D描画(CDTXMania.app.Device, x, 88);
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
                                if (CDTXMania.Tx.SongSelect_Bar_Genre[8] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Genre[8].t2D描画(CDTXMania.app.Device, x, y);
                                break;
                            default:
                                if (CDTXMania.Tx.SongSelect_Bar_Box[8] != null)
                                    CDTXMania.Tx.SongSelect_Bar_Box[8].t2D描画(CDTXMania.app.Device, x, 88);
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
                        if (CDTXMania.Tx.SongSelect_Bar_Box[9] != null)
                            CDTXMania.Tx.SongSelect_Bar_Box[9].t2D描画(CDTXMania.app.Device, x, y);
                        //-----------------
                        #endregion
                        break;
                    case "難易度ソート":
                        #region [ 難易度ソート ]
                        //-----------------
                        if (this.tx曲バー_難易度[this.n現在選択中の曲の現在の難易度レベル] != null)
                            this.tx曲バー_難易度[this.n現在選択中の曲の現在の難易度レベル].t2D描画(CDTXMania.app.Device, x, y);
                        //-----------------
                        #endregion
                        break;
                    default:
                        #region [ その他の場合 ]
                        //-----------------
                        if (CDTXMania.Tx.SongSelect_Bar_Genre[0] != null)
                            CDTXMania.Tx.SongSelect_Bar_Genre[0].t2D描画(CDTXMania.app.Device, x, y);
                        //-----------------
                        #endregion
                        break;
                }
            }
        }
        private int nStrジャンルtoNum(string strジャンル)
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
                case "VOCALOID":
                    nGenre = 7;
                    break;
                default:
                    nGenre = 8;
                    break;

            }

            return nGenre;
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

            soundJPOP.t再生位置を先頭に戻す();
            soundどうよう.t再生位置を先頭に戻す();
            soundアニメ.t再生位置を先頭に戻す();
            soundクラシック.t再生位置を先頭に戻す();
            soundゲームミュージック.t再生位置を先頭に戻す();
            soundナムコオリジナル.t再生位置を先頭に戻す();
            soundバラエティ.t再生位置を先頭に戻す();
            soundボーカロイド.t再生位置を先頭に戻す();
        }

        private TitleTextureKey ttk曲名テクスチャを生成する(string str文字, Color forecolor, Color backcolor)
        {
            return new TitleTextureKey(str文字, pfMusicName, forecolor, backcolor, 410);
        }

        private TitleTextureKey ttkサブタイトルテクスチャを生成する(string str文字)
        {
            return new TitleTextureKey(str文字, pfSubtitle, Color.White, Color.Black, 390);
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
                CTexture tx文字テクスチャ = CDTXMania.tテクスチャの生成(bmp, false);
                if (tx文字テクスチャ.szテクスチャサイズ.Height > titleTextureKey.maxHeight)
                {
                    tx文字テクスチャ.vc拡大縮小倍率.Y = (float)(((double)titleTextureKey.maxHeight) / tx文字テクスチャ.szテクスチャサイズ.Height);
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

        private void t曲名バーの生成(int nバー番号, string str曲名, Color forecolor, Color backcolor)
        {
            return;
            if (nバー番号 < 0 || nバー番号 > 12)
                return;

            try
            {
                SizeF sz曲名;

                #region [ 曲名表示に必要となるサイズを取得する。]
                //-----------------
                using (var bmpDummy = new Bitmap(1, 1))
                {
                    var g = Graphics.FromImage(bmpDummy);
                    g.PageUnit = GraphicsUnit.Pixel;
                    sz曲名 = g.MeasureString(str曲名, this.ft曲リスト用フォント);
                }
                //-----------------
                #endregion

                int n最大幅px = 392;
                int height = 25;
                int width = (int)((sz曲名.Width + 2) * 0.5f);
                if (width > (CDTXMania.app.Device.Capabilities.MaxTextureWidth / 2))
                    width = CDTXMania.app.Device.Capabilities.MaxTextureWidth / 2;  // 右端断ち切れ仕方ないよね

                float f拡大率X = (width <= n最大幅px) ? 0.5f : (((float)n最大幅px / (float)width) * 0.5f);   // 長い文字列は横方向に圧縮。

                using (var bmp = new Bitmap(width * 2, height * 2, PixelFormat.Format32bppArgb))        // 2倍（面積4倍）のBitmapを確保。（0.5倍で表示する前提。）
                using (var g = Graphics.FromImage(bmp))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    float y = (((float)bmp.Height) / 2f) - (20);
                    g.DrawString(str曲名, this.ft曲リスト用フォント, new SolidBrush(backcolor), (float)2f, (float)(y + 2f));
                    g.DrawString(str曲名, this.ft曲リスト用フォント, new SolidBrush(forecolor), 0f, y);

                    CDTXMania.t安全にDisposeする(ref this.stバー情報[nバー番号].txタイトル名);

                    this.stバー情報[nバー番号].txタイトル名 = new CTexture(CDTXMania.app.Device, bmp, CDTXMania.TextureFormat);
                    this.stバー情報[nバー番号].txタイトル名.vc拡大縮小倍率 = new Vector3(f拡大率X, 0.5f, 1f);
                }

                //Bitmap bmpSongTitle = new Bitmap(1, 1);
                //bmpSongTitle = pfMusicName.DrawSongNameFont( str曲名, Color.White, Color.Black );
                //this.stバー情報[ nバー番号 ].txタイトル名 = new CTexture( CDTXMania.app.Device, bmpSongTitle, CDTXMania.TextureFormat, false );
            }
            catch (CTextureCreateFailedException e)
            {
                Trace.TraceError(e.ToString());
                Trace.TraceError("曲名テクスチャの作成に失敗しました。[{0}]", str曲名);
                this.stバー情報[nバー番号].txタイトル名 = null;
            }
        }
        private void tアイテム数の描画()
        {
            string s = nCurrentPosition.ToString() + "/" + nNumOfItems.ToString();
            int x = 639 - 8 - 12;
            int y = 362;

            for (int p = s.Length - 1; p >= 0; p--)
            {
                tアイテム数の描画_１桁描画(x, y, s[p]);
                x -= 8;
            }
        }
        private void tアイテム数の描画_１桁描画(int x, int y, char s数値)
        {
            int dx, dy;
            if (s数値 == '/')
            {
                dx = 48;
                dy = 0;
            }
            else
            {
                int n = (int)s数値 - (int)'0';
                dx = (n % 6) * 8;
                dy = (n / 6) * 12;
            }
            //if ( this.txアイテム数数字 != null )
            //{
            //	this.txアイテム数数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( dx, dy, 8, 12 ) );
            //}
        }


        //数字フォント
        private CTexture txレベル数字フォント;
        [StructLayout(LayoutKind.Sequential)]
        private struct STレベル数字
        {
            public char ch;
            public int ptX;
        }
        private STレベル数字[] st小文字位置 = new STレベル数字[10];
        private void t小文字表示(int x, int y, string str)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.st小文字位置.Length; i++)
                {
                    if (this.st小文字位置[i].ch == ch)
                    {
                        Rectangle rectangle = new Rectangle(this.st小文字位置[i].ptX, 0, 22, 28);
                        if (this.txレベル数字フォント != null)
                        {
                            this.txレベル数字フォント.t2D描画(CDTXMania.app.Device, x, y, rectangle);
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
