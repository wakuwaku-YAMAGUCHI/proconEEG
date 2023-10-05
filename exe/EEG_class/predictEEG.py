# 実行時、後ろに被験者のIDを付ける(例：predictEEG.exe 20000001)
import os
import csv
import time
import sys
import datetime

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

from scipy.signal import welch
from watchdog.events import FileSystemEventHandler
from watchdog.observers import Observer

import sqlite3
from threading import Timer

predict=[]
user=os.getlogin()
text_path="C:\\Users\\"+user+"\\Documents\\OpenBCI_GUI\\Recordings\\OpenBCISession_EEG"
csv_path=".\\csv\\"
args = sys.argv # IDを引数としてもらう
s_id = 10009999 # 仮
p_id = args[1]  # 第一引数をp_idに

#path="C:\\Users\\Shimizu_S\\Desktop\\venv\\dist\\"


#参考：https://qiita.com/chnokrn/items/57c89382942778a20e7f
#　　　https://mori-memo.hateblo.jp/entry/2022/04/20/234122

class ChangeHandler(FileSystemEventHandler):
    
    # データベースへの接続
    def connectSQL(self):
        dbname = ('brain2.db')#データベース名.db拡張子で設定 同じフォルダにdbファイルを用意
        conn = sqlite3.connect(dbname, isolation_level=None)#データベースを作成、自動コミット機能ON
        return conn

    # SQL文を作成する用の関数、INSERT文で用いる
    def makeStrData(self, sid, eid, state, serverData):
        data = ((sid, eid, state))#挿入するレコードを指定
        # Ch0~7をdataに加えていく
        for sd in serverData:
            data = data + (sd,)
        # 現在時刻(何時かだけ)と日付を記憶
        dt_now = datetime.datetime.now()
        data = data + (dt_now.strftime('%H:%M:%S.%f'), dt_now.strftime('%Y-%m-%d'))
        return data

    # SQLにデータを挿入
    def insertSQL2(self, cursor, conn, sid, eid, state, serverData):
        sql = """INSERT INTO eeg_data2 VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)""" #?は後で値を受け取るの数
        # data = ((sid, eid, state))#挿入するレコードを指定
        cursor.execute(sql, self.makeStrData(sid, eid, state, serverData))#executeコマンドでSQL文を実行
        conn.commit()#コミット

    #テキストファイル読み込み
    def read(self,path):
        n=201

        col_names = ['c{0:02d}'.format(i) for i in range(100)]
        text = pd.read_csv (path,names = col_names)
        text.to_csv (csv_path+"to_csv.csv", index=None)

        df = pd.read_csv(csv_path+"to_csv.csv", usecols=[1,2,3,4,5,6,7,8], header=None)
        df=df.tail(n)
        df = df.T
        df=df.astype(float)

        return df

    #パワースペクトル密度
    def FFT(self,x):
        s_rate=201          #サンプリング数
        dt=1/s_rate         #サンプリング周期
        fc=30               #カットオフ周波数
        n=int(s_rate * 1)   #個数
        
        #パワースペクトル密度を算出
        freq,psd=welch(x,s_rate,window='hann')

        # fig, ax = plt.subplots(figsize=(10, 5))
        # ax.plot(freq, psd, label="PSD by Welch's method", c='C1')
        # ax.set_title('Estimation of PSD')
        # ax.set_xlabel('frequency')
        # ax.set_ylabel('PSD')
        # ax.set_xlim(0, 30)
        # ax.set_ylim(0,20)
        # ax.grid()
        # ax.legend()

        return psd

    #脳波分類
    def classification(self,path):
        alpha=[]
        beta=[]
        #other=[]
        line=0
        
        pre=self.read(path)
        #print(pre)
        for i in range(len(pre)):
            psd=self.FFT(pre.iloc[i,:,])
            alpha.append(np.mean(psd[4:12]))
            beta.append(np.mean(psd[12:30]))
            #other.append(np.mean(psd[30:50]))

        a=np.mean(alpha)
        b=np.mean(beta)
        conn = self.connectSQL() #SQLに接続
        cursor = conn.cursor() #カーソルオブジェクトを作成
        pre = pre.T # データベースに入力する用に、再度転置

        if a>b :
            with open(csv_path+"num.csv", 'a+') as f:
                writer = csv.writer(f)
                writer.writerow([0])
                # dfの先頭から最終行まで、データベースに登録(stateを0で)
                for index, row in pre.iterrows():
                    self.insertSQL2(cursor, conn, s_id, p_id, 0, row)
        
        else:
            with open(csv_path+"num.csv", 'a+') as f:
                writer = csv.writer(f)
                writer.writerow([1])
                # dfの先頭から最終行まで、データベースに登録(stateを1で)
                for index, row in pre.iterrows():
                    self.insertSQL2(cursor, conn, s_id, p_id, 1, row)

 
    #ファイルやフォルダが作成された場合
    # def on_created(self, event):
    #     filepath = event.src_path
    #     self.classification(filepath)
 
    #ファイルやフォルダが更新された場合
    def on_modified(self, event):
        filepath = event.src_path
        self.classification(filepath)



def main():
    event_handler = ChangeHandler()
    observer = Observer()

    observer.schedule(event_handler, text_path, recursive=True)
    observer.start()

    while True:
        time.sleep(1)
    

if __name__ == '__main__':
    main()