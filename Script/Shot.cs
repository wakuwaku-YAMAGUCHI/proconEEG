using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Shot : MonoBehaviourPunCallbacks
{
    [SerializeField] private MagicDataBase magicDataBase;//データベース呼び出し
    Magic magic;
    private GameObject mahoPrefab= default;

    public GameObject player= default;
    public GameObject cau= default;
    public GameObject Caunter= default;
    public  Rota Rota;
    public  MagicDamage [] MagicDamage;
    private Vector2 pos; //座標保存用
    private float Rot;
    float x;            //x座標
    float y;            //y座標

    public static int Mylife=100;
    private string LoserName;
    private TextMeshProUGUI WinorLose;
    private GameObject UI;
    float RoadTime=0.0f;
    bool flagA=false;
    public static  bool FlagB=false;
    
    public static bool caun=false;
  

    Collider2D other;


    

    // Start is called before the first frame update
    void Start()
    {
        magic =magicDataBase.magics[0];//魔法の種類
        mahoPrefab=magic.Hantei;
        UI=GameObject.Find("WinorLose");
        WinorLose = UI.GetComponent<TextMeshProUGUI>();
        Debug.Log(photonView.OwnerActorNr);
        flagA=false;
        WinorLose.text="";
        Mylife=100;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
            
            if(photonView.IsMine){
            pos=player.transform.position;
            Rot=player.transform.localEulerAngles.y;
            x=pos.x;
            y=pos.y;
            
            
            if(Input.GetMouseButtonDown(1)&&magic.lostmp<=UnityChan2DController.LMP&&caun==false){
                photonView.RPC(nameof(Mashot), RpcTarget.All,x,y,Rot);
                //Debug.Log(MagicDamage[0].MyID);
            }

            if(Input.GetKeyDown ("joystick button 1")&&magic.lostmp<=UnityChan2DController.LMP&&Gamepad.current == null&&caun==false){
                photonView.RPC(nameof(Mashot), RpcTarget.All,x,y,Rot);
                //Debug.Log(MagicDamage[0].MyID);
            }

            if(Input.GetKeyDown ("joystick button 2")&&magic.lostmp<=UnityChan2DController.LMP&&Gamepad.current == null&&caun==false){
                photonView.RPC(nameof(Mashot), RpcTarget.All,x,y,Rot);
                //Debug.Log(MagicDamage[0].MyID);
            }

            if(player.gameObject.GetComponent<Hit>().flag==true)
            {
                photonView.RPC(nameof(Damage), RpcTarget.All);
                player.gameObject.GetComponent<Hit>().flag=false;
                photonView.RPC(nameof(Des), RpcTarget.All);
            }

            if(player.gameObject.GetComponent<Hit>().Cflag==true)
            {
               
                player.gameObject.GetComponent<Hit>().Cflag=false;
                photonView.RPC(nameof(Caunte), RpcTarget.All,x,y,Rot);
                 photonView.RPC(nameof(Des), RpcTarget.All);
            }


            if(0>=Mylife){
                photonView.RPC(nameof(WorL), RpcTarget.All,this.gameObject.name);
                
            }

            if(caun==true){
                photonView.RPC(nameof(CaunOn), RpcTarget.All);
            }

            if(caun==false){
                photonView.RPC(nameof(CaunOff), RpcTarget.All);
            }

            
            if(flagA==true)
                RoadTime+=Time.deltaTime;
            
        }
        if (RoadTime >= 3.0f){
                photonView.RPC(nameof(fin), RpcTarget.All);
        }

        if (Gamepad.current == null) 
            return;            

        if (Gamepad.current.aButton.wasPressedThisFrame&&magic.lostmp<=UnityChan2DController.LMP) {
                photonView.RPC(nameof(Mashot), RpcTarget.All,x,y,Rot);
                //Debug.Log(MagicDamage[0].MyID);
        }

        if (Gamepad.current.bButton.wasPressedThisFrame&&magic.lostmp<=UnityChan2DController.LMP) {
                photonView.RPC(nameof(Mashot), RpcTarget.All,x,y,Rot);
                //Debug.Log(MagicDamage[0].MyID);
        }
    }

    [PunRPC]
    private void Mashot (float xa ,float ya,float roty) {

        
        Rota.getRotate(roty);
        MagicDamage[0].getID(photonView.OwnerActorNr);
        var bullet = Instantiate(mahoPrefab,new Vector2(xa,ya),Quaternion.identity);
       
      
    }

    [PunRPC]
    private void Caunte (float xa ,float ya,float roty) {

        
        Rota.getRotate(roty);
        MagicDamage[0].getID(photonView.OwnerActorNr);
        var bullet = Instantiate(mahoPrefab,new Vector2(xa,ya),Quaternion.identity);
       
      
    }

    [PunRPC]
    private void Des()
    {
        if(player.gameObject.GetComponent<Hit>().otherCollider!=null)
            other=player.gameObject.GetComponent<Hit>().otherCollider;
        
        Destroy(other.gameObject);
        
    }
    
     [PunRPC]
    IEnumerator Damage ()
	{
        
        
        //Text life_text = life_object.GetComponent<Text> ();
        //life_text.text = "×" + life;
		//レイヤーをPlayerDamageに変更
		player.gameObject.layer = 12;
		//while文を10回ループ
		int count = 10;
		while (count > 0){
			//透明にする
			player.gameObject.GetComponent<Renderer>().material.color = new Color (1,1,1,0);
			//0.05秒待つ
			yield return new WaitForSeconds(0.05f);
			//元に戻す
			player.gameObject.GetComponent<Renderer>().material.color = new Color (1,1,1,1);
			//0.05秒待つ
			yield return new WaitForSeconds(0.05f);
			count--;
		}
		//レイヤーをPlayerに戻す
		player.gameObject.layer = 9;
	}

    [PunRPC]
    private void WorL(string player)
    {
        this.LoserName=player;//敗者の名前を入れる
        if(this.LoserName==this.gameObject.name&&photonView.IsMine){
             WinorLose.text="  You Lose  \nリザルト画面に移ります・・・";
        }

        else{
             WinorLose.text="  You Win  \nリザルト画面に移ります・・・";
        }
        flagA=true;      
        
    }

    [PunRPC]
    private void CaunOn()
    {
        Caunter.SetActive(true);
        
    }

    [PunRPC]
    private void CaunOff()
    {
        Caunter.SetActive(false);
        
    }

    [PunRPC]
    private void fin()
    {
        PhotonNetwork.LeaveRoom();
      
        SceneManager.LoadScene("resalt");

    }


}
