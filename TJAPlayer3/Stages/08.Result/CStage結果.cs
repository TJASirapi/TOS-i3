using System;
using System.IO;
using System.Diagnostics;
using FDK;

namespace TJAPlayer3
{
    internal class CStage結果 : CStage
    {
        // プロパティ

        public STDGBVALUE<bool> b新記録スキル;
        public STDGBVALUE<bool> b新記録スコア;
        public STDGBVALUE<bool> b新記録ランク;
        public STDGBVALUE<float> fPerfect率;
        public STDGBVALUE<float> fGreat率;
        public STDGBVALUE<float> fMiss率;
        public STDGBVALUE<bool> bオート;        // #23596 10.11.16 add ikanick
                                             //        10.11.17 change (int to bool) ikanick
        public STDGBVALUE<int> nランク値;
        public STDGBVALUE<int> n演奏回数;
        public STDGBVALUE<CScoreIni.C演奏記録> st演奏記録;


        // コンストラクタ

        public CStage結果()
        {
            this.st演奏記録.Drums = new CScoreIni.C演奏記録();
            this.st演奏記録.Guitar = new CScoreIni.C演奏記録();
            this.st演奏記録.Bass = new CScoreIni.C演奏記録();
            this.st演奏記録.Taiko = new CScoreIni.C演奏記録();
            base.eステージID = CStage.Eステージ.結果;
            base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
            base.b活性化してない = true;
            base.list子Activities.Add(this.actParameterPanel = new CActResultParameterPanel());
            base.list子Activities.Add(this.actSongBar = new CActResultSongBar());
            base.list子Activities.Add(this.actFI = new CActFIFOResult());
            base.list子Activities.Add(this.actFO = new CActFIFOBlack());
        }


        // CStage 実装

        public override void On活性化()
        {
            Trace.TraceInformation("結果ステージを活性化します。");
            Trace.Indent();
            try
            {
                #region [ 初期化 ]
                //---------------------
                this.eフェードアウト完了時の戻り値 = E戻り値.継続;
                this.bアニメが完了 = false;
                this.bIsCheckedWhetherResultScreenShouldSaveOrNot = false;              // #24609 2011.3.14 yyagi
                for (int i = 0; i < 3; i++)
                {
                    this.b新記録スキル[i] = false;
                    this.b新記録スコア[i] = false;
                    this.b新記録ランク[i] = false;
                }
                //---------------------
                #endregion

                #region [ 結果の計算 ]
                //---------------------
                for (int i = 0; i < 3; i++)
                {
                    this.nランク値[i] = -1;
                    this.fPerfect率[i] = this.fGreat率[i] = this.fMiss率[i] = 0.0f;    // #28500 2011.5.24 yyagi
                    if ((((i != 0) || (TJAPlayer3.DTX.bチップがある.Drums))))
                    {
                        CScoreIni.C演奏記録 part = this.st演奏記録[i];
                        bool bIsAutoPlay = true;
                        switch (i)
                        {
                            case 0:
                                bIsAutoPlay = TJAPlayer3.ConfigIni.bAutoPlay[0];
                                break;

                            case 1:
                                bIsAutoPlay = TJAPlayer3.ConfigIni.bAutoPlay[0];
                                break;

                            case 2:
                                bIsAutoPlay = TJAPlayer3.ConfigIni.bAutoPlay[0];
                                break;
                        }
                        this.fPerfect率[i] = bIsAutoPlay ? 0f : ((100f * part.nPerfect数) / ((float)part.n全チップ数));
                        this.fGreat率[i] = bIsAutoPlay ? 0f : ((100f * part.nGreat数) / ((float)part.n全チップ数));
                        this.fMiss率[i] = bIsAutoPlay ? 0f : ((100f * part.nMiss数) / ((float)part.n全チップ数));
                        this.bオート[i] = bIsAutoPlay; // #23596 10.11.16 add ikanick そのパートがオートなら1
                                                    //        10.11.17 change (int to bool) ikanick
                        this.nランク値[i] = CScoreIni.tランク値を計算して返す(part);
                    }
                }
                //---------------------
                #endregion

                #region [ .score.ini の作成と出力 ]
                //---------------------
                string str = TJAPlayer3.DTX.strファイル名の絶対パス + ".score.ini";
                CScoreIni ini = new CScoreIni(str);

                bool[] b今までにフルコンボしたことがある = new bool[] { false, false, false };

                for (int i = 0; i < 3; i++)
                {
                    // フルコンボチェックならびに新記録ランクチェックは、ini.Record[] が、スコアチェックや演奏型スキルチェックの IF 内で書き直されてしまうよりも前に行う。(2010.9.10)

                    b今までにフルコンボしたことがある[i] = ini.stセクション[i * 2].bフルコンボである | ini.stセクション[i * 2 + 1].bフルコンボである;

                    // #24459 上記の条件だと[HiSkill.***]でのランクしかチェックしていないので、BestRankと比較するよう変更。
                    if (this.nランク値[i] >= 0 && ini.stファイル.BestRank[i] > this.nランク値[i])       // #24459 2011.3.1 yyagi update BestRank
                    {
                        this.b新記録ランク[i] = true;
                        ini.stファイル.BestRank[i] = this.nランク値[i];
                    }

                    // 新記録スコアチェック
                    if ((this.st演奏記録[i].nスコア > ini.stセクション[i * 2].nスコア) && !TJAPlayer3.ConfigIni.bAutoPlay[0])
                    {
                        this.b新記録スコア[i] = true;
                        ini.stセクション[i * 2] = this.st演奏記録[i];
                    }

                    // 新記録スキルチェック
                    if (this.st演奏記録[i].db演奏型スキル値 > ini.stセクション[(i * 2) + 1].db演奏型スキル値)
                    {
                        this.b新記録スキル[i] = true;
                        ini.stセクション[(i * 2) + 1] = this.st演奏記録[i];
                    }

                    // ラストプレイ #23595 2011.1.9 ikanick
                    // オートじゃなければプレイ結果を書き込む
                    if (TJAPlayer3.ConfigIni.bAutoPlay[0] == false)
                    {
                        ini.stセクション[i + 6] = this.st演奏記録[i];
                    }

                    // #23596 10.11.16 add ikanick オートじゃないならクリア回数を1増やす
                    //        11.02.05 bオート to t更新条件を取得する use      ikanick
                    bool[] b更新が必要か否か = new bool[3];
                    CScoreIni.t更新条件を取得する(out b更新が必要か否か[0], out b更新が必要か否か[1], out b更新が必要か否か[2]);

                    if (b更新が必要か否か[i])
                    {
                        switch (i)
                        {
                            case 0:
                                ini.stファイル.ClearCountDrums++;
                                break;
                            case 1:
                                ini.stファイル.ClearCountGuitar++;
                                break;
                            case 2:
                                ini.stファイル.ClearCountBass++;
                                break;
                            default:
                                throw new Exception("クリア回数増加のk(0-2)が範囲外です。");
                        }
                    }
                    //---------------------------------------------------------------------/
                }
                if (TJAPlayer3.ConfigIni.bScoreIniを出力する)
                    ini.t書き出し(str);
                //---------------------
                #endregion

                #region [ リザルト画面への演奏回数の更新 #24281 2011.1.30 yyagi]
                if (TJAPlayer3.ConfigIni.bScoreIniを出力する)
                {
                    this.n演奏回数.Drums = ini.stファイル.PlayCountDrums;
                    this.n演奏回数.Guitar = ini.stファイル.PlayCountGuitar;
                    this.n演奏回数.Bass = ini.stファイル.PlayCountBass;
                }
                #endregion
                #region [ 選曲画面の譜面情報の更新 ]
                //---------------------
                if (!TJAPlayer3.bコンパクトモード)
                {
                    Cスコア cスコア = TJAPlayer3.stage選曲.r確定されたスコア;
                    bool[] b更新が必要か否か = new bool[3];
                    CScoreIni.t更新条件を取得する(out b更新が必要か否か[0], out b更新が必要か否か[1], out b更新が必要か否か[2]);
                    for (int m = 0; m < 3; m++)
                    {
                        if (b更新が必要か否か[m])
                        {
                            // FullCombo した記録を FullCombo なしで超えた場合、FullCombo マークが消えてしまう。
                            // → FullCombo は、最新記録と関係なく、一度達成したらずっとつくようにする。(2010.9.11)
                            cスコア.譜面情報.フルコンボ[m] = this.st演奏記録[m].bフルコンボである | b今までにフルコンボしたことがある[m];

                            if (this.b新記録スキル[m])
                            {
                                cスコア.譜面情報.最大スキル[m] = this.st演奏記録[m].db演奏型スキル値;
                            }

                            if (this.b新記録ランク[m])
                            {
                                cスコア.譜面情報.最大ランク[m] = this.nランク値[m];
                            }
                        }
                    }
                }
                //---------------------
                #endregion

                // Discord Presenseの更新
                Discord.UpdatePresence(TJAPlayer3.DTX.TITLE + ".tja", Properties.Discord.Stage_Result + (TJAPlayer3.ConfigIni.bAutoPlay[0] == true ? " (" + Properties.Discord.Info_IsAuto + ")" : ""), TJAPlayer3.StartupTime);

                base.On活性化();
            }
            finally
            {
                Trace.TraceInformation("結果ステージの活性化を完了しました。");
                Trace.Unindent();
            }
        }
        public override void On非活性化()
        {
            if (this.rResultSound != null)
            {
                TJAPlayer3.Sound管理.tサウンドを破棄する(this.rResultSound);
                this.rResultSound = null;
            }
            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                if (TJAPlayer3.Tx.PlateEffect != null)
                {
                    this.ctNamePlateEffect = new CCounter(0, TJAPlayer3.Skin.PlateEffect_Ptn - 1, TJAPlayer3.Skin.PlateEffect_Timer, TJAPlayer3.Timer);
                }
                this.ctDan_2nd = new CCounter();
                this.ct段位時登場時透明度 = new CCounter();
                soundScroll_SE = TJAPlayer3.Sound管理.tサウンドを生成する(CSkin.Path(@"Sounds\Dan\Scroll_SE.ogg"), ESoundGroup.SoundEffect);
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                if (this.ct登場用 != null)
                {
                    this.ct登場用 = null;
                }
                rResultSound?.Dispose();
                TJAPlayer3.t安全にDisposeする(ref ct登場用);
                TJAPlayer3.t安全にDisposeする(ref ctNamePlateEffect);
                TJAPlayer3.t安全にDisposeする(ref ctDan_2nd);
                TJAPlayer3.t安全にDisposeする(ref ct段位時登場時透明度);
                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {
            if (!base.b活性化してない)
            {
                //int num;
                if (base.b初めての進行描画)
                {
                    this.ct登場用 = new CCounter(0, 100, 5, TJAPlayer3.Timer);
                    this.actFI.tフェードイン開始();
                    base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
                    if (this.rResultSound != null)
                    {
                        this.rResultSound.t再生を開始する();
                    }
                    this.b再生済み = false;
                    base.b初めての進行描画 = false;
                }
                this.bアニメが完了 = true;
                if (this.ct登場用.b進行中)
                {
                    this.ct登場用.t進行();
                    if (this.ct登場用.b終了値に達した)
                    {
                        this.ct登場用.t停止();
                    }
                    else
                    {
                        this.bアニメが完了 = false;
                    }
                }

                this.ctNamePlateEffect.t進行Loop();

                // 描画
                if (TJAPlayer3.stage選曲.n確定された曲の難易度 == (int)Difficulty.Dan)
                {
                    TJAPlayer3.Tx.Result_Background_Dan?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                }
                else
                {
                    if (TJAPlayer3.ConfigIni.Change2PSkin == true)
                    {
                        TJAPlayer3.Tx.Result_Background[2]?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                    }
                    else
                    {
                        TJAPlayer3.Tx.Result_Background[0]?.t2D描画(TJAPlayer3.app.Device, 0, 0);
                    }
                }

                if (TJAPlayer3.stage選曲.n確定された曲の難易度 == (int)Difficulty.Dan && TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 >= 4000)
                {
                    if (b再生済み != true)
                    {
                        this.soundScroll_SE.t再生を開始する();
                        b再生済み = true;
                    }
                    if (ctDan_2nd.b停止中) this.ctDan_2nd.t開始(0, 512, 1, TJAPlayer3.Timer);
                    this.ctDan_2nd?.t進行();
                    TJAPlayer3.Tx.Result_Background_Dan_2nd?.t2D描画(TJAPlayer3.app.Device, 1280f - this.ctDan_2nd.n現在の値 * 2.5f, 0);
                }

                if (TJAPlayer3.stage選曲.n確定された曲の難易度 == (int)Difficulty.Dan)
                {
                    if (TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 >= 3400)
                    {
                        if (ct段位時登場時透明度.b停止中) this.ct段位時登場時透明度.t開始(0, 255, 2, TJAPlayer3.Timer);
                        this.ct段位時登場時透明度?.t進行();
                    }
                }

                if (TJAPlayer3.stage選曲.n確定された曲の難易度 == (int)Difficulty.Dan)
                {
                    if (TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 >= 3500)
                    {
                        TJAPlayer3.Tx.Result_Header_Dan.n透明度 = ct段位時登場時透明度.n現在の値;
                        TJAPlayer3.Tx.Result_Header_Dan.t2D描画(TJAPlayer3.app.Device, 0, 0);
                    }
                }
                else
                {
                    if (TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 >= 4000)
                    {
                        TJAPlayer3.Tx.Result_Header.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                        TJAPlayer3.Tx.Result_Header.t2D描画(TJAPlayer3.app.Device, 0, 0);
                    }
                }


                if (this.actParameterPanel.On進行描画() == 0)
                {
                    this.bアニメが完了 = false;
                }

                if (this.actSongBar.On進行描画() == 0)
                {
                    this.bアニメが完了 = false;
                }

                #region ネームプレート
                if (TJAPlayer3.stage選曲.n確定された曲の難易度 == (int)Difficulty.Dan)
                {
                    if (TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 >= 3500)
                    {
                        if (TJAPlayer3.Tx.NamePlate[0] != null)
                        {
                            TJAPlayer3.Tx.NamePlate[0].n透明度 = ct段位時登場時透明度.n現在の値;
                            TJAPlayer3.Tx.NamePlate[0].t2D描画(TJAPlayer3.app.Device, -6, 583);
                        }
                        TJAPlayer3.Tx.NamePlate[0].n透明度 = ct段位時登場時透明度.n現在の値;
                        TJAPlayer3.Tx.nP[0].n透明度 = ct段位時登場時透明度.n現在の値;
                        TJAPlayer3.Tx.Taiko_PlateText.n透明度 = ct段位時登場時透明度.n現在の値;
                        TJAPlayer3.Tx.PlateEffect[this.ctNamePlateEffect.n現在の値]?.t2D描画(TJAPlayer3.app.Device, 29, 583 - 34);
                        TJAPlayer3.Tx.nP[0]?.t2D描画(TJAPlayer3.app.Device, -13, 585);
                        TJAPlayer3.Tx.Taiko_PlateText?.t2D描画(TJAPlayer3.app.Device, 77, 583);
                    }
                }
                else
                {
                    if (TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 >= 4000)
                    {
                        for (int i = 0; i < TJAPlayer3.ConfigIni.nPlayerCount; i++)
                        {
                            if (TJAPlayer3.Tx.NamePlate[i] != null)
                            {
                                TJAPlayer3.Tx.NamePlate[i].n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                                TJAPlayer3.Tx.NamePlate[i].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_NamePlate_X[i], TJAPlayer3.Skin.Result_NamePlate_Y[i]);
                            }
                            TJAPlayer3.Tx.NamePlate[i].n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                            TJAPlayer3.Tx.nP[i].n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                            TJAPlayer3.Tx.Taiko_PlateText.n透明度 = TJAPlayer3.stage結果.actSongBar.ct登場時透明度.n現在の値;
                            TJAPlayer3.Tx.PlateEffect[ctNamePlateEffect.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_NamePlate_X[i] + 35, TJAPlayer3.Skin.Result_NamePlate_Y[i] - 34);
                            TJAPlayer3.Tx.nP[i]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_NamePlate_X[i] - 7, TJAPlayer3.Skin.Result_NamePlate_Y[i] + 2);
                            TJAPlayer3.Tx.Taiko_PlateText?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Result_NamePlate_X[i] + 83, TJAPlayer3.Skin.Result_NamePlate_Y[i]);
                        }
                    }
                }
                #endregion

                if (base.eフェーズID == CStage.Eフェーズ.共通_フェードイン)
                {
                    if (this.actFI.On進行描画() != 0)
                    {
                        base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
                    }
                }
                else if ((base.eフェーズID == CStage.Eフェーズ.共通_フェードアウト))         //&& ( this.actFO.On進行描画() != 0 ) )
                {
                    return (int)this.eフェードアウト完了時の戻り値;
                }
                #region [ #24609 2011.3.14 yyagi ランク更新or演奏型スキル更新時、リザルト画像をpngで保存する ]
                if (this.bアニメが完了 == true && this.bIsCheckedWhetherResultScreenShouldSaveOrNot == false  // #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
                    && TJAPlayer3.ConfigIni.bScoreIniを出力する
                    && TJAPlayer3.ConfigIni.bIsAutoResultCapture)                                               // #25399 2011.6.9 yyagi
                {
                    CheckAndSaveResultScreen(true);
                    this.bIsCheckedWhetherResultScreenShouldSaveOrNot = true;
                }
                #endregion

                // キー入力

                if (TJAPlayer3.act現在入力を占有中のプラグイン == null)
                {
                    if (((TJAPlayer3.Pad.b押されたDGB(Eパッド.CY) || TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RD)) || (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC) || TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return))) && !this.bアニメが完了)
                    {
                        this.actFI.tフェードイン完了();                 // #25406 2011.6.9 yyagi
                        this.actParameterPanel.tアニメを完了させる();
                        this.actSongBar.tアニメを完了させる();
                        this.ct登場用.t停止();
                    }
                    if (base.eフェーズID == CStage.Eフェーズ.共通_通常状態)
                    {
                        if (TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Escape))
                        {
                            TJAPlayer3.Skin.sound取消音.t再生する();
                            TJAPlayer3.Skin.sound成績発表.t停止する();
                            TJAPlayer3.Skin.sound成績発表.n位置_現在のサウンド = 0;
                            TJAPlayer3.Skin.sound結果発表.t停止する();
                            TJAPlayer3.Skin.sound結果発表.n位置_現在のサウンド = 0;
                            this.actFO.tフェードアウト開始();
                            base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                            this.eフェードアウト完了時の戻り値 = E戻り値.完了;
                        }
                        if (TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 >= 6000)
                        {
                            if ((TJAPlayer3.Pad.b押されたDGB(Eパッド.Decide) || (TJAPlayer3.Pad.b押されたDGB(Eパッド.LRed) || TJAPlayer3.Pad.b押されたDGB(Eパッド.RRed)) || TJAPlayer3.Pad.b押されたDGB(Eパッド.CY) || TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.RD)) || (TJAPlayer3.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC) || (TJAPlayer3.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && TJAPlayer3.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return))) && this.bアニメが完了)
                            {
                                TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n現在の値 = TJAPlayer3.stage結果.actParameterPanel.ct全体待機.n終了値;
                                if (!TJAPlayer3.stage結果.actParameterPanel.b文字アニメ終了)
                                {
                                    TJAPlayer3.stage結果.actParameterPanel.b文字アニメ終了 = true;
                                }
                                else
                                {
                                    TJAPlayer3.stage結果.actParameterPanel.sound大成功音声.t再生を停止する();
                                    TJAPlayer3.stage結果.actParameterPanel.soundクリア音声.t再生を停止する();
                                    TJAPlayer3.stage結果.actParameterPanel.soundクリア失敗.t再生を停止する();
                                    TJAPlayer3.stage結果.actParameterPanel.sound惜しかった.t再生を停止する();
                                    TJAPlayer3.stage結果.actParameterPanel.sound不合格.t再生を停止する();
                                    TJAPlayer3.stage結果.actParameterPanel.sound合格.t再生を停止する();
                                    TJAPlayer3.stage結果.actParameterPanel.sound金合格.t再生を停止する();
                                    TJAPlayer3.Skin.sound取消音.t再生する();
                                    TJAPlayer3.Skin.sound成績発表.t停止する();
                                    TJAPlayer3.Skin.sound成績発表.n位置_現在のサウンド = 0;
                                    TJAPlayer3.Skin.sound結果発表.t停止する();
                                    TJAPlayer3.Skin.sound結果発表.n位置_現在のサウンド = 0;
                                    base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                                    this.eフェードアウト完了時の戻り値 = E戻り値.完了;
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }

        public enum E戻り値 : int
        {
            継続,
            完了
        }


        // その他

        #region [ private ]
        //-----------------
        private CCounter ct登場用;
        private E戻り値 eフェードアウト完了時の戻り値;
        private CActFIFOResult actFI;
        private CActFIFOBlack actFO;
        public CActResultParameterPanel actParameterPanel;
        public CActResultSongBar actSongBar;
        private bool bアニメが完了;
        private bool bIsCheckedWhetherResultScreenShouldSaveOrNot;				// #24509 2011.3.14 yyagi
        private bool b再生済み;
        private CSound rResultSound;
        private CSound soundScroll_SE;
        private CCounter ctNamePlateEffect;
        private CCounter ctDan_2nd;
        private CCounter ct段位時登場時透明度;

        #region [ #24609 リザルト画像をpngで保存する ]		// #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
        /// <summary>
        /// リザルト画像のキャプチャと保存。
        /// 自動保存モード時は、ランク更新or演奏型スキル更新時に自動保存。
        /// 手動保存モード時は、ランクに依らず保存。
        /// </summary>
        /// <param name="bIsAutoSave">true=自動保存モード, false=手動保存モード</param>
        private void CheckAndSaveResultScreen(bool bIsAutoSave)
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (bIsAutoSave)
            {
                // リザルト画像を自動保存するときは、dtxファイル名.yyMMddHHmmss_DRUMS_SS.png という形式で保存。
                for (int i = 0; i < 3; i++)
                {
                    if (this.b新記録ランク[i] == true || this.b新記録スキル[i] == true)
                    {
                        string strPart = ((E楽器パート)(i)).ToString();
                        string strRank = ((CScoreIni.ERANK)(this.nランク値[i])).ToString();
                        string strFullPath = TJAPlayer3.DTX.strファイル名の絶対パス + "." + datetime + "_" + strPart + "_" + strRank + ".png";
                        TJAPlayer3.app.SaveResultScreen(strFullPath);
                    }
                }
            }
        }
        #endregion
        //-----------------
        #endregion
    }
}
