using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.IO;
using SlimDX;
using FDK;

namespace TJAPlayer3
{
    internal class CActSelectQuickConfig : CActSelectPopupMenu
    {
        private readonly string QuickCfgTitle = "";
        // コンストラクタ

        public CActSelectQuickConfig()
        {
            CActSelectQuickConfigMain();
        }

        private void CActSelectQuickConfigMain()
        {
            /*
            •Target: Drums/Guitar/Bass 
            •Auto Mode: All ON/All OFF/CUSTOM 
            •Auto Lane: 
            •Scroll Speed: 
            •Play Speed: 
            •Risky: 
            •Hidden/Sudden: None/Hidden/Sudden/Both 
            •Conf SET: SET-1/SET-2/SET-3 
            •More... 
            •EXIT 
            */
            lci = new List<List<List<CItemBase>>>();                                    // この画面に来る度に、メニューを作り直す。
            for (int nConfSet = 0; nConfSet < 3; nConfSet++)
            {
                lci.Add(new List<List<CItemBase>>());                                   // ConfSet用の3つ分の枠。
                for (int nInst = 0; nInst < 3; nInst++)
                {
                    lci[nConfSet].Add(null);                                        // Drum/Guitar/Bassで3つ分、枠を作っておく
                    lci[nConfSet][nInst] = MakeListCItemBase(nConfSet, nInst);
                }
            }
            base.Initialize(lci[nCurrentConfigSet][0], true, QuickCfgTitle, 0); // ConfSet=0, nInst=Drums
        }

        private List<CItemBase> MakeListCItemBase(int nConfigSet, int nInst)
        {
            List<CItemBase> l = new List<CItemBase>();

            #region [ 共通 Target/AutoMode/AutoLane ]
            #endregion
            #region [ 個別 ScrollSpeed ]
            l.Add(new CItemList(nameof(TJAPlayer3.ConfigIni.真打), CItemBase.Eパネル種別.通常, TJAPlayer3.ConfigIni.真打 ? 1 : 0, "", "", new string[] { "しない", "する" }));

            l.Add(new CItemInteger("はやさ", 0, 1999, TJAPlayer3.ConfigIni.n譜面スクロール速度[nInst],
                "演奏時のドラム譜面のスクロールの\n" +
                "速度を指定します。\n" +
                "x0.5 ～ x1000.0 を指定可能です。",
                "To change the scroll speed for the\n" +
                "drums lanes.\n" +
                "You can set it from x0.5 to x1000.0.\n" +
                "(ScrollSpeed=x0.5 means half speed)"));
            #endregion
            #region [ 共通 Dark/Risky/PlaySpeed ]
            l.Add(new CItemInteger("スピード", 5, 40, TJAPlayer3.ConfigIni.n演奏速度,
                "曲の演奏速度を、速くしたり遅くした\n" +
                "りすることができます。\n" +
                "（※一部のサウンドカードでは正しく\n" +
                "再生できない可能性があります。）",
                "It changes the song speed.\n" +
                "For example, you can play in half\n" +
                " speed by setting PlaySpeed = 0.500\n" +
                " for your practice.\n" +
                "Note: It also changes the songs' pitch."));
            #endregion
            #region [ 個別 Sud/Hid ]
            l.Add(new CItemList("ドロン", CItemBase.Eパネル種別.通常, (int)TJAPlayer3.ConfigIni.eSTEALTH,
                "",
                new string[] { "なし", "ドロン", "ステルス" }));

            l.Add(new CItemList("ランダム", CItemBase.Eパネル種別.通常, (int)TJAPlayer3.ConfigIni.eRandom.Taiko,
                "いわゆるランダム。\n  RANDOM: ちょっと変わる\n  MIRROR: あべこべ \n  SUPER: そこそこヤバい\n  HYPER: 結構ヤバい\nなお、実装は適当な模様",
                "Guitar chips come randomly.\n\n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
                new string[] { "なし", "ちょこっと", "あべこべ", "きまぐれ", "でたらめ" }));

            l.Add(new CItemList("ゲーム", CItemBase.Eパネル種別.通常, (int)TJAPlayer3.ConfigIni.eGameMode,
                "ゲームモード\n" +
                "TYPE-A: 完走!叩ききりまショー!\n" +
                "TYPE-B: 完走!叩ききりまショー!(激辛)\n" +
                " \n",
                " \n" +
                " \n" +
                " ",
                new string[] { "なし", "完走!", "完走!激辛" }));

            #endregion
            #region [ 共通 SET切り替え/More/Return ]
            l.Add(new CSwitchItemList("戻る", CItemBase.Eパネル種別.通常, 0, "", "", new string[] { "", "" }));
            #endregion

            return l;
        }

        // メソッド
        public override void tActivatePopupMenu(E楽器パート einst)
        {
            this.CActSelectQuickConfigMain();
            base.tActivatePopupMenu(einst);
        }
        //public void tDeativatePopupMenu()
        //{
        //	base.tDeativatePopupMenu();
        //}

        public override void t進行描画sub()
        {

        }

        public override void tEnter押下Main(int nSortOrder)
        {
            
            switch (n現在の選択行)
            {
                case (int)EOrder.ShinuchiMode:
                    TJAPlayer3.ConfigIni.真打 = !TJAPlayer3.ConfigIni.真打;
                    if(TJAPlayer3.ConfigIni.真打 == true)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を停止する();
                    }
                    break;
                    
                case (int)EOrder.ScrollSpeed:
                    TJAPlayer3.ConfigIni.n譜面スクロール速度[nCurrentTarget] = (int)GetObj現在値((int)EOrder.ScrollSpeed);
                    if (TJAPlayer3.ConfigIni.n譜面スクロール速度.Drums == 3)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を停止する();
                    }
                    if (TJAPlayer3.ConfigIni.n譜面スクロール速度.Drums == 5)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を停止する();
                    }
                    if (TJAPlayer3.ConfigIni.n譜面スクロール速度.Drums == 7)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を停止する();
                    }
                        break;

                case (int)EOrder.PlaySpeed:
                    TJAPlayer3.ConfigIni.n演奏速度 = (int)GetObj現在値((int)EOrder.PlaySpeed);
                    break;
                case (int)EOrder.Random:
                    TJAPlayer3.ConfigIni.eRandom.Taiko = (Eランダムモード)GetIndex((int)EOrder.Random);
                    if(TJAPlayer3.ConfigIni.eRandom.Taiko == Eランダムモード.MIRROR)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を停止する();
                    }
                    if (TJAPlayer3.ConfigIni.eRandom.Taiko == Eランダムモード.SUPERRANDOM)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を停止する();
                    }
                    if(TJAPlayer3.ConfigIni.eRandom.Taiko == Eランダムモード.HYPERRANDOM)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.soundでたらめ.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を停止する();
                    }
                        break;
                case (int)EOrder.Stealth:
                    TJAPlayer3.ConfigIni.eSTEALTH = (Eステルスモード)GetIndex((int)EOrder.Stealth);
                    if(TJAPlayer3.ConfigIni.eSTEALTH == Eステルスモード.DORON)
                    {
                        TJAPlayer3.stage選曲.act難易度選択画面.soundドロン.t再生を開始する();

                        TJAPlayer3.stage選曲.act難易度選択画面.sound演奏オプション.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound真打.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound倍速.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound3倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.sound4倍.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundあべこべ.t再生を停止する();
                        TJAPlayer3.stage選曲.act難易度選択画面.soundきまぐれ.t再生を停止する();
                    }
                    break;
                case (int)EOrder.GameMode:
                    EGame game = EGame.OFF;
                    switch ((int)GetIndex((int)EOrder.GameMode))
                    {
                        case 0: game = EGame.OFF; break;
                        case 1: game = EGame.完走叩ききりまショー; break;
                        case 2: game = EGame.完走叩ききりまショー激辛; break;
                    }
                    TJAPlayer3.ConfigIni.eGameMode = game;
                    break;

                case (int)EOrder.Return:
                    SetAutoParameters();            // 簡易CONFIGメニュー脱出に伴い、簡易CONFIG内のAUTOの設定をConfigIniクラスに反映する
                    this.tDeativatePopupMenu();
                    break;
                default:
                    break;
            }
        }

        public override void tCancel()
        {
            SetAutoParameters();
            // Autoの設定値保持のロジックを書くこと！
            // (Autoのパラメータ切り替え時は実際に値設定していないため、キャンセルまたはRetern, More...時に値設定する必要有り)
        }

        /// <summary>
        /// 1つの値を、全targetに適用する。RiskyやDarkなど、全tatgetで共通の設定となるもので使う。
        /// </summary>
        /// <param name="order">設定項目リストの順番</param>
        /// <param name="index">設定値(index)</param>
        private void SetValueToAllTarget(int order, int index)
        {
            for (int i = 0; i < 3; i++)
            {
                lci[nCurrentConfigSet][i][order].SetIndex(index);
            }
        }


        /// <summary>
        /// ConfigIni.bAutoPlayに簡易CONFIGの状態を反映する
        /// </summary>
        private void SetAutoParameters()
        {
            for (int target = 0; target < 3; target++)
            {
                int[] pa = { (int)Eレーン.LC, (int)Eレーン.GtR, (int)Eレーン.BsR };
                int start = pa[target];
            }
        }

        // CActivity 実装

        public override void On活性化()
        {
            this.ft表示用フォント = new Font("Arial", 26f, FontStyle.Bold, GraphicsUnit.Pixel);
            base.On活性化();
            this.bGotoDetailConfig = false;
        }
        public override void On非活性化()
        {
            if (this.ft表示用フォント != null)
            {
                this.ft表示用フォント.Dispose();
                this.ft表示用フォント = null;
            }
            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                //string pathパネル本体 = CSkin.Path( @"Graphics\ScreenSelect popup auto settings.png" );
                //if ( File.Exists( pathパネル本体 ) )
                //{
                //	this.txパネル本体 = CDTXMania.tテクスチャの生成( pathパネル本体, true );
                //}

                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                //CDTXMania.tテクスチャの解放( ref this.txパネル本体 );
                TJAPlayer3.tテクスチャの解放(ref this.tx文字列パネル);
                base.OnManagedリソースの解放();
            }
        }

        #region [ private ]
        //-----------------
        private int nCurrentTarget = 0;
        private int nCurrentConfigSet = 0;
        private List<List<List<CItemBase>>> lci;        // DrGtBs, ConfSet, 選択肢一覧。都合、3次のListとなる。
        private enum EOrder : int
        {
            ShinuchiMode,
            ScrollSpeed,
            PlaySpeed,
            Stealth,
            Random,
            GameMode,
            Return, END,
            Default = 99
        };

        private Font ft表示用フォント;
        //private CTexture txパネル本体;
        private CTexture tx文字列パネル;
        private CTexture tx説明文1;
        //-----------------
        #endregion
    }


}
