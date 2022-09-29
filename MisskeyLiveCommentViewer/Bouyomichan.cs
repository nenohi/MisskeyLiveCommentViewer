using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MisskeyLiveCommentViewer
{
	internal class Bouyomichan
	{
		public Bouyomichan()
		{

		}
        public void Start()
		{
            Speak("ミスキーコメントビュアーと接続しました");
		}
		public void Speak(string s)
		{
            string sMessage = s;
            byte bCode = 0;
            Int16 iVoice = 1;
            Int16 iVolume = -1;
            Int16 iSpeed = -1;
            Int16 iTone = -1;
            Int16 iCommand = 0x0001;

            byte[] bMessage = Encoding.UTF8.GetBytes(sMessage);
            Int32 iLength = bMessage.Length;

            //棒読みちゃんのTCPサーバへ接続
            string sHost = "127.0.0.1"; //棒読みちゃんが動いているホスト
            int iPort = 50001;       //棒読みちゃんのTCPサーバのポート番号(デフォルト値)
            TcpClient tc = null;
            try
            {
                tc = new TcpClient(sHost, iPort);
            }
            catch (Exception)
            {
                Console.WriteLine("接続失敗");
            }

            if (tc != null)
            {
                //メッセージ送信
                using (NetworkStream ns = tc.GetStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ns))
                    {
                        bw.Write(iCommand); //コマンド（ 0:メッセージ読み上げ）
                        bw.Write(iSpeed);   //速度    （-1:棒読みちゃん画面上の設定）
                        bw.Write(iTone);    //音程    （-1:棒読みちゃん画面上の設定）
                        bw.Write(iVolume);  //音量    （-1:棒読みちゃん画面上の設定）
                        bw.Write(iVoice);   //声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
                        bw.Write(bCode);    //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
                        bw.Write(iLength);  //文字列のbyte配列の長さ
                        bw.Write(bMessage); //文字列のbyte配列
                    }
                }

                //切断
                tc.Close();
            }
        }
		

	}
}
