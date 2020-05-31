using System;
using FDK;

namespace TJAPlayer3
{
    internal class CAct演奏Drums背景 : CActivity
    {
        // 本家っぽい背景を表示させるメソッド。
        //
        // 拡張性とかないんで。はい、ヨロシクゥ!
        //
        public CAct演奏Drums背景()
        {
            base.b活性化してない = true;
        }

        public void tFadeIn(int nPlayer)
        {
            this.ct上背景クリアインタイマー[nPlayer] = new CCounter(0, 100, 2, TJAPlayer3.Timer);
        }

        public void tMissIn(int nPlayer)
        {
            this.ct上背景Missタイマー[nPlayer] = new CCounter(0, 100, 6, TJAPlayer3.Timer);
        }

        public void ClearIn(int nPlayer)
        {
            this.ct上背景クリアインタイマー[nPlayer] = new CCounter(0, 100, 2, TJAPlayer3.Timer)
            {
                n現在の値 = 0
            };
            this.ct上背景FIFOタイマー = new CCounter(0, 100, 2, TJAPlayer3.Timer)
            {
                n現在の値 = 0
            };
        }

        public override void On活性化()
        {
            base.On活性化();
        }

        public override void On非活性化()
        {
            TJAPlayer3.t安全にDisposeする(ref this.ct上背景FIFOタイマー);
            for (int i = 0; i < 2; i++)
            {
                TJAPlayer3.t安全にDisposeする(ref ct上背景スクロール用タイマー[i]);
                TJAPlayer3.t安全にDisposeする(ref ct上背景2ndスクロール用タイマー[i]);
                TJAPlayer3.t安全にDisposeする(ref ct上背景2nd下方向移動用タイマー[i]);
                TJAPlayer3.t安全にDisposeする(ref ct上背景3rdスクロール用タイマー[i]);
                TJAPlayer3.t安全にDisposeする(ref ct上背景3rd上下移動用タイマー[i]);
            }
            TJAPlayer3.t安全にDisposeする(ref ct上背景スクロール用タイマー_Dan);
            TJAPlayer3.t安全にDisposeする(ref ct上背景2ndスクロール用タイマー_Dan);
            TJAPlayer3.t安全にDisposeする(ref ct上背景2nd下方向移動用タイマー_Dan);
            TJAPlayer3.t安全にDisposeする(ref ct上背景3rdスクロール用タイマー_Dan);
            TJAPlayer3.t安全にDisposeする(ref ct上背景3rd上下移動用タイマー_Dan);
            TJAPlayer3.t安全にDisposeする(ref this.ct下背景スクロール用タイマー1);
            TJAPlayer3.t安全にDisposeする(ref this.ct下背景2ndスクロール用タイマー);
            TJAPlayer3.t安全にDisposeする(ref this.ct下背景3rdスクロール用タイマー);
            TJAPlayer3.t安全にDisposeする(ref this.ct下背景3rd波モーション用タイマー);
            TJAPlayer3.t安全にDisposeする(ref this.ct亀スクロール用タイマー);
            TJAPlayer3.t安全にDisposeする(ref this.ct亀パターン用タイマー);
            TJAPlayer3.t安全にDisposeする(ref this.ct桜X移動用タイマー1);
            TJAPlayer3.t安全にDisposeする(ref this.ct桜Y移動用タイマー1);
            TJAPlayer3.t安全にDisposeする(ref this.ct桜X移動用タイマー2);
            TJAPlayer3.t安全にDisposeする(ref this.ct桜Y移動用タイマー2);
            TJAPlayer3.t安全にDisposeする(ref this.ct桜X移動用タイマー3);
            TJAPlayer3.t安全にDisposeする(ref this.ct桜Y移動用タイマー3);
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            this.ct上背景スクロール用タイマー = new CCounter[2];
            this.ct上背景2ndスクロール用タイマー = new CCounter[2];
            this.ct上背景2nd下方向移動用タイマー = new CCounter[2];
            this.ct上背景3rdスクロール用タイマー = new CCounter[2];
            this.ct上背景3rd上下移動用タイマー = new CCounter[2];
            this.ct上背景クリアインタイマー = new CCounter[2];
            this.ct上背景Missタイマー = new CCounter[2];
            for (int i = 0; i < 2; i++)
            {
                if (TJAPlayer3.Tx.Background_Up[i] != null)
                {
                    this.ct上背景スクロール用タイマー[i] = new CCounter(1, TJAPlayer3.Tx.Background_Up[i].szテクスチャサイズ.Width, 13, TJAPlayer3.Timer);
                    this.ct上背景クリアインタイマー[i] = new CCounter();
                }

                if (TJAPlayer3.Tx.Background_Up_2nd[i] != null)
                {
                    this.ct上背景2ndスクロール用タイマー[i] = new CCounter(1, TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Width, 3, TJAPlayer3.Timer);
                    this.ct上背景2nd下方向移動用タイマー[i] = new CCounter(1, 300, 5, TJAPlayer3.Timer);
                    this.ct上背景クリアインタイマー[i] = new CCounter();
                }

                if (TJAPlayer3.Tx.Background_Up_3rd[i] != null)
                {
                    this.ct上背景3rdスクロール用タイマー[i] = new CCounter(1, TJAPlayer3.Tx.Background_Up_3rd[i].szテクスチャサイズ.Width, 13, TJAPlayer3.Timer);
                    this.ct上背景3rd上下移動用タイマー[i] = new CCounter(1, 50, 50, TJAPlayer3.Timer);
                    this.ct上背景クリアインタイマー[i] = new CCounter();
                }
            }

            if (TJAPlayer3.Tx.Background_Up_Dan != null)
            {
                this.ct上背景スクロール用タイマー_Dan = new CCounter(1, TJAPlayer3.Tx.Background_Up_Dan.szテクスチャサイズ.Width, 7, TJAPlayer3.Timer);
            }

            if (TJAPlayer3.Tx.Background_Up_2nd_Dan != null)
            {
                this.ct上背景2ndスクロール用タイマー_Dan = new CCounter(1, TJAPlayer3.Tx.Background_Up_2nd_Dan.szテクスチャサイズ.Width, 3, TJAPlayer3.Timer);
                this.ct上背景2nd下方向移動用タイマー_Dan = new CCounter(1, 300, 5, TJAPlayer3.Timer);
            }

            if (TJAPlayer3.Tx.Background_Up_3rd_Dan != null)
            {
                this.ct上背景3rdスクロール用タイマー_Dan = new CCounter(1, TJAPlayer3.Tx.Background_Up_3rd_Dan.szテクスチャサイズ.Width, 13, TJAPlayer3.Timer);
                this.ct上背景3rd上下移動用タイマー_Dan = new CCounter(1, 25, 40, TJAPlayer3.Timer);
            }

            if (TJAPlayer3.Tx.Background_Down_Scroll != null)
                this.ct下背景スクロール用タイマー1 = new CCounter(1, TJAPlayer3.Tx.Background_Down_Scroll.szテクスチャサイズ.Width, 4, TJAPlayer3.Timer);

            if (TJAPlayer3.Tx.Background_Down_Clear_2nd != null)
                this.ct下背景2ndスクロール用タイマー = new CCounter(1, TJAPlayer3.Tx.Background_Down_Clear_2nd.szテクスチャサイズ.Height, 10, TJAPlayer3.Timer);

            if (TJAPlayer3.Tx.Background_Down_Clear_3rd != null)
            {
                this.ct下背景3rdスクロール用タイマー = new CCounter(1, 750, 5, TJAPlayer3.Timer);
                this.ct下背景3rd波モーション用タイマー = new CCounter(1, 750, 5, TJAPlayer3.Timer);
            }

            if (TJAPlayer3.Tx.Background_Down_kame != null)
            {
                this.ct亀スクロール用タイマー = new CCounter(1, 1400, 4, TJAPlayer3.Timer);
                this.ct亀パターン用タイマー = new CCounter(0, 5, 170, TJAPlayer3.Timer);
            }

            if (TJAPlayer3.Tx.Background_Down_sakura != null)
            {
                this.ct桜X移動用タイマー1 = new CCounter(0, 166, 15, TJAPlayer3.Timer);
                this.ct桜Y移動用タイマー1 = new CCounter(0, 500, 5, TJAPlayer3.Timer);
                this.ct桜X移動用タイマー2 = new CCounter(0, 250, 10, TJAPlayer3.Timer);
                this.ct桜Y移動用タイマー2 = new CCounter(0, 500, 5, TJAPlayer3.Timer);
                this.ct桜X移動用タイマー3 = new CCounter(0, 333, 15, TJAPlayer3.Timer);
                this.ct桜Y移動用タイマー3 = new CCounter(0, 500, 10, TJAPlayer3.Timer);
            }

            this.ct上背景FIFOタイマー = new CCounter();
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            this.ct上背景FIFOタイマー?.t進行();

            for (int i = 0; i < 2; i++)
            {
                this.ct上背景クリアインタイマー[i]?.t進行();
            }

            for (int i = 0; i < 2; i++)
            {
                this.ct上背景スクロール用タイマー[i]?.t進行Loop();
            }

            for (int i = 0; i < 2; i++)
            {
                this.ct上背景2ndスクロール用タイマー[i]?.t進行Loop();
            }

            for (int i = 0; i < 2; i++)
            {
                this.ct上背景2nd下方向移動用タイマー[i]?.t進行Loop();
            }

            for (int i = 0; i < 2; i++)
            {
                this.ct上背景3rdスクロール用タイマー[i]?.t進行Loop();
            }

            for (int i = 0; i < 2; i++)
            {
                this.ct上背景3rd上下移動用タイマー[i]?.t進行Loop();
            }
            this.ct上背景スクロール用タイマー_Dan?.t進行Loop();

            this.ct上背景2ndスクロール用タイマー_Dan?.t進行Loop();

            this.ct上背景2nd下方向移動用タイマー_Dan?.t進行Loop();

            this.ct上背景3rdスクロール用タイマー_Dan?.t進行Loop();

            this.ct上背景3rd上下移動用タイマー_Dan?.t進行Loop();

            this.ct下背景スクロール用タイマー1?.t進行Loop();

            this.ct下背景2ndスクロール用タイマー?.t進行Loop();

            this.ct下背景3rdスクロール用タイマー?.t進行Loop();

            this.ct下背景3rd波モーション用タイマー?.t進行Loop();

            this.ct亀スクロール用タイマー?.t進行Loop();

            this.ct亀パターン用タイマー?.t進行Loop();

            this.ct桜X移動用タイマー1?.t進行Loop();

            this.ct桜Y移動用タイマー1?.t進行Loop();

            this.ct桜X移動用タイマー2?.t進行Loop();

            this.ct桜Y移動用タイマー2?.t進行Loop();

            this.ct桜X移動用タイマー3?.t進行Loop();

            this.ct桜Y移動用タイマー3?.t進行Loop();

            for (int i = 0; i < 2; i++)
            {
                this.ct上背景Missタイマー[i]?.t進行();
            }

            #region 1P-2P-上背景
            if (TJAPlayer3.stage選曲.n確定された曲の難易度 == (int)Difficulty.Dan)
            {
                TJAPlayer3.Tx.Background_Up_Base_Dan?.t2D描画(TJAPlayer3.app.Device, 0, TJAPlayer3.Skin.Background_Scroll_Y[0]);

                if (this.ct上背景スクロール用タイマー_Dan != null)
                {
                    double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_Dan.szテクスチャサイズ.Width;
                    // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                    int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                    TJAPlayer3.Tx.Background_Up_Dan?.t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー_Dan.n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[0]);
                    for (int l = 1; l < ForLoop + 1; l++)
                    {
                        TJAPlayer3.Tx.Background_Up_Dan?.t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_Dan.szテクスチャサイズ.Width) - this.ct上背景スクロール用タイマー_Dan.n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[0]);
                    }
                }

                TJAPlayer3.Tx.Background_Up_Base_2nd_Dan?.t2D描画(TJAPlayer3.app.Device, 0, TJAPlayer3.Skin.Background_Scroll_Y[0]);

                if (this.ct上背景2ndスクロール用タイマー_Dan != null && this.ct上背景2nd下方向移動用タイマー_Dan != null)
                {
                    double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_2nd_Dan.szテクスチャサイズ.Width;
                    // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                    int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                    TJAPlayer3.Tx.Background_Up_2nd_Dan?.t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景2ndスクロール用タイマー_Dan.n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[0] - 100 + this.ct上背景2nd下方向移動用タイマー_Dan.n現在の値);
                    for (int l = 1; l < ForLoop + 1; l++)
                    {
                        TJAPlayer3.Tx.Background_Up_2nd_Dan?.t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_2nd_Dan.szテクスチャサイズ.Width) - this.ct上背景2ndスクロール用タイマー_Dan.n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[0] - 100 + this.ct上背景2nd下方向移動用タイマー_Dan.n現在の値);
                    }
                }

                if (this.ct上背景3rdスクロール用タイマー_Dan != null && this.ct上背景3rd上下移動用タイマー_Dan != null)
                {
                    int ThirdMotion;

                    if (this.ct上背景3rd上下移動用タイマー_Dan.n現在の値 < 25)
                    {
                        ThirdMotion = TJAPlayer3.Skin.Background_Scroll_Y[0] - 10 - this.ct上背景3rd上下移動用タイマー_Dan.n現在の値;
                    }
                    else
                    {
                        ThirdMotion = TJAPlayer3.Skin.Background_Scroll_Y[0] - 60 + this.ct上背景3rd上下移動用タイマー_Dan.n現在の値;
                    }

                    double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_3rd_Dan.szテクスチャサイズ.Width;
                    // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                    int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                    TJAPlayer3.Tx.Background_Up_3rd_Dan?.t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景3rdスクロール用タイマー_Dan.n現在の値, ThirdMotion);
                    for (int l = 1; l < ForLoop + 1; l++)
                    {
                        TJAPlayer3.Tx.Background_Up_3rd_Dan?.t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_3rd_Dan.szテクスチャサイズ.Width) - this.ct上背景3rdスクロール用タイマー_Dan.n現在の値, ThirdMotion);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    if (this.ct上背景スクロール用タイマー[i] != null)
                    {

                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Up[i].szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;
                        //int nループ幅 = 328;
                        if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                        {
                            TJAPlayer3.Tx.Background_Up[3].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i]);
                        }
                        else
                        {
                            TJAPlayer3.Tx.Background_Up[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i]);
                        }
                        for (int l = 1; l < ForLoop + 1; l++)
                        {

                            if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                            {
                                TJAPlayer3.Tx.Background_Up[3].t2D描画(TJAPlayer3.app.Device, 0 + (TJAPlayer3.Tx.Background_Up[3].szテクスチャサイズ.Width * l) - this.ct上背景スクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i]);
                            }
                            else
                            {
                                TJAPlayer3.Tx.Background_Up[i].t2D描画(TJAPlayer3.app.Device, 0 + (TJAPlayer3.Tx.Background_Up[i].szテクスチャサイズ.Width * l) - this.ct上背景スクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i]);
                            }


                        }
                    }



                    if (this.ct上背景2ndスクロール用タイマー[i] != null && this.ct上背景2nd下方向移動用タイマー[i] != null)
                    {
                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;
                        //int nループ幅 = 328;
                        if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                        {
                            TJAPlayer3.Tx.Background_Up_2nd[2].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景2ndスクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i] - 100 + this.ct上背景2nd下方向移動用タイマー[i].n現在の値);
                        }
                        else
                        {
                            TJAPlayer3.Tx.Background_Up_2nd[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景2ndスクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i] - 100 + this.ct上背景2nd下方向移動用タイマー[i].n現在の値);
                        }
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                            {
                                TJAPlayer3.Tx.Background_Up_2nd[2].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_2nd[2].szテクスチャサイズ.Width) - this.ct上背景2ndスクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i] - 100 + this.ct上背景2nd下方向移動用タイマー[i].n現在の値);
                            }
                            else
                            {
                                TJAPlayer3.Tx.Background_Up_2nd[i].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_2nd[i].szテクスチャサイズ.Width) - this.ct上背景2ndスクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i] - 100 + this.ct上背景2nd下方向移動用タイマー[i].n現在の値);
                            }
                        }

                    }
                    if (this.ct上背景3rdスクロール用タイマー[i] != null && this.ct上背景3rd上下移動用タイマー[i] != null)
                    {
                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_3rd[i].szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;
                        //int nループ幅 = 328;

                        int y = TJAPlayer3.Skin.Background_Scroll_Y[i] + Math.Abs(ct上背景3rd上下移動用タイマー[i].n現在の値 - 25) - 35;

                        if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                        {
                            TJAPlayer3.Tx.Background_Up_3rd[2].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景3rdスクロール用タイマー[i].n現在の値, y);
                        }
                        else
                        {
                            TJAPlayer3.Tx.Background_Up_3rd[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景3rdスクロール用タイマー[i].n現在の値, y);
                        }
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                            {
                                TJAPlayer3.Tx.Background_Up_3rd[2].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_3rd[2].szテクスチャサイズ.Width) - this.ct上背景3rdスクロール用タイマー[i].n現在の値, y);
                            }
                            else
                            {
                                TJAPlayer3.Tx.Background_Up_3rd[i].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_3rd[i].szテクスチャサイズ.Width) - this.ct上背景3rdスクロール用タイマー[i].n現在の値, y);
                            }
                        }
                    }
                    if (this.ct上背景スクロール用タイマー[i] != null)
                    {
                        if (TJAPlayer3.stage演奏ドラム画面.bIsAlreadyCleared[i])
                            TJAPlayer3.Tx.Background_Up_Clear[i].n透明度 = ((this.ct上背景クリアインタイマー[i].n現在の値 * 0xff) / 100);
                        else
                            TJAPlayer3.Tx.Background_Up_Clear[i].n透明度 = 0;

                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_Clear[i].szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                        TJAPlayer3.Tx.Background_Up_Clear[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景スクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i]);
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            TJAPlayer3.Tx.Background_Up_Clear[i].t2D描画(TJAPlayer3.app.Device, (l * TJAPlayer3.Tx.Background_Up_Clear[i].szテクスチャサイズ.Width) - this.ct上背景スクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i]);
                        }
                    }
                    if (this.ct上背景2ndスクロール用タイマー[i] != null)
                    {
                        if (TJAPlayer3.stage演奏ドラム画面.bIsAlreadyCleared[i])
                            TJAPlayer3.Tx.Background_Up_Clear_2nd[i].n透明度 = ((this.ct上背景クリアインタイマー[i].n現在の値 * 0xff) / 100);
                        else
                            TJAPlayer3.Tx.Background_Up_Clear_2nd[i].n透明度 = 0;

                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_Clear_2nd[i].szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                        TJAPlayer3.Tx.Background_Up_Clear_2nd[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景2ndスクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i] - 100 + this.ct上背景2nd下方向移動用タイマー[i].n現在の値);
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            TJAPlayer3.Tx.Background_Up_Clear_2nd[i].t2D描画(TJAPlayer3.app.Device, (l * TJAPlayer3.Tx.Background_Up_Clear_2nd[i].szテクスチャサイズ.Width) - this.ct上背景2ndスクロール用タイマー[i].n現在の値, TJAPlayer3.Skin.Background_Scroll_Y[i] - 100 + this.ct上背景2nd下方向移動用タイマー[i].n現在の値);
                        }
                    }
                    if (this.ct上背景3rdスクロール用タイマー[i] != null && this.ct上背景3rd上下移動用タイマー[i] != null)
                    {

                        if (TJAPlayer3.stage演奏ドラム画面.bIsAlreadyCleared[i])
                            TJAPlayer3.Tx.Background_Up_Clear_3rd[i].n透明度 = ((this.ct上背景クリアインタイマー[i].n現在の値 * 0xff) / 100);
                        else
                            TJAPlayer3.Tx.Background_Up_Clear_3rd[i].n透明度 = 0;

                        int testmotion;

                        if (this.ct上背景3rd上下移動用タイマー[i].n現在の値 < 27)
                        {
                            testmotion = TJAPlayer3.Skin.Background_Scroll_Y[i] - 10 - this.ct上背景3rd上下移動用タイマー[i].n現在の値;
                        }
                        else
                        {
                            testmotion = TJAPlayer3.Skin.Background_Scroll_Y[i] - 64 + this.ct上背景3rd上下移動用タイマー[i].n現在の値;
                        }

                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Up_Clear_3rd[i].szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;
                        //int nループ幅 = 328;

                        TJAPlayer3.Tx.Background_Up_Clear_3rd[i].t2D描画(TJAPlayer3.app.Device, 0 - this.ct上背景3rdスクロール用タイマー[i].n現在の値, testmotion);
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            TJAPlayer3.Tx.Background_Up_Clear_3rd[i].t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Up_Clear_3rd[i].szテクスチャサイズ.Width) - this.ct上背景3rdスクロール用タイマー[i].n現在の値, testmotion);
                        }

                    }
                }
            }

            #endregion
            #region 1P-下背景
            if (!TJAPlayer3.stage演奏ドラム画面.bDoublePlay)
            {
                {
                    if (TJAPlayer3.Tx.Background_Down != null)
                    {
                        TJAPlayer3.Tx.Background_Down.t2D描画(TJAPlayer3.app.Device, 0, 360);
                    }

                    if (TJAPlayer3.Tx.Background_Down_kame[this.ct亀パターン用タイマー.n現在の値] != null)
                    {
                        TJAPlayer3.Tx.Background_Down_kame[this.ct亀パターン用タイマー.n現在の値].t2D描画(TJAPlayer3.app.Device, 1300 - this.ct亀スクロール用タイマー.n現在の値, 550);
                    }

                    if (TJAPlayer3.Tx.Background_Down_2nd != null)
                    {
                        TJAPlayer3.Tx.Background_Down_2nd.t2D描画(TJAPlayer3.app.Device, 0, 360);
                    }

                    #region 桜モーション
                    if (TJAPlayer3.Tx.Background_Down_sakura != null)
                    {
                        #region 没モーション
                        /*
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 100 + this.ct桜モーション用タイマー.n現在の値 + ((float)Math.Sin((float)this.ct桜モーション用タイマー.n現在の値 * (Math.PI / 10)) * 100), 350 + this.ct桜モーション用タイマー.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 900 - this.ct桜モーション用タイマー.n現在の値 + ((float)Math.Sin((float)this.ct桜モーション用タイマー.n現在の値 * (Math.PI / 10)) * 100), 400 + this.ct桜Y移動用タイマー1.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 1300 - this.ct桜モーション用タイマー.n現在の値 + ((float)Math.Sin((float)this.ct桜モーション用タイマー.n現在の値 * (Math.PI / 10)) * 100), 300 + this.ct桜モーション用タイマー.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 0 + this.ct桜モーション用タイマー.n現在の値 + ((float)Math.Sin((float)this.ct桜モーション用タイマー.n現在の値 * (Math.PI / 10)) * 100), 200 + this.ct桜モーション用タイマー.n現在の値);
                        */
                        #endregion

                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 900 - this.ct桜X移動用タイマー1.n現在の値, 400 + this.ct桜Y移動用タイマー1.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 100 + this.ct桜X移動用タイマー1.n現在の値, 350 + this.ct桜Y移動用タイマー1.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 1300 - this.ct桜X移動用タイマー1.n現在の値, 350 + this.ct桜Y移動用タイマー1.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 900 - this.ct桜X移動用タイマー2.n現在の値, 320 + this.ct桜Y移動用タイマー2.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 800 - this.ct桜X移動用タイマー3.n現在の値, 450 + this.ct桜Y移動用タイマー3.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 10 + this.ct桜X移動用タイマー3.n現在の値, 430 + this.ct桜Y移動用タイマー3.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 800 - this.ct桜X移動用タイマー1.n現在の値, 200 + this.ct桜Y移動用タイマー1.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 0 + this.ct桜X移動用タイマー1.n現在の値, 220 + this.ct桜Y移動用タイマー1.n現在の値);
                        TJAPlayer3.Tx.Background_Down_sakura.t2D描画(TJAPlayer3.app.Device, 1100 - this.ct桜X移動用タイマー1.n現在の値, 250 + this.ct桜Y移動用タイマー1.n現在の値);
                    }
                    #endregion

                }
                if (TJAPlayer3.stage演奏ドラム画面.bIsAlreadyCleared[0])
                {
                    if (TJAPlayer3.Tx.Background_Down_Clear_2nd != null)
                    {
                        TJAPlayer3.Tx.Background_Down_Clear_2nd.n透明度 = ((this.ct上背景FIFOタイマー.n現在の値 * 0xff) / 100);

                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Down_Clear_2nd.szテクスチャサイズ.Width;

                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                        TJAPlayer3.Tx.Background_Down_Clear_2nd.t2D描画(TJAPlayer3.app.Device, 0 - this.ct下背景2ndスクロール用タイマー.n現在の値, 360);
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            TJAPlayer3.Tx.Background_Down_Clear_2nd.t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Down_Clear_2nd.szテクスチャサイズ.Width) - this.ct下背景2ndスクロール用タイマー.n現在の値, 360);
                        }
                    }

                    if (TJAPlayer3.Tx.Background_Down_Clear != null && TJAPlayer3.Tx.Background_Down_Scroll != null)
                    {
                        TJAPlayer3.Tx.Background_Down_Clear.n透明度 = ((this.ct上背景FIFOタイマー.n現在の値 * 0xff) / 100);
                        TJAPlayer3.Tx.Background_Down_Scroll.n透明度 = ((this.ct上背景FIFOタイマー.n現在の値 * 0xff) / 100);
                        TJAPlayer3.Tx.Background_Down_Clear.t2D描画(TJAPlayer3.app.Device, 0, 360);

                        //int nループ幅 = 1257;
                        //TJAPlayer3.Tx.Background_Down_Scroll.t2D描画( TJAPlayer3.app.Device, 0 - this.ct下背景スクロール用タイマー1.n現在の値, 360 );
                        //TJAPlayer3.Tx.Background_Down_Scroll.t2D描画(TJAPlayer3.app.Device, (1 * nループ幅) - this.ct下背景スクロール用タイマー1.n現在の値, 360);
                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Down_Scroll.szテクスチャサイズ.Width;
                        // 1280をテクスチャサイズで割ったものを切り上げて、プラス+1足す。
                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                        //int nループ幅 = 328;
                        TJAPlayer3.Tx.Background_Down_Scroll.t2D描画(TJAPlayer3.app.Device, 0 - this.ct下背景スクロール用タイマー1.n現在の値, 360);
                        for (int l = 1; l < ForLoop + 1; l++)
                        {
                            TJAPlayer3.Tx.Background_Down_Scroll.t2D描画(TJAPlayer3.app.Device, +(l * TJAPlayer3.Tx.Background_Down_Scroll.szテクスチャサイズ.Width) - this.ct下背景スクロール用タイマー1.n現在の値, 360);
                        }

                    }
                    if (TJAPlayer3.Tx.Background_Down_Clear_3rd != null)
                    {
                        TJAPlayer3.Tx.Background_Down_Clear_3rd.n透明度 = ((this.ct上背景FIFOタイマー.n現在の値 * 0xff) / 100);

                        double TexSize = 1280 / TJAPlayer3.Tx.Background_Down_Clear_3rd.szテクスチャサイズ.Width;

                        int ForLoop = (int)Math.Ceiling(TexSize) + 1;

                        TJAPlayer3.Tx.Background_Down_Clear_3rd.t2D描画(TJAPlayer3.app.Device, 1500 - TJAPlayer3.Tx.Background_Down_Clear_3rd.szテクスチャサイズ.Width - this.ct下背景3rdスクロール用タイマー.n現在の値, 580 - ((float)Math.Sin((float)this.ct下背景3rd波モーション用タイマー.n現在の値 * (Math.PI / 750)) * 220));
                    }
                    if (TJAPlayer3.Tx.Background_Down_Clear_4th != null)
                    {
                        TJAPlayer3.Tx.Background_Down_Clear_4th.n透明度 = ((this.ct上背景FIFOタイマー.n現在の値 * 0xff) / 100);
                        TJAPlayer3.Tx.Background_Down_Clear_4th.t2D描画(TJAPlayer3.app.Device, 0, 360);
                    }
                }
            }
            #endregion
            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        private CCounter[] ct上背景スクロール用タイマー; //上背景のX方向スクロール用
        private CCounter[] ct上背景2ndスクロール用タイマー;
        private CCounter[] ct上背景2nd下方向移動用タイマー;
        private CCounter[] ct上背景3rdスクロール用タイマー;
        private CCounter[] ct上背景3rd上下移動用タイマー;
        private CCounter ct上背景スクロール用タイマー_Dan; //上背景のX方向スクロール用
        private CCounter ct上背景2ndスクロール用タイマー_Dan;
        private CCounter ct上背景2nd下方向移動用タイマー_Dan;
        private CCounter ct上背景3rdスクロール用タイマー_Dan;
        private CCounter ct上背景3rd上下移動用タイマー_Dan;
        private CCounter ct下背景スクロール用タイマー1; //下背景パーツ1のX方向スクロール用
        private CCounter ct下背景2ndスクロール用タイマー;
        private CCounter ct下背景3rdスクロール用タイマー;
        private CCounter ct下背景3rd波モーション用タイマー;
        private CCounter ct上背景FIFOタイマー;
        private CCounter ct亀スクロール用タイマー;
        private CCounter ct亀パターン用タイマー;
        private CCounter ct桜X移動用タイマー1;
        private CCounter ct桜Y移動用タイマー1;
        private CCounter ct桜X移動用タイマー2;
        private CCounter ct桜Y移動用タイマー2;
        private CCounter ct桜X移動用タイマー3;
        private CCounter ct桜Y移動用タイマー3;
        private CCounter[] ct上背景クリアインタイマー;
        private CCounter[] ct上背景Missタイマー;
        //-----------------
        #endregion
    }
}

