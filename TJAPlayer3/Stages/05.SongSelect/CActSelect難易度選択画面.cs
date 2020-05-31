using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Drawing.Text;

using SlimDX;
using FDK;

namespace TJAPlayer3
{
    /// <summary>
    /// 難易度選択画面。
    /// この難易度選択画面はAC7～AC14のような方式であり、WiiまたはAC15移行の方式とは異なる。
    /// </summary>
	internal class CActSelect難易度選択画面 : CActivity
    {
        // プロパティ

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
        public bool bIsDifficltSelect;

        // コンストラクタ

        public CActSelect難易度選択画面()
        {
            base.list子Activities.Add(this.actQuickConfig = new CActSelectQuickConfig());
            base.b活性化してない = true;
        }


        // メソッド
        public int t指定した方向に近い難易度番号を返す(int nDIRECTION, int pos)
        {
            if (nDIRECTION == 0)
            {
                for (int i = pos; i < 5; i++)
                {
                    if (i == pos) continue;
                    if (TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[i] != null) return i;
                    if (i == 4) return this.t指定した方向に近い難易度番号を返す(0, 0);
                }
            }
            else
            {
                for (int i = pos; i > -1; i--)
                {
                    if (pos == i) continue;
                    if (TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[i] != null) return i;
                    if (i == 0) return this.t指定した方向に近い難易度番号を返す(1, 4);
                }
            }
            return pos;
        }

        public void t次に移動()
        {
            if (this.n現在の選択行 < 4)
            {
                if (!b裏譜面)
                {
                    this.n現在の選択行 += 1;
                }
                else
                {
                    if (this.n現在の選択行 == 3)
                    {
                        this.n現在の選択行 += 2;
                    }
                    else
                    {
                        this.n現在の選択行 += 1;
                    }
                }
            }
            else if (this.n現在の選択行 >= 4)
            {
                if (this.b表裏アニメーション中 == false)
                {
                    if (this.nスイッチカウント < 9 && TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[4] >= 0)
                    {
                        this.nスイッチカウント += 1;
                    }
                    else if (this.nスイッチカウント == 9)
                    {
                        ct裏譜面へ.n現在の値 = 0;
                        ct表譜面へ.n現在の値 = 0;
                        if (!b裏譜面 && n現在の選択行 == 4)
                        {
                            this.n現在の選択行 = 5;
                        }
                        else if (b裏譜面 && n現在の選択行 == 5)
                        {
                            this.n現在の選択行 = 4;
                        }
                        C共通.bToggleBoolian(ref this.b裏譜面);
                        this.b表裏アニメーション中 = true;
                        this.nスイッチカウント = 0;
                        if (this.sound裏切り替え音 != null)
                        {
                            this.sound裏切り替え音.t再生を開始する();
                        }
                    }
                }
            }

            this.ct移動 = new CCounter(-100, 0, 1, TJAPlayer3.Timer);
        }
        public void t前に移動()
        {
            if (this.n現在の選択行 > -2)
            {
                if (!b裏譜面)
                {
                    this.n現在の選択行 -= 1;
                }
                else
                {
                    if (this.n現在の選択行 == 5)
                    {
                        this.n現在の選択行 -= 2;
                    }
                    else
                    {
                        this.n現在の選択行 -= 1;
                    }
                }
                this.nスイッチカウント = 0;
            }

            this.ct移動 = new CCounter(100, 0, 1, TJAPlayer3.Timer);
        }
        public void t次へ移動2()
        {
            if (n現在の選択行2 == 0)
            {
                n現在の選択行2 += 1;
            }
        }
        public void t前へ移動2()
        {
            if (n現在の選択行2 == 1)
            {
                n現在の選択行2 -= 1;
            }
        }
        public void t選択画面初期化()
        {
            this.b初めての進行描画 = true;
        }

        // CActivity 実装

        public override void On活性化()
        {
            if (this.b活性化してる)
                return;

            this.b登場アニメ全部完了 = false;
            this.n目標のスクロールカウンタ = 0;
            this.n現在のスクロールカウンタ = 0;
            this.nスイッチカウント = 1;

            this.ct移動 = new CCounter();

            base.On活性化();
        }
        public override void On非活性化()
        {
            if (this.b活性化してない)
                return;

            for (int i = 0; i < 13; i++)
                this.ct登場アニメ用[i] = null;

            this.ct移動 = null;

            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (this.b活性化してない)
                return;

            if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar != null)
                ctカーソル点滅アニメ = new CCounter(0, 384, 2, TJAPlayer3.Timer);
            if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back != null)
                ctミニカーソル点滅アニメ = new CCounter(0, 384, 2, TJAPlayer3.Timer);
            if (TJAPlayer3.Tx.SongSelect_Difficulty_Branch != null)
                ct譜面分岐 = new CCounter(1, 200, 10, TJAPlayer3.Timer);
            if (TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch != null)
            {
                ct裏譜面へ = new CCounter(0, 992, 1, TJAPlayer3.Timer);
                ct表譜面へ = new CCounter(0, 992, 1, TJAPlayer3.Timer);
            }

            this.sound難しさを選ぶ = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Difficulty_Select.ogg"), ESoundGroup.SoundEffect);
            this.sound段位チャレンジ選択音 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Dan_Select.wav"), ESoundGroup.SoundEffect);
            this.sound裏切り替え音 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\SongSelect\Edit_Switch.ogg"), ESoundGroup.SoundEffect);
            this.sound曲を選ぶドン = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Game start.ogg"), ESoundGroup.SoundEffect);
            this.soundエラー音 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Error.ogg"), ESoundGroup.SoundEffect);
            this.sound演奏オプション = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\Song_Option.wav"), ESoundGroup.Voice);
            this.sound真打 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\Sin.wav"), ESoundGroup.Voice);
            this.sound倍速 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\2Speed.wav"), ESoundGroup.Voice);
            this.sound3倍 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\3Speed.wav"), ESoundGroup.Voice);
            this.sound4倍 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\4Speed.wav"), ESoundGroup.Voice);
            this.soundあべこべ = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\Mirror.wav"), ESoundGroup.Voice);
            this.soundきまぐれ = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\Super_Random.wav"), ESoundGroup.Voice);
            this.soundでたらめ = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\Hyper_Random.wav"), ESoundGroup.Voice);
            this.soundドロン = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Song_Option\Doron.wav"), ESoundGroup.Voice);
            
            base.OnManagedリソースの作成();
        }
        public override void OnManagedリソースの解放()
        {
            if (this.b活性化してない)
                return;

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
                n現在の選択行2 = 1;
                for (int i = 0; i < 13; i++)
                    this.ct登場アニメ用[i] = new CCounter(-i * 10, 100, 3, TJAPlayer3.Timer);
                TJAPlayer3.stage選曲.t選択曲変更通知();

                if (TJAPlayer3.stage選曲.act曲リスト.n現在選択中の曲の現在の難易度レベル == 4)
                {
                    this.n現在の選択行 = 5;
                    this.b裏譜面 = true;
                }
                else
                {
                    this.n現在の選択行 = 1 + TJAPlayer3.stage選曲.act曲リスト.n現在選択中の曲の現在の難易度レベル;
                    this.b裏譜面 = false;
                }

                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                {
                    if (this.sound難しさを選ぶ != null)
                    {
                        this.sound難しさを選ぶ.t再生を開始する();
                    }
                }
                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Dan)
                {
                    sound段位チャレンジ選択音.t再生を開始する();
                }


                if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[4] < 0)
                {
                    b裏譜面 = false;
                }

                this.b選曲後 = false;
                base.b初めての進行描画 = false;
            }
            #endregion

            {
                #region [ (2) 通常フェーズの進行。]
                if (!this.b選曲後)
                {
                    if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                    {
                        if (TJAPlayer3.stage選曲.eフェーズID == CStage.Eフェーズ.共通_通常状態
                             && TJAPlayer3.act現在入力を占有中のプラグイン == null)
                        {
                            #region [ 簡易CONFIGでMore、またはShift+F1: 詳細CONFIG呼び出し ]
                            if (actQuickConfig.bGotoDetailConfig)
                            {   // 詳細CONFIG呼び出し
                                actQuickConfig.tDeativatePopupMenu();
                                TJAPlayer3.stage選曲.actPresound.tサウンド停止();
                                TJAPlayer3.stage選曲.eフェードアウト完了時の戻り値 = CStage選曲.E戻り値.コンフィグ呼び出し;  // #24525 2011.3.16 yyagi: [SHIFT]-[F1]でCONFIG呼び出し
                                TJAPlayer3.stage選曲.actFIFO.tフェードアウト開始();
                                TJAPlayer3.stage選曲.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                                TJAPlayer3.Skin.sound取消音.t再生する();
                                return 0;
                            }
                            #endregion
                            if (!this.actQuickConfig.bIsActivePopupMenu)
                            {
                                if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RBlue) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.RightArrow))
                                {
                                    if (this.b表裏アニメーション中 == false)
                                    {
                                        TJAPlayer3.Skin.soundカーソル移動音.t再生する();
                                        this.t次に移動();
                                    }
                                }
                                else if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LBlue) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.LeftArrow))
                                {
                                    if (this.n現在の選択行 != -2)
                                    {
                                        TJAPlayer3.Skin.soundカーソル移動音.t再生する();
                                        this.t前に移動();
                                    }
                                }
                                else if ((TJAPlayer3.Pad.b押されたDGB(Eパッド.Decide) || (TJAPlayer3.Pad.b押されたDGB(Eパッド.LRed) || TJAPlayer3.Pad.b押されたDGB(Eパッド.RRed)) || ((TJAPlayer3.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return)))))
                                {
                                    if (this.n現在の選択行 == -2)
                                    {
                                        TJAPlayer3.stage選曲.ctDiffSelect戻り待ち.n現在の値 = 0;
                                        TJAPlayer3.stage選曲.t難易度選択画面を閉じる();
                                        this.sound曲を選ぶドン.t再生を開始する();
                                        this.sound難しさを選ぶ.t再生を停止する();
                                    }
                                    else if (this.n現在の選択行 == -1)
                                    {
                                        this.sound演奏オプション.t再生を開始する();
                                        this.sound難しさを選ぶ.t再生を停止する();
                                        this.actQuickConfig.tActivatePopupMenu(E楽器パート.TAIKO);
                                    }
                                    else if (this.n現在の選択行 == 0)
                                    {
                                        this.soundエラー音.t再生を開始する();
                                    }
                                    else
                                    {
                                        //かんたん・ふつう・むずかしい・おに・エディット

                                        //if (this.sound難しさを選ぶ.b再生中) this.sound難しさを選ぶ.t再生を停止する();
                                        //TJAPlayer3.Skin.sound決定音.t再生する();
                                        if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[this.n現在の選択行 - 1] >= 0)
                                        {
                                            TJAPlayer3.stage選曲.ctどんちゃんモーションスタートソング.t開始(0, 8, 30, TJAPlayer3.Timer);
                                            TJAPlayer3.stage選曲.どんちゃんアクション中 = true;
                                            this.sound難しさを選ぶ.t再生を停止する();
                                            TJAPlayer3.Skin.sound曲決定音.t再生する();
                                            this.b選曲後 = true;
                                            TJAPlayer3.stage選曲.t曲を選択する(this.n現在の選択行 - 1);
                                        }
                                        else
                                        {
                                            this.soundエラー音.t再生を開始する();
                                        }
                                        
                                    }
                                }
                                else if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Escape))
                                {
                                    if (this.b表裏アニメーション中 == false)
                                    {
                                        TJAPlayer3.stage選曲.t難易度選択画面を閉じる();
                                        this.sound曲を選ぶドン.t再生を開始する();
                                        this.sound難しさを選ぶ.t再生を停止する();
                                    }
                                }
                                else if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Tab))
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (this.b表裏アニメーション中 == false)
                                        {
                                            if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[4] >= 1)
                                            {
                                                if (this.sound裏切り替え音 != null)
                                                {
                                                    this.sound裏切り替え音.t再生を開始する();
                                                }
                                                ct裏譜面へ.n現在の値 = 0;
                                                ct表譜面へ.n現在の値 = 0;
                                                if (n現在の選択行 == 4)
                                                {
                                                    this.n現在の選択行 = 5;
                                                }
                                                else if (b裏譜面 && n現在の選択行 == 5)
                                                {
                                                    this.n現在の選択行 = 4;
                                                }
                                                C共通.bToggleBoolian(ref this.b裏譜面);
                                                this.b表裏アニメーション中 = true;
                                            }
                                            else if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[4] <= 0)
                                            {
                                                this.soundエラー音.t再生を開始する();
                                            }
                                        }
                                    }
                                }
                                else if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.F3))
                                {
                                    TJAPlayer3.Skin.sound変更音.t再生する();
                                    C共通.bToggleBoolian(ref TJAPlayer3.ConfigIni.b太鼓パートAutoPlay);
                                }
                            }
                        }
                    }
                }
                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
                {

                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Back.t2D描画(TJAPlayer3.app.Device, 249, 114);
                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Option.t2D描画(TJAPlayer3.app.Device, 319, 114);
                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_SE.color4 = new Color4(0.5f, 0.5f, 0.5f);
                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_SE.t2D描画(TJAPlayer3.app.Device, 389, 114);

                    for (int i = 0; i < 5; i++)
                    {
                        if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i] >= 0)
                        {
                            // レベルが0以上
                            TJAPlayer3.Tx.SongSelect_Difficulty_Select[i].color4 = new Color4(1f, 1f, 1f);
                            if (i == 3 && b裏譜面) ;
                            else if (i == 4 && b裏譜面)
                            {
                                // エディット
                                TJAPlayer3.Tx.SongSelect_Difficulty_Select[i].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                            }
                            else if (i != 4)
                            {
                                TJAPlayer3.Tx.SongSelect_Difficulty_Select[i].t2D描画(TJAPlayer3.app.Device, 450 + (100 * i), 84);
                            }
                        }
                        else
                        {
                            // レベルが0未満 = 譜面がないとみなす
                            TJAPlayer3.Tx.SongSelect_Difficulty_Select[i].color4 = new Color4(0.5f, 0.5f, 0.5f);
                            if (i == 3 && b裏譜面) ;
                            else if (i == 4 && b裏譜面)
                            {
                                // エディット
                                TJAPlayer3.Tx.SongSelect_Difficulty_Select[i].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                            }
                            else if (i != 4)
                            {
                                TJAPlayer3.Tx.SongSelect_Difficulty_Select[i].t2D描画(TJAPlayer3.app.Device, 450 + (100 * i), 84);
                            }
                        }

                        #region[ 星 ]
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Branch != null)
                            ct譜面分岐.t進行Loop();
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Branch_Edit != null)
                            ct譜面分岐.t進行Loop();
                        if (TJAPlayer3.Tx.SongSelect_Level != null)
                        {
                            // 全難易度表示
                            for (int n = 0; n < TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nレベル[i]; n++)
                            {
                                // 星11以上はループ終了
                                //if (n > 9) break;
                                // 裏なら鬼と同じ場所に
                                if (i == 3 && b裏譜面) break;
                                if (i == 4 && b裏譜面)
                                {
                                    if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.b譜面分岐[i] && TJAPlayer3.Tx.SongSelect_Difficulty_Branch_Edit != null && this.ct譜面分岐.n現在の値 >= 0 && this.ct譜面分岐.n現在の値 < 100)
                                        TJAPlayer3.Tx.SongSelect_Difficulty_Branch_Edit.t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                    else
                                        TJAPlayer3.Tx.SongSelect_Difficulty_Star_Edit.t2D描画(TJAPlayer3.app.Device, 479 + (100 * 3), 474 - (n * 197 / 10));
                                }
                                if (i != 4)
                                {
                                    if (TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.b譜面分岐[i] && TJAPlayer3.Tx.SongSelect_Difficulty_Branch != null && this.ct譜面分岐.n現在の値 >= 0 && this.ct譜面分岐.n現在の値 < 100)
                                        TJAPlayer3.Tx.SongSelect_Difficulty_Branch.t2D描画(TJAPlayer3.app.Device, 450 + (100 * i), 84);
                                    else
                                        TJAPlayer3.Tx.SongSelect_Difficulty_Star.t2D描画(TJAPlayer3.app.Device, 479 + (100 * i), 474 - (n * 197 / 10));
                                }
                            }
                        }
                        #endregion
                    }

                    if (TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch != null)
                    {
                        this.ct裏譜面へ.t進行();
                        this.ct表譜面へ.t進行();
                        if (this.b表裏アニメーション中)
                        {
                            if (b裏譜面)
                            {
                                if (this.ct裏譜面へ.n現在の値 <= 120)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[0].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                }
                                else if (this.ct裏譜面へ.n現在の値 <= 240)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[1].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                }
                                else if (this.ct裏譜面へ.n現在の値 <= 360)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[2].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[3].n透明度 = 255;
                                }
                                else if (this.ct裏譜面へ.n現在の値 <= 480)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[3].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                }
                                else if (this.ct裏譜面へ.n現在の値 > 480)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[3].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[3].n透明度 = 255 - ((this.ct裏譜面へ.n現在の値 - 480) / 2);
                                }
                                else if (this.ct裏譜面へ.n現在の値 >= 992)
                                {
                                    b表裏アニメーション中 = false;
                                }
                            }

                            else if (!b裏譜面)
                            {
                                if (this.ct表譜面へ.n現在の値 <= 120)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[4].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                }
                                else if (this.ct表譜面へ.n現在の値 <= 240)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[5].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                }
                                else if (this.ct表譜面へ.n現在の値 <= 360)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[6].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[7].n透明度 = 255;
                                }
                                else if (this.ct表譜面へ.n現在の値 <= 480)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[7].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                }
                                else if (this.ct表譜面へ.n現在の値 > 480)
                                {
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[7].t2D描画(TJAPlayer3.app.Device, 450 + (100 * 3), 84);
                                    TJAPlayer3.Tx.SongSelect_Difficulty_Select_Switch[7].n透明度 = 255 - ((this.ct表譜面へ.n現在の値 - 480) / 2);
                                }
                                else if (this.ct表譜面へ.n現在の値 >= 992)
                                {
                                    b表裏アニメーション中 = false;
                                }
                            }
                        }
                    }
                    if (b表裏アニメーション中 == true)
                    {

                        if (ct表譜面へ.b終了値に達した || ct裏譜面へ.b終了値に達した)
                        {
                            
                            b表裏アニメーション中 = false;
                        }
                    }

                    #region[ カーソル ]

                    if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect != null)
                        ctカーソル点滅アニメ.t進行Loop();
                    if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect != null)
                        ctミニカーソル点滅アニメ.t進行Loop();

                    if (this.n現在の選択行 == 5)
                    {
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar != null)
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar.t2D描画(TJAPlayer3.app.Device, 450 + (3 * 100), -6);
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect != null)
                        {
                            if (ctカーソル点滅アニメ.n現在の値 < 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.n透明度 = ctカーソル点滅アニメ.n現在の値;
                            if (ctカーソル点滅アニメ.n現在の値 >= 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.n透明度 = 127;
                            if (ctカーソル点滅アニメ.n現在の値 >= 255)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.n透明度 = 384 - ctカーソル点滅アニメ.n現在の値;
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.t2D描画(TJAPlayer3.app.Device, 450 + (3 * 100), -6);
                        }
                    }
                    else if (this.n現在の選択行 == -2)
                    {
                        //case E項目種類.オプション:
                        //case E項目種類.音色:
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back != null)
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back.t2D描画(TJAPlayer3.app.Device, 231, -41);
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect != null)
                        {
                            if (ctカーソル点滅アニメ.n現在の値 < 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = ctカーソル点滅アニメ.n現在の値;
                            if (ctカーソル点滅アニメ.n現在の値 >= 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = 127;
                            if (ctカーソル点滅アニメ.n現在の値 >= 255)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = 384 - ctカーソル点滅アニメ.n現在の値;
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.t2D描画(TJAPlayer3.app.Device, 231, -41);
                        }
                    }
                    else if (this.n現在の選択行 == -1)
                    {
                        //case E項目種類.オプション:
                        //case E項目種類.音色:
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back != null)
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back.t2D描画(TJAPlayer3.app.Device, 301, -41);
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect != null)
                        {
                            if (ctカーソル点滅アニメ.n現在の値 < 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = ctカーソル点滅アニメ.n現在の値;
                            if (ctカーソル点滅アニメ.n現在の値 >= 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = 127;
                            if (ctカーソル点滅アニメ.n現在の値 >= 255)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = 384 - ctカーソル点滅アニメ.n現在の値;
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.t2D描画(TJAPlayer3.app.Device, 301, -41);
                        }
                    }
                    else if (this.n現在の選択行 == 0)
                    {
                        //case E項目種類.オプション:
                        //case E項目種類.音色:
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back != null)
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back.t2D描画(TJAPlayer3.app.Device, 371, -41);
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect != null)
                        {
                            if (ctカーソル点滅アニメ.n現在の値 < 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = ctカーソル点滅アニメ.n現在の値;
                            if (ctカーソル点滅アニメ.n現在の値 >= 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = 127;
                            if (ctカーソル点滅アニメ.n現在の値 >= 255)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.n透明度 = 384 - ctカーソル点滅アニメ.n現在の値;
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Back_Effect.t2D描画(TJAPlayer3.app.Device, 371, -41);
                        }
                    }
                    else
                    {
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar != null)
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar.t2D描画(TJAPlayer3.app.Device, 450 + (this.n現在の選択行 - 1) * 100, -6);
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect != null)
                        {
                            if (ctカーソル点滅アニメ.n現在の値 < 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.n透明度 = ctカーソル点滅アニメ.n現在の値;
                            if (ctカーソル点滅アニメ.n現在の値 >= 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.n透明度 = 127;
                            if (ctカーソル点滅アニメ.n現在の値 >= 255)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.n透明度 = 384 - ctカーソル点滅アニメ.n現在の値;
                            TJAPlayer3.Tx.SongSelect_Difficulty_Bar_Effect.t2D描画(TJAPlayer3.app.Device, 450 + (this.n現在の選択行 - 1) * 100, -6);
                        }
                    }

                    #endregion
                }
                #region 東方好きのren君 段位チャレンジ確認画面 
                if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 == (int)Difficulty.Dan)
                {
                    if (bIsDifficltSelect)
                    {

                        ctカーソル点滅アニメ.t進行Loop();
                        var Left = new Rectangle(0, 0, 640, 720);
                        var Right = new Rectangle(640, 0, 1280, 720);
                        bool SelectBar = false;
                        if (!b選曲後)
                        {
                            if (TJAPlayer3.Tx.BlackOut != null)
                            {
                                TJAPlayer3.Tx.BlackOut.n透明度 = 150;
                                TJAPlayer3.Tx.BlackOut.t2D描画(TJAPlayer3.app.Device, 0, 0);
                            }
                            if (TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Box != null)
                            {
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Box.t2D描画(TJAPlayer3.app.Device, 0, 0);
                            }
                            if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RBlue) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.RightArrow))
                            {
                                TJAPlayer3.Skin.sound変更音.t再生する();
                                this.t次へ移動2();
                            }
                            else if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LBlue) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.LeftArrow))
                            {
                                TJAPlayer3.Skin.sound変更音.t再生する();
                                this.t前へ移動2();
                            }
                            else if ((TJAPlayer3.Pad.b押されたDGB(Eパッド.Decide) || (TJAPlayer3.Pad.b押されたDGB(Eパッド.LRed) || TJAPlayer3.Pad.b押されたDGB(Eパッド.RRed)) ||
                                    ((TJAPlayer3.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return)))))
                            {
                                if (n現在の選択行2 == 0)
                                {
                                    sound段位チャレンジ選択音.tサウンドを停止する();
                                    sound段位チャレンジ選択音.t再生位置を先頭に戻す();
                                    TJAPlayer3.Skin.sound取消音.t再生する();
                                    bIsDifficltSelect = false;
                                }
                                else
                                {
                                    TJAPlayer3.Skin.sound曲決定音.t再生する();
                                    sound段位チャレンジ選択音.t再生を停止する();
                                    TJAPlayer3.stage選曲.ctどんちゃんモーションスタートソング.t開始(0, 8, 30, TJAPlayer3.Timer);
                                    TJAPlayer3.stage選曲.どんちゃんアクション中 = true;
                                    this.b選曲後 = true;
                                    TJAPlayer3.stage選曲.t曲を選択する(6);
                                }
                            }
                            else if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Escape))
                            {
                                sound段位チャレンジ選択音.tサウンドを停止する();
                                sound段位チャレンジ選択音.t再生位置を先頭に戻す();
                                TJAPlayer3.Skin.sound取消音.t再生する();
                                bIsDifficltSelect = false;
                            }

                            if (ctカーソル点滅アニメ.n現在の値 < 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select_Effect.n透明度 = ctカーソル点滅アニメ.n現在の値;
                            if (ctカーソル点滅アニメ.n現在の値 >= 127)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select_Effect.n透明度 = 127;
                            if (ctカーソル点滅アニメ.n現在の値 >= 255)
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select_Effect.n透明度 = 384 - ctカーソル点滅アニメ.n現在の値;
                            if (n現在の選択行2 == 0)
                            {
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select.t2D描画(TJAPlayer3.app.Device, 0, 0, new Rectangle(0, 0, 640, 720));
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select_Effect.t2D描画(TJAPlayer3.app.Device, 0, 0, new Rectangle(0, 0, 640, 720));
                            }
                            else if (n現在の選択行2 == 1)
                            {
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select.t2D描画(TJAPlayer3.app.Device, 640, 0, new Rectangle(640, 0, 640 * 2, 720));
                                TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select_Effect.t2D描画(TJAPlayer3.app.Device, 640, 0, new Rectangle(640, 0, 1280, 720));
                            }
                        }
                        /*
                        if(toggle)
                        {
                            TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select.t2D描画(TJAPlayer3.app.Device, 0, 0, Left);
                            TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select_Effect.t2D描画(TJAPlayer3.app.Device, 0, 0, Left);
                        }
                        if(!toggle)
                        {
                            TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select.t2D描画(TJAPlayer3.app.Device, 0, 0, Right);
                            TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Select_Effect.t2D描画(TJAPlayer3.app.Device, 0, 0, Right);
                        }
                        */

                        /*
                        int n現在の選択行2 = 0;
                        //TJAPlayer3.act文字コンソール.tPrint(60, 120, C文字コンソール.Eフォント種別.白, n現在の選択行2.ToString());
                        if (!this.b選曲後)
                        {
                            if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RBlue) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.RightArrow))
                            {
                                TJAPlayer3.Skin.soundカーソル移動音.t再生する();
                                n現在の選択行2 = 0;
                            }
                            else if (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LBlue) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.LeftArrow))
                            {
                                TJAPlayer3.Skin.soundカーソル移動音.t再生する();
                                n現在の選択行2 = 1;
                            }
                            else if ((TJAPlayer3.Pad.b押されたDGB(Eパッド.Decide) || (TJAPlayer3.Pad.b押されたDGB(Eパッド.LRed) || TJAPlayer3.Pad.b押されたDGB(Eパッド.RRed)) ||
                                    ((TJAPlayer3.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return)))))
                            {
                                if (n現在の選択行2 == 0)
                                {
                                    TJAPlayer3.Skin.sound取消音.t再生する();
                                    bDanSelect = false;
                                }
                                else
                                {
                                    //Danとして曲を決定する
                                    TJAPlayer3.Skin.sound曲決定音.t再生する();
                                    this.b選曲後 = true;
                                    TJAPlayer3.stage選曲.t曲を選択する(n現在の選択行2 + 5);
                                }

                            }
                            else if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Escape))
                            {
                                bDanSelect = false;
                            }
                        }
                        var rc = new Rectangle(0, 0, 640 * 1 + n現在の選択行2, 720);
                        if (TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Box != null)
                        {
                            TJAPlayer3.Tx.SongSelect_Difficulty_Dan_Box.t2D描画(TJAPlayer3.app.Device, 0, 0);
                        }
                        */
                    }
                }
                #endregion
                //-----------------
                #endregion
            }
            this.actQuickConfig.t進行描画();

            if (TJAPlayer3.stage選曲.どんちゃんアクション中 == true)
            {
                if (TJAPlayer3.Tx.SongSelect_Chara_Start_Song[0] != null)
                {
                    TJAPlayer3.Tx.SongSelect_Chara_Start_Song[TJAPlayer3.stage選曲.ctどんちゃんモーションスタートソング.n現在の値].t2D描画(TJAPlayer3.app.Device, 0 - 45, 339);
                }

            }

            TJAPlayer3.stage選曲.ctどんちゃんモーションスタートソング.t進行();
            if (TJAPlayer3.stage選曲.ctどんちゃんモーションスタートソング.b終了値に達した)
            {
                TJAPlayer3.stage選曲.ctどんちゃんモーションスタートソング.t停止();

            }

            if (TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Tower && TJAPlayer3.stage選曲.n現在選択中の曲の難易度 != (int)Difficulty.Dan)
            {
                if (n現在の選択行 >= 1)
                {
                    TJAPlayer3.stage選曲.act演奏履歴パネル.ct登場アニメ用.t進行();
                    int x = 20;
                    int y = 145;
                    if (TJAPlayer3.stage選曲.r現在選択中のスコア != null && TJAPlayer3.stage選曲.r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.SCORE)
                    {
                        //CDTXMania.Tx.SongSelect_ScoreWindow_Text.n透明度 = ct登場アニメ用.n現在の値 - 1745;
                        if (TJAPlayer3.Tx.SongSelect_ScoreWindow[n現在の選択行 - 1] != null)
                        {
                            //CDTXMania.Tx.SongSelect_ScoreWindow[CDTXMania.stage選曲.n現在選択中の曲の難易度].n透明度 = ct登場アニメ用.n現在の値 - 1745;
                            TJAPlayer3.Tx.SongSelect_ScoreWindow[n現在の選択行 - 1].t2D描画(TJAPlayer3.app.Device, x, y);
                            TJAPlayer3.stage選曲.act演奏履歴パネル.t小文字表示(x + 70, y + 88, string.Format("{0,7:######0}", TJAPlayer3.stage選曲.r現在選択中のスコア.譜面情報.nハイスコア[n現在の選択行 - 1].ToString()));
                            TJAPlayer3.Tx.SongSelect_ScoreWindow_Text.t2D描画(TJAPlayer3.app.Device, x + 159, y + 160, new Rectangle(0, 0, 18, 30));
                            TJAPlayer3.Tx.SongSelect_ScoreWindow_Text.t2D描画(TJAPlayer3.app.Device, x + 159, y + 233, new Rectangle(0, 0, 18, 30));

                        }
                    }
                }
            }
            return 0;
        }


        // その他

        #region [ private ]
        //-----------------
        private CActSelectQuickConfig actQuickConfig;
        private int n現在の選択行2 = 1;
        public bool bDanSelect;
        public CSound sound難しさを選ぶ;
        private CSound sound段位チャレンジ選択音;
        public CSound sound裏切り替え音;
        public CSound sound曲を選ぶドン;
        private CSound soundエラー音;
        public CSound sound演奏オプション;
        public CSound sound真打;
        public CSound sound倍速;
        public CSound sound3倍;
        public CSound sound4倍;
        public CSound soundあべこべ;
        public CSound soundきまぐれ;
        public CSound soundでたらめ;
        public CSound soundドロン;

        private bool b登場アニメ全部完了;
        private bool b選曲後;
        private bool b裏譜面;
        private bool b表裏アニメーション中;

        private CCounter[] ct登場アニメ用 = new CCounter[13];
        private CCounter ct移動;
        private CCounter ctカーソル点滅アニメ;
        private CCounter ctミニカーソル点滅アニメ;
        private CCounter ct譜面分岐;
        private CCounter ct裏譜面へ;
        private CCounter ct表譜面へ;

        private int n現在のスクロールカウンタ;
        private int n現在の選択行;
        private int n目標のスクロールカウンタ;
        private int nスイッチカウント;
        //-----------------
        #endregion
    }
}
/*
                    switch (TJAPlayer3.stage選曲.r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                            {
                                TJAPlayer3.Skin.sound決定音.t再生する();
                                TJAPlayer3.stage選曲.t曲を選択する(this.n現在の選択行);
                            }
                            break;
                    }
 */
