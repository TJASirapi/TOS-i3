using FDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJAPlayer3
{
    class TextureLoader
    {
        const string BASE = @"Graphics\";

        // Stage
        const string TITLE = @"1_Title\";
        const string CONFIG = @"2_Config\";
        const string SONGSELECT = @"3_SongSelect\";
        const string SONGLOADING = @"4_SongLoading\";
        const string GAME = @"5_Game\";
        const string RESULT = @"6_Result\";
        const string EXIT = @"7_Exit\";

        // InGame
        const string CHARA = @"1_Chara\";
        const string DANCER = @"2_Dancer\";
        const string MOB = @"3_Mob\";
        const string COURSESYMBOL = @"4_CourseSymbol\";
        const string BACKGROUND = @"5_Background\";
        const string TAIKO = @"6_Taiko\";
        const string GAUGE = @"7_Gauge\";
        const string FOOTER = @"8_Footer\";
        const string END = @"9_End\";
        const string EFFECTS = @"10_Effects\";
        const string BALLOON = @"11_Balloon\";
        const string LANE = @"12_Lane\";
        const string GENRE = @"13_Genre\";
        const string GAMEMODE = @"14_GameMode\";
        const string FAILED = @"15_Failed\";
        const string RUNNER = @"16_Runner\";
        const string PUCHICHARA = @"18_PuchiChara\";
        const string DANC = @"17_DanC\";

        // InGame_Effects
        const string FIRE = @"Fire\";
        const string HIT = @"Hit\";
        const string ROLL = @"Roll\";
        const string SPLASH = @"Splash\";


        public TextureLoader()
        {
            // コンストラクタ

        }

        internal CTexture TxC(string FileName)
        {
            return TJAPlayer3.tテクスチャの生成(CSkin.Path(BASE + FileName));
        }
        internal CTextureAf TxCAf(string FileName)
        {
            return TJAPlayer3.tテクスチャの生成Af(CSkin.Path(BASE + FileName));
        }
        internal CTexture TxCGen(string FileName)
        {
            return TJAPlayer3.tテクスチャの生成(CSkin.Path(BASE + GAME + GENRE + FileName + ".png"));
        }

        public void LoadTexture()
        {
            #region 共通
            BlackOut = TxC(@"BlackOut.png");
            Tile_Black = TxC(@"Tile_Black.png");
            Tile_White = TxC(@"Tile_White.png");
            Menu_Title = TxC(@"Menu_Title.png");
            Menu_Highlight = TxC(@"Menu_Highlight.png");
            Enum_Song = TxC(@"Enum_Song.png");
            Scanning_Loudness = TxC(@"Scanning_Loudness.png");
            Overlay = TxC(@"Overlay.png");
            NamePlate = new CTexture[2];
            NamePlate[0] = TxC(@"1P_NamePlate.png");
            NamePlate[1] = TxC(@"2P_NamePlate.png");
            nP = new CTexture[2];
            nP[0] = TxC(@"1P.png");
            nP[1] = TxC(@"2P.png");
            TJAPlayer3.Skin.PlateEffect_Ptn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + @"PlateEffect\"));
            if (TJAPlayer3.Skin.PlateEffect_Ptn != 0)
            {
                PlateEffect = new CTexture[TJAPlayer3.Skin.PlateEffect_Ptn];
                for (int i = 0; i < TJAPlayer3.Skin.PlateEffect_Ptn; i++)
                {
                    PlateEffect[i] = TxC(@"PlateEffect\" + i.ToString() + ".png");
                }
            }
            #endregion
            #region 1_タイトル画面
            Title_Background = TxC(TITLE + @"Background.png");
            Title_Menu = TxC(TITLE + @"Menu.png");
            #endregion

            #region 2_コンフィグ画面
            Config_Background = TxC(CONFIG + @"Background.png");
            Config_Cursor = TxC(CONFIG + @"Cursor.png");
            Config_ItemBox = TxC(CONFIG + @"ItemBox.png");
            Config_Arrow = TxC(CONFIG + @"Arrow.png");
            Config_KeyAssign = TxC(CONFIG + @"KeyAssign.png");
            Config_Font = TxC(CONFIG + @"Font.png");
            Config_Font_Bold = TxC(CONFIG + @"Font_Bold.png");
            Config_Enum_Song = TxC(CONFIG + @"Enum_Song.png");
            #endregion

            #region 3_選曲画面
            SongSelect_Background = TxC(SONGSELECT + @"Background.png");
            SongSelect_Bar_Center05 = TxC(SONGSELECT + @"Bar_Center_0.5.png");
            SongSelect_Bar_Center_Dan = TxC(SONGSELECT + @"Bar_Center_Dan.png");
            SongSelect_Header = TxC(SONGSELECT + @"Header.png");
            SongSelect_Header_SongSelect = TxC(SONGSELECT + @"Header_SongSelect.png");
            SongSelect_Header_Red = TxC(SONGSELECT + @"Header_Red.png");
            SongSelect_Footer = TxC(SONGSELECT + @"Footer.png");
            SongSelect_Footer_1 = TxC(SONGSELECT + @"Footer_1.png");
            SongSelect_Difficulty = TxC(SONGSELECT + @"Difficulty.png");
            SongSelect_Auto = TxC(SONGSELECT + @"Auto.png");
            SongSelect_Support = TxC(SONGSELECT + @"Support.png");
            SongSelect_ScoreWindow_3_3 = TxC(SONGSELECT + @"ScoreWindow_3_3.png");
            SongSelect_Level = TxC(SONGSELECT + @"Level.png");
            SongSelect_Branch = TxC(SONGSELECT + @"Branch.png");
            SongSelect_Branch_Text = TxC(SONGSELECT + @"Branch_Text.png");
            SongSelect_Frame_Score = TxC(SONGSELECT + @"Frame_Score.png");
            SongSelect_Frame_Box = TxC(SONGSELECT + @"Frame_Box.png");
            SongSelect_Frame_BackBox = TxC(SONGSELECT + @"Frame_BackBox.png");
            SongSelect_Frame_Random = TxC(SONGSELECT + @"Frame_Random.png");
            SongSelect_Score_Select = TxC(SONGSELECT + @"Score_Select.png");
            //SongSelect_Frame_Dani = TxC(SONGSELECT + @"Frame_Dani.png");
            SongSelect_GenreText = TxC(SONGSELECT + @"GenreText.png");
            SongSelect_Cursor_Left = TxC(SONGSELECT + @"Cursor_Left.png");
            SongSelect_Cursor_Right = TxC(SONGSELECT + @"Cursor_Right.png");
            SongSelect_Bar_BackBox = TxC(SONGSELECT + @"Bar_Box_BackBox.png");
            SongSelect_Bar_Center_BackBox = TxC(SONGSELECT + @"Bar_Center_BackBox.png");
            SongSelect_Bar_Center_BackBox_Dan = TxC(SONGSELECT + @"Bar_Center_BackBox_Dan.png");
            SongSelect_taiko = TxC(SONGSELECT + @"taiko_Effect.png");
            SongSelect_2PCoin = TxC(SONGSELECT + @"2P_1Coin.png");
            SongSelect_Timer100 = TxC(SONGSELECT + @"3_Timer\100.png");
            for (int i = 0; i < 5; i++)
            {
                SongSelect_Bar_Genre_Dan[i] = TxC(SONGSELECT + @"Genre_Bar_Dan_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 9; i++)
            {
                SongSelect_Chara_Start_Song[i] = TxC(SONGSELECT + @"5_Start_Song_Chara\" + i.ToString() + ".png");
            }
            for (int i = 0; i < 11; i++)
            {
                SongSelect_Bar_Genre[i] = TxC(SONGSELECT + @"Bar_Genre_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 11; i++)
            {
                SongSelect_Bar_Box[i] = TxC(SONGSELECT + @"Bar_Box_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 10; i++)
            {
                SongSelect_Timer[i] = TxC(SONGSELECT + @"3_Timer\" + i.ToString() + ".png");
            }
            for (int i = 0; i < 27; i++)
            {
                SongSelect_Chara[i] = TxC(SONGSELECT + @"2_Chara\" + i.ToString() + ".png");
            }
            for (int i = 0; i < 27; i++)
            {
                SongSelect_Inchara[i] = TxC(SONGSELECT + @"1_InChara\" + i.ToString() + ".png");
            }
            for (int i = 0; i < 10; i++)
            {
                SongSelect_Timerw[i] = TxC(SONGSELECT + @"3_Timer\" + i.ToString() + "w.png");
            }
            for (int i = 0; i < 10; i++)
            {
                SongSelect_Bar_Center_Header[i] = TxC(SONGSELECT + @"Bar_Center_Header_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 10; i++)
            {
                SongSelect_Bar_Center_BOX[i] = TxC(SONGSELECT + @"Bar_Center_BOX_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 10; i++)
            {
                SongSelect_Bar_Center_Text[i] = TxC(SONGSELECT + @"Bar_BOX_文字_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 12; i++)
            {
                SongSelect_Bar_Center[i] = TxC(SONGSELECT + @"Bar_Center_" + i.ToString() + ".png");
            }
            SongSelect_Bar_Center_NOT = TxC(SONGSELECT + @"BOX_Not.png");

            for (int i = 0; i < (int)Difficulty.Total; i++)
            {
                SongSelect_ScoreWindow[i] = TxC(SONGSELECT + @"ScoreWindow_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 10; i++)
            {
                SongSelect_GenreBack[i] = TxC(SONGSELECT + @"GenreBackground_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 11; i++)
            {
                SongSelect_BoxBack[i] = TxC(SONGSELECT + @"BoxBack_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 11; i++)
            {
                SongSelect_BoxBackC[i] = TxC(SONGSELECT + @"BoxBackC_" + i.ToString() + ".png");
            }
            SongSelect_ScoreWindow_Text = TxC(SONGSELECT + @"ScoreWindow_Text.png");

            #region 難易度選択画面
            SongSelect_Difficulty_Dan_Box = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Dan_Box.png");
            SongSelect_Difficulty_Dan_Select = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Dan_Select_Box.png");
            SongSelect_Difficulty_Dan_Select_Effect = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Dan_Select_Effect_Box.png");
            SongSelect_Difficulty_Bar = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Bar.png");
            SongSelect_Difficulty_Option = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Option.png");
            SongSelect_Difficulty_Cursor = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_cursor.png");
            SongSelect_Difficulty_Bar_Back = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Bar_Back.png");
            SongSelect_Header_Difficulty = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Header_difficulty.png");
            SongSelect_Difficulty_Bar_Effect = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Bar_Effect.png");
            SongSelect_Difficulty_Bar_Back_Effect = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Bar_Back_Effect.png");
            SongSelect_Difficulty_BOX = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_BOX.png");
            SongSelect_Difficulty_BOX_Shadow = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_BOX_Shadow.png");
            SongSelect_Difficulty_Branch = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Branch.png");
            SongSelect_Difficulty_Branch_Edit = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Branch_Edit.png");
            SongSelect_Difficulty_Select_Back = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Select_Back.png");
            SongSelect_Difficulty_Select_Option = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Select_Option.png");
            SongSelect_Difficulty_Select_SE = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Select_SE.png");
            for (int i = 0; i < 6; i++)
            {
                SongSelect_Difficulty_mark_Select[i] = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_mark_Select_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 6; i++)
            {
                SongSelect_Difficulty_mark_Select_White[i] = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_mark_Select_White_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 6; i++)
            {
                SongSelect_Difficulty_mark[i] = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_mark_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 5; i++)
            {
                SongSelect_Difficulty_Select[i] = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Select_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 8; i++)
            {
                SongSelect_Difficulty_Select_Switch[i] = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Select_Switch_" + i.ToString() + ".png");
            }
            SongSelect_Difficulty_Star = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Star.png");
            SongSelect_Difficulty_Star_Edit = TxC(SONGSELECT + @"4_Difficulty_Select\" + @"Difficulty_Star_Edit.png");
            SongSelect_Difficulty_Sin = TxC(SONGSELECT + @"Song_Option\" + @"Sin.png");
            SongSelect_Difficulty_2Speed = TxC(SONGSELECT + @"Song_Option\" + @"2Speed.png");
            SongSelect_Difficulty_3Speed = TxC(SONGSELECT + @"Song_Option\" + @"3Speed.png");
            SongSelect_Difficulty_4Speed = TxC(SONGSELECT + @"Song_Option\" + @"4Speed.png");
            SongSelect_Difficulty_Doron = TxC(SONGSELECT + @"Song_Option\" + @"Doron.png");
            SongSelect_Difficulty_Mirror = TxC(SONGSELECT + @"Song_Option\" + @"Mirror.png");
            SongSelect_Difficulty_Super_Random = TxC(SONGSELECT + @"Song_Option\" + @"Super_Random.png");
            SongSelect_Difficulty_Hyper_Random = TxC(SONGSELECT + @"Song_Option\" + @"Hyper_Random.png");
            #endregion
            #endregion

            #region 4_読み込み画面
            SongLoading_Chara = TxC(SONGLOADING + @"Chara.png");
            SongLoading_Plate = TxC(SONGLOADING + @"Plate.png");
            SongLoading_FadeIn = TxC(SONGLOADING + @"FadeIn.png");
            SongLoading_Dan_FadeIn = TxC(SONGLOADING + @"Dan_FadeIn.png");
            SongLoading_FadeOut = TxC(SONGLOADING + @"FadeOut.png");
            #endregion

            #region 5_演奏画面
            #region 共通
            Notes = TxC(GAME + @"Notes.png");
            Judge_Frame = TxC(GAME + @"Notes.png");
            SENotes = TxC(GAME + @"SENotes.png");
            Notes_Arm = TxC(GAME + @"Notes_Arm.png");
            Judge = TxC(GAME + @"Judge.png");

            Judge_Meter = TxC(GAME + @"Judge_Meter.png");
            Bar = TxC(GAME + @"Bar.png");
            Bar_Branch = TxC(GAME + @"Bar_Branch.png");
            ACHIEVEMENT = TxC(GAME + @"ACHIEVEMENT.png");
            #endregion
            #region キャラクター
            TJAPlayer3.Skin.Game_Chara_Ptn_Normal = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"Normal\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Normal != 0)
            {
                Chara_Normal = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_Normal];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Normal; i++)
                {
                    Chara_Normal[i] = TxC(GAME + CHARA + @"Normal\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_Clear = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"Clear\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Clear != 0)
            {
                Chara_Normal_Cleared = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_Clear];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Clear; i++)
                {
                    Chara_Normal_Cleared[i] = TxC(GAME + CHARA + @"Clear\" + i.ToString() + ".png");
                }
            }
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Clear != 0)
            {
                Chara_Normal_Maxed = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_Clear];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Clear; i++)
                {
                    Chara_Normal_Maxed[i] = TxC(GAME + CHARA + @"Clear_Max\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_GoGo = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"GoGo\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_GoGo != 0)
            {
                Chara_GoGoTime = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_GoGo];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_GoGo; i++)
                {
                    Chara_GoGoTime[i] = TxC(GAME + CHARA + @"GoGo\" + i.ToString() + ".png");
                }
            }
            if (TJAPlayer3.Skin.Game_Chara_Ptn_GoGo != 0)
            {
                Chara_GoGoTime_Maxed = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_GoGo];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_GoGo; i++)
                {
                    Chara_GoGoTime_Maxed[i] = TxC(GAME + CHARA + @"GoGo_Max\" + i.ToString() + ".png");
                }
            }

            TJAPlayer3.Skin.Game_Chara_Ptn_10combo = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"10combo\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_10combo != 0)
            {
                Chara_10Combo = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_10combo];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_10combo; i++)
                {
                    Chara_10Combo[i] = TxC(GAME + CHARA + @"10combo\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_10combo_Max = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"10combo_Max\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_10combo_Max != 0)
            {
                Chara_10Combo_Maxed = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_10combo_Max];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_10combo_Max; i++)
                {
                    Chara_10Combo_Maxed[i] = TxC(GAME + CHARA + @"10combo_Max\" + i.ToString() + ".png");
                }
            }

            TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"GoGoStart\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart != 0)
            {
                Chara_GoGoStart = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart; i++)
                {
                    Chara_GoGoStart[i] = TxC(GAME + CHARA + @"GoGoStart\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart_Max = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"GoGoStart_Max\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart_Max != 0)
            {
                Chara_GoGoStart_Maxed = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart_Max];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart_Max; i++)
                {
                    Chara_GoGoStart_Maxed[i] = TxC(GAME + CHARA + @"GoGoStart_Max\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_ClearIn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"ClearIn\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_ClearIn != 0)
            {
                Chara_Become_Cleared = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_ClearIn];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_ClearIn; i++)
                {
                    Chara_Become_Cleared[i] = TxC(GAME + CHARA + @"ClearIn\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_SoulIn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"SoulIn\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_SoulIn != 0)
            {
                Chara_Become_Maxed = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_SoulIn];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_SoulIn; i++)
                {
                    Chara_Become_Maxed[i] = TxC(GAME + CHARA + @"SoulIn\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Breaking = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"Balloon_Breaking\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Breaking != 0)
            {
                Chara_Balloon_Breaking = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Breaking];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Breaking; i++)
                {
                    Chara_Balloon_Breaking[i] = TxC(GAME + CHARA + @"Balloon_Breaking\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"Balloon_Broke\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke != 0)
            {
                Chara_Balloon_Broke = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke; i++)
                {
                    Chara_Balloon_Broke[i] = TxC(GAME + CHARA + @"Balloon_Broke\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + CHARA + @"Balloon_Miss\"));
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss != 0)
            {
                Chara_Balloon_Miss = new CTexture[TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss; i++)
                {
                    Chara_Balloon_Miss[i] = TxC(GAME + CHARA + @"Balloon_Miss\" + i.ToString() + ".png");
                }
            }
            #endregion
            #region 踊り子
            TJAPlayer3.Skin.Game_Dancer_Ptn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + DANCER + @"1\"));
            if (TJAPlayer3.Skin.Game_Dancer_Ptn != 0)
            {
                Dancer = new CTexture[5][];
                for (int i = 0; i < 5; i++)
                {
                    Dancer[i] = new CTexture[TJAPlayer3.Skin.Game_Dancer_Ptn];
                    for (int p = 0; p < TJAPlayer3.Skin.Game_Dancer_Ptn; p++)
                    {
                        Dancer[i][p] = TxC(GAME + DANCER + (i + 1) + @"\" + p.ToString() + ".png");
                    }
                }
            }
            #endregion
            #region モブ
            TJAPlayer3.Skin.Game_Mob_Ptn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + MOB));
            Mob = new CTexture[TJAPlayer3.Skin.Game_Mob_Ptn];
            for (int i = 0; i < TJAPlayer3.Skin.Game_Mob_Ptn; i++)
            {
                Mob[i] = TxC(GAME + MOB + i.ToString() + ".png");
            }
            #endregion
            #region フッター
            Mob_Footer = TxC(GAME + FOOTER + @"0.png");
            #endregion
            #region 背景
            Background = TxC(GAME + Background + @"0\" + @"Background.png");
            Background_Up = new CTexture[4];
            Background_Up[0] = TxC(GAME + BACKGROUND + @"0\" + @"1P_Up.png");
            Background_Up[1] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Up.png");
            Background_Up[2] = TxC(GAME + BACKGROUND + @"0\" + @"1P_Up_Dan.png");
            Background_Up[3] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Change_Up.png");
            Background_Up_2nd = new CTexture[3];
            Background_Up_2nd[0] = TxC(GAME + BACKGROUND + @"0\" + @"1P_Up_2nd.png");
            Background_Up_2nd[1] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Up_2nd.png");
            Background_Up_2nd[2] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Change_Up_2nd.png");
            Background_Up_3rd = new CTexture[3];
            Background_Up_3rd[0] = TxC(GAME + BACKGROUND + @"0\" + @"1P_Up_3rd.png");
            Background_Up_3rd[1] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Up_3rd.png");
            Background_Up_3rd[2] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Change_Up_3rd.png");
            Background_Up_Clear = new CTexture[2];
            Background_Up_Clear[0] = TxC(GAME + BACKGROUND + @"0\" + @"1P_Up_Clear.png");
            Background_Up_Clear[1] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Up_Clear.png");
            Background_Up_Clear_2nd = new CTexture[2];
            Background_Up_Clear_2nd[0] = TxC(GAME + BACKGROUND + @"0\" + @"1P_Up_Clear_2nd.png");
            Background_Up_Clear_2nd[1] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Up_Clear_2nd.png");
            Background_Up_Clear_3rd = new CTexture[2];
            Background_Up_Clear_3rd[0] = TxC(GAME + BACKGROUND + @"0\" + @"1P_Up_Clear_3rd.png");
            Background_Up_Clear_3rd[1] = TxC(GAME + BACKGROUND + @"0\" + @"2P_Up_Clear_3rd.png");
            Background_Up_Miss = TxC(GAME + BACKGROUND + @"0\" + @"Up_Miss.png");
            Background_Down = TxC(GAME + BACKGROUND + @"0\" + @"Down.png");
            Background_Down_2nd = TxC(GAME + BACKGROUND + @"0\" + @"Down_2nd.png");
            Background_Down_Clear = TxC(GAME + BACKGROUND + @"0\" + @"Down_Clear.png");
            Background_Down_Clear_2nd = TxC(GAME + BACKGROUND + @"0\" + @"Down_Clear_2nd.png");
            Background_Down_Clear_3rd = TxC(GAME + BACKGROUND + @"0\" + @"Down_Clear_3rd.png");
            Background_Down_Clear_4th = TxC(GAME + BACKGROUND + @"0\" + @"Down_Clear_4th.png");
            Background_Down_Scroll = TxC(GAME + BACKGROUND + @"0\" + @"Down_Scroll.png");
            Background_Down_sakura = TxC(GAME + BACKGROUND + @"0\" + @"sakura.png");
            Background_Up_Base_Dan = TxC(GAME + BACKGROUND + @"0\" + @"Dan_Up_Base.png");
            Background_Up_Base_2nd_Dan = TxC(GAME + BACKGROUND + @"0\" + @"Dan_Up_Base_2nd.png");
            Background_Up_Dan = TxC(GAME + BACKGROUND + @"0\" + @"Dan_Up.png");
            Background_Up_2nd_Dan = TxC(GAME + BACKGROUND + @"0\" + @"Dan_Up_2nd.png");
            Background_Up_3rd_Dan = TxC(GAME + BACKGROUND + @"0\" + @"Dan_Up_3rd.png");

            TJAPlayer3.Skin.Game_Background_Ptn_Kame = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + BACKGROUND + @"0\" + @"kame\"));
            if (TJAPlayer3.Skin.Game_Background_Ptn_Kame != 0)
            {
                Background_Down_kame = new CTexture[TJAPlayer3.Skin.Game_Background_Ptn_Kame];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Background_Ptn_Kame; i++)
                {
                    Background_Down_kame[i] = TxC(GAME + BACKGROUND + @"0\" + @"kame\" + i.ToString() + ".png");
                }
            }
            #endregion
            #region 太鼓
            Taiko_Stage_Text = TxC(GAME + TAIKO + @"Stage_Text.png");
            Taiko_Background = new CTexture[4];
            Taiko_Background[0] = TxC(GAME + TAIKO + @"1P_Background.png");
            Taiko_Background[1] = TxC(GAME + TAIKO + @"2P_Background.png");
            Taiko_Background[2] = TxC(GAME + TAIKO + @"1P_Background_Dan.png");
            Taiko_Background[3] = TxC(GAME + TAIKO + @"2P_Change_Background.png");
            Taiko_Frame = new CTexture[3];
            Taiko_Frame[0] = TxC(GAME + TAIKO + @"1P_Frame.png");
            Taiko_Frame[1] = TxC(GAME + TAIKO + @"2P_Frame.png");
            Taiko_Frame[2] = TxC(GAME + TAIKO + @"1P_Frame_Dan.png");
            Taiko_PlayerNumber = new CTexture[3];
            Taiko_PlayerNumber[0] = TxC(GAME + TAIKO + @"1P_PlayerNumber.png");
            Taiko_PlayerNumber[1] = TxC(GAME + TAIKO + @"2P_PlayerNumber.png");
            Taiko_PlayerNumber[2] = TxC(GAME + TAIKO + @"2P_Change_PlayerNumber.png");
            Taiko_NamePlate = new CTexture[2];
            Taiko_NamePlate[0] = TxC(GAME + TAIKO + @"1P_NamePlate.png");
            Taiko_NamePlate[1] = TxC(GAME + TAIKO + @"2P_NamePlate.png");
            Taiko_Base = TxC(GAME + TAIKO + @"Base.png");
            Taiko_Base_Old = TxC(GAME + TAIKO + @"Old_Base.png");
            Taiko_Don_Left_Old = TxC(GAME + TAIKO + @"Don_Old.png");
            Taiko_Don_Right_Old = TxC(GAME + TAIKO + @"Don_Old.png");
            Taiko_Ka_Left_Old = TxC(GAME + TAIKO + @"Ka_Old.png");
            Taiko_Ka_Right_Old = TxC(GAME + TAIKO + @"Ka_Old.png");
            Taiko_Don_Left = TxC(GAME + TAIKO + @"Don.png");
            Taiko_Don_Right = TxC(GAME + TAIKO + @"Don.png");
            Taiko_Ka_Left = TxC(GAME + TAIKO + @"Ka.png");
            Taiko_Ka_Right = TxC(GAME + TAIKO + @"Ka.png");
            Taiko_LevelUp = TxC(GAME + TAIKO + @"LevelUp.png");
            Taiko_LevelDown = TxC(GAME + TAIKO + @"LevelDown.png");
            Couse_Symbol = new CTexture[(int)Difficulty.Total + 1]; // +1は真打ちモードの分
            string[] Couse_Symbols = new string[(int)Difficulty.Total + 1] { "Easy", "Normal", "Hard", "Oni", "Edit", "Tower", "Dan", "Shin" };
            for (int i = 0; i < (int)Difficulty.Total + 1; i++)
            {
                Couse_Symbol[i] = TxC(GAME + COURSESYMBOL + @"New\" + Couse_Symbols[i] + ".png");
            }
            Couse_Symbol_Dan = new CTexture[(int)Difficulty.Total + 1]; // +1は真打ちモードの分
            string[] Couse_Symbols_Dan = new string[(int)Difficulty.Total + 1] { "Easy", "Normal", "Hard", "Oni", "Edit", "Tower", "Dan", "Shin" };
            for (int i = 0; i < (int)Difficulty.Total + 1; i++)
            {
                Couse_Symbol_Dan[i] = TxC(GAME + COURSESYMBOL + @"Old\" + Couse_Symbols_Dan[i] + ".png");
            }
            Taiko_PlateText = TxC(GAME + TAIKO + @"Plate_Text.png");
            Taiko_Score = new CTexture[4];
            Taiko_Score[0] = TxC(GAME + TAIKO + @"Score.png");
            Taiko_Score[1] = TxC(GAME + TAIKO + @"Score_1P.png");
            Taiko_Score[2] = TxC(GAME + TAIKO + @"Score_2P.png");
            Taiko_Score[3] = TxC(GAME + TAIKO + @"Score_2P_Change.png");
            Taiko_Combo = new CTexture[4];
            Taiko_Combo[0] = TxC(GAME + TAIKO + @"Combo.png");
            Taiko_Combo[1] = TxC(GAME + TAIKO + @"Combo_Big.png");
            Taiko_Combo[2] = TxC(GAME + TAIKO + @"Combo_Old.png");
            Taiko_Combo[3] = TxC(GAME + TAIKO + @"Combo_Big_Old.png");
            Taiko_Combo_Effect = TxC(GAME + TAIKO + @"Combo_Effect.png");
            Taiko_Combo_Text = TxC(GAME + TAIKO + @"Combo_Text.png");
            Taiko_Combo_Text_Old = TxC(GAME + TAIKO + @"Combo_Text_Old.png");
            #endregion
            #region ゲージ
            Gauge = new CTexture[8];
            Gauge[0] = TxC(GAME + GAUGE + @"1P.png");
            Gauge[1] = TxC(GAME + GAUGE + @"2P.png");
            Gauge[2] = TxC(GAME + GAUGE + @"1P_Dan.png");
            Gauge[3] = TxC(GAME + GAUGE + @"Flash.png");
            Gauge[4] = TxC(GAME + GAUGE + @"2P_Change.png");
            Gauge[5] = TxC(GAME + GAUGE + @"2P_Change_Dan.png");
            Gauge[6] = TxC(GAME + GAUGE + @"1P_Easy.png");
            Gauge[7] = TxC(GAME + GAUGE + @"1P_Hard.png");
            Gauge_Base = new CTexture[5];
            Gauge_Base[0] = TxC(GAME + GAUGE + @"1P_Base.png");
            Gauge_Base[1] = TxC(GAME + GAUGE + @"2P_Base.png");
            Gauge_Base[2] = TxC(GAME + GAUGE + @"1P_Base_Dan.png");
            Gauge_Base[3] = TxC(GAME + GAUGE + @"2P_Change_Base.png");
            Gauge_Base[4] = TxC(GAME + GAUGE + @"2P_Change_Base_Dan.png");
            Gauge_Line = new CTexture[2];
            Gauge_Line[0] = TxC(GAME + GAUGE + @"1P_Line.png");
            Gauge_Line[1] = TxC(GAME + GAUGE + @"2P_Line.png");
            TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + GAUGE + @"Rainbow\"));
            if (TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn != 0)
            {
                Gauge_Rainbow = new CTexture[TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn; i++)
                {
                    Gauge_Rainbow[i] = TxC(GAME + GAUGE + @"Rainbow\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + GAUGE + @"Rainbow_Dan\"));
            if (TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn != 0)
            {
                Gauge_Rainbow_Dan = new CTexture[TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn];
                for (int i = 0; i < TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn; i++)
                {
                    Gauge_Rainbow_Dan[i] = TxC(GAME + GAUGE + @"Rainbow_Dan\" + i.ToString() + ".png");
                }
            }
            Gauge_Soul = TxC(GAME + GAUGE + @"Soul.png");
            Gauge_Soul_Fire = TxC(GAME + GAUGE + @"Fire.png");
            Gauge_Soul_Explosion = new CTexture[2];
            Gauge_Soul_Explosion[0] = TxC(GAME + GAUGE + @"1P_Explosion.png");
            Gauge_Soul_Explosion[1] = TxC(GAME + GAUGE + @"2P_Explosion.png");
            #endregion
            #region 吹き出し
            Balloon_Combo = new CTexture[3];
            Balloon_Combo[0] = TxC(GAME + BALLOON + @"Combo_1P.png");
            Balloon_Combo[1] = TxC(GAME + BALLOON + @"Combo_2P.png");
            Balloon_Combo[2] = TxC(GAME + BALLOON + @"Combo_2P_Change.png");
            Shin_Balloon_Combo = new CTexture[3];
            Shin_Balloon_Combo[0] = TxC(GAME + BALLOON + @"Combo_1P_Shin.png");
            Shin_Balloon_Combo[1] = TxC(GAME + BALLOON + @"Combo_2P_Shin.png");
            Shin_Balloon_Combo[2] = TxC(GAME + BALLOON + @"Combo_2P_Shin_Change.png");
            Balloon_Roll = TxC(GAME + BALLOON + @"Roll.png");
            Balloon_Balloon = TxC(GAME + BALLOON + @"Balloon.png");
            Balloon_Number_Roll = TxC(GAME + BALLOON + @"Number_Roll.png");
            Balloon_Number_Combo = TxC(GAME + BALLOON + @"Number_Combo.png");
            Balloon_Combo_Flash = TxC(GAME + BALLOON + @"Combo_Flash.png");

            Balloon_Breaking = new CTexture[6];
            for (int i = 0; i < 6; i++)
            {
                Balloon_Breaking[i] = TxC(GAME + BALLOON + @"Breaking_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 13; i++)
            {
                Balloon_Combo_Bonus[i] = TxC(GAME + BALLOON + @"Bonus\" + i.ToString() + ".png");
            }
            #endregion
            #region エフェクト
            Effects_Hit_FireWorks_1P = TxCAf(GAME + EFFECTS + @"Hit\FireWorks_1P.png");
            if (Effects_Hit_FireWorks_1P != null) Effects_Hit_FireWorks_1P.b加算合成 = true;
            Effects_Hit_Explosion = TxCAf(GAME + EFFECTS + @"Hit\Explosion.png");
            if (Effects_Hit_Explosion != null) Effects_Hit_Explosion.b加算合成 = TJAPlayer3.Skin.Game_Effect_HitExplosion_AddBlend;
            Effects_Hit_Explosion_Big = TxC(GAME + EFFECTS + @"Hit\Explosion_Big.png");
            if (Effects_Hit_Explosion_Big != null) Effects_Hit_Explosion_Big.b加算合成 = TJAPlayer3.Skin.Game_Effect_HitExplosionBig_AddBlend;
            
            

            Effects_Fire = TxC(GAME + EFFECTS + @"Fire.png");
            if (Effects_Fire != null) Effects_Fire.b加算合成 = TJAPlayer3.Skin.Game_Effect_Fire_AddBlend;

            Effects_Rainbow = TxC(GAME + EFFECTS + @"Rainbow.png");

            Effects_GoGoSplash = TxC(GAME + EFFECTS + @"GoGoSplash.png");
            if (Effects_GoGoSplash != null) Effects_GoGoSplash.b加算合成 = TJAPlayer3.Skin.Game_Effect_GoGoSplash_AddBlend;
            Effects_Hit_Great = new CTexture[15];
            Effects_Hit_Great_Big = new CTexture[15];
            Effects_Hit_Good = new CTexture[15];
            Effects_Hit_Good_Big = new CTexture[15];
            for (int i = 0; i < 15; i++)
            {
                Effects_Hit_Great[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Great\" + i.ToString() + ".png");
                Effects_Hit_Great_Big[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Great_Big\" + i.ToString() + ".png");
                Effects_Hit_Good[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Good\" + i.ToString() + ".png");
                Effects_Hit_Good_Big[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Good_Big\" + i.ToString() + ".png");
            }
            TJAPlayer3.Skin.Game_Effect_Roll_Ptn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + GAME + EFFECTS + @"Roll\"));
            Effects_Roll = new CTexture[TJAPlayer3.Skin.Game_Effect_Roll_Ptn];
            for (int i = 0; i < TJAPlayer3.Skin.Game_Effect_Roll_Ptn; i++)
            {
                Effects_Roll[i] = TxC(GAME + EFFECTS + @"Roll\" + i.ToString() + ".png");
            }
            #endregion
            #region レーン
            Lane_Base = new CTexture[3];
            Lane_Text = new CTexture[3];
            string[] Lanes = new string[3] { "Normal", "Expert", "Master" };
            for (int i = 0; i < 3; i++)
            {
                Lane_Base[i] = TxC(GAME + LANE + "Base_" + Lanes[i] + ".png");
                Lane_Text[i] = TxC(GAME + LANE + "Text_" + Lanes[i] + ".png");
            }
            Lane_Red = TxC(GAME + LANE + @"Red.png");
            Lane_Blue = TxC(GAME + LANE + @"Blue.png");
            Lane_Yellow = TxC(GAME + LANE + @"Yellow.png");
            Lane_Background_Main = TxC(GAME + LANE + @"Background_Main.png");
            Lane_Background_Sub = TxC(GAME + LANE + @"Background_Sub.png");
            Lane_Background_GoGo = TxC(GAME + LANE + @"Background_GoGo.png");

            #endregion
            #region 終了演出
            End_Clear_L = new CTexture[5];
            End_Clear_R = new CTexture[5];
            for (int i = 0; i < 5; i++)
            {
                End_Clear_L[i] = TxC(GAME + END + @"Clear_L_" + i.ToString() + ".png");
                End_Clear_R[i] = TxC(GAME + END + @"Clear_R_" + i.ToString() + ".png");
            }
            End_Clear_Text = TxC(GAME + END + @"Clear_Text.png");
            End_Clear_Text_Effect = TxC(GAME + END + @"Clear_Text_Effect.png");
            if (End_Clear_Text_Effect != null) End_Clear_Text_Effect.b加算合成 = true;
            #endregion
            #region ゲームモード
            GameMode_Timer_Tick = TxC(GAME + GAMEMODE + @"Timer_Tick.png");
            GameMode_Timer_Frame = TxC(GAME + GAMEMODE + @"Timer_Frame.png");
            #endregion
            #region ステージ失敗
            Failed_Game = TxC(GAME + FAILED + @"Game.png");
            Failed_Stage = TxC(GAME + FAILED + @"Stage.png");
            #endregion
            #region ランナー
            Runner = TxC(GAME + RUNNER + @"0.png");
            Runner_Dan = TxC(GAME + RUNNER + @"0_Dan.png");
            #endregion
            #region DanC
            DanC_Background = TxC(GAME + DANC + @"Background.png");
            DanC_Gauge = new CTexture[4];
            DanC_Gauge[0] = TxC(GAME + DANC + @"Gauge_20percent.png");
            DanC_Gauge[1] = TxC(GAME + DANC + @"Gauge_Normal.png");
            DanC_Gauge[2] = TxC(GAME + DANC + @"Gauge_Reach.png");
            DanC_Gauge[3] = TxC(GAME + DANC + @"Gauge_Clear.png");
            DanC_Gauge_Effect = new CTexture[2];
            DanC_Gauge_Effect[0] = TxC(GAME + DANC + @"Gauge_Effect.png");
            DanC_Gauge_Effect[1] = TxC(GAME + DANC + @"Gauge_Effect.png");
            DanC_Base = TxC(GAME + DANC + @"Base.png");
            DanC_Failed = TxC(GAME + DANC + @"Failed.png");
            DanC_Number = TxC(GAME + DANC + @"Number.png");
            DanC_ExamType = TxC(GAME + DANC + @"ExamType.png");
            DanC_ExamRange = TxC(GAME + DANC + @"ExamRange.png");
            DanC_ExamUnit = TxC(GAME + DANC + @"ExamUnit.png");
            DanC_Screen = TxC(GAME + DANC + @"Screen.png");
            #endregion
            #region PuichiChara
            PuchiChara = TxC(GAME + PUCHICHARA + @"0.png");
            #endregion
            #endregion

            #region 6_結果発表
            Result_Judge = TxC(RESULT + @"Judge.png");
            Result_Number = TxC(RESULT + @"Number.png");
            Result_Number_Red = TxC(RESULT + @"Number_Red.png");
            Result_Number_Normal = TxC(RESULT + @"Number_Normal.png");
            Result_Number_Period = TxC(RESULT + @"Number_Period.png");
            Result_Number_Period_Red = TxC(RESULT + @"Number_Period_Red.png");
            Result_Number_Period_Normal = TxC(RESULT + @"Number_Period_Normal.png");
            Result_Number_Percent = TxC(RESULT + @"Number_Percent.png");
            Result_Number_Percent_Red = TxC(RESULT + @"Number_Percent_Red.png");
            Result_Number_Percent_Normal = TxC(RESULT + @"Number_Percent_Normal.png");
            Result_Rank = new CTexture[12];
            Result_Rank[0] = TxC(RESULT + @"Rank\" + @"F.png");
            Result_Rank[1] = TxC(RESULT + @"Rank\" + @"E.png");
            Result_Rank[2] = TxC(RESULT + @"Rank\" + @"D.png");
            Result_Rank[3] = TxC(RESULT + @"Rank\" + @"C.png");
            Result_Rank[4] = TxC(RESULT + @"Rank\" + @"B.png");
            Result_Rank[5] = TxC(RESULT + @"Rank\" + @"A.png");
            Result_Rank[6] = TxC(RESULT + @"Rank\" + @"AA.png");
            Result_Rank[7] = TxC(RESULT + @"Rank\" + @"AAA.png");
            Result_Rank[8] = TxC(RESULT + @"Rank\" + @"S.png");
            Result_Rank[9] = TxC(RESULT + @"Rank\" + @"SS.png");
            Result_Rank[10] = TxC(RESULT + @"Rank\" + @"SSS.png");
            Result_Rank[11] = TxC(RESULT + @"Rank\" + @"Rank.png");
            Result_Panel = TxC(RESULT + @"Panel.png");
            Result_Shin_Panel = TxC(RESULT + @"Panel_Shin.png");
            Result_Score_Text = TxC(RESULT + @"Score_Text.png");
            Result_Score_Number = TxC(RESULT + @"Score_Number.png");
            Result_Dan = TxC(RESULT + @"Dan.png");
            Result_Daiseikou = TxC(RESULT + @"ResultDaiseikou.png");
            Result_Clear = TxC(RESULT + @"ResultClear.png");
            Result_Failed = TxC(RESULT + @"ResultFailed.png");
            Result_Oshii = TxC(RESULT + @"ResultOshii.png");
            Result_Background[0] = TxC(RESULT + @"Background.png");
            Result_Background[1] = TxC(RESULT + @"Background_2P.png");
            Result_Background[2] = TxC(RESULT + @"2P_Change_Background.png");
            Result_Background_Dan = TxC(RESULT + @"Background_Dan.png");
            Result_Background_Dan_2nd = TxC(RESULT + @"Background_Dan_2nd.png");
            Result_FadeIn[0] = TxC(RESULT + @"FadeIn.png");
            Result_FadeIn[1] = TxC(RESULT + @"FadeIn_2P.png");
            Result_FadeIn[2] = TxC(RESULT + @"2P_Change_FadeIn.png");
            Result_FadeIn_Dan = TxC(RESULT + @"FadeIn_Dan.png");
            Result_Header_Clear[0] = TxC(RESULT + @"Header_Clear.png");
            Result_Header_Clear[1] = TxC(RESULT + @"Header_Clear_2P.png");
            Result_Header_Clear[2] = TxC(RESULT + @"2P_Change_Header_Clear.png");
            Result_Mob = TxC(RESULT + @"Result_Mob.png");
            Result_Mob_Clear[0] = TxC(RESULT + @"Result_Mob_Clear_0.png");
            Result_Mob_Clear[1] = TxC(RESULT + @"Result_Mob_Clear_1.png");
            Result_Mob_Failed[0] = TxC(RESULT + @"Result_Failed_Mob_0.png");
            Result_Mob_Failed[1] = TxC(RESULT + @"Result_Failed_Mob_1.png");
            Result_Header = TxC(RESULT + @"Result_Header.png");
            Result_Header_Dan = TxC(RESULT + @"Result_Header_Dan.png");
            Result_Coin = TxC(RESULT + @"Result_Coin.png");
            for (int i = 0; i < 2; i++)
            {
                Result_Gauge[i] = TxC(RESULT + @"Gauge_" + (i + 1).ToString() + "P.png");
                Result_Gauge_Base[i] = TxC(RESULT + @"Gauge_Base_" + (i + 1).ToString() + "P.png");
            }
            TJAPlayer3.Skin.Result_Rainbow_Ptn = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Rainbow\"));
            if (TJAPlayer3.Skin.Result_Rainbow_Ptn != 0)
            {
                Result_Rainbow = new CTexture[TJAPlayer3.Skin.Result_Rainbow_Ptn];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Rainbow_Ptn; i++)
                {
                    Result_Rainbow[i] = TxC(RESULT + @"Rainbow\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Flower = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Flower\"));
            if (TJAPlayer3.Skin.Result_Flower != 0)
            {
                Result_Flower = new CTexture[TJAPlayer3.Skin.Result_Flower];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Flower; i++)
                {
                    Result_Flower[i] = TxC(RESULT + @"Flower\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Daiseikou_In = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Daiseikou_In\"));
            if (TJAPlayer3.Skin.Result_Daiseikou_In != 0)
            {
                Result_Daiseikou_In = new CTexture[TJAPlayer3.Skin.Result_Daiseikou_In];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Daiseikou_In; i++)
                {
                    Result_Daiseikou_In[i] = TxC(RESULT + @"Daiseikou_In\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Daiseikou_Loop = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Daiseikou_Loop\"));
            if (TJAPlayer3.Skin.Result_Daiseikou_Loop != 0)
            {
                Result_Daiseikou_Loop = new CTexture[TJAPlayer3.Skin.Result_Daiseikou_Loop];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Daiseikou_Loop; i++)
                {
                    Result_Daiseikou_Loop[i] = TxC(RESULT + @"Daiseikou_Loop\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Normal_Loop = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Normal_Loop\"));
            if (TJAPlayer3.Skin.Result_Normal_Loop != 0)
            {
                Result_Normal_Loop = new CTexture[TJAPlayer3.Skin.Result_Normal_Loop];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Normal_Loop; i++)
                {
                    Result_Normal_Loop[i] = TxC(RESULT + @"Normal_Loop\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Clear_Loop = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Clear_Loop\"));
            if (TJAPlayer3.Skin.Result_Clear_Loop != 0)
            {
                Result_Clear_Loop = new CTexture[TJAPlayer3.Skin.Result_Clear_Loop];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Clear_Loop; i++)
                {
                    Result_Clear_Loop[i] = TxC(RESULT + @"Clear_Loop\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_In = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Result_In\"));
            if (TJAPlayer3.Skin.Result_Daiseikou_In != 0)
            {
                Result_In = new CTexture[TJAPlayer3.Skin.Result_In];
                for (int i = 0; i < TJAPlayer3.Skin.Result_In; i++)
                {
                    Result_In[i] = TxC(RESULT + @"Result_In\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Failed_Loop = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Failed_Loop\"));
            if (TJAPlayer3.Skin.Result_Failed_Loop != 0)
            {
                Result_Failed_Loop = new CTexture[TJAPlayer3.Skin.Result_Failed_Loop];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Failed_Loop; i++)
                {
                    Result_Failed_Loop[i] = TxC(RESULT + @"Failed_Loop\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Oukan_Clear = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"Clear\"));
            if (TJAPlayer3.Skin.Result_Oukan_Clear != 0)
            {
                Result_Oukan_Clear = new CTexture[TJAPlayer3.Skin.Result_Oukan_Clear];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Oukan_Clear; i++)
                {
                    Result_Oukan_Clear[i] = TxC(RESULT + @"Clear\" + i.ToString() + ".png");
                }
            }
            TJAPlayer3.Skin.Result_Oukan_FullCombo = TJAPlayer3.t連番画像の枚数を数える(CSkin.Path(BASE + RESULT + @"FullCombo\"));
            if (TJAPlayer3.Skin.Result_Oukan_FullCombo != 0)
            {
                Result_Oukan_FullCombo = new CTexture[TJAPlayer3.Skin.Result_Oukan_FullCombo];
                for (int i = 0; i < TJAPlayer3.Skin.Result_Oukan_FullCombo; i++)
                {
                    Result_Oukan_FullCombo[i] = TxC(RESULT + @"FullCombo\" + i.ToString() + ".png");
                }
            }
            Result_Course_Symbol = new CTexture[5];
            string[] Result_Course_Symbols = new string[5] { "Easy", "Normal", "Hard", "Oni", "Edit" };
            for (int i = 0; i < 5; i++)
            {
                Result_Course_Symbol[i] = TxC(RESULT + @"Course_Symbol\" + Result_Course_Symbols[i] + ".png");
            }
            #endregion

            #region 7_終了画面
            Exit_Background = TxC(EXIT + @"Background.png");
            #endregion

        }

        public void DisposeTexture()
        {
            TJAPlayer3.tテクスチャの解放(ref Title_Background);
            TJAPlayer3.tテクスチャの解放(ref Title_Menu);
            #region 共通
            TJAPlayer3.tテクスチャの解放(ref Tile_Black);
            TJAPlayer3.tテクスチャの解放(ref Tile_White);
            TJAPlayer3.tテクスチャの解放(ref Menu_Title);
            TJAPlayer3.tテクスチャの解放(ref Menu_Highlight);
            TJAPlayer3.tテクスチャの解放(ref Enum_Song);
            TJAPlayer3.tテクスチャの解放(ref Scanning_Loudness);
            TJAPlayer3.tテクスチャの解放(ref Overlay);
            for (int i = 0; i < 2; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref NamePlate[i]);
            }
            for (int i = 0; i < 2; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref nP[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.PlateEffect_Ptn; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref PlateEffect[i]);
            }
            #endregion
            #region 1_タイトル画面
            TJAPlayer3.tテクスチャの解放(ref Title_Background);
            TJAPlayer3.tテクスチャの解放(ref Title_Menu);
            #endregion

            #region 2_コンフィグ画面
            TJAPlayer3.tテクスチャの解放(ref Config_Background);
            TJAPlayer3.tテクスチャの解放(ref Config_Cursor);
            TJAPlayer3.tテクスチャの解放(ref Config_ItemBox);
            TJAPlayer3.tテクスチャの解放(ref Config_Arrow);
            TJAPlayer3.tテクスチャの解放(ref Config_KeyAssign);
            TJAPlayer3.tテクスチャの解放(ref Config_Font);
            TJAPlayer3.tテクスチャの解放(ref Config_Font_Bold);
            TJAPlayer3.tテクスチャの解放(ref Config_Enum_Song);
            #endregion

            #region 3_選曲画面
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Background);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Header);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Footer);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Difficulty);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Auto);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Level);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Branch);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Branch_Text);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Frame_Score);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Frame_Box);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Frame_BackBox);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Frame_Random);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Score_Select);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_GenreText);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Cursor_Left);
            TJAPlayer3.tテクスチャの解放(ref SongSelect_Cursor_Right);
            for (int i = 0; i < SongSelect_Bar_Genre.Length; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref SongSelect_Bar_Genre[i]);
            }
            for (int i = 0; i < (int)Difficulty.Total; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref SongSelect_ScoreWindow[i]);
            }

            for (int i = 0; i < SongSelect_GenreBack.Length; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref SongSelect_GenreBack[i]);
            }
            TJAPlayer3.tテクスチャの解放(ref SongSelect_ScoreWindow_Text);
            #endregion

            #region 4_読み込み画面
            TJAPlayer3.tテクスチャの解放(ref SongLoading_Plate);
            TJAPlayer3.tテクスチャの解放(ref SongLoading_FadeIn);
            TJAPlayer3.tテクスチャの解放(ref SongLoading_FadeOut);
            #endregion

            #region 5_演奏画面
            #region 共通
            TJAPlayer3.tテクスチャの解放(ref Notes);
            TJAPlayer3.tテクスチャの解放(ref Judge_Frame);
            TJAPlayer3.tテクスチャの解放(ref SENotes);
            TJAPlayer3.tテクスチャの解放(ref Notes_Arm);
            TJAPlayer3.tテクスチャの解放(ref Judge);

            TJAPlayer3.tテクスチャの解放(ref Judge_Meter);
            TJAPlayer3.tテクスチャの解放(ref Bar);
            TJAPlayer3.tテクスチャの解放(ref Bar_Branch);
            TJAPlayer3.tテクスチャの解放(ref ACHIEVEMENT);

            #endregion
            #region キャラクター

            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Normal; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_Normal[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Clear; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_Normal_Cleared[i]);
                TJAPlayer3.tテクスチャの解放(ref Chara_Normal_Maxed[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_GoGo; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_GoGoTime[i]);
                TJAPlayer3.tテクスチャの解放(ref Chara_GoGoTime_Maxed[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_10combo; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_10Combo[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_10combo_Max; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_10Combo_Maxed[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_GoGoStart[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart_Max; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_GoGoStart_Maxed[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_ClearIn; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_Become_Cleared[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_SoulIn; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_Become_Maxed[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Breaking; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_Balloon_Breaking[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_Balloon_Broke[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Chara_Balloon_Miss[i]);
            }
            #endregion
            #region 踊り子
            for (int i = 0; i < 5; i++)
            {
                for (int p = 0; p < TJAPlayer3.Skin.Game_Dancer_Ptn; p++)
                {
                    TJAPlayer3.tテクスチャの解放(ref Dancer[i][p]);
                }
            }
            #endregion
            #region モブ
            for (int i = 0; i < TJAPlayer3.Skin.Game_Mob_Ptn; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Mob[i]);
            }
            #endregion
            #region フッター
            TJAPlayer3.tテクスチャの解放(ref Mob_Footer);
            #endregion
            #region 背景
            TJAPlayer3.tテクスチャの解放(ref Background);
            TJAPlayer3.tテクスチャの解放(ref Background_Up_Base_Dan);
            TJAPlayer3.tテクスチャの解放(ref Background_Up_Base_2nd_Dan);
            TJAPlayer3.tテクスチャの解放(ref Background_Up_Dan);
            TJAPlayer3.tテクスチャの解放(ref Background_Up_2nd_Dan);
            TJAPlayer3.tテクスチャの解放(ref Background_Up_3rd_Dan);
            TJAPlayer3.tテクスチャの解放(ref Background_Up[0]);
            TJAPlayer3.tテクスチャの解放(ref Background_Up[1]);
            TJAPlayer3.tテクスチャの解放(ref Background_Up[2]);
            TJAPlayer3.tテクスチャの解放(ref Background_Up[3]);
            TJAPlayer3.tテクスチャの解放(ref Background_Up_Clear[0]);
            TJAPlayer3.tテクスチャの解放(ref Background_Up_Clear[1]);
            TJAPlayer3.tテクスチャの解放(ref Background_Down);
            TJAPlayer3.tテクスチャの解放(ref Background_Down_Clear);
            TJAPlayer3.tテクスチャの解放(ref Background_Down_Scroll);


            #endregion
            #region 太鼓
            TJAPlayer3.tテクスチャの解放(ref Taiko_Stage_Text);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Background[0]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Background[1]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Background[2]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Background[3]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Frame[0]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Frame[1]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Frame[2]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_PlayerNumber[0]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_PlayerNumber[1]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_PlayerNumber[2]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_NamePlate[0]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_NamePlate[1]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Base_Old);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Base);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Don_Left_Old);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Don_Right_Old);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Ka_Left_Old);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Ka_Right_Old);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Don_Left);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Don_Right);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Ka_Left);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Ka_Right);
            TJAPlayer3.tテクスチャの解放(ref Taiko_LevelUp);
            TJAPlayer3.tテクスチャの解放(ref Taiko_LevelDown);
            for (int i = 0; i < 6; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Couse_Symbol[i]);
            }
            for (int i = 0; i < 6; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Couse_Symbol_Dan[i]);
            }
            TJAPlayer3.tテクスチャの解放(ref Taiko_PlateText);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Score[0]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Score[1]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Score[2]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Score[3]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Combo[0]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Combo[1]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Combo[2]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Combo[3]);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Combo_Effect);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Combo_Text);
            TJAPlayer3.tテクスチャの解放(ref Taiko_Combo_Text_Old);
            #endregion
            #region ゲージ
            TJAPlayer3.tテクスチャの解放(ref Gauge[0]);
            TJAPlayer3.tテクスチャの解放(ref Gauge[1]);
            TJAPlayer3.tテクスチャの解放(ref Gauge[2]);
            TJAPlayer3.tテクスチャの解放(ref Gauge[3]);
            TJAPlayer3.tテクスチャの解放(ref Gauge[4]);
            TJAPlayer3.tテクスチャの解放(ref Gauge[5]);
            TJAPlayer3.tテクスチャの解放(ref Gauge[6]);
            TJAPlayer3.tテクスチャの解放(ref Gauge[7]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Base[0]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Base[1]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Base[2]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Base[3]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Base[4]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Line[0]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Line[1]);
            for (int i = 0; i < TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Gauge_Rainbow[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Gauge_Rainbow_Ptn; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Gauge_Rainbow_Dan[i]);
            }
            TJAPlayer3.tテクスチャの解放(ref Gauge_Soul);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Soul_Fire);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Soul_Explosion[0]);
            TJAPlayer3.tテクスチャの解放(ref Gauge_Soul_Explosion[1]);
            #endregion
            #region 吹き出し
            TJAPlayer3.tテクスチャの解放(ref Balloon_Combo[0]);
            TJAPlayer3.tテクスチャの解放(ref Balloon_Combo[1]);
            TJAPlayer3.tテクスチャの解放(ref Balloon_Combo[2]);
            TJAPlayer3.tテクスチャの解放(ref Shin_Balloon_Combo[0]);
            TJAPlayer3.tテクスチャの解放(ref Shin_Balloon_Combo[1]);
            TJAPlayer3.tテクスチャの解放(ref Shin_Balloon_Combo[2]);
            TJAPlayer3.tテクスチャの解放(ref Balloon_Roll);
            TJAPlayer3.tテクスチャの解放(ref Balloon_Balloon);
            TJAPlayer3.tテクスチャの解放(ref Balloon_Number_Roll);
            TJAPlayer3.tテクスチャの解放(ref Balloon_Number_Combo);

            for (int i = 0; i < 6; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Balloon_Breaking[i]);
            }
            for (int i = 0; i < 13; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Balloon_Combo_Bonus[i]);
            }
            #endregion
            #region エフェクト
            TJAPlayer3.tテクスチャの解放(ref Effects_Hit_Explosion);
            TJAPlayer3.tテクスチャの解放(ref  Effects_Hit_Explosion_Big);
            TJAPlayer3.tテクスチャの解放(ref Effects_Hit_FireWorks_1P);
            TJAPlayer3.tテクスチャの解放(ref Effects_Hit_FireWorks);
            TJAPlayer3.tテクスチャの解放(ref Effects_Hit_FireWorks);
            TJAPlayer3.tテクスチャの解放(ref Effects_Fire);
            TJAPlayer3.tテクスチャの解放(ref Effects_Rainbow);

            TJAPlayer3.tテクスチャの解放(ref Effects_GoGoSplash);

            for (int i = 0; i < 15; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Effects_Hit_Great[i]);
                TJAPlayer3.tテクスチャの解放(ref Effects_Hit_Great_Big[i]);
                TJAPlayer3.tテクスチャの解放(ref Effects_Hit_Good[i]);
                TJAPlayer3.tテクスチャの解放(ref Effects_Hit_Good_Big[i]);
            }
            for (int i = 0; i < TJAPlayer3.Skin.Game_Effect_Roll_Ptn; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Effects_Roll[i]);
            }
            #endregion
            #region レーン
            for (int i = 0; i < 3; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref Lane_Base[i]);
                TJAPlayer3.tテクスチャの解放(ref Lane_Text[i]);
            }
            TJAPlayer3.tテクスチャの解放(ref Lane_Red);
            TJAPlayer3.tテクスチャの解放(ref Lane_Blue);
            TJAPlayer3.tテクスチャの解放(ref Lane_Yellow);
            TJAPlayer3.tテクスチャの解放(ref Lane_Background_Main);
            TJAPlayer3.tテクスチャの解放(ref Lane_Background_Sub);
            TJAPlayer3.tテクスチャの解放(ref Lane_Background_GoGo);

            #endregion
            #region 終了演出
            for (int i = 0; i < 5; i++)
            {
                TJAPlayer3.tテクスチャの解放(ref End_Clear_L[i]);
                TJAPlayer3.tテクスチャの解放(ref End_Clear_R[i]);
            }
            TJAPlayer3.tテクスチャの解放(ref End_Clear_Text);
            TJAPlayer3.tテクスチャの解放(ref End_Clear_Text_Effect);
            #endregion
            #region ゲームモード
            TJAPlayer3.tテクスチャの解放(ref GameMode_Timer_Tick);
            TJAPlayer3.tテクスチャの解放(ref GameMode_Timer_Frame);
            #endregion
            #region ステージ失敗
            TJAPlayer3.tテクスチャの解放(ref Failed_Game);
            TJAPlayer3.tテクスチャの解放(ref Failed_Stage);
            #endregion
            #region ランナー
            TJAPlayer3.tテクスチャの解放(ref Runner);
            TJAPlayer3.tテクスチャの解放(ref Runner_Dan);
            #endregion
            #region DanC
            DanC_Background?.Dispose();
            DanC_Gauge[0]?.Dispose();
            DanC_Gauge[1]?.Dispose();
            DanC_Gauge[2]?.Dispose();
            DanC_Gauge[3]?.Dispose();
            DanC_Gauge_Effect[0]?.Dispose();
            DanC_Gauge_Effect[1]?.Dispose();
            DanC_20percent?.Dispose();
            DanC_Base?.Dispose();
            DanC_Failed?.Dispose();
            DanC_Number?.Dispose();
            DanC_ExamRange?.Dispose();
            DanC_ExamUnit?.Dispose();
            DanC_ExamType?.Dispose();
            DanC_Screen?.Dispose();
            #endregion
            #region PuchiChara
            TJAPlayer3.tテクスチャの解放(ref PuchiChara);
            #endregion
            #endregion

            #region 6_結果発表
            Result_Rank[0]?.Dispose();
            Result_Rank[1]?.Dispose();
            Result_Rank[2]?.Dispose();
            Result_Rank[3]?.Dispose();
            Result_Rank[4]?.Dispose();
            Result_Rank[5]?.Dispose();
            Result_Rank[6]?.Dispose();
            Result_Rank[7]?.Dispose();
            Result_Rank[8]?.Dispose();
            Result_Rank[9]?.Dispose();
            Result_Rank[10]?.Dispose();
            Result_Rank[11]?.Dispose();
            Result_Judge?.Dispose();
            Result_Number?.Dispose();
            Result_Number_Red?.Dispose();
            Result_Number_Normal?.Dispose();
            Result_Number_Period?.Dispose();
            Result_Number_Period_Red?.Dispose();
            Result_Number_Period_Normal?.Dispose();
            Result_Number_Percent?.Dispose();
            Result_Number_Percent_Red?.Dispose();
            Result_Number_Percent_Normal?.Dispose();
            Result_Panel?.Dispose();
            Result_Shin_Panel?.Dispose();
            Result_Score_Text?.Dispose();
            Result_Mob?.Dispose();
            Result_Score_Number?.Dispose();
            Result_Dan?.Dispose();
            Result_Daiseikou?.Dispose();
            Result_Clear?.Dispose();
            Result_Failed?.Dispose();
            Result_Oshii?.Dispose();
            Result_Background_Dan?.Dispose();
            Result_Background_Dan_2nd?.Dispose();
            Result_FadeIn_Dan?.Dispose();
            Result_Header?.Dispose();
            Result_Header_Dan?.Dispose();
            Result_Rank[0]?.Dispose();
            for (int i = 0; i < 2; i++)
            {
                Result_Gauge[i]?.Dispose();
                Result_Gauge_Base[i]?.Dispose();
                Result_Mob_Clear[i]?.Dispose();
                Result_Mob_Failed[i]?.Dispose();
            }
            for (int i = 0; i < 3; i++)
            {
                Result_Header_Clear[i]?.Dispose();
                Result_Background[i]?.Dispose();
                Result_FadeIn[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Rainbow_Ptn; i++)
            {
                Result_Rainbow[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Flower; i++)
            {
                Result_Flower[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Daiseikou_In; i++)
            {
                Result_Daiseikou_In[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Daiseikou_Loop; i++)
            {
                Result_Daiseikou_Loop[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Clear_Loop; i++)
            {
                Result_Clear_Loop[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Normal_Loop; i++)
            {
                Result_Normal_Loop[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Oukan_Clear; i++)
            {
                Result_Oukan_Clear[i]?.Dispose();
            }
            for (int i = 0; i < TJAPlayer3.Skin.Result_Oukan_FullCombo; i++)
            {
                Result_Oukan_FullCombo[i]?.Dispose();
            }
            for (int i = 0; i < 5; i++)
            {
                Result_Course_Symbol[i]?.Dispose();
            }
            #endregion

            #region 7_終了画面
            TJAPlayer3.tテクスチャの解放(ref Exit_Background);
            #endregion

        }

        #region 共通
        public CTexture Tile_Black,
            Tile_White,
            Menu_Title,
            Menu_Highlight,
            Enum_Song,
            BlackOut,
            Scanning_Loudness,
            Overlay;
        public CTexture[] NamePlate,
              nP,
           PlateEffect;
        #endregion
        #region 1_タイトル画面
        public CTexture Title_Background,
            Title_Menu;
        #endregion

        #region 2_コンフィグ画面
        public CTexture Config_Background,
            Config_Cursor,
            Config_ItemBox,
            Config_Arrow,
            Config_KeyAssign,
            Config_Font,
            Config_Font_Bold,
            Config_Enum_Song;
        #endregion

        #region 3_選曲画面
        public CTexture SongSelect_Background,
            SongSelect_Bar_Center05,
            SongSelect_Bar_Center_Dan,
            SongSelect_Header,
            SongSelect_Header_Red,
            SongSelect_Header_SongSelect,
            SongSelect_Footer,
            SongSelect_Footer_1,
            SongSelect_Difficulty,
            SongSelect_Auto,
            SongSelect_Support,
            SongSelect_ScoreWindow_3_3,
            SongSelect_Level,
            SongSelect_Branch,
            SongSelect_Branch_Text,
            SongSelect_Frame_Score,
            SongSelect_Frame_Box,
            SongSelect_Frame_BackBox,
            SongSelect_Frame_Random,
            SongSelect_Score_Select,
            SongSelect_GenreText,
            SongSelect_Cursor_Left,
            SongSelect_Cursor_Right,
            SongSelect_Bar_BackBox,
            SongSelect_Bar_Center_BackBox,
            SongSelect_Bar_Center_BackBox_Dan,
            SongSelect_taiko,
            SongSelect_2PCoin,
            SongSelect_Timer100,
            SongSelect_ScoreWindow_Text;
        public CTexture[] SongSelect_Bar_Center_BOX = new CTexture[10];
        public CTexture[] SongSelect_GenreBack = new CTexture[10],
            SongSelect_Chara_Start_Song = new CTexture[9],
            SongSelect_BoxBack = new CTexture[11],
            SongSelect_ScoreWindow = new CTexture[(int)Difficulty.Total],
            SongSelect_Bar_Genre = new CTexture[11],
            SongSelect_Bar_Box = new CTexture[11],
            SongSelect_BoxBackC = new CTexture[11],
            SongSelect_Chara = new CTexture[27],
            SongSelect_Inchara = new CTexture[27],
            SongSelect_Bar_Center_Text = new CTexture[10],
            SongSelect_Bar_Center_Header = new CTexture[10],
            SongSelect_Bar_Center = new CTexture[12],
            SongSelect_Timer = new CTexture[10],
            SongSelect_Timerw = new CTexture[10],
            SongSelect_Bar_Genre_Dan = new CTexture[5],
            SongSelect_NamePlate = new CTexture[1];

        #region 難易度選択画面
        public CTexture SongSelect_Difficulty_Dan_Box,
                        SongSelect_Difficulty_Dan_Select,
                        SongSelect_Difficulty_Dan_Select_Effect;
        public CTexture SongSelect_Difficulty_Bar,
            SongSelect_Difficulty_Option,
            SongSelect_Difficulty_Cursor,
            SongSelect_Difficulty_Bar_Back,
            SongSelect_Header_Difficulty,
            SongSelect_Difficulty_Bar_Effect,
            SongSelect_Difficulty_Bar_Back_Effect,
            SongSelect_Difficulty_Select_Back,
            SongSelect_Difficulty_Select_Option,
            SongSelect_Difficulty_Select_SE,
            SongSelect_Difficulty_BOX,
            SongSelect_Difficulty_BOX_Shadow,
            SongSelect_Difficulty_Branch,
            SongSelect_Difficulty_Branch_Edit,
            SongSelect_Difficulty_Star,
            SongSelect_Difficulty_Star_Edit,
            SongSelect_Difficulty_Sin,
            SongSelect_Difficulty_2Speed,
            SongSelect_Difficulty_3Speed,
            SongSelect_Difficulty_4Speed,
            SongSelect_Difficulty_Doron,
            SongSelect_Difficulty_Mirror,
            SongSelect_Difficulty_Super_Random,
            SongSelect_Difficulty_Hyper_Random;

        public CTexture[] SongSelect_Difficulty_Select = new CTexture[5],
            SongSelect_Difficulty_mark_Select = new CTexture[6],
            SongSelect_Difficulty_mark_Select_White = new CTexture[6],
            SongSelect_Difficulty_mark = new CTexture[6],
            SongSelect_Difficulty_Select_Switch = new CTexture[8];
        #endregion

        #endregion

        #region 4_読み込み画面
        public CTexture SongLoading_Plate,
            SongLoading_FadeIn,
            SongLoading_Dan_FadeIn,
            SongLoading_Chara,
            SongLoading_FadeOut;
        #endregion

        #region 5_演奏画面
        #region 共通
        public CTexture Notes,
            Judge_Frame,
            SENotes,
            Notes_Arm,
            Judge;
        public CTexture Judge_Meter,
            Bar,
            Bar_Branch,
            ACHIEVEMENT;
        #endregion
        #region キャラクター
        public CTexture[] Chara_Normal,
            Chara_Normal_Cleared,
            Chara_Normal_Maxed,
            Chara_GoGoTime,
            Chara_GoGoTime_Maxed,
            Chara_10Combo,
            Chara_10Combo_Maxed,
            Chara_GoGoStart,
            Chara_GoGoStart_Maxed,
            Chara_Become_Cleared,
            Chara_Become_Maxed,
            Chara_Balloon_Breaking,
            Chara_Balloon_Broke,
            Chara_Balloon_Miss;
        #endregion
        #region 踊り子
        public CTexture[][] Dancer;
        #endregion
        #region モブ
        public CTexture[] Mob;
        public CTexture Mob_Footer;
        #endregion
        #region 背景
        public CTexture Background,
            Background_Up_Base_Dan,
            Background_Up_Base_2nd_Dan,
            Background_Up_Dan,
            Background_Up_2nd_Dan,
            Background_Up_3rd_Dan,
        Background_Up_Miss,
            Background_Down,
            Background_Down_2nd,
            Background_Down_sakura,
            Background_Down_Clear,
            Background_Down_Clear_2nd,
            Background_Down_Clear_3rd,
            Background_Down_Clear_4th,
            Background_Down_Scroll;
        public CTexture[] Background_Up,
            Background_Up_2nd,
            Background_Up_3rd,
            Background_Up_Clear,
            Background_Up_Clear_2nd,
            Background_Up_Clear_3rd;

        public CTexture[] Background_Down_kame;
        #endregion
        #region 太鼓
        
        public CTexture[] Taiko_Frame, // MTaiko下敷き
            Taiko_Background;

        public CTexture Taiko_Base,
            Taiko_Base_Old,
            Taiko_Don_Left_Old,
            Taiko_Don_Right_Old,
            Taiko_Ka_Left_Old,
            Taiko_Ka_Right_Old,
            Taiko_Don_Left,
            Taiko_Don_Right,
            Taiko_Ka_Left,
            Taiko_Ka_Right,
            Taiko_LevelUp,
            Taiko_LevelDown,
            Taiko_Combo_Effect,
            Taiko_Combo_Text,
            Taiko_Combo_Text_Old,
            Taiko_PlateText,
            Taiko_Stage_Text;
        public CTexture[] Couse_Symbol;// コースシンボル
        public CTexture[] Couse_Symbol_Dan,
            Taiko_PlayerNumber,
            Taiko_NamePlate; // ネームプレート
        public CTexture[] Taiko_Score,
            Taiko_Combo;
        #endregion
        #region ゲージ
        public CTexture[] Gauge,
            Gauge_Base,
            Gauge_Line,
            Gauge_Rainbow,
            Gauge_Rainbow_Dan,
            Gauge_Soul_Explosion;
        public CTexture Gauge_Soul,
            Gauge_Soul_Fire;
        #endregion
        #region 吹き出し
        public CTexture[] Balloon_Combo;
        public CTexture[] Shin_Balloon_Combo = new CTexture[2];
        public CTexture Balloon_Roll,
            Balloon_Balloon,
            Balloon_Number_Roll,
            Balloon_Number_Combo,/*,*/
            Balloon_Combo_Flash;
                                /*Balloon_Broken*/
        public CTexture[] Balloon_Breaking;
        public CTexture[] Balloon_Combo_Bonus = new CTexture[13];
        #endregion
        #region エフェクト
        public CTexture Effects_Hit_Explosion,
            Effects_Hit_Explosion_Big,
            Effects_Hit_FireWorks_1P,
            Effects_Fire,
            Effects_Rainbow,
            Effects_GoGoSplash,
            Effects_Hit_FireWorks;
        public CTexture[] Effects_Hit_Great,
            Effects_Hit_Good,
            Effects_Hit_Great_Big,
            Effects_Hit_Good_Big;
        public CTexture[] Effects_Roll;
        #endregion
        #region レーン
        public CTexture[] Lane_Base,
            Lane_Text;
        public CTexture Lane_Red,
            Lane_Blue,
            Lane_Yellow;
        public CTexture Lane_Background_Main,
            Lane_Background_Sub,
            Lane_Background_GoGo;
        #endregion
        #region 終了演出
        public CTexture[] End_Clear_L,
            End_Clear_R;
        public CTexture End_Clear_Text,
            End_Clear_Text_Effect;
        #endregion
        #region ゲームモード
        public CTexture GameMode_Timer_Frame,
            GameMode_Timer_Tick;
        #endregion
        #region ステージ失敗
        public CTexture Failed_Game,
            Failed_Stage;
        #endregion
        #region ランナー
        public CTexture Runner;
        public CTexture Runner_Dan;
        #endregion
        #region DanC
        public CTexture DanC_Background;
        public CTexture[] DanC_Gauge_Effect;
        public CTexture[] DanC_Gauge;
        public CTexture DanC_Base;
        public CTexture DanC_Failed;
        public CTexture DanC_Number,
            DanC_20percent,
            DanC_ExamType,
            DanC_ExamRange,
            DanC_ExamUnit;
        public CTexture DanC_Screen;
        #endregion
        #region PuchiChara
        public CTexture PuchiChara;
        #endregion
        #endregion

        #region 6_結果発表
        public CTexture Result_Judge,
            Result_Number,
            Result_Number_Red,
            Result_Number_Normal,
            Result_Number_Period,
            Result_Number_Period_Red,
            Result_Number_Period_Normal,
            Result_Number_Percent,
            Result_Number_Percent_Red,
            Result_Number_Percent_Normal,
            Result_Panel,
            Result_Shin_Panel,
            Result_Score_Text,
            Result_Score_Number,
            Result_Dan,
            Result_Background_Dan,
            Result_Background_Dan_2nd,
            Result_FadeIn_Dan,
            Result_Header,
            Result_Header_Dan,
            Result_Coin,
            Result_Mob;
        public CTexture[] Result_Rainbow;
        public CTexture[] Result_Rank;
        public CTexture Result_Daiseikou,
            Result_Clear,
            Result_Failed,
            Result_Oshii;
        public CTexture[] Result_Gauge = new CTexture[2],
            Result_Gauge_Base = new CTexture[2],
            Result_Background = new CTexture[3],
            Result_Header_Clear = new CTexture[3],
            Result_FadeIn = new CTexture[3],
            Result_Mob_Clear = new CTexture[2];
        public CTexture[] Result_Flower = new CTexture[1],
            Result_Mob_Failed = new CTexture[2],
            Result_Daiseikou_In = new CTexture[1],
            Result_Daiseikou_Loop = new CTexture[1],
            Result_Normal_Loop = new CTexture[1],
            Result_Clear_Loop = new CTexture[1],
            Result_In = new CTexture[1],
            Result_Failed_Loop = new CTexture[1];
        public CTexture[] Result_Oukan_Clear = new CTexture[1],
            Result_Oukan_FullCombo = new CTexture[1],
            Result_Course_Symbol;
        #endregion

        #region 7_終了画面
        public CTexture Exit_Background/* , */
                                       /*Exit_Text */;
        private CTexture Taiko_Background_Dan;

        public CTexture SongSelect_Bar_Center_NOT { get; private set; }
        #endregion

    }
}
