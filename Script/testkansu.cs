using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;



public class testkansu : MonoBehaviour
{
    private TextAsset NouhaFile;
    List<string[]> NouhaData=new List<string[]>();
    //public GameObject Path=null;//パスを確認する用
    private string File="/csv/num.csv";//ファイル名
    //ファイル読み取り用

    public static float mp=0;
    public static int life=0;
    public static float con;
    public static float Gametime=0;
    
    private float time;
    private int line=0;


    private static readonly string FoldePath = Application.streamingAssetsPath + "/EEG_class/dist";
    private static readonly string ExePath = FoldePath + "/predictEEG.exe";//実行ファイル
    private Process kakikomiexe;
    public static string flag;
    


  private void Awake()
  {
    kakikomiexe=new Process();
    flag="-1";

    //プロセス起動に必要な値をセット
    kakikomiexe.StartInfo = new ProcessStartInfo
    {

            FileName = ExePath,                        // 起動するファイルのパスを指定する
            UseShellExecute = false,                    // プロセスの起動にオペレーティング システムのシェルを使用するかどうか(既定値:true)
            WorkingDirectory = FoldePath,              // 開始するプロセスの作業ディレクトリを取得または設定する(既定値:"")
            RedirectStandardInput = true,               // StandardInput から入力を読み取る(既定値：false)
            RedirectStandardOutput = true,              // 出力を StandardOutput に書き込むかどうか(既定値：false)
            CreateNoWindow = true,                     // プロセス用の新しいウィンドウを作成せずにプロセスを起動するかどうか(既定値：false)
            ArgumentList =
            {
                GetID.ID
            }                     
            
    };

    //外部プロセスの終了を検知する
        kakikomiexe.EnableRaisingEvents = true;
        kakikomiexe.Exited += DisposeProcess;

        // プロセスを起動する
        kakikomiexe.Start();
        kakikomiexe.BeginOutputReadLine();
        UnityEngine.Debug.Log("起動完了");


  }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log(Application.streamingAssetsPath);
        con=0;
        Gametime=0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        time=time+Time.deltaTime;
       
        if(time>1.001f){
          
          using(var Str=new FileStream(FoldePath+File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)){
            Str.Seek(line,System.IO.SeekOrigin.Begin);
           // Str.Seek(-4,System.IO.SeekOrigin.End);
            using(StreamReader ReadFile=new StreamReader(Str)){
             // Debug.Log("1+");
               Gametime++;
               mp=0.001f;
               life=0;
              if(!ReadFile.EndOfStream){
               //   Debug.Log("2+");
                  
                  flag=ReadFile.ReadLine();
                  UnityEngine.Debug.Log(flag);
                  if(flag=="1"){
                      con++;
                      mp = 3f;

                          //Debug.Log("1");
                  }

                  if(flag=="0"){
                           life =  1;
                  }
                 // Debug.Log("3+");
                 line+=12;
                  time=0f;

              }
              else
              {
                 line=0;
              }
            }
          }
         


            
        }
     }

     void SceneLoaded (Scene nextScene, LoadSceneMode mode) {
       flag="2";
        UnityEngine.Debug.Log(nextScene.name);
        UnityEngine.Debug.Log(mode);
        DisposeProcess();
        kakikomiexe.Close();

    }

     private void OnApplicationQuit()
    {
      flag="2";
       DisposeProcess();
    }

    private static void OnStandardOut(object sender, DataReceivedEventArgs e)
        => UnityEngine.Debug.Log($"外部プロセスの標準出力 : {e.Data}");
    
    private void DisposeProcess(object sender, EventArgs e)
        => DisposeProcess();


    

    


    private void DisposeProcess()
    {
        if (kakikomiexe == null || kakikomiexe.HasExited) return;
        
        kakikomiexe.StandardInput.Close();
        kakikomiexe.CloseMainWindow();
        kakikomiexe.Dispose();
        kakikomiexe = null;
    }
}
