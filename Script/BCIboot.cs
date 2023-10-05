using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;



public class BCIboot : MonoBehaviour
{
    public static bool flag=true;

    private static readonly string FoldePath = Application.streamingAssetsPath + "/OpenBCI_GUI";
    private static readonly string ExePath = FoldePath + "/OpenBCI_GUI.exe";//実行ファイル
    private Process BCIexe;
    public void OnClick()
    {
        if(flag=true){
          BCIexe=new Process();


        //プロセス起動に必要な値をセット
        BCIexe.StartInfo = new ProcessStartInfo
        {
                FileName = ExePath,                        // 起動するファイルのパスを指定する
                UseShellExecute = false,                    // プロセスの起動にオペレーティング システムのシェルを使用するかどうか(既定値:true)
                WorkingDirectory = FoldePath,              // 開始するプロセスの作業ディレクトリを取得または設定する(既定値:"")
                RedirectStandardInput = true,               // StandardInput から入力を読み取る(既定値：false)
                RedirectStandardOutput = true,              // 出力を StandardOutput に書き込むかどうか(既定値：false)
                CreateNoWindow = true,                      // プロセス用の新しいウィンドウを作成せずにプロセスを起動するかどうか(既定値：false)
                
        };

        //外部プロセスの終了を検知する
            BCIexe.EnableRaisingEvents = true;
            BCIexe.Exited += DisposeProcess;
        
            // プロセスを起動する
            BCIexe.Start();
            BCIexe.BeginOutputReadLine();
            UnityEngine.Debug.Log("起動完了");
            flag=true;
        }

    }
  private void Awake()
  {
   

  }
//    void SceneLoaded (Scene nextScene, LoadSceneMode mode) {
//         UnityEngine.Debug.Log(nextScene.name);
//         UnityEngine.Debug.Log(mode);
//         DisposeProcess();
//         BCIexe.Close();

//     }

//      private void OnApplicationQuit()
//     {
      
//        DisposeProcess();
//     }

    private static void OnStandardOut(object sender, DataReceivedEventArgs e)
        => UnityEngine.Debug.Log($"外部プロセスの標準出力 : {e.Data}");
    
    private void DisposeProcess(object sender, EventArgs e)
        => DisposeProcess();


    

    

    private void DisposeProcess()
    {
        if (BCIexe == null || BCIexe.HasExited) return;
        
        BCIexe.StandardInput.Close();
        BCIexe.CloseMainWindow();
        BCIexe.Dispose();
        BCIexe = null;
    }
}
