//Lesson 26 DCモータ編
//https://omoroya.com/

// 電子部品は電源モジュールとドライバIC（L293D）を使用

// 変数の設定
const int ENABLE = 7;
const int CH1 = 5;
const int CH2 = 3;
const int ENABLE2 = 12;
const int CH3 = 10;
const int CH4 = 9;
 
void setup() {
  //Pinの方向を定義
  pinMode(ENABLE,OUTPUT); // 7番ピンをOUTPUT指定
  pinMode(CH1,OUTPUT);    // 5番ピンをOUTPUT指定
  pinMode(CH2,OUTPUT);    // 3版ピンをOUTPUT指定

  pinMode(ENABLE2,OUTPUT); // 8番ピンをOUTPUT指定
  pinMode(CH3,OUTPUT);    // 11番ピンをOUTPUT指定
  pinMode(CH4,OUTPUT);    // 10版ピンをOUTPUT指定
  Serial.begin(9600);

  // 初期化 DCモータが突然動きださないように
  digitalWrite(ENABLE,LOW); // disable
  digitalWrite(ENABLE2,LOW); // disable
  delay(500);
}

void loop() {
  if (Serial.available() > 0 )     // 受信したデータが存在した場合以下を実行
  {
    char val = Serial.read();      // char文字として受信データの読み込み

    // CH1をPWM制御するこで回転スピードを調整
    // 4の場合 低速回転
    if (val == '1')
    {
      //Serial.println("4:PWM Low speed");
      digitalWrite(ENABLE2,LOW); // disable
      digitalWrite(ENABLE,HIGH); // enable on
      analogWrite(CH1,255);      // 低速で回転させるためのきっかけ
      delay(100);                // 低速で回転させるための調整時間
      analogWrite(CH1,190);      // CH1をパルス変調 約50%
      digitalWrite(CH2,LOW);     // CH2はLow固定
    }

    else if (val == '0')
    {
      //Serial.println("0:STOP");
      digitalWrite(ENABLE,LOW); // disable
      digitalWrite(ENABLE2,HIGH); // enable on
      analogWrite(CH3,255);      // 低速で回転させるためのきっかけ
      delay(100);                // 低速で回転させるための調整時間
      analogWrite(CH3,255);      // CH1をパルス変調 約50%
      digitalWrite(CH4,LOW);     // CH2はLow固定
    }




    // 0の場合 停止
    else if (val == '2')
    {
      //Serial.println("0:STOP");
      digitalWrite(ENABLE,LOW); // disable
      digitalWrite(ENABLE2,LOW); // disable
    }

    
  }
}