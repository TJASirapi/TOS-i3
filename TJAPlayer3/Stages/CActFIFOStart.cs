using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace TJAPlayer3
{
	internal class CActFIFOStart : CActivity
	{
        // メソッド

        public void tフェードアウト開始()
        {
            this.mode = EFIFOモード.フェードアウト;

            this.counter = new CCounter(0, 1500, 2, TJAPlayer3.Timer);
        }
        public void tフェードイン開始()
        {
            this.mode = EFIFOモード.フェードイン;
            this.counter = new CCounter(0, 1500, 2, TJAPlayer3.Timer);
        }
        public void tフェードイン完了()     // #25406 2011.6.9 yyagi
        {
            this.counter.n現在の値 = this.counter.n終了値;
        }

        // CActivity 実装

        public override void On非活性化()
        {
            if (!base.b活性化してない)
            {
                this.ctどんちゃんモーション = new CCounter();
                this.counter_Chara = new CCounter();
                this.ctどんちゃんモーション = null;

                this.counter_Chara = null;
                this.counter_FI = new CCounter();
                this.counter_FI = null;
                counter_L_R = new CCounter();
                counter_L_R = null;
                //TJAPlayer3.tテクスチャの解放( ref this.tx幕 );
                base.On非活性化();
            }
        }
        public override void OnManagedリソースの作成()
        {

            if (!base.b活性化してない)
            {

                this.ct虹透明度 = new CCounter(0, 10 - 1, 1, TJAPlayer3.Timer);
                this.ct曲名表示 = new CCounter(1, 1, 1, TJAPlayer3.Timer);
                this.counter_Chara = new CCounter(0, 1000, 2, TJAPlayer3.Timer);
                this.counter_FI = new CCounter(0, 2700, 2, TJAPlayer3.Timer);
                this.counter_L_R = new CCounter(0, 300, 3, TJAPlayer3.Timer);
                //this.tx幕 = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\6_FO.png" ) );
                //	this.tx幕2 = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\6_FI.png" ) );
                base.OnManagedリソースの作成();
            }
        }
        public override int On進行描画()
        {
            if (base.b活性化してない || (this.counter == null))
            {
                return 0;
            }
            this.counter.t進行();
            this.ct曲名表示.t進行();
            counter_L_R.t進行();
            // Size clientSize = TJAPlayer3.app.Window.ClientSize;	// #23510 2010.10.31 yyagi: delete as of no one use this any longer.
            this.counter_Chara.t進行();
            this.counter_FI.t進行();

            if (this.mode == EFIFOモード.フェードアウト)

            {





                if (TJAPlayer3.Tx.SongLoading_FadeOut != null)
                {

                    int y = this.counter.n現在の値 >= 1100 ? 1100 : this.counter.n現在の値;
                    TJAPlayer3.Tx.SongLoading_FadeOut.t2D描画(TJAPlayer3.app.Device, 0, -3 * y + 3188);

                    if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Dan)
                    {

                    }
                    else
                    {
                        if (TJAPlayer3.Tx.SongLoading_Chara != null)
                        {
                            int y2 = this.counter.n現在の値 >= 1300 ? 1300 : this.counter.n現在の値;
                            int yL = this.counter.n現在の値 >= 1000 ? 1000 : this.counter.n現在の値;
                            //       TJAPlayer3.Tx.SongLoading_Chara_Chara_L.t2D描画(TJAPlayer3.app.Device, 0 * -400, -6 * y2 + 7800);
                            //       TJAPlayer3.Tx.SongLoading_Chara_Chara_R.t2D描画(TJAPlayer3.app.Device, 0, -6 * y2 + 7800);
                            TJAPlayer3.Tx.SongLoading_Chara.t2D描画(TJAPlayer3.app.Device, 0, -6 * y2 + 7800);
                        }
                    }

                }

            }
            else
            {
                if (TJAPlayer3.Tx.SongLoading_FadeIn != null)
                {
                    //  int y = this.counter_FI.n現在の値 >= 1000 ? 1000 : this.counter_FI.n現在の値;
                    TJAPlayer3.Tx.SongLoading_FadeIn.t2D描画(TJAPlayer3.app.Device, 0, (-this.counter_Chara.n現在の値 * this.counter_Chara.n現在の値) / 180);

                    if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Dan)
                    {

                    }
                    else
                    {
                        if (TJAPlayer3.Tx.SongLoading_Chara != null)
                        {
                            //     TJAPlayer3.Tx.SongLoading_Chara_Chara_L.t2D描画(TJAPlayer3.app.Device, 0, (-this.counter_Chara.n現在の値 * this.counter_Chara.n現在の値) / 180);
                            //    TJAPlayer3.Tx.SongLoading_Chara_Chara_R.t2D描画(TJAPlayer3.app.Device, 0, (-this.counter_Chara.n現在の値 * this.counter_Chara.n現在の値) / 180);
                            TJAPlayer3.Tx.SongLoading_Chara.t2D描画(TJAPlayer3.app.Device, 0, (-this.counter_Chara.n現在の値 * this.counter_Chara.n現在の値) / 180);
                        }
                    }

                    //TJAPlayer3.act文字コンソール.tPrint( 0, 16, C文字コンソール.Eフォント種別.灰, C変換.nParsentTo255( ( this.ct曲名表示.n現在の値 / 30.0 ) ).ToString() );

                }
            }

            if (this.mode == EFIFOモード.フェードアウト)
            {
                if (this.counter.n現在の値 != 1500)
                {
                    return 0;
                }
            }
            else if (this.mode == EFIFOモード.フェードイン)
            {
                if (this.counter.n現在の値 != 1500)
                {
                    return 0;
                }
            }
            return 1;
        }


        // その他

        #region [ private ]
        //-----------------
        private CCounter counter_Chara;
        private CCounter counter_FI;
        private CCounter counter;
        private CCounter ct待機;
        private EFIFOモード mode;
        protected CCounter ct虹透明度;
        public CCounter ctどんちゃんモーション;
        private CCounter ct曲名表示;
        public CCounter counter_L_R;
        //private CTexture tx幕;
        //private CTexture tx幕2;
        //private CTexture tx幕;
        //private CTexture tx幕2;
        //-----------------
        #endregion
    }
}
