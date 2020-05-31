using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using FDK;

namespace TJAPlayer3
{
    internal class CActResultParameterPanel : CActivity
    {
        // コンストラクタ

        public CActResultParameterPanel()
        {
            ST文字位置[] st文字位置Array = new ST文字位置[11];
            ST文字位置 st文字位置 = new ST文字位置
            {
                ch = '0',
                pt = new Point(0, 0)
            };
            st文字位置Array[0] = st文字位置;
            ST文字位置 st文字位置2 = new ST文字位置
            {
                ch = '1',
                pt = new Point(32, 0)
            };
            st文字位置Array[1] = st文字位置2;
            ST文字位置 st文字位置3 = new ST文字位置
            {
                ch = '2',
                pt = new Point(64, 0)
            };
            st文字位置Array[2] = st文字位置3;
            ST文字位置 st文字位置4 = new ST文字位置
            {
                ch = '3',
                pt = new Point(96, 0)
            };
            st文字位置Array[3] = st文字位置4;
            ST文字位置 st文字位置5 = new ST文字位置
            {
                ch = '4',
                pt = new Point(128, 0)
            };
            st文字位置Array[4] = st文字位置5;
            ST文字位置 st文字位置6 = new ST文字位置
            {
                ch = '5',
                pt = new Point(160, 0)
            };
            st文字位置Array[5] = st文字位置6;
            ST文字位置 st文字位置7 = new ST文字位置
            {
                ch = '6',
                pt = new Point(192, 0)
            };
            st文字位置Array[6] = st文字位置7;
            ST文字位置 st文字位置8 = new ST文字位置
            {
                ch = '7',
                pt = new Point(224, 0)
            };
            st文字位置Array[7] = st文字位置8;
            ST文字位置 st文字位置9 = new ST文字位置
            {
                ch = '8',
                pt = new Point(256, 0)
            };
            st文字位置Array[8] = st文字位置9;
            ST文字位置 st文字位置10 = new ST文字位置
            {
                ch = '9',
                pt = new Point(288, 0)
            };
            st文字位置Array[9] = st文字位置10;
            ST文字位置 st文字位置11 = new ST文字位置
            {
                ch = ' ',
                pt = new Point(0, 0)
            };
            st文字位置Array[10] = st文字位置11;
            st小文字位置 = st文字位置Array;

            ST文字位置[] st文字位置Array2 = new ST文字位置[11];
            ST文字位置 st文字位置12 = new ST文字位置
            {
                ch = '0',
                pt = new Point(0, 0)
            };
            st文字位置Array2[0] = st文字位置12;
            ST文字位置 st文字位置13 = new ST文字位置
            {
                ch = '1',
                pt = new Point(32, 0)
            };
            st文字位置Array2[1] = st文字位置13;
            ST文字位置 st文字位置14 = new ST文字位置
            {
                ch = '2',
                pt = new Point(64, 0)
            };
            st文字位置Array2[2] = st文字位置14;
            ST文字位置 st文字位置15 = new ST文字位置
            {
                ch = '3',
                pt = new Point(96, 0)
            };
            st文字位置Array2[3] = st文字位置15;
            ST文字位置 st文字位置16 = new ST文字位置
            {
                ch = '4',
                pt = new Point(128, 0)
            };
            st文字位置Array2[4] = st文字位置16;
            ST文字位置 st文字位置17 = new ST文字位置
            {
                ch = '5',
                pt = new Point(160, 0)
            };
            st文字位置Array2[5] = st文字位置17;
            ST文字位置 st文字位置18 = new ST文字位置
            {
                ch = '6',
                pt = new Point(192, 0)
            };
            st文字位置Array2[6] = st文字位置18;
            ST文字位置 st文字位置19 = new ST文字位置
            {
                ch = '7',
                pt = new Point(224, 0)
            };
            st文字位置Array2[7] = st文字位置19;
            ST文字位置 st文字位置20 = new ST文字位置
            {
                ch = '8',
                pt = new Point(256, 0)
            };
            st文字位置Array2[8] = st文字位置20;
            ST文字位置 st文字位置21 = new ST文字位置
            {
                ch = '9',
                pt = new Point(288, 0)
            };
            st文字位置Array2[9] = st文字位置21;
            ST文字位置 st文字位置22 = new ST文字位置
            {
                ch = '%',
                pt = new Point(0x37, 0)
            };
            st文字位置Array2[10] = st文字位置22;
            //st大文字位置 = st文字位置Array2;

            ST文字位置[] stScore文字位置Array = new ST文字位置[10];
            ST文字位置 stScore文字位置 = new ST文字位置
            {
                ch = '0',
                pt = new Point(0, 0)
            };
            stScore文字位置Array[0] = stScore文字位置;
            ST文字位置 stScore文字位置2 = new ST文字位置
            {
                ch = '1',
                pt = new Point(24, 0)
            };
            stScore文字位置Array[1] = stScore文字位置2;
            ST文字位置 stScore文字位置3 = new ST文字位置
            {
                ch = '2',
                pt = new Point(48, 0)
            };
            stScore文字位置Array[2] = stScore文字位置3;
            ST文字位置 stScore文字位置4 = new ST文字位置
            {
                ch = '3',
                pt = new Point(72, 0)
            };
            stScore文字位置Array[3] = stScore文字位置4;
            ST文字位置 stScore文字位置5 = new ST文字位置
            {
                ch = '4',
                pt = new Point(96, 0)
            };
            stScore文字位置Array[4] = stScore文字位置5;
            ST文字位置 stScore文字位置6 = new ST文字位置
            {
                ch = '5',
                pt = new Point(120, 0)
            };
            stScore文字位置Array[5] = stScore文字位置6;
            ST文字位置 stScore文字位置7 = new ST文字位置
            {
                ch = '6',
                pt = new Point(144, 0)
            };
            stScore文字位置Array[6] = stScore文字位置7;
            ST文字位置 stScore文字位置8 = new ST文字位置
            {
                ch = '7',
                pt = new Point(168, 0)
            };
            stScore文字位置Array[7] = stScore文字位置8;
            ST文字位置 stScore文字位置9 = new ST文字位置
            {
                ch = '8',
                pt = new Point(192, 0)
            };
            stScore文字位置Array[8] = stScore文字位置9;
            ST文字位置 stScore文字位置10 = new ST文字位置
            {
                ch = '9',
                pt = new Point(216, 0)
            };
            stScore文字位置Array[9] = stScore文字位置10;
            stScoreFont = stScore文字位置Array;

            base.b活性化してない = true;
        }


        // メソッド

        public void tアニメを完了させる()
        {
            ct表示用.n現在の値 = ct表示用.n終了値;
        }


        // CActivity 実装

        public override void On活性化()
        {
            Mode = new ResultMode[3];
            base.On活性化();
        }
        public override void On非活性化()
        {
            ctMob0 = null;
            ctMob1 = null;
            if (ct表示用 != null)
            {
                ct表示用 = null;
            }
            if (ct文字アニメ用 != null)
            {
                ct文字アニメ用 = null;
            }
            if (ct待機用 != null)
            {
                ct待機用 = null;
            }
            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                for (int nPlayer = 0; nPlayer < TJAPlayer3.ConfigIni.nPlayerCount; nPlayer++)
                {
                    if (TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[nPlayer] >= 100)
                    {
                        Mode[nPlayer] = ResultMode.StageDaiseikou;
                    }
                    else if (TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[nPlayer] >= 80)
                    {
                        Mode[nPlayer] = ResultMode.StageCleared;
                    }
                    else
                    {
                        Mode[nPlayer] = ResultMode.StageFailed;
                    }
                }
                ct全体待機 = new CCounter(0, 10000, 1, TJAPlayer3.Timer);
                ctResult_Normal_Loop = new CCounter();
                ctResult_In = new CCounter();
                ctMobIn0 = new CCounter();
                ctMobIn1 = new CCounter();
                ctFailed_Mob = new CCounter();
                ctOukan = new CCounter();
                ctOukan_In = new CCounter();
                ctMob0 = new CCounter();
                ctMob1 = new CCounter();
                ctResult_Daiseikou_In = new CCounter();
                ctResult_Daiseikou_Loop = new CCounter();
                ctResult_Flower = new CCounter();
                ctResult_Flower_In = new CCounter();
                ctResult_Header_In = new CCounter();
                ctResult_Clear_Loop = new CCounter();
                ctFailed_Loop = new CCounter();
                ct吹き出しアニメーション = new CCounter();
                ct吹き出しアニメーション_2P = new CCounter();
                ct炎 = new CCounter(0, 6, 50, TJAPlayer3.Timer);
                ct虹アニメ = new CCounter(0, TJAPlayer3.Skin.Result_Rainbow_Ptn - 1, TJAPlayer3.Skin.Game_Gauge_Rainbow_Timer, TJAPlayer3.Timer);
                ct虹透明度 = new CCounter(0, TJAPlayer3.Skin.Game_Gauge_Rainbow_Timer - 1, 1, TJAPlayer3.Timer);

                sound大成功音声 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\DaiseikouSE.ogg"), ESoundGroup.SoundEffect);
                soundクリア音声 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\ClearSE.ogg"), ESoundGroup.SoundEffect);
                soundクリア失敗 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\FailedSE.ogg"), ESoundGroup.SoundEffect);
                sound惜しかった = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\OshiiSE.ogg"), ESoundGroup.SoundEffect);
                soundFullCombo_In = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\FullCombo_In.ogg"), ESoundGroup.SoundEffect);
                soundClear_In = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Clear_In.ogg"), ESoundGroup.SoundEffect);
                sound回転音 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Rotate.ogg"), ESoundGroup.SoundEffect);
                sound不合格 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Dan\Failure.ogg"), ESoundGroup.SoundEffect);
                sound合格 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Dan\Success.ogg"), ESoundGroup.SoundEffect);
                sound金合格 = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Dan\Better_Success.ogg"), ESoundGroup.SoundEffect);
                soundBar_SE = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Dan\Bar_SE.ogg"), ESoundGroup.SoundEffect);

                Dan_Plate = TJAPlayer3.tテクスチャの生成(Path.GetDirectoryName(TJAPlayer3.DTX.strファイル名の絶対パス) + @"\Dan_Plate.png");
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                TJAPlayer3.t安全にDisposeする(ref ct表示用);
                TJAPlayer3.t安全にDisposeする(ref ctResult_Normal_Loop);
                TJAPlayer3.t安全にDisposeする(ref ctResult_In);
                TJAPlayer3.t安全にDisposeする(ref ct全体待機);
                TJAPlayer3.t安全にDisposeする(ref ctResult_Flower);
                TJAPlayer3.t安全にDisposeする(ref ctResult_Flower_In);
                TJAPlayer3.t安全にDisposeする(ref ctResult_Header_In);
                TJAPlayer3.t安全にDisposeする(ref ctMobIn0);
                TJAPlayer3.t安全にDisposeする(ref ctMobIn1);
                TJAPlayer3.t安全にDisposeする(ref ctMob0);
                TJAPlayer3.t安全にDisposeする(ref ctMob1);
                TJAPlayer3.t安全にDisposeする(ref ctOukan);
                TJAPlayer3.t安全にDisposeする(ref ctOukan_In);
                TJAPlayer3.t安全にDisposeする(ref ctFailed_Mob);
                TJAPlayer3.t安全にDisposeする(ref ctResult_Daiseikou_In);
                TJAPlayer3.t安全にDisposeする(ref ctResult_Daiseikou_Loop);
                TJAPlayer3.t安全にDisposeする(ref ctResult_Clear_Loop);
                TJAPlayer3.t安全にDisposeする(ref ctFailed_Loop);
                TJAPlayer3.t安全にDisposeする(ref ct吹き出しアニメーション);
                TJAPlayer3.t安全にDisposeする(ref ct吹き出しアニメーション_2P);
                TJAPlayer3.t安全にDisposeする(ref ct炎);
                TJAPlayer3.t安全にDisposeする(ref ct文字アニメ用);
                TJAPlayer3.t安全にDisposeする(ref ct待機用);
                TJAPlayer3.t安全にDisposeする(ref ct虹アニメ);
                TJAPlayer3.t安全にDisposeする(ref ct虹透明度);
                sound大成功音声?.Dispose();
                soundクリア音声?.Dispose();
                soundクリア失敗?.Dispose();
                sound惜しかった?.Dispose();
                soundFullCombo_In?.Dispose();
                soundClear_In?.Dispose();
                sound回転音?.Dispose();
                sound不合格?.Dispose();
                sound合格?.Dispose();
                sound金合格?.Dispose();
                soundBar_SE?.Dispose();

                Dan_Plate?.Dispose();
                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {
            if (base.b活性化してない)
            {
                return 0;
            }
            if (base.b初めての進行描画)
            {
                for (int nPlayer = 0; nPlayer < TJAPlayer3.ConfigIni.nPlayerCount; nPlayer++)
                {
                    b吹き出し再生[nPlayer] = false;
                    bOukan再生[nPlayer] = false;
                }
                for (int i = 0; i < 3; i++)
                {
                    bBar_SE再生[i] = false;
                }
                n文字アニメ = 1;
                bスコアアニメ終了 = false;
                bスコア待機終了 = false;
                bGreatアニメ終了 = false;
                bGreat待機終了 = false;
                bGoodアニメ終了 = false;
                bGood待機終了 = false;
                bBadアニメ終了 = false;
                bBad待機終了 = false;
                bComboアニメ終了 = false;
                bCombo待機終了 = false;
                bRollアニメ終了 = false;
                bRoll待機終了 = false;
                bHitアニメ終了 = false;
                bHit待機終了 = false;
                b文字アニメ終了 = false;
                b回転音再生中 = false;
                ct表示用 = new CCounter(0, 0x3e7, 2, TJAPlayer3.Timer);
                ct文字アニメ用 = new CCounter();
                ct文字アニメ用.t開始(0, 文字アニメ用回転回数, 文字アニメ用Timer, TJAPlayer3.Timer);
                ct待機用 = new CCounter();
                ct待機用.t開始(0, 1, 文字アニメ用Timer2, TJAPlayer3.Timer);
                base.b初めての進行描画 = false;
            }
            int 虹ベース = ct虹アニメ.n現在の値 + 1;
            if (虹ベース == ct虹アニメ.n終了値 + 1) 虹ベース = 0;
            ct表示用?.t進行();
            ct全体待機?.t進行();
            ct待機用?.t進行();
            ct文字アニメ用?.t進行();

            double dbたたけた率Auto = (100.0 * TJAPlayer3.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect / TJAPlayer3.DTX.nノーツ数[3]) + ((100.0 * TJAPlayer3.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great / TJAPlayer3.DTX.nノーツ数[3]) / 2);
            double dbたたけた率 = (100.0 * TJAPlayer3.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Perfect / TJAPlayer3.DTX.nノーツ数[3]) + ((100.0 * TJAPlayer3.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Great / TJAPlayer3.DTX.nノーツ数[3]) / 2) + dbたたけた率Auto;

            if (TJAPlayer3.stage選曲.n確定された曲の難易度 == (int)Difficulty.Dan)
            {
                #region 段位認定モード用
                int どんちゃんY = 260;
                if (ct全体待機.n現在の値 >= 5000)
                {
                    TJAPlayer3.Tx.Result_Panel.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Panel?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Panel_X[0], TJAPlayer3.Skin.Result_Panel_Y[0]);
                    TJAPlayer3.Tx.Result_Score_Text.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Score_Text?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_ScoreText_X[0], TJAPlayer3.Skin.Result_ScoreText_Y[0]); //点
                    TJAPlayer3.Tx.Result_Judge.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Judge?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Judge_X[0], TJAPlayer3.Skin.Result_Judge_Y[0]);
                    TJAPlayer3.Tx.Result_Gauge_Base[0].n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Gauge_Base[0]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeBase_X[0], TJAPlayer3.Skin.Result_GaugeBase_Y[0], new Rectangle(0, 0, 691, 47));
                    // Dan_Plate
                    // Dan_Plate.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    Dan_Plate?.t2D中心基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Dan_Plate_XY[0], TJAPlayer3.Skin.Result_Dan_Plate_XY[1]);
                }

                if (b文字アニメ終了)
                {
                    TJAPlayer3.stage演奏ドラム画面.actDan.DrawExam_Result(TJAPlayer3.stage結果.st演奏記録.Drums.Dan_C);
                    if (bBar_SE再生[0] != true && TJAPlayer3.stage演奏ドラム画面.actDan.ctDan_In[0].b進行中)
                    {
                        soundBar_SE?.tサウンドを先頭から再生する();
                        bBar_SE再生[0] = true;
                    }
                    else if (bBar_SE再生[1] != true && TJAPlayer3.stage演奏ドラム画面.actDan.ctDan_In[1].b進行中)
                    {
                        soundBar_SE?.tサウンドを先頭から再生する();
                        bBar_SE再生[1] = true;
                    }
                    else if (bBar_SE再生[2] != true && TJAPlayer3.stage演奏ドラム画面.actDan.ctDan_In[2].b進行中)
                    {
                        soundBar_SE?.tサウンドを先頭から再生する();
                        bBar_SE再生[2] = true;
                    }
                }

                if (TJAPlayer3.stage演奏ドラム画面.actDan.ctDan_In[1].n現在の値 == (1280 - TJAPlayer3.Skin.Game_DanC_X[2]) / 2 && TJAPlayer3.stage演奏ドラム画面.actDan.ctDan_Bar.n現在の値 == 1500)
                {
                    #region[ランク]
                    TJAPlayer3.Tx.Result_Rank[11].t2D描画(TJAPlayer3.app.Device, 565, 206);

                    if (dbたたけた率 >= 100)
                    {
                        TJAPlayer3.Tx.Result_Rank[10].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 99)
                    {
                        TJAPlayer3.Tx.Result_Rank[9].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 97)
                    {
                        TJAPlayer3.Tx.Result_Rank[8].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 94)
                    {
                        TJAPlayer3.Tx.Result_Rank[7].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 90)
                    {
                        TJAPlayer3.Tx.Result_Rank[6].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 80)
                    {
                        TJAPlayer3.Tx.Result_Rank[5].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 60)
                    {
                        TJAPlayer3.Tx.Result_Rank[4].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 40)
                    {
                        TJAPlayer3.Tx.Result_Rank[3].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 20)
                    {
                        TJAPlayer3.Tx.Result_Rank[2].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 10)
                    {
                        TJAPlayer3.Tx.Result_Rank[1].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    else if (dbたたけた率 >= 0)
                    {
                        TJAPlayer3.Tx.Result_Rank[0].t2D描画(TJAPlayer3.app.Device, 658, 179);
                    }
                    #endregion
                    if (ctOukan_In.b停止中) ctOukan_In.t開始(0, 255, 3, TJAPlayer3.Timer);
                    ctOukan_In?.t進行();
                    int Count = (ctOukan_In.n現在の値 * ctOukan_In.n現在の値 + 1) / (ctOukan_In.n現在の値 + 1);
                    if (ctOukan_In.b進行中)
                    {
                        TJAPlayer3.Tx.Result_Dan.n透明度 = Count;
                        TJAPlayer3.Tx.Result_Dan.vc拡大縮小倍率.X = 3.55f - Count / 100f;
                        TJAPlayer3.Tx.Result_Dan.vc拡大縮小倍率.Y = 3.55f - Count / 100f;
                    }
                    else
                    {
                        TJAPlayer3.Tx.Result_Dan.vc拡大縮小倍率.X = 1f;
                        TJAPlayer3.Tx.Result_Dan.vc拡大縮小倍率.Y = 1f;
                    }
                    switch (TJAPlayer3.stage演奏ドラム画面.actDan.GetExamStatus(TJAPlayer3.stage結果.st演奏記録.Drums.Dan_C))
                    {
                        case Exam.Status.Failure:
                            if (ctFailed_Loop.b停止中) ctFailed_Loop.t開始(0, 29, 20, TJAPlayer3.Timer);
                            ctFailed_Loop?.t進行Loop();
                            if (bOukan再生[0] != true && ctOukan_In.b終了値に達した)
                            {
                                sound不合格.t再生を開始する();
                                bOukan再生[0] = true;
                            }
                            TJAPlayer3.Tx.Result_Dan?.t2D拡大率考慮中央基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Dan_XY[0] + TJAPlayer3.Skin.Result_Dan_XY[0] / 2, TJAPlayer3.Skin.Result_Dan_XY[1] + TJAPlayer3.Skin.Result_Dan_XY[1] / 2, new Rectangle(0, 0, TJAPlayer3.Skin.Result_Dan[0], TJAPlayer3.Skin.Result_Dan[1]));
                            TJAPlayer3.Tx.Result_Failed_Loop[ctFailed_Loop.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Failed_Loop_X[0], TJAPlayer3.Skin.Result_Failed_Loop_Y[0] + どんちゃんY);
                            break;
                        case Exam.Status.Success:
                            if (ctResult_Clear_Loop.b停止中) ctResult_Clear_Loop.t開始(0, 60, 20, TJAPlayer3.Timer);
                            ctResult_Clear_Loop?.t進行Loop();
                            if (bOukan再生[0] != true && ctOukan_In.b終了値に達した)
                            {
                                sound合格.t再生を開始する();
                                bOukan再生[0] = true;
                            }
                            TJAPlayer3.Tx.Result_Dan?.t2D拡大率考慮中央基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Dan_XY[0] + TJAPlayer3.Skin.Result_Dan_XY[0] / 2, TJAPlayer3.Skin.Result_Dan_XY[1] + TJAPlayer3.Skin.Result_Dan_XY[1] / 2, new Rectangle(TJAPlayer3.Skin.Result_Dan[0], 0, TJAPlayer3.Skin.Result_Dan[0], TJAPlayer3.Skin.Result_Dan[1]));
                            TJAPlayer3.Tx.Result_Clear_Loop[ctResult_Clear_Loop.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Clear_Loop_X[0], TJAPlayer3.Skin.Result_Clear_Loop_Y[0] + どんちゃんY);
                            break;
                        case Exam.Status.Better_Success:
                            if (ctResult_Daiseikou_In.b停止中) ctResult_Daiseikou_In.t開始(0, 69, 20, TJAPlayer3.Timer);
                            ctResult_Daiseikou_In?.t進行();
                            if (bOukan再生[0] != true && ctOukan_In.b終了値に達した)
                            {
                                sound金合格.t再生を開始する();
                                bOukan再生[0] = true;
                            }
                            TJAPlayer3.Tx.Result_Dan?.t2D拡大率考慮中央基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Dan_XY[0] + TJAPlayer3.Skin.Result_Dan_XY[0] / 2, TJAPlayer3.Skin.Result_Dan_XY[1] + TJAPlayer3.Skin.Result_Dan_XY[1] / 2, new Rectangle(TJAPlayer3.Skin.Result_Dan[0] * 2, 0, TJAPlayer3.Skin.Result_Dan[0], TJAPlayer3.Skin.Result_Dan[1]));
                            if (ctResult_Daiseikou_In.b進行中) TJAPlayer3.Tx.Result_Daiseikou_In[ctResult_Daiseikou_In.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Daiseikou_In_X[0], TJAPlayer3.Skin.Result_Daiseikou_In_Y[0] + どんちゃんY);
                            if (ctResult_Daiseikou_In.b終了値に達した)
                            {
                                if (ctResult_Daiseikou_Loop.b停止中) ctResult_Daiseikou_Loop.t開始(0, 42, 20, TJAPlayer3.Timer);
                                ctResult_Daiseikou_Loop?.t進行Loop();
                                TJAPlayer3.Tx.Result_Daiseikou_Loop[ctResult_Daiseikou_Loop.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.ctResult_Daiseikou_Loop_X[0], TJAPlayer3.Skin.ctResult_Daiseikou_Loop_Y[0] + どんちゃんY);
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (ct全体待機.n現在の値 >= 3500)
                {
                    if (ctResult_In.b停止中) ctResult_In.t開始(0, 26, 10, TJAPlayer3.Timer);
                    ctResult_In?.t進行();
                    if (ctResult_In.b進行中) TJAPlayer3.Tx.Result_In[ctResult_In.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_In_X[0], TJAPlayer3.Skin.Result_In_Y[0] + どんちゃんY);
                    if (ctResult_In.b終了値に達した)
                    {
                        if (ctResult_Normal_Loop.b停止中) ctResult_Normal_Loop.t開始(0, 17, 31, TJAPlayer3.Timer);
                        ctResult_Normal_Loop?.t進行Loop();
                        if (!(TJAPlayer3.stage演奏ドラム画面.actDan.ctDan_In[1].n現在の値 == (1280 - TJAPlayer3.Skin.Game_DanC_X[2]) / 2 && TJAPlayer3.stage演奏ドラム画面.actDan.ctDan_Bar.n現在の値 == 1500)) TJAPlayer3.Tx.Result_Normal_Loop?[ctResult_Normal_Loop.n現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Normal_Loop_X[0], TJAPlayer3.Skin.Result_Normal_Loop_Y[0] + どんちゃんY);
                    }
                }

                if (ct全体待機.n現在の値 >= 6000)
                {
                    for (int nGauge = 2; nGauge <= TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ; nGauge += 2)
                    {
                        int px = (nGauge - 2) * 616 / 98;
                        Rectangle rec = new Rectangle(0, 0, 13, 40);
                        if (nGauge >= 100) rec = new Rectangle(49, 0, 10, 40);
                        else if (nGauge > 80) rec = new Rectangle(37, 0, 13, 40);
                        else if (nGauge == 80) rec = new Rectangle(25, 0, 13, 40);
                        else if (nGauge == 40) rec = new Rectangle(12, 0, 13, 40);
                        if (TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ >= nGauge + 2)
                            TJAPlayer3.Tx.Result_Gauge[0]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeBody_X[0] + px, TJAPlayer3.Skin.Result_GaugeBody_Y[0], rec);
                    }
                    //演奏中のやつ使いまわせなかった。ファック。
                    //↑使い回すことはできるけれど、一度格納したほうが安全
                    if (TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 100)
                    {
                        ct虹アニメ?.t進行Loop();
                        ct虹透明度?.t進行Loop();
                        if (TJAPlayer3.Tx.Result_Rainbow[ct虹アニメ.n現在の値] != null)
                        {
                            TJAPlayer3.Tx.Result_Rainbow[ct虹アニメ.n現在の値].n透明度 = 255;
                            TJAPlayer3.Tx.Result_Rainbow[ct虹アニメ.n現在の値].t2D描画(TJAPlayer3.app.Device, 555, 123);
                            TJAPlayer3.Tx.Result_Rainbow[虹ベース].n透明度 = (ct虹透明度.n現在の値 * 255 / ct虹透明度.n終了値) / 1;
                            TJAPlayer3.Tx.Result_Rainbow[虹ベース].t2D描画(TJAPlayer3.app.Device, 555, 123);
                        }
                    }
                    if (TJAPlayer3.Tx.Gauge_Soul != null)
                    {
                        //仮置き
                        if (TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ >= 100.0f)
                        {
                            ct炎?.t進行Loop();
                            TJAPlayer3.Tx.Gauge_Soul_Fire.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeSoulFire_X[0], TJAPlayer3.Skin.Result_GaugeSoulFire_Y[0], 52, new Rectangle(230 * (ct炎.n現在の値), 0, 230, 230));
                        }
                    }
                    if (TJAPlayer3.Tx.Gauge_Soul_Fire != null)
                    {
                        //仮置き
                        if (TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ >= 100.0f)
                        {
                            TJAPlayer3.Tx.Gauge_Soul.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeSoul_X[0], TJAPlayer3.Skin.Result_GaugeSoul_Y[0], 125, new Rectangle(0, 0, 80, 80));
                        }
                    }
                }

                if (ct全体待機.n現在の値 >= 8000)
                {
                    文字表示_Dan();
                }
                #endregion
            }
            else
            {
                #region 一人プレイ用
                if (ct全体待機.n現在の値 >= 4000)
                {
                    if (TJAPlayer3.ConfigIni.真打 != true && TJAPlayer3.Tx.Result_Panel != null)
                    {
                        TJAPlayer3.Tx.Result_Panel.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                        TJAPlayer3.Tx.Result_Panel.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Panel_X[0], TJAPlayer3.Skin.Result_Panel_Y[0]);
                    }
                    if (TJAPlayer3.ConfigIni.真打 == true && TJAPlayer3.Tx.Result_Shin_Panel != null)
                    {
                        TJAPlayer3.Tx.Result_Shin_Panel.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                        TJAPlayer3.Tx.Result_Shin_Panel.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Panel_X[0], TJAPlayer3.Skin.Result_Panel_Y[0]);
                    }
                    TJAPlayer3.Tx.Result_Score_Text.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Score_Text?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_ScoreText_X[0], TJAPlayer3.Skin.Result_ScoreText_Y[0]); //点
                    TJAPlayer3.Tx.Result_Judge.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Judge?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Judge_X[0], TJAPlayer3.Skin.Result_Judge_Y[0]);
                    TJAPlayer3.Tx.Result_Gauge_Base[0].n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Gauge_Base[0]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeBase_X[0], TJAPlayer3.Skin.Result_GaugeBase_Y[0], new Rectangle(0, 0, 691, 47));
                    TJAPlayer3.Tx.Result_Course_Symbol[TJAPlayer3.stage選曲.n確定された曲の難易度].n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                    TJAPlayer3.Tx.Result_Course_Symbol[TJAPlayer3.stage選曲.n確定された曲の難易度]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_CourseSymbol_X[0], TJAPlayer3.Skin.Result_CourseSymbol_Y[0]);
                    

                }

                if (ct全体待機.n現在の値 >= 4000)
                {
                    if (ctResult_In.b停止中) ctResult_In.t開始(0, 26, 10, TJAPlayer3.Timer);
                    ctResult_In?.t進行();
                    if (ctResult_In.b進行中) TJAPlayer3.Tx.Result_In[ctResult_In.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_In_X[0], TJAPlayer3.Skin.Result_In_Y[0]);
                    if (ctResult_In.b終了値に達した)
                    {
                        if (ctMobIn0.b停止中) ctMobIn0.t開始(0, TJAPlayer3.Tx.Result_Mob.szテクスチャサイズ.Height, 1, TJAPlayer3.Timer);
                        ctMobIn0?.t進行();
                        if (ctMobIn0.b終了値に達してない) TJAPlayer3.Tx.Result_Mob?.t2D描画(TJAPlayer3.app.Device, 0, 720 - ctMobIn0.n現在の値);
                        if (ctMobIn0.b終了値に達した && ct全体待機.n現在の値 < 7000) TJAPlayer3.Tx.Result_Mob?.t2D描画(TJAPlayer3.app.Device, 0, 720 - TJAPlayer3.Tx.Result_Mob.szテクスチャサイズ.Height);
                        if (ctResult_Normal_Loop.b停止中) ctResult_Normal_Loop.t開始(0, 17, 31, TJAPlayer3.Timer);
                        ctResult_Normal_Loop?.t進行Loop();
                        if (ct全体待機.n現在の値 < 5500) TJAPlayer3.Tx.Result_Normal_Loop?[ctResult_Normal_Loop.n現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Normal_Loop_X[0], TJAPlayer3.Skin.Result_Normal_Loop_Y[0]);
                    }
                }

                if (ct全体待機.n現在の値 >= 5000)
                {
                    for (int nGauge = 2; nGauge <= TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ; nGauge += 2)
                    {
                        int px = (nGauge - 2) * 616 / 98;
                        Rectangle rec = new Rectangle(0, 0, 13, 40);
                        if (nGauge >= 100) rec = new Rectangle(49, 0, 10, 40);
                        else if (nGauge > 80) rec = new Rectangle(37, 0, 13, 40);
                        else if (nGauge == 80) rec = new Rectangle(25, 0, 13, 40);
                        else if (nGauge == 40) rec = new Rectangle(12, 0, 13, 40);
                        if (TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ >= nGauge + 2)
                            TJAPlayer3.Tx.Result_Gauge[0]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeBody_X[0] + px, TJAPlayer3.Skin.Result_GaugeBody_Y[0], rec);
                    }
                    //演奏中のやつ使いまわせなかった。ファック。
                    //↑使い回すことはできるけれど、一度格納したほうが安全
                    if (TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 100)
                    {
                        ct虹アニメ?.t進行Loop();
                        ct虹透明度?.t進行Loop();
                        if (TJAPlayer3.Tx.Result_Rainbow[ct虹アニメ.n現在の値] != null)
                        {
                            TJAPlayer3.Tx.Result_Rainbow[ct虹アニメ.n現在の値].n透明度 = 255;
                            TJAPlayer3.Tx.Result_Rainbow[ct虹アニメ.n現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeBase_X[0], TJAPlayer3.Skin.Result_GaugeBase_Y[0] + 1);
                            TJAPlayer3.Tx.Result_Rainbow[虹ベース].n透明度 = (ct虹透明度.n現在の値 * 255 / ct虹透明度.n終了値) / 1;
                            TJAPlayer3.Tx.Result_Rainbow[虹ベース].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeBase_X[0], TJAPlayer3.Skin.Result_GaugeBase_Y[0] + 1);
                        }
                    }
                    if (TJAPlayer3.Tx.Gauge_Soul != null)
                    {
                        //仮置き
                        if (TJAPlayer3.Tx.Gauge_Soul_Fire != null && TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ >= 100.0f)
                        {
                            ct炎?.t進行Loop();
                            TJAPlayer3.Tx.Gauge_Soul_Fire.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeSoulFire_X[0], TJAPlayer3.Skin.Result_GaugeSoulFire_Y[0], 52, new Rectangle(230 * (ct炎.n現在の値), 0, 230, 230));
                        }
                    }
                    if (TJAPlayer3.Tx.Gauge_Soul_Fire != null)
                    {
                        //仮置き
                        if (TJAPlayer3.Tx.Gauge_Soul != null && TJAPlayer3.stage結果.st演奏記録.Drums.fゲージ >= 100.0f)
                            TJAPlayer3.Tx.Gauge_Soul.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_GaugeSoul_X[0], TJAPlayer3.Skin.Result_GaugeSoul_Y[0], 125, new Rectangle(0, 0, 80, 80));
                    }
                }

                if (ct全体待機.n現在の値 >= 5500)
                {
                    if (TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 80)
                    {
                        if (ctOukan_In.b停止中) ctOukan_In.t開始(0, 255, 3, TJAPlayer3.Timer);
                        ctOukan_In?.t進行();
                        if (ctOukan_In.b終了値に達した)
                        {
                            if (ctOukan.b停止中) ctOukan.t開始(0, 4, 50, TJAPlayer3.Timer);
                            ctOukan?.t進行();
                        }
                        int Count = (ctOukan_In.n現在の値 * ctOukan_In.n現在の値 + 1) / (ctOukan_In.n現在の値 + 1);

                        if (TJAPlayer3.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Miss == 0)
                        {
                            if (bOukan再生[0] != true)
                            {
                                soundFullCombo_In.t再生を開始する();
                                bOukan再生[0] = true;
                            }
                            if (ctOukan_In.b終了値に達した) TJAPlayer3.Tx.Result_Oukan_FullCombo[ctOukan.n現在の値]?.t2D拡大率考慮中央基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Oukan_X[0], TJAPlayer3.Skin.Result_Oukan_Y[0]);
                            else
                            {
                                TJAPlayer3.Tx.Result_Oukan_FullCombo[0].n透明度 = Count;
                                TJAPlayer3.Tx.Result_Oukan_FullCombo[0].vc拡大縮小倍率.X = 3.55f - Count / 100f;
                                TJAPlayer3.Tx.Result_Oukan_FullCombo[0].vc拡大縮小倍率.Y = 3.55f - Count / 100f;
                                TJAPlayer3.Tx.Result_Oukan_FullCombo[0]?.t2D拡大率考慮中央基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Oukan_X[0], TJAPlayer3.Skin.Result_Oukan_Y[0]);
                            }
                        }
                        else
                        {
                            if (bOukan再生[0] != true)
                            {
                                soundClear_In.t再生を開始する();
                                bOukan再生[0] = true;
                            }
                            if (ctOukan_In.b終了値に達した) TJAPlayer3.Tx.Result_Oukan_Clear[ctOukan.n現在の値]?.t2D拡大率考慮中央基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Oukan_X[0], TJAPlayer3.Skin.Result_Oukan_Y[0]);
                            else
                            {
                                TJAPlayer3.Tx.Result_Oukan_Clear[0].n透明度 = Count;
                                TJAPlayer3.Tx.Result_Oukan_Clear[0].vc拡大縮小倍率.X = 3.55f - Count / 100f;
                                TJAPlayer3.Tx.Result_Oukan_Clear[0].vc拡大縮小倍率.Y = 3.55f - Count / 100f;
                                TJAPlayer3.Tx.Result_Oukan_Clear[0]?.t2D拡大率考慮中央基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Oukan_X[0], TJAPlayer3.Skin.Result_Oukan_Y[0]);
                            }
                        }
                    }

                    if (ctResult_In.b終了値に達した)
                    {
                        switch (Mode[0])
                        {
                            case ResultMode.StageFailed:
                                if (ctFailed_Loop.b停止中) ctFailed_Loop.t開始(0, 29, 20, TJAPlayer3.Timer);
                                ctFailed_Loop?.t進行Loop();
                                TJAPlayer3.Tx.Result_Failed_Loop[ctFailed_Loop.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Failed_Loop_X[0], TJAPlayer3.Skin.Result_Failed_Loop_Y[0]);
                                break;

                            case ResultMode.StageCleared:
                                if (ctResult_Clear_Loop.b停止中) ctResult_Clear_Loop.t開始(0, 60, 20, TJAPlayer3.Timer);
                                ctResult_Clear_Loop?.t進行Loop();
                                TJAPlayer3.Tx.Result_Clear_Loop[ctResult_Clear_Loop.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Clear_Loop_X[0], TJAPlayer3.Skin.Result_Clear_Loop_Y[0]);
                                break;

                            case ResultMode.StageDaiseikou:
                                if (ctResult_Daiseikou_In.b停止中) ctResult_Daiseikou_In.t開始(0, 69, 20, TJAPlayer3.Timer);
                                ctResult_Daiseikou_In?.t進行();
                                if (ctResult_Daiseikou_In.b進行中) TJAPlayer3.Tx.Result_Daiseikou_In[ctResult_Daiseikou_In.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Daiseikou_In_X[0], TJAPlayer3.Skin.Result_Daiseikou_In_Y[0]);
                                if (ctResult_Daiseikou_In.b終了値に達した)
                                {
                                    if (ctResult_Daiseikou_Loop.b停止中) ctResult_Daiseikou_Loop.t開始(0, 42, 20, TJAPlayer3.Timer);
                                    ctResult_Daiseikou_Loop?.t進行Loop();
                                    TJAPlayer3.Tx.Result_Daiseikou_Loop[ctResult_Daiseikou_Loop.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.ctResult_Daiseikou_Loop_X[0], TJAPlayer3.Skin.ctResult_Daiseikou_Loop_Y[0]);
                                }
                                break;
                        }
                    }
                }

                if (ct全体待機.n現在の値 >= 7000)
                {
                    if (ctResult_Header_In.b停止中) ctResult_Header_In.t開始(0, 255, 3, TJAPlayer3.Timer);
                    ctResult_Header_In?.t進行();
                    if (ctResult_Flower_In.b停止中) ctResult_Flower_In.t開始(0, TJAPlayer3.Tx.Result_Flower[0].szテクスチャサイズ.Height, 1, TJAPlayer3.Timer);
                    ctResult_Flower_In?.t進行();
                    if (ctResult_Flower.b停止中 && ctResult_Flower_In.b終了値に達した) ctResult_Flower.t開始(0, 2, 50, TJAPlayer3.Timer);
                    if (ctResult_Flower.b停止中 != true) ctResult_Flower?.t進行();
                    if (ctMob0.b停止中) ctMob0.t開始(0, 180, 3, TJAPlayer3.Timer);
                    ctMob0?.t進行Loop();

                    switch (Mode[0])
                    {
                        case ResultMode.StageFailed:
                            if (ctFailed_Mob.b停止中) ctFailed_Mob.t開始(0, 1, 500, TJAPlayer3.Timer);
                            ctFailed_Mob?.t進行Loop();
                            TJAPlayer3.Tx.Result_Mob_Failed[ctFailed_Mob.n現在の値]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, TJAPlayer3.Tx.Result_Mob_Failed[ctFailed_Mob.n現在の値].szテクスチャサイズ.Height);
                            break;

                        case ResultMode.StageCleared:
                            if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                            {
                                TJAPlayer3.Tx.Result_Header_Clear[2].n透明度 = ctResult_Header_In.n現在の値;
                                TJAPlayer3.Tx.Result_Header_Clear[2]?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                            }
                            else
                            {
                                TJAPlayer3.Tx.Result_Header_Clear[0].n透明度 = ctResult_Header_In.n現在の値;
                                TJAPlayer3.Tx.Result_Header_Clear[0]?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                            }
                            if (ctResult_Flower_In.b終了値に達してない) TJAPlayer3.Tx.Result_Flower[0]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, 720 - ctResult_Flower_In.n現在の値);
                            if (ctResult_Flower_In.b終了値に達した) TJAPlayer3.Tx.Result_Flower[ctResult_Flower.n現在の値]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, 720 - TJAPlayer3.Tx.Result_Flower[ctResult_Flower.n現在の値].szテクスチャサイズ.Height);
                            TJAPlayer3.Tx.Result_Mob_Clear[0]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, (720 - (TJAPlayer3.Tx.Result_Mob_Clear[0].szテクスチャサイズ.Height - 30)) + -((float)Math.Sin((float)this.ctMob0.n現在の値 * (Math.PI / 180)) * 30));
                            break;

                        case ResultMode.StageDaiseikou:
                            if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                            {
                                TJAPlayer3.Tx.Result_Header_Clear[2].n透明度 = ctResult_Header_In.n現在の値;
                                TJAPlayer3.Tx.Result_Header_Clear[2]?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                            }
                            else
                            {
                                TJAPlayer3.Tx.Result_Header_Clear[0].n透明度 = ctResult_Header_In.n現在の値;
                                TJAPlayer3.Tx.Result_Header_Clear[0]?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                            }
                            if (ctResult_Flower_In.b終了値に達してない) TJAPlayer3.Tx.Result_Flower[0]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, 720 - ctResult_Flower_In.n現在の値);
                            if (ctResult_Flower_In.b終了値に達した)
                            {
                                TJAPlayer3.Tx.Result_Flower[ctResult_Flower.n現在の値]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, 720 - TJAPlayer3.Tx.Result_Flower[ctResult_Flower.n現在の値].szテクスチャサイズ.Height);
                                if (ctResult_Flower.b終了値に達した)
                                {
                                    if (ctMobIn1.b停止中) ctMobIn1.t開始(0, TJAPlayer3.Tx.Result_Mob_Clear[1].szテクスチャサイズ.Height, 1, TJAPlayer3.Timer);
                                    ctMobIn1?.t進行();
                                    if (ctMobIn1.b終了値に達してない) TJAPlayer3.Tx.Result_Mob_Clear[1]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, 720 - ctMobIn1.n現在の値);
                                    if (ctMob1.b停止中 && ctMobIn1.b終了値に達した) ctMob1.t開始(0, 180, 3, TJAPlayer3.Timer);
                                    if (ctMob1.b停止中 != true)
                                    {
                                        ctMob1?.t進行Loop();
                                        TJAPlayer3.Tx.Result_Mob_Clear[1]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, (720 - (TJAPlayer3.Tx.Result_Mob_Clear[1].szテクスチャサイズ.Height - 30)) + -((float)Math.Sin((float)this.ctMob1.n現在の値 * (Math.PI / 180)) * 30));
                                    }
                                }
                            }
                            TJAPlayer3.Tx.Result_Mob_Clear[0]?.t2D上中心基準描画(TJAPlayer3.app.Device, 640, (720 - (TJAPlayer3.Tx.Result_Mob_Clear[0].szテクスチャサイズ.Height - 30)) + -((float)Math.Sin((float)this.ctMob0.n現在の値 * (Math.PI / 180)) * 30));
                            break;
                    }

                    if (b文字アニメ終了)
                    {
                        if (ct吹き出しアニメーション.b停止中) ct吹き出しアニメーション.t開始(0, 4255, 1, TJAPlayer3.Timer);
                        ct吹き出しアニメーション?.t進行();

                        #region[ランク]
                        TJAPlayer3.Tx.Result_Rank[11].t2D描画(TJAPlayer3.app.Device, 565, 206);

                        if (dbたたけた率 >= 100)
                        {
                            TJAPlayer3.Tx.Result_Rank[10].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 99)
                        {
                            TJAPlayer3.Tx.Result_Rank[9].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 97)
                        {
                            TJAPlayer3.Tx.Result_Rank[8].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 94)
                        {
                            TJAPlayer3.Tx.Result_Rank[7].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 90)
                        {
                            TJAPlayer3.Tx.Result_Rank[6].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 80)
                        {
                            TJAPlayer3.Tx.Result_Rank[5].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 60)
                        {
                            TJAPlayer3.Tx.Result_Rank[4].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 40)
                        {
                            TJAPlayer3.Tx.Result_Rank[3].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 20)
                        {
                            TJAPlayer3.Tx.Result_Rank[2].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 10)
                        {
                            TJAPlayer3.Tx.Result_Rank[1].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        else if (dbたたけた率 >= 0)
                        {
                            TJAPlayer3.Tx.Result_Rank[0].t2D描画(TJAPlayer3.app.Device, 658, 179);
                        }
                        #endregion

                        switch (Mode[0])
                        {
                            case ResultMode.StageFailed:
                                if (TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 40)
                                {
                                    if (b吹き出し再生[0] != true)
                                    {
                                        sound惜しかった.t再生を開始する();
                                        b吹き出し再生[0] = true;
                                    }
                                    if (ct吹き出しアニメーション.n現在の値 <= 200)
                                    {
                                        TJAPlayer3.Tx.Result_Oshii.n透明度 = ct吹き出しアニメーション.n現在の値 * 1.275f;
                                        TJAPlayer3.Tx.Result_Oshii.vc拡大縮小倍率.X = ct吹き出しアニメーション.n現在の値 / 200f;
                                        TJAPlayer3.Tx.Result_Oshii.vc拡大縮小倍率.Y = ct吹き出しアニメーション.n現在の値 / 200f;
                                    }
                                    else
                                    {
                                        TJAPlayer3.Tx.Result_Oshii.vc拡大縮小倍率.X = 1f;
                                        TJAPlayer3.Tx.Result_Oshii.vc拡大縮小倍率.Y = 1f;
                                    }
                                    TJAPlayer3.Tx.Result_Oshii.n透明度 = 4255 - ct吹き出しアニメーション.n現在の値;
                                    TJAPlayer3.Tx.Result_Oshii?.t2D拡大率考慮下基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Hukidashi_X[0], TJAPlayer3.Skin.Result_Hukidashi_Y[0]);
                                }
                                else
                                {
                                    if (b吹き出し再生[0] != true)
                                    {
                                        soundクリア失敗.t再生を開始する();
                                        b吹き出し再生[0] = true;
                                    }
                                    if (ct吹き出しアニメーション.n現在の値 <= 200)
                                    {
                                        TJAPlayer3.Tx.Result_Failed.n透明度 = ct吹き出しアニメーション.n現在の値 * 1.275f;
                                        TJAPlayer3.Tx.Result_Failed.vc拡大縮小倍率.X = ct吹き出しアニメーション.n現在の値 / 200f;
                                        TJAPlayer3.Tx.Result_Failed.vc拡大縮小倍率.Y = ct吹き出しアニメーション.n現在の値 / 200f;
                                    }
                                    else
                                    {
                                        TJAPlayer3.Tx.Result_Failed.vc拡大縮小倍率.X = 1f;
                                        TJAPlayer3.Tx.Result_Failed.vc拡大縮小倍率.Y = 1f;
                                    }
                                    TJAPlayer3.Tx.Result_Failed.n透明度 = 4255 - ct吹き出しアニメーション.n現在の値;
                                    TJAPlayer3.Tx.Result_Failed?.t2D拡大率考慮下基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Hukidashi_X[0], TJAPlayer3.Skin.Result_Hukidashi_Y[0]);
                                }
                                break;

                            case ResultMode.StageCleared:
                                if (b吹き出し再生[0] != true)
                                {
                                    soundクリア音声.t再生を開始する();
                                    b吹き出し再生[0] = true;
                                }
                                if (ct吹き出しアニメーション.n現在の値 <= 200)
                                {
                                    TJAPlayer3.Tx.Result_Clear.n透明度 = ct吹き出しアニメーション.n現在の値 * 1.275f;
                                    TJAPlayer3.Tx.Result_Clear.vc拡大縮小倍率.X = ct吹き出しアニメーション.n現在の値 / 200f;
                                    TJAPlayer3.Tx.Result_Clear.vc拡大縮小倍率.Y = ct吹き出しアニメーション.n現在の値 / 200f;
                                }
                                else
                                {
                                    TJAPlayer3.Tx.Result_Clear.vc拡大縮小倍率.X = 1f;
                                    TJAPlayer3.Tx.Result_Clear.vc拡大縮小倍率.Y = 1f;
                                }
                                TJAPlayer3.Tx.Result_Clear.n透明度 = 4255 - ct吹き出しアニメーション.n現在の値;
                                TJAPlayer3.Tx.Result_Clear?.t2D拡大率考慮下基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Hukidashi_X[0], TJAPlayer3.Skin.Result_Hukidashi_Y[0] + 14);
                                break;

                            case ResultMode.StageDaiseikou:
                                if (b吹き出し再生[0] != true)
                                {
                                    sound大成功音声.t再生を開始する();
                                    b吹き出し再生[0] = true;
                                }
                                if (ct吹き出しアニメーション.n現在の値 <= 200)
                                {
                                    TJAPlayer3.Tx.Result_Daiseikou.n透明度 = ct吹き出しアニメーション.n現在の値 * 1.275f;
                                    TJAPlayer3.Tx.Result_Daiseikou.vc拡大縮小倍率.X = ct吹き出しアニメーション.n現在の値 / 200f;
                                    TJAPlayer3.Tx.Result_Daiseikou.vc拡大縮小倍率.Y = ct吹き出しアニメーション.n現在の値 / 200f;
                                }
                                else
                                {
                                    TJAPlayer3.Tx.Result_Daiseikou.vc拡大縮小倍率.X = 1f;
                                    TJAPlayer3.Tx.Result_Daiseikou.vc拡大縮小倍率.Y = 1f;
                                }
                                TJAPlayer3.Tx.Result_Daiseikou.n透明度 = 4255 - ct吹き出しアニメーション.n現在の値;
                                TJAPlayer3.Tx.Result_Daiseikou?.t2D拡大率考慮下基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_Hukidashi_X[0], TJAPlayer3.Skin.Result_Hukidashi_Y[0] + 14);
                                break;
                        }
                    }
                    文字表示(0);
                }
                #endregion
            }

            if (!ct表示用.b終了値に達した)
            {
                return 0;
            }
            return 1;
        }
        private void 文字表示(int nPlayer)
        {
            tスコア文字表示(TJAPlayer3.Skin.Result_Score_X[nPlayer], TJAPlayer3.Skin.Result_Score_Y[nPlayer], string.Format("{0,7:######0}", TJAPlayer3.stage結果.st演奏記録.Drums.nスコア), ref bスコアアニメ終了, ref bスコア待機終了);
            if ((!(bスコアアニメ終了 && ct待機用.b終了値に達した)) && !bスコア待機終了 && !b文字アニメ終了)
            {
                return;
            }
            t小文字表示(TJAPlayer3.Skin.Result_Great_X[nPlayer], TJAPlayer3.Skin.Result_Great_Y[nPlayer], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.nPerfect数.ToString()), ref bGreatアニメ終了, ref bGreat待機終了);
            if ((!(bGreatアニメ終了 && ct待機用.b終了値に達した)) && !bGreat待機終了 && !b文字アニメ終了)
            {
                return;
            }
            t小文字表示(TJAPlayer3.Skin.Result_Good_X[nPlayer], TJAPlayer3.Skin.Result_Good_Y[nPlayer], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.nGreat数.ToString()), ref bGoodアニメ終了, ref bGood待機終了);
            if ((!(bGoodアニメ終了 && ct待機用.b終了値に達した)) && !bGood待機終了 && !b文字アニメ終了)
            {
                return;
            }
            t小文字表示(TJAPlayer3.Skin.Result_Bad_X[nPlayer], TJAPlayer3.Skin.Result_Bad_Y[nPlayer], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.nMiss数.ToString()), ref bBadアニメ終了, ref bBad待機終了);
            if ((!(bBadアニメ終了 && ct待機用.b終了値に達した)) && !bBad待機終了 && !b文字アニメ終了)
            {
                return;
            }
            t小文字表示(TJAPlayer3.Skin.Result_Combo_X[nPlayer], TJAPlayer3.Skin.Result_Combo_Y[nPlayer], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.n最大コンボ数.ToString()), ref bComboアニメ終了, ref bCombo待機終了);
            if ((!(bComboアニメ終了 && ct待機用.b終了値に達した)) && !bCombo待機終了 && !b文字アニメ終了)
            {
                return;
            }
            t小文字表示(TJAPlayer3.Skin.Result_Roll_X[nPlayer], TJAPlayer3.Skin.Result_Roll_Y[nPlayer], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.n連打数.ToString()), ref bRollアニメ終了, ref bRoll待機終了);
            if ((!(bRollアニメ終了 && ct待機用.b終了値に達した)) && !bRoll待機終了 && !b文字アニメ終了)
            {
                return;
            }
            b文字アニメ終了 = true;
        }

        private void 文字表示_Dan()
        {
            tスコア文字表示(TJAPlayer3.Skin.Result_Score_X[0], TJAPlayer3.Skin.Result_Score_Y[0], string.Format("{0,7:######0}", TJAPlayer3.stage結果.st演奏記録.Drums.nスコア), ref bスコアアニメ終了, ref bスコア待機終了);
            t小文字表示(TJAPlayer3.Skin.Result_Great_X[0], TJAPlayer3.Skin.Result_Great_Y[0], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.nPerfect数.ToString()), ref bスコアアニメ終了, ref bスコア待機終了);
            t小文字表示(TJAPlayer3.Skin.Result_Good_X[0], TJAPlayer3.Skin.Result_Good_Y[0], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.nGreat数.ToString()), ref bスコアアニメ終了, ref bスコア待機終了);
            t小文字表示(TJAPlayer3.Skin.Result_Bad_X[0], TJAPlayer3.Skin.Result_Bad_Y[0], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.nMiss数.ToString()), ref bスコアアニメ終了, ref bスコア待機終了);
            t小文字表示(TJAPlayer3.Skin.Result_Combo_X[0], TJAPlayer3.Skin.Result_Combo_Y[0], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.n最大コンボ数.ToString()), ref bスコアアニメ終了, ref bスコア待機終了);
            t小文字表示(TJAPlayer3.Skin.Result_Roll_X[0], TJAPlayer3.Skin.Result_Roll_Y[0], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.n連打数.ToString()), ref bスコアアニメ終了, ref bスコア待機終了);
            t小文字表示(TJAPlayer3.Skin.Result_Hit_X[0], TJAPlayer3.Skin.Result_Hit_Y[0], string.Format("{0,4:###0}", TJAPlayer3.stage結果.st演奏記録.Drums.nPerfect数 + TJAPlayer3.stage結果.st演奏記録.Drums.nGreat数 + TJAPlayer3.stage結果.st演奏記録.Drums.n連打数), ref bスコアアニメ終了, ref bスコア待機終了);
            if (!(bスコアアニメ終了 || b文字アニメ終了))
            {
                return;
            }
            b文字アニメ終了 = true;
        }

        // その他

        #region [ private ]
        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置
        {
            public char ch;
            public Point pt;
        }

        public ResultMode[] Mode;
        public enum ResultMode
        {
            StageFailed,
            StageCleared,
            StageDaiseikou
        }

        private CCounter ct表示用;
        private CCounter ctResult_Normal_Loop;
        private CCounter ctResult_In;
        public CCounter ct全体待機;
        private CCounter ctResult_Flower;
        private CCounter ctResult_Flower_In;
        private CCounter ctResult_Header_In;
        private CCounter ctMobIn0;
        private CCounter ctMobIn1;
        private CCounter ctMob0;
        private CCounter ctMob1;
        private CCounter ctOukan;
        private CCounter ctOukan_In;
        private CCounter ctFailed_Mob;
        private CCounter ctResult_Daiseikou_In;
        private CCounter ctResult_Daiseikou_Loop;
        private CCounter ctResult_Clear_Loop;
        private CCounter ctFailed_Loop;
        private CCounter ct吹き出しアニメーション;
        private CCounter ct吹き出しアニメーション_2P;
        private CCounter ct炎;
        private CCounter ct文字アニメ用;
        private CCounter ct待機用;
        protected CCounter ct虹アニメ;
        protected CCounter ct虹透明度;
        private int 文字アニメ用Timer = 30;
        private int 文字アニメ用回転回数 = 22;
        private int 文字アニメ用Timer2 = 600;
        private int n文字アニメ = 1;
        private bool bスコアアニメ終了;
        private bool bスコア待機終了;
        private bool bGreatアニメ終了;
        private bool bGreat待機終了;
        private bool bGoodアニメ終了;
        private bool bGood待機終了;
        private bool bBadアニメ終了;
        private bool bBad待機終了;
        private bool bComboアニメ終了;
        private bool bCombo待機終了;
        private bool bRollアニメ終了;
        private bool bRoll待機終了;
        private bool bHitアニメ終了;
        private bool bHit待機終了;
        private bool b回転音再生中;
        public bool b文字アニメ終了;
        private bool[] bOukan再生 = new bool[2];
        private bool[] b吹き出し再生 = new bool[2];
        private bool[] bBar_SE再生 = new bool[3];
        public CSound sound大成功音声;
        public CSound soundクリア音声;
        public CSound soundクリア失敗;
        public CSound sound惜しかった;
        private CSound soundFullCombo_In;
        private CSound soundClear_In;
        private CSound sound回転音;
        public CSound sound不合格;
        public CSound sound合格;
        public CSound sound金合格;
        public CSound soundBar_SE;
        private readonly ST文字位置[] st小文字位置;
        private ST文字位置[] stScoreFont;

        public CTexture Dan_Plate;

        private void t小文字表示(int x, int y, string str, ref bool b終了, ref bool b待機終了)
        {
            int width = 22;

            x -= width;
            foreach (char ch in str)
            {
                x += width;
            }

            str = 文字反転(str.Replace(" ", ""));

            if (!b文字アニメ終了)
            {
                if (b終了 && !b待機終了)
                {
                    if (b回転音再生中)
                    {
                        sound回転音.t再生を停止する();
                        b回転音再生中 = false;
                    }
                }
                else if (!b回転音再生中)
                {
                    sound回転音.t再生を開始する();
                    b回転音再生中 = true;
                }
            }
            else if (b回転音再生中)
            {
                sound回転音.t再生を停止する();
                b回転音再生中 = false;
            }

            if (!b終了 && b文字アニメ終了)
            {
                b終了 = true;
            }
            if (!b終了)
            {
                ct待機用.n現在の値 = 0;
            }
            else if (!ct待機用.b終了値に達した && !b待機終了)
            {
                ct文字アニメ用.n現在の値 = 0;
            }
            else if (!b待機終了)
            {
                b待機終了 = true;
            }

            if (ct文字アニメ用.b終了値に達した && !b終了)
            {
                ct文字アニメ用.n現在の値 = 0;
                if (str.Length <= n文字アニメ)
                {
                    n文字アニメ = 1;
                    TJAPlayer3.Skin.sound決定音.t再生する();
                    b終了 = true;
                }
                else
                {
                    n文字アニメ++;
                }
            }

            int n文字カウント = 0;

            foreach (char ch in str)
            {
                n文字カウント++;
                if (n文字カウント > n文字アニメ && !b終了)
                {
                    return;
                }

                for (int i = 0; i < st小文字位置.Length; i++)
                {
                    if (ch == ' ')
                    {
                        break;
                    }

                    if (st小文字位置[i].ch == ch)
                    {
                        Rectangle rectangle;
                        if (n文字カウント == n文字アニメ && !b終了)
                        {
                            rectangle = new Rectangle(st小文字位置[n0から9の値(ct文字アニメ用.n現在の値)].pt.X, st小文字位置[i].pt.Y, 32, 38);
                        }
                        else
                        {
                            rectangle = new Rectangle(st小文字位置[i].pt.X, st小文字位置[i].pt.Y, 32, 38);
                        }
                        TJAPlayer3.Tx.Result_Number?.t2D描画(TJAPlayer3.app.Device, x, y, rectangle);
                        break;
                    }
                }
                x -= width;
            }
        }
        protected void tスコア文字表示(int x, int y, string str, ref bool b終了, ref bool b待機終了)
        {
            str = 文字反転(str.Replace(" ", ""));

            if (!b文字アニメ終了)
            {
                if (b終了 && !b待機終了)
                {
                    if (b回転音再生中)
                    {
                        sound回転音.t再生を停止する();
                        b回転音再生中 = false;
                    }
                }
                else if (!b回転音再生中)
                {
                    sound回転音.t再生を開始する();
                    b回転音再生中 = true;
                }
            }
            else if (b回転音再生中)
            {
                sound回転音.t再生を停止する();
                b回転音再生中 = false;
            }

            if (!b終了 && b文字アニメ終了)
            {
                b終了 = true;
            }
            if (!b終了)
            {
                ct待機用.n現在の値 = 0;
            }
            else if (!ct待機用.b終了値に達した && !b待機終了)
            {
                ct文字アニメ用.n現在の値 = 0;
            }
            else if (!b待機終了)
            {
                b待機終了 = true;
            }
            if (ct文字アニメ用.b終了値に達した && !b終了)
            {
                ct文字アニメ用.n現在の値 = 0;
                if (str.Length <= n文字アニメ)
                {
                    n文字アニメ = 1;
                    TJAPlayer3.Skin.sound決定音.t再生する();
                    b終了 = true;
                }
                else
                {
                    n文字アニメ++;
                }
            }
            int width = 24;
            int n文字カウント = 0;

            x += width * 6;

            foreach (char ch in str)
            {
                n文字カウント++;
                if (n文字カウント > n文字アニメ && !b終了)
                {
                    return;
                }
                for (int i = 0; i < stScoreFont.Length; i++)
                {
                    if (stScoreFont[i].ch == ch)
                    {
                        Rectangle rectangle;
                        if (n文字カウント == n文字アニメ && !b終了)
                        {
                            rectangle = new Rectangle(stScoreFont[n0から9の値(ct文字アニメ用.n現在の値)].pt.X, 0, 24, 40);
                        }
                        else
                        {
                            rectangle = new Rectangle(stScoreFont[i].pt.X, 0, 24, 40);
                        }
                        TJAPlayer3.Tx.Result_Score_Number?.t2D描画(TJAPlayer3.app.Device, x, y, rectangle);
                        break;
                    }
                }
                x -= width;
            }
        }
        private string 文字反転(string str)
        {
            char[] 反転 = str.ToCharArray();
            Array.Reverse(反転);
            str = new string(反転);
            return str;
        }
        private int n0から9の値(int i)
        {
            double i2 = i;
            if (i2 >= 10)
            {
                i2 = Math.Floor(i2 / 10);
                i2 *= 10;
                i -= (int)i2;
            }
            return i;
        }
        //-----------------
        #endregion
    }
}
